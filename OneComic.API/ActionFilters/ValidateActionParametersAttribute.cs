using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace OneComic.API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ValidateActionParametersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var descriptor = actionContext.ActionDescriptor as ReflectedHttpActionDescriptor;
            if (descriptor == null)
                return;

            foreach (var parameter in descriptor.MethodInfo.GetParameters())
            {
                var argument = actionContext.ActionArguments[parameter.Name];
                EvaluateValidationAttributes(parameter, argument, actionContext.ModelState);
            }
        }

        private void EvaluateValidationAttributes(ParameterInfo parameter, object argument, ModelStateDictionary modelState)
        {
            foreach (var attribute in parameter.CustomAttributes)
            {
                var attributeInstance = CustomAttributeExtensions.GetCustomAttribute(parameter, attribute.AttributeType);

                var validationAttribute = attributeInstance as ValidationAttribute;
                if (validationAttribute == null)
                    continue;

                if (!validationAttribute.IsValid(argument))
                    modelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
            }
        }
    }
}