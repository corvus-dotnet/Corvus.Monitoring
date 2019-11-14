// <copyright file="InstrumentationServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Corvus.Monitoring.Instrumentation;

    /// <summary>
    /// Extension methods for adding services relating to Instrumentation.
    /// </summary>
    public static class InstrumentationServiceCollectionExtensions
    {
        /// <summary>
        /// Ensure that the various instrumentation interfaces can all be resolved successfully
        /// through DI.
        /// </summary>
        /// <param name="services">The service collection to which to add the services.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// <para>
        /// If an application performs no other instrumentation setup, the instrumentation
        /// implementations available will all do nothing. The intent of this is to enable
        /// libraries to presume that these interfaces are always available, even in applications
        /// that turn out not to want any of the instrumentation information.
        /// </para>
        /// <para>
        /// Applications that want to take advantage of instrumentation should register
        /// implementations of the non-generic interfaces either before or after this method is
        /// called. (E.g., they could take a dependency on Corvus.Monitoring.ApplicationInsights
        /// and call <c>AddApplicationInsightsInstrumentationTelemetry</c> in their DI startup.)
        /// Doing so either before or after calling this method will result in that real
        /// implementation being used instead of the null implementations this method provides as
        /// fallbacks.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddInstrumentation(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            bool operationsAlreadyPresent = false;
            bool exceptionsAlreadyPresent = false;
            bool genericImplementationAlreadyPresent = false;
            foreach (ServiceDescriptor service in services)
            {
                if (service.ServiceType == typeof(IOperationsInstrumentation))
                {
                    operationsAlreadyPresent = true;
                }

                if (service.ServiceType == typeof(IExceptionsInstrumentation))
                {
                    exceptionsAlreadyPresent = true;
                }

                if (service.ServiceType == typeof(IOperationsInstrumentation<>) ||
                    service.ServiceType == typeof(IExceptionsInstrumentation<>))
                {
                    genericImplementationAlreadyPresent = true;
                }
            }

            if (!operationsAlreadyPresent)
            {
                services.AddSingleton<IOperationsInstrumentation, NullOperationsInstrumentation>();
            }

            if (!exceptionsAlreadyPresent)
            {
                services.AddSingleton<IExceptionsInstrumentation, NullExceptionsInstrumentation>();
            }

            if (!genericImplementationAlreadyPresent)
            {
                services.AddInstrumentationSourceTagging();
            }

            return services;
        }

        /// <summary>
        /// Add implementations of the generic instrumentation interfaces that add information
        /// based on the generic type argument.
        /// </summary>
        /// <param name="services">The service collection to which to add the services.</param>
        /// <param name="propertyName">
        /// The property name through which to indicate the type argument.
        /// </param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddInstrumentationSourceTagging(
            this IServiceCollection services,
            string propertyName = "Category")
        {
            return services
                .AddSingleton(new TaggingPropertySource(propertyName))
                .AddSingleton(typeof(IOperationsInstrumentation<>), typeof(TaggingOperationsInstrumentation<>))
                .AddSingleton(typeof(IExceptionsInstrumentation<>), typeof(TaggingExceptionsInstrumentation<>));
        }
    }
}