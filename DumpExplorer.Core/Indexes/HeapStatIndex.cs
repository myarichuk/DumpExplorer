using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.MoreLikeThis;
using System.Linq;

namespace DumpExplorer.Core.Indexes
{
    //index to output similar data to "!dumpheap -stat"
    public class HeapStatIndex : AbstractIndexCreationTask<Object, HeapStatIndex.Result>
    {
        public class Result
        {
            public string Type { get; set; }
            public long TotalSize { get; set; }
            public long Count { get; set; }
        }

        public HeapStatIndex()
        {
            Map = objects => from obj in objects
                             where obj.Type.Name != null
                             select new Result
                             {
                                 Type = obj.Type.Name,
                                 Count = 1,
                                 TotalSize = (long)obj.Size
                             };

            Reduce = results => from result in results
                                group result by result.Type into g
                                select new Result
                                {
                                    Type = g.Key,
                                    Count = g.Sum(x => x.Count),
                                    TotalSize = g.Sum(x => x.TotalSize)
                                };
        }
    }
}
