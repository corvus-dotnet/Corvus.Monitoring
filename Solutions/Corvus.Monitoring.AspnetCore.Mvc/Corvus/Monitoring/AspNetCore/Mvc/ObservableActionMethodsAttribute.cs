// <copyright file="ObservableActionMethodsAttribute.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc
{
    using System.Threading.Tasks;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Action filter which logs execution times for action methods.
    /// </summary>
    public class ObservableActionMethodsAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IOperationsInstrumentation instrumentation = context.HttpContext.RequestServices.GetRequiredService<IOperationsInstrumentation>();

            // ActionFilters can only be applied to Controllers, so we know that the ActionDescriptor we have
            // will be a ControllerActionDescriptor and can use a cast expression.
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            using IOperationInstance operation = instrumentation.StartOperation($"{actionDescriptor.ControllerTypeInfo.FullName}.{actionDescriptor.MethodInfo.Name}");

            // Add route parameters to the current operation data
            operation.AddRouteData(context.RouteData);

            if (context.Controller is IHaveObservableActionMethods observableController)
            {
                observableController.CurrentOperation = operation;
            }

            await next.Invoke();
        }

        /// <inheritdoc/>
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            IOperationsInstrumentation instrumentation = context.HttpContext.RequestServices.GetRequiredService<IOperationsInstrumentation>();

            // ActionFilters can only be applied to Controllers, so we know that the ActionDescriptor we have
            // will be a ControllerActionDescriptor and can use a cast expression.
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            using IOperationInstance operation = instrumentation.StartOperation($"{actionDescriptor.ControllerTypeInfo.FullName}.{actionDescriptor.MethodInfo.Name}::ResultExecution");

            await next.Invoke();
        }
    }
}