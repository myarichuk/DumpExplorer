using DumpExplorer.Core;
using DumpExplorer.Core.DataExtractors;
using Microsoft.Diagnostics.Runtime;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Embedded;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace DumpExplorer.Tests
{
    public class Basics
    {
        private readonly IDocumentStore _store;

        public class ContractSerializer : DefaultRavenContractResolver
        {
            protected override List<MemberInfo> GetSerializableMembers(System.Type objectType)
            {
                var serializableMembers = base.GetSerializableMembers(objectType);
                foreach (var toRemove in serializableMembers
                    .Where(MembersToFilterOut)
                    .ToArray())
                {
                    serializableMembers.Remove(toRemove);
                }
                return serializableMembers;
            }


            private static bool MembersToFilterOut(MemberInfo info)
            {
                if (info is EventInfo)
                    return true;
                var fieldInfo = info as FieldInfo;
                if (fieldInfo != null && 
                    (fieldInfo.IsPublic == false ||
                        fieldInfo.FieldType.IsByRef  ||
                        fieldInfo.FieldType.IsByRefLike ||
                        fieldInfo.FieldType.IsPointer ||
                        fieldInfo.Name == "Helpers" ||
                        fieldInfo.Name == "ClrObjectHelpers" ||
                        fieldInfo.Name == "Heap" ||
                        fieldInfo.Name == "DataReader" ||
                        fieldInfo.Name == "ClrInfo" ||
                        fieldInfo.Name == "DacLibrary" ||
                        fieldInfo.Name == "ClrVersions" ||
                        fieldInfo.Name == "Threads" ||
                        fieldInfo.Name == "Runtime" ||
                        fieldInfo.Name == "Module" ||
                        fieldInfo.FieldType == typeof(IntPtr)))
                    return true;

                var propertyInfo = info as PropertyInfo;
                if (propertyInfo != null &&
                    (propertyInfo.PropertyType.IsByRef ||
                    propertyInfo.PropertyType.IsByRefLike ||
                    propertyInfo.PropertyType.IsPointer ||
                    propertyInfo.PropertyType == typeof(IntPtr) ||
                    propertyInfo.Name == "Helpers" ||
                    propertyInfo.Name == "ClrObjectHelpers" ||
                    propertyInfo.Name == "Heap" ||
                    propertyInfo.Name == "DataReader" ||
                    propertyInfo.Name == "ClrInfo" ||
                    propertyInfo.Name == "DacLibrary" ||
                    propertyInfo.Name == "ClrVersions" ||
                    propertyInfo.Name == "Threads" ||
                    propertyInfo.Name == "Runtime" ||
                    propertyInfo.Name == "Module" ||
                    propertyInfo.PropertyType == typeof(ImmutableArray<ILToNativeMap>)))
                    return true;

                return info.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length > 0;
            }
        }

        public Basics()
        {
            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                AcceptEula = true,
                CommandLineArgs = new List<string> { "Setup.Mode=None", "RunInMemory=true" },
                FrameworkVersion = Environment.Version.ToString()
            });
            _store = EmbeddedServer.Instance.GetDocumentStore(new DatabaseOptions("Dump")
            {
                Conventions = new DocumentConventions
                {
                    CustomizeJsonSerializer = serializer =>
                    {
                        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        serializer.NullValueHandling = NullValueHandling.Ignore;
                        serializer.ContractResolver = new ContractSerializer();
                    }
                }
            });
        }

        [Fact]
        public async Task Can_load_dump()
        {
            using var dumpContext = new DumpContext(_store);

            dumpContext.LoadDump("allocations.dmp");

            await dumpContext.ExtractDataWithAsync(new HeapObjectsExtractor(), new StringsExtractor());

            using var session = _store.OpenSession();

            var objectCount = session.Query<Core.Object>().Count();
            var stringCount = session.Query<Core.String>().Count();

            //sanity check...
            Assert.Equal(146, objectCount);
            Assert.Equal(34, stringCount);
        }
    }
}
