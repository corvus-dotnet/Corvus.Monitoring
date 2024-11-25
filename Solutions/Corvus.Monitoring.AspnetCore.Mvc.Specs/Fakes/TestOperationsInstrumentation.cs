// <copyright file="TestOperationsInstrumentation.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Specs.Fakes
{
    using Corvus.Monitoring.Instrumentation;

    public class TestOperationsInstrumentation : IOperationsInstrumentation
    {
        private TestOperationsInstrumentation()
        {
        }

        public static TestOperationsInstrumentation Instance { get; } = new TestOperationsInstrumentation();

        public List<TestOperationInstance> StartedOperations { get; } = [];

        public IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail? additionalDetail = null)
        {
            var operation = new TestOperationInstance(name, additionalDetail);
            this.StartedOperations.Add(operation);
            return operation;
        }
    }
}