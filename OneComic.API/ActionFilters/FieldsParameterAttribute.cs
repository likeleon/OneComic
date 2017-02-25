using Core.Common.Extensions;
using OneComic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OneComic.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FieldsParameterAttribute : ActionFilterAttribute
    {
        public string ParameterName { get; }
        public Type DtoType { get; }

        private readonly string[] _dtoFields; 

        public FieldsParameterAttribute(string parameterName, Type dtoType)
        {
            ParameterName = parameterName;
            DtoType = dtoType;

            _dtoFields = DtoType.GetDtoFields().ToArray();
            if (_dtoFields.Length <= 0)
                throw new Exception("Empty DTO fields");
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ActionArguments.ContainsKey(ParameterName))
                return;

            var value = GetParameterValue(actionContext);
            if (value.IsNullOrEmpty())
                return;

            var fields = new List<string>();
            var invalidFields = new List<string>();
            foreach (var word in value.ToLower().Split(',').ToArray())
            {
                var field = _dtoFields.FirstOrDefault(f => string.Equals(word, f, StringComparison.OrdinalIgnoreCase));
                if (field != null)
                    fields.Add(field);
                else
                    invalidFields.Add(field);
            }
            
            if (invalidFields.Count > 0)
                actionContext.ThrowBadRequestResponse($"Invalid fields: {invalidFields.JoinWith(", ")}");

            actionContext.ActionArguments[ParameterName] = fields.ToArray();
        }

        private string GetParameterValue(HttpActionContext actionContext)
        {
            var urlParamValues = actionContext.ControllerContext.RouteData.Values;
            if (urlParamValues.ContainsKey(ParameterName))
                return (string)urlParamValues[ParameterName];

            var queryStrings = actionContext.ControllerContext.Request.RequestUri.ParseQueryString();
            if (queryStrings[ParameterName] != null)
                return queryStrings[ParameterName];

            return null;
        }
    }
}