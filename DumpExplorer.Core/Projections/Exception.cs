using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace DumpExplorer.Core
{
    public class Exception
    {
        public string Id { get; set; }
        //
        // Summary:
        //     Gets the original thread this exception was thrown from. This may be null if
        //     we do not know.
        public Thread Thread { get; set; }
        //
        // Summary:
        //     Gets the address of the exception object.
        public ulong Address { get; set; }
        
        public string TypeName { get; set; }

        //
        // Summary:
        //     Gets the exception message.
        public string Message { get; set; }
        
        //
        // Summary:
        //     Gets the inner exception, if one exists, null otherwise.
        public Exception Inner { get; set; }
        //
        // Summary:
        //     Gets the HRESULT associated with this exception (or S_OK if there isn't one).
        public int HResult { get; set; }

        //
        // Summary:
        //     Gets the StackTrace for this exception. Note that this may be empty or partial
        //     depending on the state of the exception in the process. (It may have never been
        //     thrown or we may be in the middle of constructing the stackwalk.) This returns
        //     an empty list if no stack trace is associated with this exception object.
        public List<StackFrame> StackTrace { get; set; }
    }
}
