// <copyright file="AiOperationsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
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
        public IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail? additionalDetail)
        {
            IOperationHolder<RequestTelemetry> operationHolder = this.telemetryClient.StartOperation<RequestTelemetry>(name);
            if (additionalDetail?.Properties != null)
            {
                foreach (KeyValuePair<string, string> property in additionalDetail.Properties)
                {
                    operationHolder.Telemetry.Properties.Add(property);
                }
            }

            if (additionalDetail?.Metrics != null)
            {
                foreach (KeyValuePair<string, double> metric in additionalDetail.Metrics)
                {
                    operationHolder.Telemetry.Metrics.Add(metric);
                }
            }

            return new Operation(operationHolder);
        }

        private class Operation : IOperationInstance
        {
            private readonly IOperationHolder<RequestTelemetry> operationHolder;

            public Operation(IOperationHolder<RequestTelemetry> operationHolder)
            {
                this.operationHolder = operationHolder;
            }

            public void AddOperationProperty(string name, string value)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.operationHolder.Telemetry.Properties.Add(name, value);
            }

            public void AddOperationMetric(string name, double value)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                this.operationHolder.Telemetry.Metrics.Add(name, value);
            }

            public void Dispose()
            {
                this.operationHolder.Dispose();
            }
        }
    }
}