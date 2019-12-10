// <copyright file="AiSpecsBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.ApplicationInsights.Specs
{
    using NUnit.Framework;

    /// <summary>
    /// Base class of tests for Application Insights telemetry.
    /// </summary>
    public abstract class AiSpecsBase
    {
        private readonly bool telemetryClientViaDi;

        /// <summary>
        /// Creates a <see cref="AiSpecsBase"/>.
        /// </summary>
        /// <param name="telemetryClientViaDi">
        /// Pass <c>true</c> to use the DI initialization mechanism in which the Application Insights
        /// <c>TelemetryClient</c> is obtained through DI. Pass <c>false</c> to use the mechanism in
        /// which it is not available via DI and is instead passed in directly during initialization.
        /// </param>
        protected AiSpecsBase(bool telemetryClientViaDi = false)
        {
            this.telemetryClientViaDi = telemetryClientViaDi;
        }

        /// <summary>
        /// Gets the <see cref="AiTestContext"/>.
        /// </summary>
        private protected AiTestContext Ai { get; private set; }

        /// <summary>
        /// Creates the <see cref="AiTestContext"/> and starts an Activity.
        /// </summary>
        /// <remarks>
        /// Application Insights uses the <see cref="System.Diagnostics.Activity"/> class to track
        /// operations. It generates ids and handles nesting. A root activity needs to be in place
        /// for the tests to be realistic, since there will be one in the scenarios in which we
        /// are using Application Insights in production.
        /// </remarks>
        [SetUp]
        public void Setup()
        {
            this.Ai = new AiTestContext(this.telemetryClientViaDi);
        }

        /// <summary>
        /// Stops the activity created for the test.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            this.Ai.Dispose();
        }
    }
}