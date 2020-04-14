// <copyright file="NullOperationsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation
{
    /// <summary>
    /// Fallback do-nothing sink for operations instrumentation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This gets used when an application uses some library or framework that uses
    /// Corvus.Monitoring, but where the application doesn't want to do anything with the
    /// information.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This warning does not recognize usage through DI")]
    internal class NullOperationsInstrumentation : IOperationsInstrumentation
    {
        /// <inheritdoc/>
        public IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail? additionalDetail = null)
        {
            return Operation.Instance;
        }

        private class Operation : IOperationInstance
        {
            public static readonly Operation Instance = new Operation();

            public void AddOperationDetail(AdditionalInstrumentationDetail detail)
            {
            }

            public void Dispose()
            {
            }
        }
    }
}