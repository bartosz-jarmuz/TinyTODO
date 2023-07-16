using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.App.Windows.UnitTests.Helpers
{
    public class NullToDoItemGenerator : IToDoItemGenerator
    {
        private NullToDoItemGenerator() { }
        public static NullToDoItemGenerator Instance = new NullToDoItemGenerator();
        public ToDoItem? GenerateItem()
        {
            return null;
        }
    }

}