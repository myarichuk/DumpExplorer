using Raven.Client.Documents.Indexes;
using System.Collections.Generic;
using System.Linq;

namespace DumpExplorer.Core.Indexes
{
    //TODO: fix this, at the moment this doesn't work properly
    public class GcRootIndex : AbstractIndexCreationTask<GcRoot, GcRootIndex.Result>
    {
        public class Result
        {
            public Object Root { get; set; }
            public IEnumerable<IEnumerable<Object>> Paths { get; set; }
        }

        public GcRootIndex()
        {
            Map = gcRoots => from rootRecord in gcRoots
                             where rootRecord.Root.Address != 0 && rootRecord.Path.Count > 0
                             select new Result
                             {
                                 Root = LoadDocument<Object>(rootRecord.Root.Id),
                                 Paths = new [] { LoadDocument<Object>(rootRecord.Path.Select(address => $"{nameof(Object)}/{address}")) }
                             };

            Reduce = results => from result in results                                
                                group result by result.Root into g
                                select new Result
                                {
                                    Root = g.Key,
                                    Paths = g.SelectMany(x => x.Paths)
                                };
        }
    }
}
