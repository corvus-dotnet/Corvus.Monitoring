// <copyright file="OperationDetail.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs.Fakes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes operation instrumentation that was delivered to one of our fakes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This represents a single call to <see cref="IOperationsInstrumentation.StartOperation(string, AdditionalInstrumentationDetail)"/>.
    /// (It also acts as the return value from that call.)
    /// </para>
    /// </remarks>
    public class OperationDetail : IOperationInstance
    {
        private AdditionalInstrumentationDetail furtherDetails = new(
            new Dictionary<string, string>(),
            new Dictionary<string, double>());

        public OperationDetail(
            string name,
            AdditionalInstrumentationDetail? additionalDetail)
        {
            this.Name = name;
            this.AdditionalDetail = additionalDetail ?? new();
        }

        /// <summary>
        /// Gets the operation name passed to the call to <see cref="IOperationsInstrumentation.StartOperation(string, AdditionalInstrumentationDetail)"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the additional instrumentation detail (if any) passed to the call to
        /// <see cref="IOperationsInstrumentation.StartOperation(string, AdditionalInstrumentationDetail)"/>.
        /// </summary>
        public AdditionalInstrumentationDetail? AdditionalDetail { get; }

        /// <summary>
        /// Gets a list of any further instrumentation detail provided by calls to
        /// <see cref="IOperationInstance.AddOperationProperty(string, string)"/> and
        /// <see cref="IOperationInstance.AddOperationMetric(string, double)"/>.
        /// </summary>
        public AdditionalInstrumentationDetail FurtherDetails => this.furtherDetails;

        /// <summary>
        /// Gets a value indicating whether the code providing instrumentation to our fake has
        /// called <c>Dispose</c> on the object returned by
        /// <see cref="IOperationsInstrumentation.StartOperation(string, AdditionalInstrumentationDetail)"/>.
        /// </summary>
        public bool IsDisposed { get; private set; }

        public void AddOperationMetric(string name, double value)
        {
            this.furtherDetails.Metrics.Add(name, value);
        }

        public void AddOperationProperty(string name, string value)
        {
            this.furtherDetails.Properties.Add(name, value);
        }

        void IDisposable.Dispose()
        {
            this.IsDisposed = true;
        }
    }
}