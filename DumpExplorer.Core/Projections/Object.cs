using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class Object : IEquatable<Object>
    {
        public string Id { get; set; }

        public int Generation { get; set; }

        //
        // Summary:
        //     Gets a value indicating whether this object possibly contians GC pointers.
        public bool ContainsPointers { get; set; }
        //
        // Summary:
        //     Returns true if this object is a RuntimeCallableWrapper.
        public bool HasRuntimeCallableWrapper { get; set; }
        //
        // Summary:
        //     Returns true if this object is a ComCallableWrapper.
        public bool HasComCallableWrapper { get; set; }
        //
        // Summary:
        //     Returns true if this object is a COM class factory.
        public bool IsComClassFactory { get; set; }
        //
        // Summary:
        //     Obtains the SyncBlock for this object. Returns null if there is no SyncBlock
        //     associated with this object.
        public SyncBlock? SyncBlock { get; set; }
        //
        // Summary:
        //     Gets the size of the object.
        public ulong Size { get; set; }
        //
        // Summary:
        //     Returns if the object value is null.
        public bool IsNull { get; set; }
        //
        // Summary:
        //     Returns whether this is a valid object. This will return null
        public bool IsValid { get; set; }
        //
        // Summary:
        //     Returns whether this is free space on the GC heap and not a real object.
        public bool IsFree { get; set; }
        //
        // Summary:
        //     Gets the type of the object.
        public Type? Type { get; set; }
        //
        // Summary:
        //     Gets the address of the object.
        public ulong Address { get; set; }
        public bool IsException { get; set; }
        //
        // Summary:
        //     Returns true if this object is a boxed struct or primitive type that
        public bool IsBoxedValue { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this object is an array.
        public bool IsArray { get; set; }

        public bool IsRuntimeType { get; set; }

        public override bool Equals(object obj) => Equals(obj as Object);

        public bool Equals(Object other) => other != null && Id == other.Id;

        public override int GetHashCode() => HashCode.Combine(Id);

        public static bool operator ==(Object left, Object right) => EqualityComparer<Object>.Default.Equals(left, right);

        public static bool operator !=(Object left, Object right) => !(left == right);
    }
}
