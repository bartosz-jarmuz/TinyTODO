using ToDoLite.Core.Contracts;

namespace ToDoLite.App.Windows.UnitTests.Helpers
{
    public class NullConfirmationEmitter : IConfirmationEmitter
    {
        private NullConfirmationEmitter() { }
        public static readonly NullConfirmationEmitter Instance = new NullConfirmationEmitter();
        public void Done() { }
        public void NoData() { }
    }
}