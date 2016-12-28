using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace Core.Common.Tests
{
    [TestClass]
    public class NotificationObjectTests
    {
        class TestClass : NotificationObject
        {
            private string _property;

            public string Property
            {
                get { return _property; }
                set { Set(ref _property, value); }
            }
        }

        [TestMethod]
        public void PropertyChangeNotification()
        {
            var obj = new TestClass();
            var propertyChanged = false;
            obj.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(TestClass.Property))
                    propertyChanged = true;
            };

            obj.Property = "New Value";
            Assert.IsTrue(propertyChanged);
        }

        [TestMethod]
        public void PropertyChangeSingleSubscription()
        {
            var obj = new TestClass();
            var notificationCounter = 0;
            var handler1 = new PropertyChangedEventHandler((_, e) => ++notificationCounter);
            var handler2 = new PropertyChangedEventHandler((_, e) => ++notificationCounter);

            obj.PropertyChanged += handler1;
            obj.PropertyChanged += handler1;
            obj.PropertyChanged += handler1;
            obj.PropertyChanged += handler2;
            obj.PropertyChanged += handler2;

            obj.Property = "New Value";
            Assert.AreEqual(2, notificationCounter, "Property change notification should only have been called once.");
        }
    }
}
