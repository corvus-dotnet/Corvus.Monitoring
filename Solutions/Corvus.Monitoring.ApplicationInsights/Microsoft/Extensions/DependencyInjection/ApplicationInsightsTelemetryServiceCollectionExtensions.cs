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
        /// <remarks>
        /// This overload supports scenarios in which the <see cref="TelemetryClient"/> is not
        /// available via DI. Since the <c>Corvus.Monitoring</c> libraries are designed to allow
        /// instrumentation to be added to components without depending directly on any particular
        /// telemetry mechanism, it is often desirable to avoid putting the <c>TelemetryClient</c>
        /// into the services available via DI. This extension method makes it possible to
        /// pass the <c>TelemetryClient</c> in to the Application-Insights-specific parts of
        /// <c>Corvus.Monitoring</c> without putting it into the DI services.
        /// </remarks>
        public static IServiceCollection AddApplicationInsightsInstrumentationTelemetry(
            this IServiceCollection services,
            TelemetryClient telemetryClient)
        {
            return services
                .AddSingleton<IOperationsInstrumentation>(new AiOperationsInstrumentation(telemetryClient))
                .AddSingleton<IExceptionsInstrumentation>(new AiExceptionsInstrumentation(telemetryClient));
        }

        /// <summary>
        /// Adds services to deliver Instrumentation telemetry to Application Insights.
        /// </summary>
        /// <param name="services">The service collection to which to add the services.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// This overload supports scenarios in which the <see cref="TelemetryClient"/> must be
        /// obtained via DI. For example, the Azure Functions SDK makes the <c>TelemetryClient</c>
        /// available only through DI.
        /// </remarks>
        public static IServiceCollection AddApplicationInsightsInstrumentationTelemetry(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IOperationsInstrumentation, AiOperationsInstrumentation>()
                .AddSingleton<IExceptionsInstrumentation, AiExceptionsInstrumentation>();
        }
    }
}