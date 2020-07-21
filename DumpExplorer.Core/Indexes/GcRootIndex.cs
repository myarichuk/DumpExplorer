using Raven.Client.Documents.Indexes;
using System.Collections.Generic;
using System.Linq;

namespace DumpExplorer.Core.Indexes
{
    //TODO: fix this, at the moment this doesn't work properly
    public class GcRootIndex : AbstractIndexCreationTask<GcRootPath, GcRootIndex.Result>
    {
        public class Result
        {
            public ulong RootAddress { get; set; }
            public IEnumerable<IEnumerable<Object>> Paths { get; set; }
        }

        public GcRootIndex()
        {
            Map = gcRoots => from rootRecord in gcRoots
                             where rootRecord.Root.RootAddress != 0 && rootRecord.Path.Count > 0
                             select new Result
                             {
                                 RootAddress = rootRecord.Root.RootAddress,
                                 Paths = new [] { LoadDocument<Object>(rootRecord.Path.Select(address => $"{nameof(Object)}/{address}")) }
                             };

            Reduce = results => from result in results
                                group result by result.RootAddress into g
                                select new Result
                                {
                                    RootAddress = g.Key,
                                    Paths = g.SelectMany(x => x.Paths)
                                };
        }
    }
}
