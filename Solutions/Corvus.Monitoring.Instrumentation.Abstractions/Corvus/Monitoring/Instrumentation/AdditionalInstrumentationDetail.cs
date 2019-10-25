// <copyright file="AdditionalInstrumentationDetail.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System.Collections.Generic;

    /// <summary>
    /// Additional information supplied with instrumentation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each of the supported instrumentation types can have additional information attached -
    /// key value pairs that are either strings or numbers.
    /// </para>
    /// </remarks>
    public class AdditionalInstrumentationDetail
    {
        private IDictionary<string, string> properties;
        private IDictionary<string, double> metrics;

        /// <summary>
        /// Creates a <see cref="AdditionalInstrumentationDetail"/>.
        /// </summary>
        public AdditionalInstrumentationDetail()
        {
        }

        /// <summary>
        /// Creates a <see cref="AdditionalInstrumentationDetail"/> with the specified properties and/or metrics.
        /// </summary>
        /// <param name="properties">Value for <see cref="Properties"/>.</param>
        /// <param name="metrics">Value for <see cref="Metrics"/>.</param>
        public AdditionalInstrumentationDetail(IDictionary<string, string> properties, IDictionary<string, double> metrics)
        {
            this.properties = properties;
            this.metrics = metrics;
        }

        /// <summary>
        /// Gets the dictionary of string properties to associate with some instrumentation.
        /// </summary>
        public IDictionary<string, string> Properties => this.properties ?? (this.properties = new Dictionary<string, string>());

        /// <summary>
        /// Gets the dictionary of numeric properties to associate with some instrumentation.
        /// </summary>
        public IDictionary<string, double> Metrics => this.metrics ?? (this.metrics = new Dictionary<string, double>());
    }
}
