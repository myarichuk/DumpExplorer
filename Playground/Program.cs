using DumpExplorer.Core;
using DumpExplorer.Core.DataExtractors;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;
using Raven.Embedded;
using Sparrow.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        var logger = logRepository.GetLogger("FooBar");

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
                },
                RequestTimeout = TimeSpan.FromMinutes(5)
            }
        });

        using var dumpContext = new DumpContext(store);
        dumpContext.OperationEvent += DumpContext_OperationEvent;
        dumpContext.LoadFromDump("allocations.dmp");

        EmbeddedServer.Instance.OpenStudioInBrowser();  

        dumpContext.ExtractDataWith(new HeapObjectsExtractor(), new ExceptionExtractor(), new StringsExtractor(), new GcRootsExtractor());

        Console.ReadKey();
    }

    private static void DumpContext_OperationEvent(object sender, DumpExplorer.Core.Events.OperationEventArgs e)
    {
        Console.WriteLine($"{e.OperationName} -> {e.Message}");
    }
}