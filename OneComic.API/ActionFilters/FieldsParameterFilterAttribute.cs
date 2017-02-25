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
    public sealed class FieldsParameterFilterAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;
        private readonly HashSet<string> _dtoProperties; 

        public FieldsParameterFilterAttribute(string parameterName, Type dtoType)
        {
            _parameterName = parameterName;
            _dtoProperties = new HashSet<string>(dtoType.GetDtoProperties().Select(p => p.Name));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ActionArguments.ContainsKey(_parameterName))
                return;

            var parameter = GetParameter(actionContext);
            if (parameter.IsNullOrEmpty())
                return;

            var fields = parameter.ToLower().Split(',').ToArray();

            var invalidFields = fields.Except(_dtoProperties, StringComparer.InvariantCultureIgnoreCase).ToArray();
            if (invalidFields.Length > 0)
            {
                var message = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest,
                    $"Invalid fields: {invalidFields.JoinWith(", ")}");
                throw new HttpResponseException(message);
            }

            actionContext.ActionArguments[_parameterName] = fields;
        }

        private string GetParameter(HttpActionContext actionContext)
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