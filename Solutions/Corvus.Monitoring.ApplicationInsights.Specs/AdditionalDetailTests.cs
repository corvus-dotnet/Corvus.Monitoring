// <copyright file="AdditionalDetailTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights.Specs
{
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Common code used when testing <see cref="AdditionalInstrumentationDetail"/> handling.
    /// </summary>
    internal static class AdditionalDetailTests
    {
        private const string PropertyKey1 = "pk1";
        private const string PropertyKey2 = "pk2";
        private const string MetricKey1 = "mk1";
        private const string MetricKey2 = "mk2";
        private const string PropertyValue1 = "pv1";
        private const string PropertyValue2 = "pv2";
        private const double MetricValue1 = 42.0;
        private const double MetricValue2 = 99.0;

        /// <summary>
        /// Gets an <see cref="AdditionalInstrumentationDetail"/> with properties and metrics.
        /// </summary>
        public static AdditionalInstrumentationDetail DetailWithPropertiesAndMetrics { get; } = new()
            {
                Properties =
                    {
                        { PropertyKey1, PropertyValue1 },
                        { PropertyKey2, PropertyValue2 },
                    },
                Metrics =
                    {
                        { MetricKey1, MetricValue1 },
                        { MetricKey2, MetricValue2 },
                    },
            };

        /// <summary>
        /// Gets an <see cref="AdditionalInstrumentationDetail"/> with properties.
        /// </summary>
        public static AdditionalInstrumentationDetail DetailWithProperties { get; } = new(DetailWithPropertiesAndMetrics.Properties, null);

        /// <summary>
        /// Gets an <see cref="AdditionalInstrumentationDetail"/> with metrics.
        /// </summary>
        public static AdditionalInstrumentationDetail DetailWithMetrics { get; } = new(null, DetailWithPropertiesAndMetrics.Metrics);

        /// <summary>
        /// Verifies that the telemetry reports the same properties as
        /// <see cref="DetailWithProperties"/> contains.
        /// </summary>
        /// <param name="telemetry">The telemetry to test.</param>
        public static void AssertPropertiesPresent(ISupportProperties telemetry)
        {
            Assert.AreEqual(2, telemetry.Properties.Count, "Property count");
            Assert.AreEqual(PropertyValue1, telemetry.Properties[PropertyKey1], "Value for " + PropertyKey1);
            Assert.AreEqual(PropertyValue2, telemetry.Properties[PropertyKey2], "Value for " + PropertyKey2);
        }

        /// <summary>
        /// Verifies that the telemetry reports the same metrics as
        /// <see cref="DetailWithMetrics"/> contains.
        /// </summary>
        /// <param name="telemetry">The telemetry to test.</param>
        public static void AssertMetricsPresent(ISupportMetrics telemetry)
        {
            Assert.AreEqual(2, telemetry.Metrics.Count, "Metrics count");
            Assert.AreEqual(MetricValue1, telemetry.Metrics[MetricKey1], "Value for " + MetricKey1);
            Assert.AreEqual(MetricValue2, telemetry.Metrics[MetricKey2], "Value for " + MetricKey2);
        }

        /// <summary>
        /// Verifies that the telemetry reports the same properties and metrics as
        /// <see cref="DetailWithPropertiesAndMetrics"/> contains.
        /// </summary>
        /// <typeparam name="TTelemetry">
        /// The telemetry type. This must implement both <see cref="ISupportProperties"/> and
        /// <see cref="ISupportMetrics"/>.
        /// </typeparam>
        /// <param name="telemetry">The telemetry to test.</param>
        public static void AssertPropertiesAndMetricsPresent<TTelemetry>(TTelemetry telemetry)
            where TTelemetry : ISupportProperties, ISupportMetrics
        {
            AssertPropertiesPresent(telemetry);
            AssertMetricsPresent(telemetry);
        }
    }
}