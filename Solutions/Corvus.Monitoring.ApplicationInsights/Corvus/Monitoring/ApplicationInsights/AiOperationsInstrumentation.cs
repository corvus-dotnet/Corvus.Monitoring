// <copyright file="AiOperationsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights
{
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Delivers operations instrumentation data to Application Insights.
    /// </summary>
    internal class AiOperationsInstrumentation : IOperationsInstrumentation
    {
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Creates a <see cref="AiOperationsInstrumentation"/>.
        /// </summary>
        /// <param name="telemetryClient">
        /// The Application Insights client through which to deliver the telemetry.
        /// </param>
        public AiOperationsInstrumentation(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        /// <inheritdoc />
        public IOperationInstance StartOperation(string name)
        {
            return new Operation(this.telemetryClient.StartOperation<RequestTelemetry>(name));
        }

        private class Operation : IOperationInstance
        {
            private readonly IOperationHolder<RequestTelemetry> operationHolder;

            public Operation(IOperationHolder<RequestTelemetry> operationHolder)
            {
                this.operationHolder = operationHolder;
            }

            public void AddOperationProperty(string key, string value)
            {
                this.operationHolder.Telemetry.Properties.Add(key, value);
            }

            public void Dispose()
            {
                this.operationHolder.Dispose();
            }
        }
    }
}
