using Microsoft.Diagnostics.Runtime;

namespace DumpExplorer.Core
{
    public class Field
    {
        //
        // Summary:
        //     Gets the Microsoft.Diagnostics.Runtime.ClrType containing this field.
        public  Type ContainingType { get; set; }
        //
        // Summary:
        //     Gets the name of the field.
        public  string? Name { get; set; }
        //
        // Summary:
        //     Gets the type token of this field.
        public  int Token { get; set; }
        //
        // Summary:
        //     Gets the type of the field. Note this property may return null on error. There
        //     is a bug in several versions of our debugging layer which causes this. You should
        //     always null-check the return value of this field.
        public  Type? Type { get; set; }
        //
        // Summary:
        //     Gets the element type of this field. Note that even when Type is null, this should
        //     still tell you the element type of the field.
        public  ClrElementType ElementType { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is a primitive (System.Int32, System.Single,
        //     etc).
        //
        // Returns:
        //     True if this field is a primitive (System.Int32, System.Single, etc), false otherwise.
        public virtual bool IsPrimitive { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is a value type.
        //
        // Returns:
        //     True if this field is a value type, false otherwise.
        public virtual bool IsValueType { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is an object reference.
        //
        // Returns:
        //     True if this field is an object reference, false otherwise.
        public virtual bool IsObjectReference { get; set; }
        //
        // Summary:
        //     Gets the size of this field.
        public  int Size { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is public.
        public  bool IsPublic { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is private.
        public  bool IsPrivate { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is internal.
        public  bool IsInternal { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this field is protected.
        public  bool IsProtected { get; set; }
        //
        // Summary:
        //     For instance fields, this is the offset of the field within the object. For static
        //     fields this is the offset within the block of memory allocated for the module's
        //     static fields.
        public  int Offset { get; set; }
    }
}
