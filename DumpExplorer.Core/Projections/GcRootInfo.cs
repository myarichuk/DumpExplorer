using Microsoft.Diagnostics.Runtime;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class GcRootInfo
    {
        public string Id { get; set; }

        public ulong Address { get; set; }
        public ClrRootKind RootKind { get; set; }
        public bool IsInterior { get; set; }
        public bool IsPinned { get; set; }
    }
}
