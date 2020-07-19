using Microsoft.Diagnostics.Runtime;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class GcRoot
    {
        public ulong Address { get; set; }
        public ClrRootKind RootKind { get; set; }
        public int Generation { get; set; }
        public bool IsInterior { get; set; }
        public bool IsPinned { get; set; }
    }
}
