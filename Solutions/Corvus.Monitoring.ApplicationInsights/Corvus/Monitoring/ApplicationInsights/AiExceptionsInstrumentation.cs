// <copyright file="AiExceptionsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights
{
    using System;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights;

    /// <summary>
    /// Delivers exception instrumentation data to Application Insights.
    /// </summary>
    internal class AiExceptionsInstrumentation : IExceptionsInstrumentation
    {
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Creates a <see cref="AiExceptionsInstrumentation"/>.
        /// </summary>
        /// <param name="telemetryClient">
        /// The Application Insights client through which to deliver the telemetry.
        /// </param>
        public AiExceptionsInstrumentation(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        /// <inheritdoc />
        public void ReportException(Exception x, AdditionalInstrumentationDetail additionalDetail)
        {
            this.telemetryClient.TrackException(x, additionalDetail?.PropertiesIfPresent, additionalDetail?.MetricsIfPresent);
        }
    }
}
