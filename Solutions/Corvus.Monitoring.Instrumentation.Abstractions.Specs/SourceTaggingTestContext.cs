// <copyright file="SourceTaggingTestContext.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using System.Collections.Generic;
    using Corvus.Monitoring.Instrumentation.Abstractions.Specs.Fakes;
    using Microsoft.Extensions.DependencyInjection;

    public class SourceTaggingTestContext : IDisposable
    {
        private readonly ServiceProvider serviceProvider;
        private readonly FakeInstrumentationSinks fakeInstrumentationSinks = new();

        public SourceTaggingTestContext()
        {
            var services = new ServiceCollection();
            services.AddInstrumentationSourceTagging();
            this.fakeInstrumentationSinks.AddNonGenericImplementationsToServices(services);
            this.serviceProvider = services.BuildServiceProvider();
        }

        public string SourcePropertyName => "Endjin.Source"; // TBD: parameterise tests so we can vary this

        public IReadOnlyList<OperationDetail> Operations => this.fakeInstrumentationSinks.Operations;

        public IReadOnlyList<ExceptionDetail> Exceptions => this.fakeInstrumentationSinks.Exceptions;

        public IOperationsInstrumentation<T> GetOperationsInstrumentation<T>() => this.serviceProvider.GetRequiredService<IOperationsInstrumentation<T>>();

        public IExceptionsInstrumentation<T> GetExceptionsInstrumentation<T>() => this.serviceProvider.GetRequiredService<IExceptionsInstrumentation<T>>();

        public void Dispose()
        {
            this.serviceProvider.Dispose();
        }
    }
}