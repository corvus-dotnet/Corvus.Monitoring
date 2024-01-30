// <copyright file="DemoWebApplicationFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Specs.Bindings
{
    using Corvus.Monitoring.AspnetCore.Mvc.Specs.Fakes;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;

    public class DemoWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                ServiceDescriptor existingOperationsInstrumentation = services.First(x => x.ServiceType == typeof(IOperationsInstrumentation));
                services.Remove(existingOperationsInstrumentation);
                services.AddSingleton<IOperationsInstrumentation>(TestOperationsInstrumentation.Instance);
            });
        }
    }
}