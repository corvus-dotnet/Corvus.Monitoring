// <copyright file="OperationInstanceExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;
    using System.Collections.Generic;

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

            operation.AddOperationProperty(name, value.ToString());
        }

        /// <summary>
        /// Adds one or more entries to the additional data associated with this request.
        /// </summary>
        /// <param name="operation">The operation to add detail to.</param>
        /// <param name="detail">The detail to add.</param>
        public static void AddOperationDetail(this IOperationInstance operation, AdditionalInstrumentationDetail detail)
        {
            if (detail.Properties != null)
            {
                foreach (KeyValuePair<string, string> property in detail.Properties)
                {
                    operation.AddOperationProperty(property.Key, property.Value);
                }
            }

            if (detail.Metrics != null)
            {
                foreach (KeyValuePair<string, double> metric in detail.Metrics)
                {
                    operation.AddOperationMetric(metric.Key, metric.Value);
                }
            }
        }
    }
}