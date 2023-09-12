using System.Text.Json;
using System.Text.Json.Serialization;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;

namespace ToDoLite.Core
{
    public class JsonDataExporter : IDataExporter
    {
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        public void ExportData(IEnumerable<ToDoItem> toDoItems, string destinationFilePath)
        {
            var serialized = JsonSerializer.Serialize(toDoItems, _serializerOptions);
            File.WriteAllText(destinationFilePath, serialized);
        }

        public IReadOnlyCollection<ToDoItem> ImportData(string jsonFilePath)
        {
            var items = JsonSerializer.Deserialize<IEnumerable<ToDoItem>>(File.ReadAllText(jsonFilePath), _serializerOptions)?.ToList();
            return items ?? new List<ToDoItem>();
        }
    }
}