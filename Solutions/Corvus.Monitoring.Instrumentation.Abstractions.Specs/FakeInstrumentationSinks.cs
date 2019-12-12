namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using System.Collections.Generic;
    using Corvus.Monitoring.Instrumentation.Abstractions.Specs.Fakes;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides fake implementations of the generic and non-generic instrumentation interfaces to
    /// tests that need them.
    /// </summary>
    public class FakeInstrumentationSinks
    {
        private readonly List<OperationDetail> operations = new List<OperationDetail>();
        private readonly List<ExceptionDetail> exceptions = new List<ExceptionDetail>();
        private readonly List<OperationDetail> genericOperations = new List<OperationDetail>();
        private readonly List<ExceptionDetail> genericExceptions = new List<ExceptionDetail>();

        /// <summary>
        /// Gets a list of the operations supplied to the underyling (non-generic) fake
        /// <see cref="IOperationsInstrumentation"/> implementation.
        /// </summary>
        public IReadOnlyList<OperationDetail> Operations => this.operations;

        /// <summary>
        /// Gets a list of the exceptions supplied to the underyling (non-generic) fake
        /// <see cref="IExceptionsInstrumentation"/> implementation.
        /// </summary>
        public IReadOnlyList<ExceptionDetail> Exceptions => this.exceptions;

        /// <summary>
        /// Gets a list of the operations supplied to the fake generic
        /// <see cref="IOperationsInstrumentation{T}"/> implementation.
        /// </summary>
        public IReadOnlyList<OperationDetail> GenericOperations => this.genericOperations;

        /// <summary>
        /// Gets a list of the operations supplied to the fake generic
        /// <see cref="IExceptionsInstrumentation{T}"/> implementation.
        /// </summary>
        public IReadOnlyList<ExceptionDetail> GenericExceptions => this.genericExceptions;

        public void AddNonGenericImplementationsToServices(IServiceCollection services)
        {
            services
                .AddSingleton(this)
                .AddSingleton<IOperationsInstrumentation, OperationsTarget>()
                .AddSingleton<IExceptionsInstrumentation, ExceptionsTarget>();
        }

        public void AddGenericImplementationsToServices(IServiceCollection services)
        {
            services
                .AddSingleton(this)
                .AddSingleton(typeof(IOperationsInstrumentation<>), typeof(GenericOperationsTarget<>))
                .AddSingleton(typeof(IExceptionsInstrumentation<>), typeof(GenericExceptionsTarget<>));
        }

        private class OperationsTargetBase<T> : IOperationsInstrumentation<T>
        {
            private readonly List<OperationDetail> operationsList;

            public OperationsTargetBase(List<OperationDetail> operationsList)
            {
                this.operationsList = operationsList;
            }

            public IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail additionalDetail = null)
            {
                var op = new OperationDetail(name, additionalDetail);
                this.operationsList.Add(op);
                return op;
            }
        }

        private class OperationsTarget : OperationsTargetBase<object>
        {
            public OperationsTarget(FakeInstrumentationSinks parent) : base(parent.operations) { }
        }

        private class GenericOperationsTarget<T> : OperationsTargetBase<T>
        {
            public GenericOperationsTarget(FakeInstrumentationSinks parent) : base(parent.genericOperations) { }
        }

        private class ExceptionsTargetBase<T> : IExceptionsInstrumentation<T>
        {
            private readonly List<ExceptionDetail> exceptionsList;

            public ExceptionsTargetBase(List<ExceptionDetail> exceptionsList)
            {
                this.exceptionsList = exceptionsList;
            }

            public void ReportException(Exception x, AdditionalInstrumentationDetail additionalDetail = null)
            {
                this.exceptionsList.Add(new ExceptionDetail(x, additionalDetail));
            }
        }

        private class ExceptionsTarget : ExceptionsTargetBase<object>
        {
            public ExceptionsTarget(FakeInstrumentationSinks parent) : base(parent.exceptions) { }
        }

        private class GenericExceptionsTarget<T> : ExceptionsTargetBase<T>
        {
            public GenericExceptionsTarget(FakeInstrumentationSinks parent) : base(parent.genericExceptions) { }
        }
    }
}