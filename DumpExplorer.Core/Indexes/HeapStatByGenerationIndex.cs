using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DumpExplorer.Core.Indexes
{
    public class HeapStatByGenerationIndex : AbstractIndexCreationTask<Object, HeapStatByGenerationIndex.Result>
    {
        public class Result
        {
            public string Type { get; set; }
            public int Generation { get; set; }
            public long TotalSize { get; set; }
            public long Count { get; set; }
        }

        public HeapStatByGenerationIndex()
        {
            Map = objects => from obj in objects
                             where obj.Type.Name != null
                             select new Result
                             {
                                 Type = obj.Type.Name,
                                 Generation = obj.Generation,
                                 Count = 1,
                                 TotalSize = (long)obj.Size
                             };

            Reduce = results => from result in results
                                group result by new { result.Type, result.Generation } into g
                                select new Result
                                {
                                    Type = g.Key.Type,
                                    Generation = g.Key.Generation,
                                    Count = g.Sum(x => x.Count),
                                    TotalSize = g.Sum(x => x.TotalSize)
                                };
        }
    }
}
