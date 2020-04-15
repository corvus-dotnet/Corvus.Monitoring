// <copyright file="AiOperationsInstrumentationSpecs.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights.Specs
{
    using System.Diagnostics;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.ApplicationInsights.DataContracts;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for Application Insights telemetry for operations.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Test members are public so the test framework can see them, but they're not intended for public consumption, so they don't require documentation")]
    public class AiOperationsInstrumentationSpecs : AiSpecsBase
    {
        [Test]
        public void WhenOperationFinishesTelemetryIsSent()
        {
            const string operationName = "MyOp";
            using (this.Ai!.OperationsInstrumentation.StartOperation(operationName))
            {
                // We need to dispose the operation, because Application Insights doesn't
                // send any telemetry for it until it's done.
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            Assert.AreEqual(operationName, telemetry.Name);
            Assert.AreEqual(this.Ai!.Activity?.RootId, telemetry.Context.Operation.Id);
            Assert.AreEqual(this.Ai!.Activity?.Id, telemetry.Context.Operation.ParentId);
        }

        [Test]
        public void WhenOperationWithUpFrontPropertiesFinishesTelemetryIncludesProperties()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation(
                "op", AdditionalDetailTests.DetailWithProperties))
            {
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertPropertiesPresent(telemetry);
        }

        [Test]
        public void WhenOperationWithPostStartPropertiesFinishesTelemetryIncludesProperties()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation("op"))
            {
                operation.AddOperationDetail(AdditionalDetailTests.DetailWithProperties);
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertPropertiesPresent(telemetry);
        }

        [Test]
        public void WhenOperationWithUpFrontMetricsFinishesTelemetryIncludesMetrics()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation(
                "op", AdditionalDetailTests.DetailWithMetrics))
            {
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertMetricsPresent(telemetry);
        }

        [Test]
        public void WhenOperationWithPostStartMetricsFinishesTelemetryIncludesMetrics()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation("op"))
            {
                operation.AddOperationDetail(AdditionalDetailTests.DetailWithMetrics);
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertMetricsPresent(telemetry);
        }

        [Test]
        public void WhenOperationWithUpFrontPropertiesAndMetricsFinishesTelemetryIncludesPropertiesAndMetrics()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation(
                "op", AdditionalDetailTests.DetailWithPropertiesAndMetrics))
            {
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertPropertiesAndMetricsPresent(telemetry);
        }

        [Test]
        public void WhenOperationWithPropertiesAndMetricsFinishesTelemetryIncludesPropertiesAndMetrics()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation("op"))
            {
                operation.AddOperationDetail(AdditionalDetailTests.DetailWithPropertiesAndMetrics);
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertPropertiesAndMetricsPresent(telemetry);
        }

        [Test]
        public void WhenOperationWithPostStartPropertiesAndMetricsAddedSeparatelyFinishesTelemetryIncludesPropertiesAndMetrics()
        {
            using (IOperationInstance operation = this.Ai!.OperationsInstrumentation.StartOperation("op"))
            {
                operation.AddOperationDetail(AdditionalDetailTests.DetailWithProperties);
                operation.AddOperationDetail(AdditionalDetailTests.DetailWithMetrics);
            }

            RequestTelemetry telemetry = this.GetSingleRequestTelemetry();
            AdditionalDetailTests.AssertPropertiesAndMetricsPresent(telemetry);
        }

        [Test]
        public void WhenChildOperationFinishesTelemetryIncludesParentId()
        {
            string testActivityId = Activity.Current.Id;
            string parentOpActivityId;
            string childOpActivityId;
            using (this.Ai!.OperationsInstrumentation.StartOperation("ParentOp"))
            {
                parentOpActivityId = Activity.Current.Id;
                using (this.Ai!.OperationsInstrumentation.StartOperation("ChildOp"))
                {
                    childOpActivityId = Activity.Current.Id;
                }
            }

            (RequestTelemetry child, RequestTelemetry parent) = this.GetChildParentRequestTelemetry();

            Assert.AreNotEqual(parent.Id, child.Id);
            Assert.IsTrue(child.Id.StartsWith(parent.Id));

            // The activity root id should permeate through all telemetry associated
            // with the request. It becomes the ai.operation.id tag, which shows up
            // as "Operation Id" in the Application Insights event details, and this
            // tag is how the portal decides which items belong to the same timeline
            // in the end-to-end transaction view.
            Assert.AreEqual(this.Ai!.Activity?.RootId, parent.Context.Operation.Id);
            Assert.AreEqual(this.Ai!.Activity?.RootId, child.Context.Operation.Id);

            // The operation ids reported should be consistent with the ones shown
            // by System.Diagnostics.Activity.Current, because the Application Insights
            // client creates child activities for request tracking.
            // There are three ids: the one for the root activity object created during
            // test setup, the one created to represent the parent activity started
            // in this test, and the one created to represent the child activity
            // created in this test.
            Assert.AreEqual(testActivityId, parent.Context.Operation.ParentId);
            Assert.AreEqual(parentOpActivityId, parent.Id);
            Assert.AreEqual(parentOpActivityId, child.Context.Operation.ParentId);
            Assert.AreEqual(childOpActivityId, child.Id);
        }

        private RequestTelemetry GetSingleRequestTelemetry()
        {
            Assert.AreEqual(1, this.Ai!.Items.Count, "Number of telemetry items");
            return (RequestTelemetry)this.Ai!.Items[0];
        }

        private (RequestTelemetry child, RequestTelemetry parent) GetChildParentRequestTelemetry()
        {
            Assert.AreEqual(2, this.Ai!.Items.Count, "Number of telemetry items");
            var child = (RequestTelemetry)this.Ai!.Items[0];
            var parent = (RequestTelemetry)this.Ai!.Items[1];

            return (child, parent);
        }
    }
}