// <copyright file="AiExceptionsInstrumentationSpecs.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights.Specs
{
    using System;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights.DataContracts;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for Application Insights telemetry for exceptions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Test members are public so the test framework can see them, but they're not intended for public consumption, so they don't require documentation")]
    public class AiExceptionsInstrumentationSpecs : AiSpecsBase
    {
        [Test]
        public void WhenExceptionReportedTelemetryIsSent()
        {
            ArgumentException ax;

            ax = this.ThrowReportAndCatchException();

            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            Assert.AreSame(ax, telemetry.Exception);
            Assert.AreEqual(this.Ai.Activity.RootId, telemetry.Context.Operation.Id);
            Assert.AreEqual(this.Ai.Activity.Id, telemetry.Context.Operation.ParentId);
        }

        [Test]
        public void WhenExceptionReportedInsideOperationIncludesRootAndParentIds()
        {
            ArgumentException ax;

            using (this.Ai.OperationsInstrumentation.StartOperation("ParentOp"))
            {
                ax = this.ThrowReportAndCatchException();
            }

            (ExceptionTelemetry exceptionTelemetry, RequestTelemetry requestTelemetry) = this.GetExceptionAndParentRequestTelemetry();
            Assert.AreEqual(this.Ai.Activity.RootId, exceptionTelemetry.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Id, exceptionTelemetry.Context.Operation.ParentId);
        }

        [Test]
        public void WhenExceptionReportedWithPropertiesTelemetryIncludesProperties()
        {
            this.ThrowReportAndCatchException(AdditionalDetailTests.DetailWithProperties);

            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            AdditionalDetailTests.AssertPropertiesPresent(telemetry);
        }

        [Test]
        public void WhenExceptionReportedWithMetricsTelemetryIncludesMetrics()
        {
            this.ThrowReportAndCatchException(AdditionalDetailTests.DetailWithMetrics);

            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            AdditionalDetailTests.AssertMetricsPresent(telemetry);
        }

        [Test]
        public void WhenExceptionReportedWithPropertiesAndMetricsTelemetryIncludesPropertiesAndMetrics()
        {
            this.ThrowReportAndCatchException(AdditionalDetailTests.DetailWithPropertiesAndMetrics);
            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            AdditionalDetailTests.AssertPropertiesAndMetricsPresent(telemetry);
        }

        private ExceptionTelemetry GetSingleExceptionTelemetry()
            => this.Ai.GetSingleTelemetry<ExceptionTelemetry>();

        private (ExceptionTelemetry exception, RequestTelemetry operation) GetExceptionAndParentRequestTelemetry()
            => this.Ai.GetParentOperationAndExceptionTelemetry<ExceptionTelemetry, RequestTelemetry>();

        private ArgumentException ThrowReportAndCatchException(
            AdditionalInstrumentationDetail additionalDetail = null)
        {
            ArgumentException ax;
            try
            {
                throw new ArgumentException("That was never 5 minutes!", "duration");
            }
            catch (ArgumentException x)
            {
                ax = x;
                this.Ai.ExceptionsInstrumentation.ReportException(x, additionalDetail);
            }

            return ax;
        }
    }
}