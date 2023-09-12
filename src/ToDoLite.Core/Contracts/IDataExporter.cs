using ToDoLite.Core.DataModel;

namespace ToDoLite.Core.Contracts
{
    public interface IDataExporter
    {
        void ExportData(IEnumerable<ToDoItem> toDoItems, string destinationFilePath);
        IReadOnlyCollection<ToDoItem> ImportData(string sourceFilePath);
    }
}