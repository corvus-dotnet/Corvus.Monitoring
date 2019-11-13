// <copyright file="IExceptionsInstrumentation{T}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    /// <summary>
    /// Enables reporting of exceptions for monitoring and diagnostic purposes.
    /// </summary>
    /// <typeparam name="T">
    /// Identifies the source of the instrumentation data. If a type obtains this interface
    /// through DI, it will typically supply itself as the type argument.
    /// </typeparam>
    public interface IExceptionsInstrumentation<T> : IExceptionsInstrumentation
    {
    }
}