using FluentValidation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OneComic.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, INotifyDataErrorInfo
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

            var oldErrors = _errors;
            _errors = _validator.Validate(this).Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            var changedProperties = DictionaryExtensions.GetDiffKeys(oldErrors ?? new Dictionary<string, string[]>(), _errors);
            foreach (var propertyName in changedProperties)
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public bool HasErrors => _errors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            string[] errors;
            _errors.TryGetValue(propertyName, out errors);
            return errors;
        }
        #endregion
    }
}
