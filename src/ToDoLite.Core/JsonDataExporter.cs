using System.Text.Json;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core
{
    public class JsonDataExporter : IDataExporter
    {
        public void ExportData(IEnumerable<ToDoItem> toDoItems, string destinationFilePath)
        {
            var serialized = JsonSerializer.Serialize(toDoItems);
            File.WriteAllText(destinationFilePath, serialized);
        }

        public IReadOnlyCollection<ToDoItem> ImportData(string jsonFilePath)
        {
            var items = JsonSerializer.Deserialize<IEnumerable<ToDoItem>>(File.ReadAllText(jsonFilePath))?.ToList();
            return items ?? new List<ToDoItem>();
        }
    }
}