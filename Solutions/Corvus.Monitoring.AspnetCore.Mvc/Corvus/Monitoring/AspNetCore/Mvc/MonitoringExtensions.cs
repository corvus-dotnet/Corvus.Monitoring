// <copyright file="MonitoringExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc
{
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Web-specific extensions for monitoring.
    /// </summary>
    public static class MonitoringExtensions
    {
        /// <summary>
        /// Adds all the values in the supplied <see cref="RouteData"/> object to the supplied operation data.
        /// </summary>
        /// <param name="operationData">The <see cref="IOperationInstance"/> to add to.</param>
        /// <param name="routeData">The <see cref="RouteData"/> to add.</param>
        /// <remarks>
        /// Each value in the route data will be added as a property to the specified <see cref="IOperationInstance"/>
        /// using the property name "RouteData[tokenName]".
        /// </remarks>
        public static void AddRouteData(
            this IOperationInstance operationData,
            RouteData routeData)
        {
            if (routeData.Values.Count == 0)
            {
                operationData.AddOperationProperty("RouteData", "[empty]");
            }
            else
            {
                foreach (KeyValuePair<string, object?> current in routeData.Values)
                {
                    operationData.AddOperationProperty($"RouteData[{current.Key}]", current.Value?.ToString() ?? "[null]");
                }
            }
        }
    }
}