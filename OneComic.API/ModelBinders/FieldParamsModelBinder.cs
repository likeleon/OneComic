using Core.Common.Data;
using Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace OneComic.API.ModelBinders
{
    public sealed class FieldParams<T>
    {
        public Type Type { get; }
        public string[] Fields { get; }
        public IReadOnlyDictionary<string, List<string>> AssociatedFields { get; }

        public FieldParams(string[] fields, IReadOnlyDictionary<string, List<string>> associatedFields)
        {
            Type = typeof(T);
            Fields = fields;
            AssociatedFields = associatedFields;
        }
    }

    public abstract class FieldParamsModelBinder<T> : IModelBinder where T : class
    {
        private readonly Type _type = typeof(T);
        private readonly string[] _fields;
        private readonly Dictionary<string, string[]> _associatedFields = new Dictionary<string, string[]>();

        public FieldParamsModelBinder()
        {
            _fields = _type.GetDtoFields().ToArray();
            if (_fields.Length <= 0)
                throw new Exception("Empty fields");
        }

        protected void AddAssociatedType<U>(string field)
        {
            _associatedFields.Add(field, typeof(U).GetDtoFields().ToArray());
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(FieldParams<T>))
                return false;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null)
                return false;

            var value = valueProviderResult.RawValue as string;
            if (value == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong value type");
                return false;
            }

            var fields = new List<string>();
            var associatedFields = new Dictionary<string, List<string>>();
            var invalidFields = new List<string>();
            foreach (var word in value.ToLower().Split(',').ToArray())
            {
                var field = _fields.FirstOrDefault(f => string.Equals(word, f, StringComparison.OrdinalIgnoreCase));
                if (field != null)
                {
                    fields.Add(field);
                    continue;
                }

                var associatedField = ParseAsAssociatedField(word);
                if (associatedField != null)
                {
                    if (!associatedFields.ContainsKey(associatedField.Item1))
                        associatedFields.Add(associatedField.Item1, new List<string>());

                    if (!associatedField.Item2.IsNullOrEmpty())
                        associatedFields[associatedField.Item1].Add(associatedField.Item2);

                    continue;
                }

                invalidFields.Add(word);
            }

            if (invalidFields.Count > 0)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"Invalid fields: {invalidFields.JoinWith(", ")}");
                return false;
            }

            bindingContext.Model = new FieldParams<T>(fields.ToArray(), associatedFields);
            return true;
        }

        private Tuple<string, string> ParseAsAssociatedField(string field)
        {
            foreach (var kvp in _associatedFields)
            {
                var associationName = kvp.Key;
                if (!field.StartsWith(associationName, StringComparison.OrdinalIgnoreCase))
                    continue;

                var fieldPart = string.Empty;
                if (field.StartsWith(associationName + ".", StringComparison.OrdinalIgnoreCase))
                {
                    fieldPart = field.Substring(field.IndexOf(".") + 1);
                    fieldPart = kvp.Value.FirstOrDefault(f => string.Equals(fieldPart, f, StringComparison.OrdinalIgnoreCase));
                    if (fieldPart == null)
                        return null;
                }

                return Tuple.Create(associationName, fieldPart);
            }

            return null;
        }
    }
}