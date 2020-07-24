using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class GcRoot
    {
        public string Id { get; set; }

        public int Generation { get; set; }

        // Summary:
        //     Gets the location that roots the object.
        public GcRootInfo Root { get; set; }

        // Summary:
        //     Gets the path from Root to a given target object (object addresses)
        public List<string> Path { get; set; }
    }
}
