using Microsoft.Diagnostics.Runtime;

namespace DumpExplorer.Core
{
    public class StackFrame
    {
        //
        // Summary:
        //     The thread parent of this frame. Note that this may be null when inspecting the
        //     stack of ClrExceptions.
        public Thread? Thread { get; set; }

        //
        // Summary:
        //     Gets the instruction pointer of this frame.
        public ulong InstructionPointer { get; set; }
        //
        // Summary:
        //     Gets the stack pointer of this frame.
        public ulong StackPointer { get; set; }
        //
        // Summary:
        //     Gets the type of frame (managed or internal).
        public ClrStackFrameKind Kind { get; set; }
        
        //
        // Summary:
        //     Gets the Microsoft.Diagnostics.Runtime.ClrMethod which corresponds to the current
        //     stack frame. This may be null if the current frame is actually a CLR "Internal
        //     Frame" representing a marker on the stack, and that stack marker does not have
        //     a managed method associated with it.
        public Method? Method { get; set; }
        //
        // Summary:
        //     Gets the helper method frame name if Microsoft.Diagnostics.Runtime.ClrStackFrame.Kind
        //     is Microsoft.Diagnostics.Runtime.ClrStackFrameKind.Runtime, null otherwise.
        public string? FrameName { get; set; }
    }
}
