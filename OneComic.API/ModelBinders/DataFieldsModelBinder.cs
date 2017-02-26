using Core.Common.Contracts;
using Core.Common.Data;
using Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace OneComic.API.ModelBinders
{
    public abstract class DataFieldsModelBinder<T> : IModelBinder where T : class
    {
        private sealed class AssociatedFieldInfo
        {
            public string AssociationName { get; }
            public Type Type { get; }
            public List<string> Fields { get; } = new List<string>();

            public AssociatedFieldInfo(string associationName, Type type)
            {
                AssociationName = associationName;
                Type = type;
            }
        }

        private readonly DataFields<T> _fields;
        private readonly Dictionary<string, DataFields> _associatedFields = new Dictionary<string, DataFields>();

        public DataFieldsModelBinder()
        {
            _fields = new DataFields<T>();
            if (!_fields.Fields.Any())
                throw new Exception("Empty fields");
        }

        protected void AddAssociatedType<U>(string associationName)
        {
            _associatedFields.Add(associationName, new DataFields<U>());
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(DataFields<T>))
                return false;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null)
                return false;

            var value = valueProviderResult.RawValue as string;
            if (value == null)
            {
                AddModelError(bindingContext, "Wrong value type");
                return false;
            }

            var tokens = value.Split(',').ToArray();
            if (tokens.Length <= 0)
            {
                AddModelError(bindingContext, $"Empty fields");
                return false;
            }

            var fields = new List<string>();
            var associatedFieldInfos = new Dictionary<string, AssociatedFieldInfo>();
            var invalidFields = new List<string>();
            foreach (var token in tokens)
            {
                if (TryAddAsField(fields, token))
                    continue;

                if (TryAddAsAssociatedField(associatedFieldInfos, token))
                    continue;

                invalidFields.Add(token);
            }

            if (invalidFields.Count > 0)
            {
                AddModelError(bindingContext, $"Invalid fields: {invalidFields.JoinWith(", ")}");
                return false;
            }

            var associatedFields = associatedFieldInfos.ToDictionary(
                kvp => kvp.Key, 
                kvp => new DataFields(kvp.Value.Type, kvp.Value.Fields.ToArray()) as IDataFields);
            bindingContext.Model = new DataFields<T>(fields, associatedFields);
            return true;
        }

        private bool TryAddAsAssociatedField(Dictionary<string, AssociatedFieldInfo> associatedFieldInfos, string field)
        {
            string associationName;
            Type associatedType;
            string associatedField;
            if (!ParseAsAssociatedField(field, out associationName, out associatedType, out associatedField))
                return false;
            
            if (!associatedFieldInfos.ContainsKey(associationName))
            {
                var associationInfo = new AssociatedFieldInfo(associationName, associatedType);
                associatedFieldInfos.Add(associationName, associationInfo);
            }

            if (associatedField != null)
                associatedFieldInfos[associationName].Fields.Add(associatedField);

            return true;
        }

        private bool TryAddAsField(List<string> fields, string field)
        {
            var matchingField = _fields.Fields.FirstOrDefault(f => string.Equals(field, f, StringComparison.OrdinalIgnoreCase));
            if (matchingField == null)
                return false;

            fields.Add(matchingField);
            return true;
        }

        private bool ParseAsAssociatedField(
            string field, 
            out string associationName,
            out Type associatedType,
            out string associatedField)
        {
            associationName = null;
            associatedType = null;
            associatedField = null;

            foreach (var kvp in _associatedFields)
            {
                if (!field.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase))
                    continue;

                associationName = kvp.Key;
                associatedType = kvp.Value.Type;

                if (field.StartsWith(associationName + ".", StringComparison.OrdinalIgnoreCase))
                {
                    var fieldPart = field.Substring(field.IndexOf(".") + 1);
                    fieldPart = kvp.Value.Fields.FirstOrDefault(f => string.Equals(fieldPart, f, StringComparison.OrdinalIgnoreCase));
                    if (fieldPart == null)
                        return false;

                    associatedField = fieldPart;
                }

                return true;
            }

            return false;
        }

        private void AddModelError(ModelBindingContext context, string errorMessage)
        {
            context.ModelState.AddModelError(context.ModelName, errorMessage);
        }
    }
}