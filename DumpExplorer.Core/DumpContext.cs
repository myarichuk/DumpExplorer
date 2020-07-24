using AutoMapper.Internal;
using DumpExplorer.Core.Events;
using DumpExplorer.Core.Indexes;
using Microsoft.Diagnostics.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using SuperDump;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DumpExplorer.Core
{
    public sealed class DumpContext : IDisposable
    {
        private readonly IDocumentStore _documentStore;
        private DataTarget _target;
        private ClrRuntime _runtime;
        private string _databaseName;

        public event EventHandler<OperationEventArgs> OperationEvent;

        public DumpContext(IDocumentStore documentStore, string databaseName = null)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
            _databaseName = databaseName;
        }

        public void ExtractDataWith(params IDataExtractor[] dataExtractors) =>
            dataExtractors.ForAll(extractor => ExtractDataWith(extractor));

        public void ExtractDataWith(IDataExtractor dataExtractor, CancellationToken token = default)
        {
            OnOperationEvent(dataExtractor.Name, TaskStatus.Created);
            using var bulkInsert = _documentStore.BulkInsert(_databaseName, token);
            
            OnOperationEvent(dataExtractor.Name, TaskStatus.Running);
            try
            {
                foreach (var dataItem in dataExtractor.ExtractData(_runtime, message => OnOperationEvent(dataExtractor.Name, TaskStatus.Running, message)))
                {                    
                    var newId = bulkInsert.Store(dataItem);
                    OnOperationEvent(dataExtractor.Name, TaskStatus.Running, $"Imported data item with Id = {newId} (data item type = {dataExtractor.TypeName})");
                }
            }
            catch(System.Exception e)
            {
                OnOperationEvent(dataExtractor.Name, TaskStatus.Faulted, $"Failed data extraction task. Message: {e.Message}, Stack trace: {e.StackTrace}. Retrying one-by-one");
                try
                {
                    foreach (var dataItem in dataExtractor.ExtractData(_runtime, message => OnOperationEvent(dataExtractor.Name, TaskStatus.Running, message)))
                    {
                        using (var session = _documentStore.OpenSession(_databaseName))
                        {
                            session.Store(dataItem);
                            session.SaveChanges();
                            OnOperationEvent(dataExtractor.Name, TaskStatus.Running, $"Imported data item(data item type = {dataExtractor.TypeName})");
                        }
                    }
                }
                catch (System.Exception inner)
                {
                    OnOperationEvent(dataExtractor.Name, TaskStatus.Faulted, $"Failed data extraction task. Message: {inner.Message}, Stack trace: {inner.StackTrace}");
                    throw;
                }
            }

            OnOperationEvent(dataExtractor.Name, TaskStatus.RanToCompletion);
        }

        public void LoadFromDump(string dumpPath)
        {
            Debug.Assert(_documentStore != null);

            if (_target != null)
                Dispose();
            _databaseName ??= dumpPath;

            var getDatabaseRecordResult = _documentStore.Maintenance.Server.Send(new GetDatabaseRecordOperation(_databaseName));
            if(getDatabaseRecordResult == null)//no such database
                _documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(_databaseName)));
            //ensure indexes exist
            _documentStore.ExecuteIndex(new GcRootIndex(), _databaseName);
            _documentStore.ExecuteIndex(new HeapStatIndex(), _databaseName);
            _documentStore.ExecuteIndex(new HeapStatByGenerationIndex(), _databaseName);
            _documentStore.ExecuteIndex(new DuplicateStringsIndex(), _databaseName);


            _target = DataTarget.LoadDump(dumpPath);

            if (_target.ClrVersions.Length == 0)
                throw new InvalidOperationException("Haven't found relevant CLR versions for the dump, cannot continue with the import");

            _runtime = _target.CreateRuntime();           
        }

        public IReadOnlyList<dynamic> Query(string rql)
        {
            using var session = !string.IsNullOrWhiteSpace(_databaseName) ? 
                _documentStore.OpenSession() :_documentStore.OpenSession(_databaseName);
            return session.Advanced.RawQuery<dynamic>(rql).ToList();
        }

        public void Dispose()
        {
            _runtime?.Dispose();
            _runtime = null;

            _target?.Dispose();
            _target = null;

            GC.SuppressFinalize(this);
        }

        ~DumpContext() => Dispose();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnOperationEvent(string operationName, TaskStatus status, string message = null) =>
            OnOperationEvent(new OperationEventArgs(operationName, status, message));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnOperationEvent(OperationEventArgs e) => OperationEvent?.Invoke(this, e);
    }
}
