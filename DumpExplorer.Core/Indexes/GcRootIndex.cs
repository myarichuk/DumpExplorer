using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DumpExplorer.Core.Indexes
{
    //TODO: fix this, at the moment this doesn't work properly
    public class GcRootIndex : AbstractIndexCreationTask<GcRootPath, GcRootIndex.Result>
    {
        public class Result
        {
            public Object Root { get; set; }
            public IEnumerable<IEnumerable<Object>> Paths { get; set; }
        }

        public GcRootIndex()
        {
            Map = gcRoots => from rootRecord in gcRoots                                                          
                             select new Result
                             {
                                 Root = LoadDocument<Object>($"{nameof(Object)}/{rootRecord.Root.Address}"),
                                 Paths = new [] { LoadDocument<Object>(rootRecord.Path.Select(address => $"{nameof(Object)}/{address}")) }
                             };

            Reduce = results => from result in results
                                group result by result.Root.Address into g
                                select new Result
                                {
                                    Root = LoadDocument<Object>($"{nameof(Object)}/{g.Key}"),
                                    Paths = g.SelectMany(x => x.Paths)
                                };
        }
    }
}
