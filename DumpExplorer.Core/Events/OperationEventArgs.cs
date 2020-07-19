using System;
using System.Threading.Tasks;

namespace DumpExplorer.Core.Events
{
    public readonly struct OperationEventArgs
    {
        public ReadOnlyMemory<char> OperationName { get; }
        public ReadOnlyMemory<char> Message { get; }
        public TaskStatus OperationState { get; }

        public OperationEventArgs(string operationName, TaskStatus operationState, string message = null)
        {
            OperationName = operationName.AsMemory();
            OperationState = operationState;
            Message = message.AsMemory();
        }
    }
}
