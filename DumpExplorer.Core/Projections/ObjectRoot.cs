using Microsoft.Diagnostics.Runtime;

namespace DumpExplorer.Core
{
    public class ObjectRoot
    {
        public ulong Address { get; set; }
        public Object Object { get; set; }
        public ClrRootKind RootKind { get; set; }
        public int Generation { get; set; }
        public bool IsInterior { get; set; }
        public bool IsPinned { get; set; }
    }
}
