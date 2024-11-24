// <copyright file="TelemetryClientViaDiSpecs.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights.Specs
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// In general it's preferable not to put the Application Insights <c>TelemetryClient</c> into the DI service
    /// collection, because that encourages code to depend on it directly, which in turn makes that code resistant
    /// to enhancements in our monitoring libraries. However, in Azure Functions, that's the only supported way
    /// to get hold of a correctly-initialized <c>TelemetryClient</c>, so it's a scenario we need to support.
    /// </summary>
    [TestClass]
    public class TelemetryClientViaDiSpecs : AiSpecsBase
    {
        public TelemetryClientViaDiSpecs()
            : base(true)
        {
        }

        [TestMethod]
        public void InstrumentationReportsTelemetryToAiWhenClientObtainedViaDi()
        {
            // We're not going to exercise everything. Just perform a basic smoke test, and some
            // simple verification. The only goal here is to ensure that the alternative DI
            // initialization mechanims works. We leave the full exercising to all the other tests.
            ArgumentException ax;

            using (this.Ai.OperationsInstrumentation.StartOperation("ParentOp"))
            {
                try
                {
                    throw new ArgumentException("That was never 5 minutes!", "duration");
                }
                catch (ArgumentException x)
                {
                    ax = x;
                    this.Ai.ExceptionsInstrumentation.ReportException(x);
                }
            }

            (ExceptionTelemetry exceptionTelemetry, RequestTelemetry requestTelemetry) = this.Ai.GetParentOperationAndExceptionTelemetry<ExceptionTelemetry, RequestTelemetry>();
            Assert.AreEqual(this.Ai.Activity!.RootId, exceptionTelemetry.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Id, exceptionTelemetry.Context.Operation.ParentId);
        }
    }
}