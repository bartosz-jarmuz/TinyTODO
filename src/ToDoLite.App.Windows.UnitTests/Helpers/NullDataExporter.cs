using System.Collections.Generic;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.App.Windows.UnitTests.Helpers
{
    public class NullDataExporter : IDataExporter
    {
        private NullDataExporter() { }

        public static readonly IDataExporter Instance = new NullDataExporter();
        public void ExportData(IEnumerable<ToDoItem> toDoItems, string destinationFilePath)
        {
        }

        public IReadOnlyCollection<ToDoItem> ImportData(string sourceFilePath)
        {
            throw new System.NotImplementedException();
        }
    }
}