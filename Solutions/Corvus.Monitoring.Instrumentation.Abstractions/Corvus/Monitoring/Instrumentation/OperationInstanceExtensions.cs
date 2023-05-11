// <copyright file="OperationInstanceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;

    /// <summary>
    /// Extension methods for <see cref="IOperationInstance"/>s.
    /// </summary>
    public static class OperationInstanceExtensions
    {
        /// <summary>
        /// Adds a property to the additional data associated with this request.
        /// </summary>
        /// <param name="operation">The operation to add detail to.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void AddOperationProperty(this IOperationInstance operation, string name, object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            operation.AddOperationProperty(name, value.ToString()!);
        }
    }
}