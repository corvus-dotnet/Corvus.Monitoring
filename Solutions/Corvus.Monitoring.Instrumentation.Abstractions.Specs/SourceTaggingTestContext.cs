namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public class SourceTaggingTestContext : IDisposable
    {
        private readonly ServiceProvider serviceProvider;
        private readonly List<OperationDetail> operations = new List<OperationDetail>();
        private readonly List<ExceptionDetail> exceptions = new List<ExceptionDetail>();

        public SourceTaggingTestContext()
        {
            var services = new ServiceCollection();
            services
                .AddInstrumentationSourceTagging()
                .AddSingleton<IOperationsInstrumentation>(new OperationsTarget(this))
                .AddSingleton<IExceptionsInstrumentation>(new ExceptionsTarget(this));
            this.serviceProvider = services.BuildServiceProvider();
        }

        public string SourcePropertyName => "Category"; // TBD: parameterise tests so we can vary this

        public IReadOnlyList<OperationDetail> Operations => this.operations;

        public IReadOnlyList<ExceptionDetail> Exceptions => this.exceptions;

        public IOperationsInstrumentation<T> GetOperationsInstrumentation<T>() => this.serviceProvider.GetRequiredService<IOperationsInstrumentation<T>>();
        
        public IExceptionsInstrumentation<T> GetExceptionsInstrumentation<T>() => this.serviceProvider.GetRequiredService<IExceptionsInstrumentation<T>>();

        public void Dispose()
        {
            this.serviceProvider.Dispose();
        }

        private class OperationsTarget : IOperationsInstrumentation
        {
            private readonly SourceTaggingTestContext context;

            public OperationsTarget(SourceTaggingTestContext context)
            {
                this.context = context;
            }

            public IOperationInstance StartOperation(string name, AdditionalInstrumentationDetail additionalDetail = null)
            {
                var op = new OperationDetail(name, additionalDetail);
                this.context.operations.Add(op);
                return op;
            }
        }

        private class ExceptionsTarget : IExceptionsInstrumentation
        {
            private readonly SourceTaggingTestContext context;

            public ExceptionsTarget(SourceTaggingTestContext context)
            {
                this.context = context;
            }

            public void ReportException(Exception x, AdditionalInstrumentationDetail additionalDetail = null)
            {
                this.context.exceptions.Add(new ExceptionDetail(x, additionalDetail));
            }
        }

        public class OperationDetail : IOperationInstance
        {
            private readonly List<AdditionalInstrumentationDetail> furtherDetails = new List<AdditionalInstrumentationDetail>();

            public OperationDetail(
                string name,
                AdditionalInstrumentationDetail additionalDetail)
            {
                this.Name = name;
                this.AdditionalDetail = additionalDetail;
            }

            public string Name { get; }

            public AdditionalInstrumentationDetail AdditionalDetail { get; }
            public IReadOnlyList<AdditionalInstrumentationDetail> FurtherDetails => this.furtherDetails;

            public bool IsDisposed { get; private set; }

            void IOperationInstance.AddOperationDetail(AdditionalInstrumentationDetail detail)
            {
                this.furtherDetails.Add(detail);
            }

            void IDisposable.Dispose()
            {
                this.IsDisposed = true;
            }
        }

        public class ExceptionDetail
        {
            public ExceptionDetail(
                Exception x,
                AdditionalInstrumentationDetail additionalDetail)
            {
                this.Exception = x;
                this.AdditionalDetail = additionalDetail;
            }

            public Exception Exception { get; }
            public AdditionalInstrumentationDetail AdditionalDetail { get; }
        }
    }
}