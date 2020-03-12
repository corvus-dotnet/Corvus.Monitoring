// <copyright file="AiTestContext.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    /// <summary>
    /// Common aspects of tests involving Application Insights client and telemetry.
    /// </summary>
    internal class AiTestContext : IDisposable
    {
        private readonly ServiceProvider serviceProvider;

        /// <summary>
        /// Creates a <see cref="AiTestContext"/>.
        /// </summary>
        /// <param name="telemetryClientViaDi">
        /// Indicates whether to use the DI initialization mechanism that obtains the <see cref="TelemetryClient"/>
        /// as a dependency. (In general it's preferable not to do that, but the Azure Functions SDK requires it,
        /// so we support it.)
        /// </param>
        public AiTestContext(bool telemetryClientViaDi)
        {
            var items = new List<ITelemetry>();
            this.Items = items;
            var channel = new FakeTelemetryChannel(items);

            var telemetryConfig = new TelemetryConfiguration("not-used", channel);
            telemetryConfig.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.TelemetryClient = new TelemetryClient(telemetryConfig);

            var services = new ServiceCollection();
            if (telemetryClientViaDi)
            {
                services.AddSingleton(this.TelemetryClient);
                services.AddApplicationInsightsInstrumentationTelemetry();
            }
            else
            {
                services.AddApplicationInsightsInstrumentationTelemetry(this.TelemetryClient);
            }

            this.serviceProvider = services.BuildServiceProvider();

            this.OperationsInstrumentation = this.serviceProvider.GetRequiredService<IOperationsInstrumentation>();
            this.ExceptionsInstrumentation = this.serviceProvider.GetRequiredService<IExceptionsInstrumentation>();

            this.Activity = new Activity("UnitTest").Start();
        }

        /// <summary>
        /// Gets the Application Insights client.
        /// </summary>
        public TelemetryClient TelemetryClient { get; }

        /// <summary>
        /// Gets a list of the telemetry items sent to Application Insights.
        /// </summary>
        public IReadOnlyList<ITelemetry> Items { get; }

        /// <summary>
        /// Gets the <see cref="IOperationsInstrumentation"/>.
        /// </summary>
        public IOperationsInstrumentation OperationsInstrumentation { get; }

        /// <summary>
        /// Gets the <see cref="IExceptionsInstrumentation"/>.
        /// </summary>
        public IExceptionsInstrumentation ExceptionsInstrumentation { get; }

        /// <summary>
        /// Gets the Activity created with this object.
        /// </summary>
        public Activity Activity { get; private set; }

        /// <summary>
        /// Verifies that there is just one item of telemetry, and returns it.
        /// </summary>
        /// <typeparam name="T">The expected telemetry type.</typeparam>
        /// <returns>The telemetry.</returns>
        public T GetSingleTelemetry<T>()
        {
            Assert.AreEqual(1, this.Items.Count, "Number of telemetry items");
            return (T)this.Items[0];
        }

        /// <summary>
        /// Verifies that there are just two items of telemetry and returns them.
        /// </summary>
        /// <typeparam name="T1">The expected type of the first item.</typeparam>
        /// <typeparam name="T2">The expected type of the second item.</typeparam>
        /// <returns>The items.</returns>
        /// <remarks>
        /// When telemetry is nested, the child items appear first. For example, when
        /// an exception is reported inside an operation, the exception will appear first
        /// even though the call to create the operation happens first. The operation telemetry
        /// doesn't get created until the operation completes.
        /// </remarks>
        public (T1, T2) GetParentOperationAndExceptionTelemetry<T1, T2>()
        {
            Assert.AreEqual(2, this.Items.Count, "Number of telemetry items");
            return ((T1)this.Items[0], (T2)this.Items[1]);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.Activity != null)
            {
                this.Activity.Stop();
                this.Activity = null;
                this.serviceProvider.Dispose();
            }
        }

        /// <summary>
        /// Target for Application Insights telemetry - enables us to inspect what the library
        /// under test asked AI to do.
        /// </summary>
        private class FakeTelemetryChannel : ITelemetryChannel
        {
            private readonly List<ITelemetry> items;

            public FakeTelemetryChannel(List<ITelemetry> items)
            {
                this.items = items;
            }

            public bool? DeveloperMode { get; set; }

            public string EndpointAddress { get; set; }

            public void Dispose()
            {
            }

            public void Flush()
            {
            }

            public void Send(ITelemetry item)
            {
                this.items.Add(item);
            }
        }
    }
}
