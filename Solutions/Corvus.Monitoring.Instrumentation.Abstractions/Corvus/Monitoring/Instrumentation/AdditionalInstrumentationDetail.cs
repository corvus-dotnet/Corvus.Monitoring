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
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "CA2227:Collection properties should be read only",
        Justification = "TODO: needs some thought - we want it to be possible for either property to be null, and we want object initializer syntax to work so I'm not sure I agree with this warning")]
    public class AdditionalInstrumentationDetail
    {
        /// <summary>
        /// Gets or sets the dictionary of string properties to associate with some instrumentation.
        /// </summary>
        public IDictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of numeric properties to associate with some instrumentation.
        /// </summary>
        public IDictionary<string, double> Metrics { get; set; }
    }
}
