namespace TinyTODO.Core.Contracts
{
    public interface IConfirmationEmitter
    {
        void Done();
        void NoData();
    }
}