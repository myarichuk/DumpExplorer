using System;
using System.Collections.Generic;
using System.Text;

namespace DumpExplorer.Core
{
    public class ObjectRootPath
    {
        // Summary:
        //     Gets the location that roots the object.
        public ObjectRoot Root { get; set; }
        
        // Summary:
        //     Gets the path from Root to a given target object.
        public List<Object> Path { get; set; }
    }
}
