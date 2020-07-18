using Microsoft.Diagnostics.Runtime;
using System.Collections.Generic;

namespace DumpExplorer.Core
{
    public class Type
    {
        //
        // Summary:
        //     Gets a value indicating whether this type is a primitive (System.Int32, System.Single,
        //     etc).
        //
        // Returns:
        //     True if this type is a primitive (System.Int32, System.Single, etc), false otherwise.
        public virtual bool IsPrimitive { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether objects of this type are finalizable.
        public bool IsFinalizable { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is marked Public.
        public bool IsPublic { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is marked Private.
        public bool IsPrivate { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is accessible only by items in its
        //     own assembly.
        public bool IsInternal { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this nested type is accessible only by subtypes
        //     of its outer type.
        public bool IsProtected { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this class is abstract.
        public bool IsAbstract { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this class is sealed.
        public bool IsSealed { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is an interface.
        public bool IsInterface { get; set; }
        //
        // Summary:
        //     Gets all possible fields in this type. It does not return dynamically typed fields.
        //     Returns an empty list if there are no fields.
        public List<Field> Fields { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is an object reference.
        //
        // Returns:
        //     True if this type is an object reference, false otherwise.
        public virtual bool IsObjectReference { get; set; }
        //
        // Summary:
        //     Gets a list of static fields on this type. Returns an empty list if there are
        //     no fields.
        public List<Field> StaticFields { get; set; }
        //
        // Summary:
        //     If this type inherits from another type, this is that type. Can return null if
        //     it does not inherit (or is unknown).
        public Type? BaseType { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the type is in fact a pointer. If so, the pointer
        //     operators may be used.
        public virtual bool IsPointer { get; set; }
        //
        // Summary:
        //     Gets the type of the element referenced by the pointer.
        public Type? ComponentType { get; set; }
        //
        // Summary:
        //     A type is an array if you can use the array operators below, Abstractly arrays
        //     are objects that whose children are not statically known by just knowing the
        //     type.
        public bool IsArray { get; set; }
        //
        // Summary:
        //     Gets the static size of objects of this type when they are created on the CLR
        //     heap.
        public int StaticSize { get; set; }
        //
        // Summary:
        //     Gets the size of elements of this object.
        public int ComponentSize { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is System.String.
        public virtual bool IsString { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type represents free space on the heap.
        public virtual bool IsFree { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is an exception (that is, it derives
        //     from System.Exception).
        public virtual bool IsException { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is an enum.
        public bool IsEnum { get; set; }
        //
        // Summary:
        //     Gets the list of methods this type implements.

        public List<Method> Methods { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is a value type.
        //
        // Returns:
        //     True if this type is a value type, false otherwise.
        public virtual bool IsValueType { get; set; }

        //
        // Summary:
        //     Gets the Microsoft.Diagnostics.Runtime.ClrElementType of this Type. Can return
        //     Microsoft.Diagnostics.Runtime.ClrElementType.Unknown on error.
        public ClrElementType ElementType { get; set; }

        //
        // Summary:
        //     Gets the handle to the LoaderAllocator object for collectible types.
        public virtual ulong LoaderAllocatorHandle { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this is a collectible type.
        public virtual bool IsCollectible { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the type can contain references to other objects.
        //     This is used in optimizations and 'true' can always be returned safely.
        public virtual bool ContainsPointers { get; set; }
        //
        // Summary:
        //     Gets the name of this type.
        public string? Name { get; set; }
        //
        // Summary:
        //     Gets the metadata token of this type.
        public int MetadataToken { get; set; }
        //
        // Summary:
        //     Gets the MethodTable of this type (this is the TypeHandle if this is a type without
        //     a MethodTable).
        public ulong MethodTable { get; set; }
        //
        // Summary:
        //     Gets the Microsoft.Diagnostics.Runtime.ClrType.GCDesc associated with this type.
        //     Only valid if Microsoft.Diagnostics.Runtime.ClrType.ContainsPointers is true.
        public GCDesc GCDesc { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this type is shared across multiple AppDomains.
        public bool IsShared { get; set; }
    }
}
