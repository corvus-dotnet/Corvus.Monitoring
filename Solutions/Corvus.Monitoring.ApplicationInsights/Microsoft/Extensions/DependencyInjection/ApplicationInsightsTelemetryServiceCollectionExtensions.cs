// <copyright file="ApplicationInsightsTelemetryServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Corvus.Monitoring.ApplicationInsights;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights;

    /// <summary>
    /// Extension methods for adding services to deliver Instrumentation telemetry to Application
    /// Insights.
    /// </summary>
    public static class ApplicationInsightsTelemetryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services to deliver Instrumentation telemetry to Application Insights.
        /// </summary>
        /// <param name="services">The service collection to which to add the services.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddApplicationInsightsInstrumentationTelemetry(
            this IServiceCollection services,
            TelemetryClient telemetryClient)
        {
            return services
                .AddSingleton<IOperationsInstrumentation>(new AiOperationsInstrumentation(telemetryClient))
                .AddSingleton<IExceptionsInstrumentation>(new AiExceptionsInstrumentation(telemetryClient));
        }
    }
}
