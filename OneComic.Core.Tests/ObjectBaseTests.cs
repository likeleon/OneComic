using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace OneComic.Core.Tests
{
    [TestClass]
    public class ObjectBaseTests
    {
        class TestClass : ObjectBase
        {
            private string _dirtyProperty;
            private string _cleanProperty;
            private string _propertyShouldNotEmpty;

            class Validator : AbstractValidator<TestClass>
            {
                public Validator()
                {
                    RuleFor(obj => obj.PropertyShouldNotEmpty).NotEmpty();
                }
            }

            protected override IValidator CreateValidator()
            {
                return new Validator();
            }

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

            public string PropertyShouldNotEmpty
            {
                get { return _propertyShouldNotEmpty; }
                set { Set(ref _propertyShouldNotEmpty, value); }
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

        [TestMethod]
        public void ObjectValidation()
        {
            var obj = new TestClass();
            var errorsChangedPropertyNames = new List<string>();
            obj.ErrorsChanged += (_, e) => errorsChangedPropertyNames.Add(e.PropertyName);

            Assert.IsTrue(obj.HasErrors);
            Assert.AreEqual(
                "'Property Should Not Empty' should not be empty.", 
                obj.GetErrors(nameof(TestClass.PropertyShouldNotEmpty)).Cast<string>().Single());

            obj.PropertyShouldNotEmpty = "Some Value";
            Assert.IsFalse(obj.HasErrors);
            CollectionAssert.AreEqual(new[] { nameof(TestClass.PropertyShouldNotEmpty) }, errorsChangedPropertyNames);
        }
    }
}
