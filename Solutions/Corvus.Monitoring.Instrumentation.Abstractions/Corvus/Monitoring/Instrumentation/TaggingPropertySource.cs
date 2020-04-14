// <copyright file="TaggingPropertySource.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    using System;
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
        internal AdditionalInstrumentationDetail GetDetail<TSource>(AdditionalInstrumentationDetail? additionalDetail)
        {
            return new AdditionalInstrumentationDetail(
                additionalDetail?.PropertiesIfPresent == null ? null : new Dictionary<string, string>(additionalDetail.PropertiesIfPresent),
                additionalDetail?.MetricsIfPresent)
            {
                Properties =
                {
                    { this.PropertyName, GetTypeDisplayName(typeof(TSource)) },
                },
            };
        }

        /// <summary>
        /// Ensure type name isn't bonkers when generic.
        /// </summary>
        /// <param name="t">The type for which to produce a displayable name.</param>
        /// <returns>A string representing the type.</returns>
        /// <remarks>
        /// <para>
        /// Although <see cref="Type.FullName"/> works well to identify non-generic types, there's a problem using
        /// it as a display name for generic types: it produces more or less unreadable strings. This is because the
        /// full name uses assembly-qualified names for type arguments.
        /// </para>
        /// <para>
        /// The effect is that something as simple as <c>IEnumerable&lt;int&gt;</c> becomes
        /// <c>System.Collections.Generic.IEnumerable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]</c>.
        /// </para>
        /// <para>
        /// While this may be useful for guaranteeing full fidelity (although it seems odd to do that when the main
        /// type itself is not also qualified) it just makes things unreadable when these strings appear in telemetry.
        /// So we need a simpler representation. For now, we're just stripping off the type arguments, so the preceding
        /// example would become <c>System.Collections.Generic.IEnumerable</c>.
        /// </para>
        /// </remarks>
        private static string GetTypeDisplayName(Type t)
        {
            if (t.IsGenericType)
            {
                string nameWithArity = t.GetGenericTypeDefinition().FullName;
                int arityMarkerIndex = nameWithArity.LastIndexOf('`');
                return nameWithArity.Substring(0, arityMarkerIndex);
            }

            return t.FullName;
        }
    }
}