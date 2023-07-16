using System;
using ToDoLite.Core.ClipboardModel;
using ToDoLite.Core.Contracts;
using ToDoLite.Core.DataModel;
using ToDoLite.Core.Windows.DataConversion;

namespace ToDoLite.App.Windows.UnitTests.Helpers
{
    public class MockToDoItemGenerator : IToDoItemGenerator
    {
        public void SetGenerateItemDelegate(string text)
        {
            var item = new ToDoItem(
                    new TextualClipboardData(CapturedDataType.PlainText, text, StringConverter.GetBytes(text)),
                    new ItemCreationContext("UnitTest"));

            GenerateItemDelegate = new Func<ToDoItem?>(() => item);
        }
        public Func<ToDoItem?> GenerateItemDelegate { get; set; } = () => null;
        public ToDoItem? GenerateItem() => GenerateItemDelegate();
    }
}