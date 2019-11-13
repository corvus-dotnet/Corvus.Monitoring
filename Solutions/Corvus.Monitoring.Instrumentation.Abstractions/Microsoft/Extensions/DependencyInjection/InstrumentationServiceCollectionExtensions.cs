// <copyright file="InstrumentationServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Corvus.Monitoring.Instrumentation;

    /// <summary>
    /// Extension methods for adding services relating to Instrumentation.
    /// </summary>
    public static class InstrumentationServiceCollectionExtensions
    {
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