﻿using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Core.Common.Tests
{
    [TestClass]
    public class ObjectBaseTests
    {
        class TestClass : ObjectBase
        {
            private string _dirtyProperty;
            private string _cleanProperty;

            public string DirtyProperty
            {
                get { return _dirtyProperty; }
                set { Set(ref _dirtyProperty, value); }
            }

            public string CleanProperty
            {
                get { return _cleanProperty; }
                set { Set(ref _cleanProperty, value, makeDirty: false); }
            }

            public class ChildClass : ObjectBase
            {
                private string _name;

                public string Name
                {
                    get { return _name; }
                    set { Set(ref _name, value); }
                }
            }

            public ChildClass Child { get; } = new ChildClass();
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

        [TestMethod]
        public void ChildDirtyTracking()
        {
            var obj = new TestClass();
            Assert.IsFalse(obj.GetDirtyObjects().Any());

            obj.Child.Name = "New Value";
            Assert.IsTrue(obj.GetDirtyObjects().Any());

            obj.CleanDirtyObjects();
            Assert.IsFalse(obj.GetDirtyObjects().Any());
        }

        [TestMethod]
        public void DirtyObjectAggregation()
        {
            var obj = new TestClass();
            Assert.IsFalse(obj.GetDirtyObjects().Any());

            obj.Child.Name = "New Value";
            Assert.AreEqual(obj.Child, obj.GetDirtyObjects().Single());

            obj.DirtyProperty = "New Value";
            CollectionAssert.AreEquivalent(new ObjectBase[] { obj, obj.Child }, obj.GetDirtyObjects().ToArray());

            obj.CleanDirtyObjects();
            Assert.IsFalse(obj.GetDirtyObjects().Any());
        }
    }
}
