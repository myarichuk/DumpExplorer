using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace DumpExplorer.Core.DataExtractors
{
    public class GcRootsExtractor : IDataExtractor
    {
        public string Name => "GC Roots Extractor";
        public string Description => "Extract information about GC roots";
        public string TypeName => "gcroot";

        public IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null)
        {
            if(!clr.Heap.CanWalkHeap)
                throw new InvalidOperationException("Cannot extract gc roots, heap is not in a walkable state");

            var gcRootHandle = new GCRoot(clr.Heap);
            var currentObjectName = string.Empty;
            try
            {
                gcRootHandle.ProgressUpdated += ReportGcRootEnumerationProgress;
                foreach (var obj in clr.Heap.EnumerateObjects())
                {
                    currentObjectName = $"{obj.Type.Name} - #{obj.Address}";
                    actionLog?.Invoke($"Enumerating roots for ${currentObjectName}");
                    foreach (var rootPath in gcRootHandle.EnumerateGCRoots(obj.Address, true, Environment.ProcessorCount))
                    {
                        yield return AutoMapper.Instance.Map<GcRootPath>(rootPath,
                            opts => opts.AfterMap((_, dst) => {
                                var segment = clr.Heap.GetSegmentByAddress(rootPath.Root.Object);
                                dst.Generation = segment.GetGeneration(rootPath.Root.Object.Address);
                            }));
                    }
                }
            }
            finally
            {
                gcRootHandle.ProgressUpdated -= ReportGcRootEnumerationProgress;
            }

            void ReportGcRootEnumerationProgress(GCRoot source, long processed) => 
                actionLog?.Invoke($"Finding GC Roots for {currentObjectName}: {processed} objects processed.");
        }
    }
}
