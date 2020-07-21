using Microsoft.Diagnostics.Runtime;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class GcRoot
    {
        public string RootObjectId { get; set; }

        public ulong RootAddress { get; set; }
        public ClrRootKind RootKind { get; set; }
        public int Generation { get; set; }
        public bool IsInterior { get; set; }
        public bool IsPinned { get; set; }
    }
}
