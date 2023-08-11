// <copyright file="HttpContextExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspNetCore.Mvc
{
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Extension methods for <see cref="HttpContext"/> that make it easier to work with
    /// Corvus.Monitoring.
    /// </summary>
    public static class HttpContextExtensions
    {
        private const string OperationInstanceContextKey = "CurrentOperationInstance";

        /// <summary>
        /// Adds the specified <see cref="IOperationInstance"/> to the <see cref="HttpContext.Items"/> collection.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <param name="operationInstance">The <see cref="IOperationInstance"/> for the current request.</param>
        public static void SetCurrentOperationInstance(this HttpContext httpContext, IOperationInstance operationInstance)
        {
            httpContext.Items.Add(OperationInstanceContextKey, operationInstance);
        }

        /// <summary>
        /// Remove the current <see cref="IOperationInstance"/> from the <see cref="HttpContext.Items"/> collection.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        public static void ClearCurrentOperationInstance(this HttpContext httpContext)
        {
            httpContext.Items.Remove(OperationInstanceContextKey);
        }

        /// <summary>
        /// Gets the current <see cref="IOperationInstance"/> from the <see cref="HttpContext.Items"/> collection.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="IOperationInstance"/> for the current request.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="IOperationInstance"/> has not been set.</exception>
        public static IOperationInstance GetCurrentOperationInstance(this HttpContext httpContext)
        {
            httpContext.Items.TryGetValue(OperationInstanceContextKey, out object? operationInstance);

            return operationInstance as IOperationInstance
                ?? throw new InvalidOperationException("The current operation instance is not available. Please ensure you have either added the [ObservableActionMethods] action filter to your controller (or its base class) or have other code in place to set the current operation by calling SetCurrentOperationInstance on the current HttpContext.");
        }
    }
}