// <copyright file="TaggingExceptionsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;

    /// <summary>
    /// Implementation of <see cref="IExceptionsInstrumentation{T}"/> that adds source information
    /// based on the type argument.
    /// </summary>
    /// <typeparam name="T">The source type.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This warning does not recognize usage through DI")]
    internal class TaggingExceptionsInstrumentation<T> : IExceptionsInstrumentation<T>
    {
        private readonly TaggingPropertySource propertySource;
        private readonly IExceptionsInstrumentation underlying;

        /// <summary>
        /// Creates a <see cref="TaggingExceptionsInstrumentation{T}"/>.
        /// </summary>
        /// <param name="propertySource">
        /// Determines the property through which the source will be reported.
        /// </param>
        /// <param name="underlying">
        /// The underlying instrumentation handler.
        /// </param>
        public TaggingExceptionsInstrumentation(
            TaggingPropertySource propertySource,
            IExceptionsInstrumentation underlying)
        {
            this.propertySource = propertySource;
            this.underlying = underlying;
        }

        /// <inheritdoc/>
        public void ReportException(Exception x, AdditionalInstrumentationDetail? additionalDetail = null)
        {
            this.underlying.ReportException(x, this.propertySource.GetDetail<T>(additionalDetail));
        }
    }
}