// <copyright file="IOperationInstance.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;

    /// <summary>
    /// A logical operation in progress.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When you call <see cref="IOperationsInstrumentation.StartOperation(string, AdditionalInstrumentationDetail)"/>,
    /// it returns an implementation of this interface. You should dispose it when the operation
    /// completes. So the normal usage model would look something like this:
    /// </para>
    /// <code>
    /// <![CDATA[
    /// public async IActionResult DoSomething()
    /// {
    ///     using (this.operationsInstrumentation.StartOperation(nameof(DoSomething)))
    ///     {
    ///         await this.something.DoAsync();
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </remarks>
    public interface IOperationInstance : IDisposable
    {
        /// <summary>
        /// Adds a property to the additional data associated with this request.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        void AddOperationProperty(string name, string value);

        /// <summary>
        /// Adds a metric value to the additional data associated with this request.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        void AddOperationMetric(string name, double value);
    }
}