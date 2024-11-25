// <copyright file="SourceTaggingSpecsBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Base class of tests for source tagging instrumentation interfaces.
    /// </summary>
    public abstract class SourceTaggingSpecsBase
    {
        private SourceTaggingTestContext? context;

        /// <summary>
        /// Gets or sets the <see cref="SourceTaggingTestContext"/>.
        /// </summary>
        private protected SourceTaggingTestContext Context
        {
            get => this.context ?? throw new InvalidOperationException($"The property {nameof(this.Context)} has not been set.");
            set => this.context = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Creates the <see cref="SourceTaggingTestContext"/>.
        /// </summary>
        /// <remarks>
        /// Tests for source tagging need the source tagging implementations to have been
        /// registered in DI in the normal way, and we also need implementations of the
        /// non-generic interfaces that enable us to test whether the calls were forwarded and,
        /// where required, augmented.
        /// </remarks>
        [TestInitialize]
        public void Setup()
        {
            this.Context = new SourceTaggingTestContext();
        }

        /// <summary>
        /// Stops the activity created for the test.
        /// </summary>
        [TestCleanup]
        public void Teardown()
        {
            this.Context.Dispose();
        }

        public class TestType1
        {
        }

        public class TestType2
        {
        }

        public class GenericTestType<T1, T2>
        {
        }
    }
}