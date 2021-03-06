﻿using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DumpExplorer.Core.DataExtractors
{
    public class HeapObjectsExtractor : IDataExtractor
    {
        public string Name => "Heap Object Extractor";

        public string Description => "Extract information about all managed objects located on the managed heap";

        public string TypeName => nameof(Object);

        public IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null)
        {
            if (!clr.Heap.CanWalkHeap)
                throw new InvalidOperationException("Cannot extract heap objects, heap is not in a walkable state");
            
            return from obj in clr.Heap.EnumerateObjects()
                   where obj.Type != null
                   let segment = clr.Heap.GetSegmentByAddress(obj.Address)
                   let references = obj.EnumerateReferences(true)
                   select AutoMapper.Instance.Map<Object>(obj, 
                        opts => opts.AfterMap((_, dst) =>
                        {                            
                            dst.Id = $"{TypeName}/{dst.Address}";
                            
                            dst.References = references.Select(refObj => $"{nameof(Object)}/{refObj.Address}").ToList();
                            dst.Generation = segment.GetGeneration(dst.Address);
                            actionLog?.Invoke($"Extracted object of type = {dst.Type.Name}");
                        }));
        }
    }
}
