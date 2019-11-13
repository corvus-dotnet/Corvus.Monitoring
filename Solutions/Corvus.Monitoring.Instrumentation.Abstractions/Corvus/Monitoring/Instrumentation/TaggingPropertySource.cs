// <copyright file="TaggingPropertySource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides properties for all the Tagging instrumentation implementations.
    /// </summary>
    internal class TaggingPropertySource
    {
        /// <summary>
        /// Creates a <see cref="TaggingPropertySource"/>.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property in which to report the source type.
        /// </param>
        public TaggingPropertySource(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the name of the property that will be used to report the source type.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Returns instrumentation detail augmented with the property source details.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="additionalDetail">
        /// Other detail passed by the client application, or null if none was supplied.
        /// </param>
        /// <returns>The fully populated detail.</returns>
        internal AdditionalInstrumentationDetail GetDetail<TSource>(AdditionalInstrumentationDetail additionalDetail)
        {
            return new AdditionalInstrumentationDetail(
                additionalDetail?.PropertiesIfPresent == null ? null : new Dictionary<string, string>(additionalDetail.PropertiesIfPresent),
                additionalDetail?.MetricsIfPresent)
            {
                Properties =
                {
                    { this.PropertyName, typeof(TSource).FullName },
                },
            };
        }
    }
}