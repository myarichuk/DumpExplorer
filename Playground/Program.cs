using Raven.Embedded;
using System;
using System.Collections.Generic;

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
        var store = EmbeddedServer.Instance.GetDocumentStore("TestDB");

        using (var session = store.OpenSession())
        {
            session.Store(new FooBar());
            session.SaveChanges();
        }
    }
}