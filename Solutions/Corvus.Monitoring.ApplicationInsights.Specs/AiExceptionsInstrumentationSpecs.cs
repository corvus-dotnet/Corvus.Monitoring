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
        public void WhenExceptionIncludesProperties()
        {
            const string k1 = "k1", v1 = "v1";
            const string k2 = "k2", v2 = "v2";

            this.ThrowReportAndCatchException(
                new AdditionalInstrumentationDetail
                {
                    Properties =
                    {
                        { k1, v1 },
                        { k2, v2 },
                    },
                });

            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            Assert.AreEqual(2, telemetry.Properties.Count, "Property count");
            Assert.AreEqual(v1, telemetry.Properties[k1], "Value for " + k1);
            Assert.AreEqual(v2, telemetry.Properties[k2], "Value for " + k2);
        }

        [Test]
        public void WhenExceptionIncludesMetrics()
        {
            const string mk1 = "mk1", mk2 = "mk2";
            const double mv1 = 42.0, mv2 = 99.0;

            this.ThrowReportAndCatchException(
                new AdditionalInstrumentationDetail
                {
                    Metrics =
                    {
                        { mk1, mv1 },
                        { mk2, mv2 },
                    },
                });

            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            Assert.AreEqual(2, telemetry.Metrics.Count, "Metrics count");
            Assert.AreEqual(mv1, telemetry.Metrics[mk1], "Value for " + mk1);
            Assert.AreEqual(mv2, telemetry.Metrics[mk2], "Value for " + mk2);
        }

        [Test]
        public void WhenExceptionIncludesPropertiesAndMetrics()
        {
            const string k1 = "k1", v1 = "v1";
            const string k2 = "k2", v2 = "v2";
            const string mk1 = "mk1", mk2 = "mk2";
            const double mv1 = 42.0, mv2 = 99.0;

            this.ThrowReportAndCatchException(
                new AdditionalInstrumentationDetail
                {
                    Properties =
                    {
                        { k1, v1 },
                        { k2, v2 },
                    },
                    Metrics =
                    {
                        { mk1, mv1 },
                        { mk2, mv2 },
                    },
                });

            ExceptionTelemetry telemetry = this.GetSingleExceptionTelemetry();
            Assert.AreEqual(2, telemetry.Properties.Count, "Property count");
            Assert.AreEqual(v1, telemetry.Properties[k1], "Value for " + k1);
            Assert.AreEqual(v2, telemetry.Properties[k2], "Value for " + k2);
            Assert.AreEqual(2, telemetry.Metrics.Count, "Metrics count");
            Assert.AreEqual(mv1, telemetry.Metrics[mk1], "Value for " + mk1);
            Assert.AreEqual(mv2, telemetry.Metrics[mk2], "Value for " + mk2);
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