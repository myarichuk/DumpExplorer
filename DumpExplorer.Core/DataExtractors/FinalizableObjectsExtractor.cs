using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DumpExplorer.Core.DataExtractors
{
    public class FinalizerRootsExtractor : IDataExtractor
    {
        public string Name => "Finalizable Queue Extractor";
        public string Description => "Extract information about managed objects in finalization queue";
        public string TypeName => nameof(FinalizableObjectRoot);

        public IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null)
        {
            if (!clr.Heap.CanWalkHeap)
                throw new InvalidOperationException("Cannot extract heap objects, heap is not in a walkable state");
            
            return from finalizerRoot in clr.Heap.EnumerateFinalizerRoots()
                   let segment = clr.Heap.GetSegmentByAddress(finalizerRoot.Object.Address)
                   select AutoMapper.Instance.Map<FinalizableObjectRoot>(finalizerRoot,
                        opts => opts.AfterMap((_, dst) =>
                                    dst.Generation = segment.GetGeneration(dst.Object.Address)));
        }
    }
}
