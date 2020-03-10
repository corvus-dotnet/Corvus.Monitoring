// <copyright file="ExceptionDetail.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs.Fakes
{
    using System;

    /// <summary>
    /// Describes exception instrumentation that was delivered to one of our fakes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This represents a single call to <see cref="IExceptionsInstrumentation.ReportException(Exception, AdditionalInstrumentationDetail)"/>.
    /// </para>
    /// </remarks>
    public class ExceptionDetail
    {
        public ExceptionDetail(
            Exception x,
            AdditionalInstrumentationDetail additionalDetail)
        {
            this.Exception = x;
            this.AdditionalDetail = additionalDetail;
        }

        /// <summary>
        /// Gets the exception passed to the call to <see cref="IExceptionsInstrumentation.ReportException(Exception, AdditionalInstrumentationDetail)"/>.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the additional instrumentation detail (if any) passed to the call to
        /// <see cref="IExceptionsInstrumentation.ReportException(Exception, AdditionalInstrumentationDetail)"/>.
        /// </summary>
        public AdditionalInstrumentationDetail AdditionalDetail { get; }
    }
}
