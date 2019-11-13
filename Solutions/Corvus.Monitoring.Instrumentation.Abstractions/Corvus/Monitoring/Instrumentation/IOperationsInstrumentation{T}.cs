// <copyright file="IOperationsInstrumentation{T}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    /// <summary>
    /// Enables reporting of operations for monitoring and diagnostic purposes.
    /// </summary>
    /// <typeparam name="T">
    /// Identifies the source of the instrumentation data. If a type obtains this interface
    /// through DI, it will typically supply itself as the type argument.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// This provides a way for components to report the start and end of logical operations in a
    /// way that can be made visible to monitoring systems such as Application Insights without
    /// needing to take a direct dependency on those systems' client libraries.
    /// </para>
    /// </remarks>
    public interface IOperationsInstrumentation<T> : IOperationsInstrumentation
    {
    }
}