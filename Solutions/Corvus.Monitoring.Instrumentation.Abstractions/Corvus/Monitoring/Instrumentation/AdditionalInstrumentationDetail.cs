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
        private Dictionary<string, string> properties;
        private Dictionary<string, double> metrics;

        /// <summary>
        /// Gets the dictionary of string properties to associate with some instrumentation.
        /// </summary>
        public Dictionary<string, string> Properties => this.properties ?? (this.properties = new Dictionary<string, string>());

        /// <summary>
        /// Gets the dictionary of numeric properties to associate with some instrumentation.
        /// </summary>
        public IDictionary<string, double> Metrics => this.metrics ?? (this.metrics = new Dictionary<string, double>());
    }
}
