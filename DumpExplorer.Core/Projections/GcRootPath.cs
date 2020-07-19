using Microsoft.Diagnostics.Runtime;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class GcRootPath
    {
        public int Generation { get; set; }

        // Summary:
        //     Gets the location that roots the object.
        public GcRoot Root { get; set; }

        // Summary:
        //     Gets the path from Root to a given target object (object addresses)
        public List<ulong> Path { get; set; }
    }
}
