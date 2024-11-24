// <copyright file="SourceTaggingExceptionsInstrumentationSpecs.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using System.Collections.Generic;
    using Corvus.Monitoring.Instrumentation.Abstractions.Specs.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SourceTaggingExceptionsInstrumentationSpecs : SourceTaggingSpecsBase
    {
        private const string ExistingDetailKey = "Edk";
        private const string ExistingDetailValue = "Edv";

        [TestMethod]
        public void WhenExceptionWithNoAdditionalInfoIsSentItShouldIncludeSource()
        {
            ExceptionDetail ex1Detail = this.ThrowReportAndCatchException1();

            IExceptionsInstrumentation<TestType2> exi2 = this.Context.GetExceptionsInstrumentation<TestType2>();
            Exception ex2 = new("Another");
            exi2.ReportException(ex2);

            Assert.AreEqual(2, this.Context.Exceptions.Count, "Exception count");
            ExceptionDetail ex2Detail = this.Context.Exceptions[1];
            Assert.AreSame(ex2, ex2Detail.Exception, "Exception (2)");

            Assert.AreEqual(1, ex1Detail.AdditionalDetail!.Properties.Count, "Property count (1)");
            Assert.AreEqual(typeof(TestType1).FullName, ex1Detail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source (1)");

            Assert.AreEqual(1, ex2Detail.AdditionalDetail!.Properties.Count, "Property count (2)");
            Assert.AreEqual(typeof(TestType2).FullName, ex2Detail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source (1)");
        }

        [TestMethod]
        public void WhenSourceTypeIsGenericSourceDoesNotIncludeTypeArguments()
        {
            IExceptionsInstrumentation<GenericTestType<string, List<int>>> exig = this.Context.GetExceptionsInstrumentation<GenericTestType<string, List<int>>>();
            Exception ex = new("Another");
            exig.ReportException(ex);

            Assert.AreEqual(1, this.Context.Exceptions.Count, "Exception count");
            ExceptionDetail exDetail = this.Context.Exceptions[0];
            Assert.AreSame(ex, exDetail.Exception, "Exception (3)");

            Assert.AreEqual($"{typeof(SourceTaggingSpecsBase).Namespace}.{nameof(SourceTaggingSpecsBase)}+{nameof(GenericTestType<string, List<int>>)}", exDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source (2)");
        }

        [TestMethod]
        public void WhenExceptionWithAdditionalInfoWithNoPropertiesOrMetricsIsSentItShouldIncludeSource()
        {
            AdditionalInstrumentationDetail suppliedDetail = new();
            ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(1, exDetail.AdditionalDetail!.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, exDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source");
        }

        [TestMethod]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldIncludeSourceAndSuppliedProperties()
        {
            AdditionalInstrumentationDetail suppliedDetail = new()
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } },
            };

            ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(2, exDetail.AdditionalDetail!.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, exDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source");
            Assert.AreEqual(ExistingDetailValue, exDetail.AdditionalDetail!.Properties[ExistingDetailKey], "ExistingDetailKey");
        }

        [TestMethod]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldPassMetricsThrough()
        {
            AdditionalInstrumentationDetail suppliedDetail = new()
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } },
            };
            ExceptionDetail opDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreSame(suppliedDetail.Metrics, opDetail.AdditionalDetail!.Metrics);
        }

        [TestMethod]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldNotModifyInputDictionaries()
        {
            AdditionalInstrumentationDetail suppliedDetail = new()
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } },
            };
            this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(1, suppliedDetail.Properties.Count);
            Assert.AreEqual(ExistingDetailValue, suppliedDetail.Properties[ExistingDetailKey], "ExistingDetailKey");

            Assert.AreEqual(1, suppliedDetail.Metrics.Count);
            Assert.AreEqual(42.0, suppliedDetail.Metrics["m1"], "Metric m1");
        }

        [TestMethod]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndNoMetricsIsSentItShouldIncludeSource()
        {
            AdditionalInstrumentationDetail suppliedDetail = new()
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
            };
            ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(2, exDetail.AdditionalDetail!.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, exDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source");
            Assert.AreEqual(ExistingDetailValue, exDetail.AdditionalDetail!.Properties[ExistingDetailKey], "ExistingDetailKey");
        }

        [TestMethod]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndNoMetricsIsSentMetricsShouldBeNull()
        {
            AdditionalInstrumentationDetail suppliedDetail = new()
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
            };
            ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.IsNull(exDetail.AdditionalDetail!.MetricsIfPresent);
        }

        private ExceptionDetail ThrowReportAndCatchException1(AdditionalInstrumentationDetail? additionalDetail = null)
        {
            IExceptionsInstrumentation<TestType1> exi1 = this.Context.GetExceptionsInstrumentation<TestType1>();

            ArgumentException ax;
            try
            {
                throw new ArgumentException("That was never 5 minutes!", "duration");
            }
            catch (ArgumentException x)
            {
                ax = x;
                exi1.ReportException(x, additionalDetail);
            }

            Assert.AreEqual(1, this.Context.Exceptions.Count, "Exception count");
            ExceptionDetail exDetail = this.Context.Exceptions[0];

            Assert.AreSame(ax, exDetail.Exception, "Exception");
            return exDetail;
        }
    }
}