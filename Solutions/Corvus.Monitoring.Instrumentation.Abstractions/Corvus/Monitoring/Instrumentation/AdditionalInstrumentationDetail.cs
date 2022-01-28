// <copyright file="AdditionalInstrumentationDetail.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable enable

namespace Corvus.Monitoring.Instrumentation
{
    using System.Collections.Generic;
    using System.Diagnostics;

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
        public AdditionalInstrumentationDetail(IDictionary<string, string>? properties, IDictionary<string, double>? metrics)
        {
            this.PropertiesIfPresent = properties;
            this.MetricsIfPresent = metrics;
        }

        /// <summary>
        /// Gets the dictionary of string properties to associate with some instrumentation. If
        /// this property does not yet contain a dictionary, reading its value will cause the
        /// dictionary to be created.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] // Evaluation has side effects.
        public IDictionary<string, string> Properties => this.PropertiesIfPresent ??= new Dictionary<string, string>();

        /// <summary>
        /// Gets the dictionary of numeric properties to associate with some instrumentation. If
        /// this property does not yet contain a dictionary, reading its value will cause the
        /// dictionary to be created.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] // Evaluation has side effects.
        public IDictionary<string, double> Metrics => this.MetricsIfPresent ??= new Dictionary<string, double>();

        /// <summary>
        /// Gets the dictionary of string properties to associate with some instrumentation, or
        /// null if properties are not being set.
        /// </summary>
        public IDictionary<string, string>? PropertiesIfPresent { get; private set; }

        /// <summary>
        /// Gets the dictionary of metrics to associate with some instrumentation, or
        /// null if metrics are not being set.
        /// </summary>
        public IDictionary<string, double>? MetricsIfPresent { get; private set; }
    }
}