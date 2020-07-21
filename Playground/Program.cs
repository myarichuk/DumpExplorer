using DumpExplorer.Core;
using DumpExplorer.Core.DataExtractors;
using Newtonsoft.Json;
using Raven.Client.Documents.Conventions;
using Raven.Embedded;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Program
{
    public class FooBar
    {
        private int _x;
        public ref int X => ref _x;
    }
    public static void Main()
    {
        EmbeddedServer.Instance.StartServer(new ServerOptions
        {
            AcceptEula = true,
            CommandLineArgs = new List<string> { "Setup.Mode=None", "RunInMemory=true" },
            FrameworkVersion = Environment.Version.ToString()
        });
        var store = EmbeddedServer.Instance.GetDocumentStore(new DatabaseOptions("Dump")
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

        using var dumpContext = new DumpContext(store);
        dumpContext.LoadFromDump("allocations.dmp");

        EmbeddedServer.Instance.OpenStudioInBrowser();

        dumpContext.ExtractDataWith(new GcRootsExtractor(), new HeapObjectsExtractor(), new StringsExtractor());

        Console.ReadKey();
    }
}