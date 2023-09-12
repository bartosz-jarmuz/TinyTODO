namespace ToDoLite.Core.Contracts
{
    public class ItemCompletedStateChangedEventArgs : EventArgs
    {
        public ItemCompletedStateChangedEventArgs(bool newIsCompletedState)
        {
            NewIsCompletedState = newIsCompletedState;
        }

        public bool NewIsCompletedState { get; }
    }
}