// <copyright file="TaggingOperationsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    /// <summary>
    /// Implementation of <see cref="IOperationsInstrumentation{T}"/> that adds source information
    /// based on the type argument.
    /// </summary>
    /// <typeparam name="T">The source type.</typeparam>
    internal class TaggingOperationsInstrumentation<T> : IOperationsInstrumentation<T>
    {
        private readonly TaggingPropertySource propertySource;
        private readonly IOperationsInstrumentation underlying;

        /// <summary>
        /// Creates a <see cref="TaggingOperationsInstrumentation{T}"/>.
        /// </summary>
        /// <param name="propertySource">
        /// Determines the property through which the source will be reported.
        /// </param>
        /// <param name="underlying">
        /// The underlying instrumentation handler.
        /// </param>
        public TaggingOperationsInstrumentation(
            TaggingPropertySource propertySource,
            IOperationsInstrumentation underlying)
        {
            this.propertySource = propertySource;
            this.underlying = underlying;
        }

        /// <inheritdoc/>
        public IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail? additionalDetail = null)
        {
            return this.underlying.StartOperation(name, this.propertySource.GetDetail<T>(additionalDetail));
        }
    }
}