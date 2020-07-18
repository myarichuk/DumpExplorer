using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace DumpExplorer.Core
{
    public class Method
    {
        //
        // Summary:
        //     Gets a value indicating whether this method is abstract.
        public bool IsAbstract { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is virtual.
        public bool IsVirtual { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is a runtime special method.
        public bool IsRTSpecialName { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is a special method.
        public bool IsSpecialName { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is a P/Invoke.
        public bool IsPInvoke { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is final.
        public bool IsFinal { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is static.
        public bool IsStatic { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is protected.
        public bool IsProtected { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is internal.
        public bool IsInternal { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is private.
        public bool IsPrivate { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is public.
        public bool IsPublic { get; set; }
        //
        // Summary:
        //     Gets the enclosing type of this method.
        public Type Type { get; set; }
        //
        // Summary:
        //     Gets the metadata token of the current method.
        public int MetadataToken { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is a static constructor.
        public virtual bool IsClassConstructor { get; set; }
        //
        // Summary:
        //     Gets the way this method was compiled.
        public MethodCompilationType CompilationType { get; set; }
        //
        // Summary:
        //     Gets the regions of memory that
        public HotColdRegions HotColdInfo { get; set; }
        //
        // Summary:
        //     Gets the instruction pointer in the target process for the start of the method's
        //     assembly.
        public ulong NativeCode { get; set; }
        //
        // Summary:
        //     Gets the full signature of the function. For example, "void System.Foo.Bar(object
        //     o, int i)" would return "System.Foo.Bar(System.Object, System.Int32)"
        public string? Signature { get; set; }
        //
        // Summary:
        //     Gets the name of the method. For example, "void System.Foo.Bar(object o, int
        //     i)" would return "Bar".
        public string? Name { get; set; }
        //
        // Summary:
        //     Gets the first MethodDesc in EnumerateMethodDescs(). For single AppDomain programs
        //     this is the only MethodDesc. MethodDescs are unique to an Method/AppDomain pair,
        //     so when there are multiple domains there will be multiple MethodDescs for a method.
        public ulong MethodDesc { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this method is an instance constructor.
        public virtual bool IsConstructor { get; set; }
    }
}
