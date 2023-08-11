// <copyright file="TestOperationInstance.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Specs.Fakes
{
    using System.Collections.ObjectModel;
    using Corvus.Monitoring.Instrumentation;

    public class TestOperationInstance : IOperationInstance
    {
        public TestOperationInstance(string name, AdditionalInstrumentationDetail? additionalDetail = null)
        {
            this.Name = name;

            if (additionalDetail?.MetricsIfPresent != null)
            {
                foreach (KeyValuePair<string, double> metric in additionalDetail.Metrics)
                {
                    this.Metrics.Add(metric);
                }
            }

            if (additionalDetail?.PropertiesIfPresent != null)
            {
                foreach (KeyValuePair<string, string> property in additionalDetail.Properties)
                {
                    this.Properties.Add(property);
                }
            }
        }

        public IDictionary<string, double> Metrics { get; private set; } = new Dictionary<string, double>();

        public IDictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();

        public string Name { get; }

        public bool IsDisposed { get; private set; } = false;

        public void AddOperationMetric(string name, double value)
        {
            this.Metrics.Add(name, value);
        }

        public void AddOperationProperty(string name, string value)
        {
            this.Properties.Add(name, value);
        }

        public void Dispose()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.Name);
            }

            this.IsDisposed = true;

            this.Metrics = new ReadOnlyDictionary<string, double>(this.Metrics);
            this.Properties = new ReadOnlyDictionary<string, string>(this.Properties);
        }
    }
}