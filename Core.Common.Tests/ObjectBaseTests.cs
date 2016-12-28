using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Common.Tests
{
    [TestClass]
    public class ObjectBaseTests
    {
        class TestClass : ObjectBase
        {
            private string _dirtyProperty;

            public string DirtyProperty
            {
                get { return _dirtyProperty; }
                set { Set(ref _dirtyProperty, value, setDirty: true); }
            }

            private string _cleanProperty;

            public string CleanProperty
            {
                get { return _cleanProperty; }
                set { Set(ref _cleanProperty, value); }
            }
        }

        [TestMethod]
        public void DirtySet()
        {
            var obj = new TestClass();
            Assert.IsFalse(obj.IsDirty);

            obj.DirtyProperty = "New Value";
            Assert.IsTrue(obj.IsDirty);
        }

        [TestMethod]
        public void RemainsClean()
        {
            var obj = new TestClass();
            Assert.IsFalse(obj.IsDirty);

            obj.CleanProperty = "New Value";
            Assert.IsFalse(obj.IsDirty);
        }
    }
}
