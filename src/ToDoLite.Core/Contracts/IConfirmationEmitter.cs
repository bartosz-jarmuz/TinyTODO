namespace ToDoLite.Core.Contracts
{
    public interface IConfirmationEmitter
    {
        void Done();
        void NoData();
    }
}