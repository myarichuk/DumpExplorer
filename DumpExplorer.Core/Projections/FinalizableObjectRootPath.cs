using System;
using System.Collections.Generic;
using System.Text;

namespace DumpExplorer.Core
{
    public class FinalizableObjectRootPath
    {
        // Summary:
        //     Gets the location that roots the object.
        public FinalizableObjectRoot Root { get; set; }
        
        // Summary:
        //     Gets the path from Root to a given target object.
        public List<Object> Path { get; set; }
    }
}
