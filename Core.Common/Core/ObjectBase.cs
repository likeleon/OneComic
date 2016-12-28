using Core.Common.Contracts;
using FluentValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, INotifyDataErrorInfo
    {
        private readonly IValidator _validator;
        private Dictionary<string, string[]> _errors;
        private bool _isDirty;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ObjectBase()
        {
            _validator = CreateValidator();
            Validate();
        }

        public ExtensionDataObject ExtensionData { get; set; }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { Set(ref _isDirty, value, makeDirty: false); }
        }

        public override bool Set<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            return Set(ref currentValue, newValue, makeDirty: true, propertyName: propertyName);
        }

        public bool Set<T>(ref T currentValue, T newValue, bool makeDirty, [CallerMemberName] string propertyName = "")
        {
            if (!base.Set(ref currentValue, newValue, propertyName))
                return false;

            if (makeDirty)
                IsDirty = true;

            Validate();
            return true;
        }

        #region Dirty Tracking
        public IEnumerable<IDirtyCapable> GetDirtyObjects()
        {
            return ObjectGraph.GetSelfAndDescendants(this).Where(o => o.IsDirty);
        }

        public void CleanDirtyObjects()
        {
            GetDirtyObjects().OfType<ObjectBase>().Do(o => o.IsDirty = false);
        }
        #endregion

        #region Validation
        protected virtual IValidator CreateValidator()
        {
            return null;
        }

        public void Validate()
        {
            if (_validator == null)
                return;

            _errors = _validator.Validate(this).Errors?
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        public bool HasErrors => _errors?.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            string[] errors = null;
            _errors?.TryGetValue(propertyName, out errors);
            return errors;
        }
        #endregion
    }
}
