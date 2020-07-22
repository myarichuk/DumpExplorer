using Microsoft.Diagnostics.Runtime;

namespace DumpExplorer.Core
{
    public class Thread
    {
        //
        // Summary:
        //     Gets the base of the stack for this thread, or 0 if the value could not be obtained.
        public ulong StackBase { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the CLR called CoInitialize for this thread.
        public bool IsCoInitialized { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this thread was created, but not started.
        public bool IsUnstarted { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this thread is a background thread. (That is,
        //     if the thread does not keep the managed execution environment alive and running.)
        public bool IsBackground { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the debugger has suspended the thread.
        public bool IsDebugSuspended { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the user has suspended the thread (using System.Threading.Thread.Suspend).
        public bool IsUserSuspended { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the GC is attempting to suspend this thread.
        public bool IsGCSuspendPending { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this thread was aborted.
        public bool IsAborted { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether an abort was requested for this thread (such
        //     as System.Threading.Thread.Abort, or System.AppDomain.Unload(System.AppDomain)).
        public bool IsAbortRequested { get; set; }
        //
        // Summary:
        //     Gets the exception currently on the thread. Note that this field may be null.
        //     Also note that this is basically the "last thrown exception", and may be stale...meaning
        //     the thread could be done processing the exception but a crash dump was taken
        //     before the current exception was cleared off the field.
        public Exception? CurrentException { get; set; }
        //
        // Summary:
        //     Gets the limit of the stack for this thread, or 0 if the value could not be obtained.
        public ulong StackLimit { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the thread is a COM multithreaded apartment.
        public bool IsMTA { get; set; }
        //
        // Summary:
        //     Gets the number of managed locks (Monitors) the thread has currently entered
        //     but not left. This will be highly inconsistent unless the process is stopped.
        public uint LockCount { get; set; }
        //
        // Summary:
        //     Gets the managed thread ID (this is equivalent to System.Threading.Thread.ManagedThreadId
        //     in the target process).
        public int ManagedThreadId { get; set; }
        //
        // Summary:
        //     Gets the OS thread id for the thread.
        public uint OSThreadId { get; set; }
        //
        // Summary:
        //     Returns true if the thread is alive in the process, false if this thread was
        //     recently terminated.
        public bool IsAlive { get; set; }
        //
        // Summary:
        //     Gets the address of the underlying datastructure which makes up the Thread object.
        //     This serves as a unique identifier.
        public ulong Address { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether this is the finalizer thread.
        public bool IsFinalizer { get; set; }
        //
        // Summary:
        //     Gets the suspension state of the thread according to the runtime.
        public GCMode GCMode { get; set; }

        //
        // Summary:
        //     Gets a value indicating whether this thread is in a COM single threaded apartment.
        public bool IsSTA { get; set; }
    }
}
