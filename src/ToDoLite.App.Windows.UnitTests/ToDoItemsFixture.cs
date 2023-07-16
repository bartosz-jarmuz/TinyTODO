using NUnit.Framework;
using System;
using System.Data.Common;
using System.Linq;
using ToDoLite.App.Windows.UnitTests.Helpers;
using ToDoLite.App.Windows.ViewModel;
using ToDoLite.Core.Persistence;

namespace ToDoLite.App.Windows.UnitTests
{

    [TestFixture]
    public class ToDoItemsFixture 
    {

        [Test]
        public void MainWindowViewModel_CreateToDoItemFromClipboard_InitialItemPropertiesAreCorrect()
        {
            //arrange
            using var context = TestContextProvider.Get();
            var storage = new SqliteToDoItemStorage(context.DbContext);
            var generator = new MockToDoItemGenerator();
            var mainVm = new MainWindowViewModel(storage, storage, generator, NullConfirmationEmitter.Instance);
            generator.SetGenerateItemDelegate("Foo");

            //act
            mainVm.CreateToDoItemFromClipboardContent(null, null);

            //assert
            var item = mainVm.ToDoItems.Single();
            Assert.AreEqual("Foo", item.TextData);
            Assert.AreEqual("Foo", item.Item.PlainText);
            Assert.AreEqual(DateTime.MinValue, item.Item.CompletedDateTime);
            Assert.AreEqual(false, item.Item.IsCompleted);
            Assert.AreEqual(false, item.IsCompleted);
        }
    }
}