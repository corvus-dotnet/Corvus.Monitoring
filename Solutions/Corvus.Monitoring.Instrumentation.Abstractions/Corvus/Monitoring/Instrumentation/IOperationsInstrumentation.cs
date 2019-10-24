// <copyright file="IOperationsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    /// <summary>
    /// Enables reporting of operations for monitoring and diagnostic purposes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This provides a way for components to report the start and end of logical operations in a
    /// way that can be made visible to monitoring systems such as Application Insights without
    /// needing to take a direct dependency on those systems' client libraries.
    /// </para>
    /// </remarks>
    public interface IOperationsInstrumentation
    {
        /// <summary>
        /// Reports that a new operation has started.
        /// </summary>
        /// <param name="name">A short, descriptive operation name.</param>
        /// <param name="otherDetail">Optional additional properties and metrics.</param>
        /// <returns>
        /// An <see cref="IOperationInstance"/> that must be disposed once this operation
        /// completes.
        /// </returns>
        IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail otherDetail = null);
    }
}
