using Core.Common.Extensions;
using OneComic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OneComic.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class FieldsParameterAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;
        private readonly string[] _dtoFields; 

        public FieldsParameterAttribute(string parameterName, Type dtoType)
        {
            _parameterName = parameterName;

            _dtoFields = dtoType.GetDtoFields().ToArray();
            if (_dtoFields.Length <= 0)
                throw new Exception("Empty DTO fields");
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ActionArguments.ContainsKey(_parameterName))
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
            {
                var response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    $"Invalid fields: {invalidFields.JoinWith(", ")}");
                throw new HttpResponseException(response);
            }

            actionContext.ActionArguments[_parameterName] = fields.ToArray();
        }

        private string GetParameterValue(HttpActionContext actionContext)
        {
            var urlParamValues = actionContext.ControllerContext.RouteData.Values;
            if (urlParamValues.ContainsKey(_parameterName))
                return (string)urlParamValues[_parameterName];

            var queryStrings = actionContext.ControllerContext.Request.RequestUri.ParseQueryString();
            if (queryStrings[_parameterName] != null)
                return queryStrings[_parameterName];

            return null;
        }
    }
}