using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DumpExplorer.Core.Indexes
{
    //display candidates for string interning (see https://blog.maartenballiauw.be/post/2016/11/15/exploring-memory-allocation-and-strings.html)
    public class DuplicateStringsIndex : AbstractIndexCreationTask<String, DuplicateStringsIndex.Result>
    {
        public class Result
        {
            public string Value { get; set; }
            public long Count { get; set; }
        }

        public DuplicateStringsIndex()
        {
            Map = strings => from obj in strings
                             where obj.Value != null //precaution
                             select new Result
                             {
                                 Value = obj.Value,
                                 Count = 1,
                             };

            Reduce = results => from result in results
                                group result by result.Value into g
                                select new Result
                                {
                                    Value = g.Key,
                                    Count = g.Sum(x => x.Count),
                                };
        }
    }
}
