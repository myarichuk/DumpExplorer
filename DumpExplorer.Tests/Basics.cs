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

       

        static Basics()
        {
            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                AcceptEula = true,
                CommandLineArgs = new List<string> { "Setup.Mode=None", "RunInMemory=true" },
                FrameworkVersion = Environment.Version.ToString()
            });
        }

        public Basics()
        {            
            _store = EmbeddedServer.Instance.GetDocumentStore(new DatabaseOptions("Dump")
            {
                Conventions = new DocumentConventions
                {
                    CustomizeJsonSerializer = serializer =>
                    {
                        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        serializer.NullValueHandling = NullValueHandling.Ignore;
                        serializer.ContractResolver = new DumpExplorerContractSerializer();
                    }
                }
            });
        }

        [Fact]
        public async Task Can_load_dump()
        {
            using var dumpContext = new DumpContext(_store);
            dumpContext.LoadFromDump("allocations.dmp");

            await dumpContext.ExtractDataWithAsync(new HeapObjectsExtractor(), new StringsExtractor());

            using var session = _store.OpenSession();

            var objectCount = session.Query<Core.Object>().Count();
            var stringCount = session.Query<Core.String>().Count();

            //sanity check...
            Assert.Equal(146, objectCount);
            Assert.Equal(34, stringCount);
        }

        [Fact]
        public async Task Can_extract_gc_roots()
        {
            using var dumpContext = new DumpContext(_store);
            dumpContext.LoadFromDump("allocations.dmp");


            await dumpContext.ExtractDataWithAsync(new GcRootsExtractor(), new HeapObjectsExtractor());
            using var session = _store.OpenSession();

            var gcRootCount = session.Query<GcRootPath>().Count();

            //sanity check...
            Assert.Equal(60, gcRootCount);
        }

    }
}
