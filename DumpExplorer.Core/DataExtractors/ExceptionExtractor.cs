using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DumpExplorer.Core.DataExtractors
{
    public class ExceptionExtractor : IDataExtractor
    {
        public string Name => "Exception Extractor";

        public string Description => "Extract information about all exceptions located on the managed heap";

        public string TypeName => nameof(Exception);

        public IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null)
        {
            if (!clr.Heap.CanWalkHeap)
                throw new InvalidOperationException("Cannot extract exceptions, heap is not in a walkable state");

            return from obj in clr.Heap.EnumerateObjects()
                   where obj.IsException && obj.AsException() != null
                   let segment = clr.Heap.GetSegmentByAddress(obj.Address)
                   let message = GetMessageWithIgnoreException(obj.AsException())
                   let references = obj.EnumerateReferences(true)
                   select AutoMapper.Instance.Map<Core.Exception>(obj.AsException(),
                        opts => opts.AfterMap((_, dst) =>
                        {
                            dst.Id = $"{TypeName}/{dst.Address}";
                            dst.Message = message; //workaround to prevent mapping exceptions
                        }));
        }

        private static string GetMessageWithIgnoreException(ClrException? exception)
        {
            try
            {
                return exception?.Message ?? string.Empty;
            }
            catch(System.Exception)
            {
                return string.Empty;
            }
        }
    }

    
}
