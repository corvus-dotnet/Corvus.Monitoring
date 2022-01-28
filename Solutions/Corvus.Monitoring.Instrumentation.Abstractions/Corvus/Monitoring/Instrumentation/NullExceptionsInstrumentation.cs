// <copyright file="NullExceptionsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;

    /// <summary>
    /// Fallback do-nothing sink for exception instrumentation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This gets used when an application uses some library or framework that uses
    /// Corvus.Monitoring, but where the application doesn't want to do anything with the
    /// information.
    /// </para>
    /// </remarks>
    internal class NullExceptionsInstrumentation : IExceptionsInstrumentation
    {
        /// <inheritdoc/>
        public void ReportException(Exception x, AdditionalInstrumentationDetail? additionalDetail = null)
        {
        }
    }
}