using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DumpExplorer.Core.DataExtractors
{
    public class HeapObjectsExtractor : IDataExtractor
    {
        public string Name => "Heap Object Extractor";

        public string Description => "Extract information about all managed objects located on the managed heap";

        public string TypeName => "object";

        public IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null)
        {
            if (!clr.Heap.CanWalkHeap)
                throw new InvalidOperationException("Cannot extract heap objects, heap is not in a walkable state");
            return from obj in clr.Heap.EnumerateObjects()
                   //known issue in importing to RavenDB --> TODO: investigate later
                   where !obj.Type.Name.StartsWith("System.Diagnostics.Tracing")
                   let segment = clr.Heap.GetSegmentByAddress(obj.Address)
                   select AutoMapper.Instance.Map<Object>(obj, 
                        opts => opts.AfterMap((_, dst) => dst.Generation = segment.GetGeneration(dst.Address)));
        }
    }
}
