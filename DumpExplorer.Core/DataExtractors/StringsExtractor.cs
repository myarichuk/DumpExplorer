using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DumpExplorer.Core.DataExtractors
{
    public class StringsExtractor : IDataExtractor
    {
        public string Name => "String Extractor";
        public string Description => "Extract information about all strings on the managed heap";
        public string TypeName => nameof(String);

        public IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null)
        {
            if (!clr.Heap.CanWalkHeap)
                throw new InvalidOperationException("Cannot extract strings, heap is not in a walkable state");

            return from obj in clr.Heap.EnumerateObjects()
                   where obj.Type == clr.Heap.StringType
                   let segment = clr.Heap.GetSegmentByAddress(obj.Address)
                   select AutoMapper.Instance.Map<String>(obj,
                        opts => opts.AfterMap((_, dst) => dst.Generation = segment.GetGeneration(dst.Address)));
        }
    }
}
