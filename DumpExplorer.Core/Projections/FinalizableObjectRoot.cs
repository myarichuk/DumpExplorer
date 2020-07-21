using Microsoft.Diagnostics.Runtime;

namespace DumpExplorer.Core
{
    public class FinalizableObjectRoot
    {
        public string Id { get; set; }
        public ulong Address { get; set; }
        public ClrRootKind RootKind { get; set; }
        public int Generation { get; set; }
        public bool IsInterior { get; set; }
        public bool IsPinned { get; set; }
    }
}
