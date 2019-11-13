// <copyright file="IExceptionsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;

    /// <summary>
    /// Enables reporting of exceptions for monitoring and diagnostic purposes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This provides a way for components to report exceptions in a way that can be made visible
    /// to monitoring systems such as Application Insights without needing to take a direct
    /// dependency on those systems' client libraries.
    /// </para>
    /// </remarks>
    public interface IExceptionsInstrumentation
    {
        /// <summary>
        /// Report an exception.
        /// </summary>
        /// <param name="x">The exception that occurred.</param>
        /// <param name="additionalDetail">Optional additional properties and metrics.</param>
        void ReportException(Exception x, AdditionalInstrumentationDetail additionalDetail = null);
    }
}