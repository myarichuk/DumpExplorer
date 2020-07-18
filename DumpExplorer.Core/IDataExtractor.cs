using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public interface IDataExtractor
    {
        string Name { get; }
        string Description { get; }
        string TypeName { get; }

        IEnumerable<dynamic> ExtractData(ClrRuntime clr, Action<string> actionLog = null);
    }
}
