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
            this.Ai = new AiTestContext();
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
