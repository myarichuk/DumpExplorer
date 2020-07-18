using Microsoft.Diagnostics.Runtime;
using Raven.Client.Documents;
using SuperDump;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;

namespace DumpExplorer.Core
{
    public class DumpContext
    {
        private readonly IEnumerable<IDataExtractor> _dataExtractors;
        private readonly IDocumentStore _documentStore;

        public event Action<string> ActivityLog;

        public DumpContext(IEnumerable<IDataExtractor> dataExtractors, IDocumentStore documentStore)
        {
            _dataExtractors = dataExtractors ?? throw new ArgumentNullException(nameof(dataExtractors));
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public void ImportFromDump(string dumpPath)
        {
            Debug.Assert(_documentStore != null);

            using var target = DataTarget.LoadDump(dumpPath);

            if (target.ClrVersions.Length == 0)
                throw new InvalidOperationException("Haven't found relevant CLR versions for the dump, cannot continue with the import");

            var runtime = target.CreateRuntime();

            using var bulkInsert = _documentStore.BulkInsert();
            bulkInsert.CompressionLevel = CompressionLevel.Optimal;

            OnActivity("Starting data extraction from the dump.");
            foreach (var dataExtractor in _dataExtractors)
            {
                OnActivity($"Data extractor starting: {dataExtractor.Name}");
                foreach (var dataItem in dataExtractor.ExtractData(runtime, ActivityLog))
                {
                    var newId = bulkInsert.Store(dataItem);
                    OnActivity($"{dataExtractor} recorded '{newId}'");
                }
                OnActivity($"Data extractor finished: {dataExtractor.Name}");
            }
        }

        protected virtual void OnActivity(string message) => ActivityLog?.Invoke(message);
    }
}
