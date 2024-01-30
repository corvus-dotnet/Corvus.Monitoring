// <copyright file="ObservableActionMethodsAttribute.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc
{
    using System.Threading.Tasks;
    using Corvus.Monitoring.AspNetCore.Mvc;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Action filter which logs execution times for action methods.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Adding this action filter to a controller will cause two operations to be started (using the current
    /// IOperationsInstrumentation) per request. The first is for execution of the action method and the second
    /// is for execution of the result.
    /// </para>
    /// <para>
    /// The <see cref="IOperationInstance"/> instances will also be added to the HttpContext for the duration of
    /// action method or result execution, and are accessible via the
    /// <see cref="HttpContextExtensions.GetCurrentOperationInstance(Microsoft.AspNetCore.Http.HttpContext)"/>
    /// extension method.
    /// </para>
    /// </remarks>
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

            context.HttpContext.SetCurrentOperationInstance(operation);

            await next.Invoke();

            context.HttpContext.ClearCurrentOperationInstance();
        }

        /// <inheritdoc/>
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            IOperationsInstrumentation instrumentation = context.HttpContext.RequestServices.GetRequiredService<IOperationsInstrumentation>();

            // ActionFilters can only be applied to Controllers, so we know that the ActionDescriptor we have
            // will be a ControllerActionDescriptor and can use a cast expression.
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            using IOperationInstance operation = instrumentation.StartOperation($"{actionDescriptor.ControllerTypeInfo.FullName}.{actionDescriptor.MethodInfo.Name}::ResultExecution");

            context.HttpContext.SetCurrentOperationInstance(operation);

            await next.Invoke();

            context.HttpContext.ClearCurrentOperationInstance();
        }
    }
}