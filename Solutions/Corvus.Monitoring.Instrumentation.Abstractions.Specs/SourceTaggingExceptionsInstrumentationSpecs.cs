namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using NUnit.Framework;

    public class SourceTaggingExceptionsInstrumentationSpecs : SourceTaggingSpecsBase
    {
        private const string ExistingDetailKey = "Edk";
        private const string ExistingDetailValue = "Edv";

        [Test]
        public void WhenExceptionWithNoAdditionalInfoIsSentItShouldIncludeSource()
        {
            SourceTaggingTestContext.ExceptionDetail ex1Detail = this.ThrowReportAndCatchException1();

            IExceptionsInstrumentation<TestType2> exi2 = this.Context.GetExceptionsInstrumentation<TestType2>();
            var ex2 = new Exception("Another");
            exi2.ReportException(ex2);

            Assert.AreEqual(2, this.Context.Exceptions.Count, "Exception count");
            SourceTaggingTestContext.ExceptionDetail ex2Detail = this.Context.Exceptions[1];
            Assert.AreSame(ex2, ex2Detail.Exception, "Exception (2)");

            Assert.AreEqual(1, ex1Detail.AdditionalDetail?.Properties.Count, "Property count (1)");
            Assert.AreEqual(typeof(TestType1).FullName, ex1Detail.AdditionalDetail.Properties[this.Context.SourcePropertyName], "Source (1)");

            Assert.AreEqual(1, ex2Detail.AdditionalDetail?.Properties.Count, "Property count (2)");
            Assert.AreEqual(typeof(TestType2).FullName, ex2Detail.AdditionalDetail.Properties[this.Context.SourcePropertyName], "Source (1)");
        }

        [Test]
        public void WhenExceptionWithAdditionalInfoWithNoPropertiesOrMetricsIsSentItShouldIncludeSource()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail();
            SourceTaggingTestContext.ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(1, exDetail.AdditionalDetail?.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, exDetail.AdditionalDetail.Properties[this.Context.SourcePropertyName], "Source");
        }

        [Test]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldIncludeSourceAndSuppliedProperties()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } }
            };
            SourceTaggingTestContext.ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(2, exDetail.AdditionalDetail?.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, exDetail.AdditionalDetail.Properties[this.Context.SourcePropertyName], "Source");
            Assert.AreEqual(ExistingDetailValue, exDetail.AdditionalDetail.Properties[ExistingDetailKey], "ExistingDetailKey");
        }

        [Test]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldPassMetricsThrough()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } }
            };
            SourceTaggingTestContext.ExceptionDetail opDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreSame(suppliedDetail.Metrics, opDetail.AdditionalDetail.Metrics);
        }

        [Test]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldNotModifyInputDictionaries()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } }
            };
            this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(1, suppliedDetail.Properties.Count);
            Assert.AreEqual(ExistingDetailValue, suppliedDetail.Properties[ExistingDetailKey], "ExistingDetailKey");

            Assert.AreEqual(1, suppliedDetail.Metrics.Count);
            Assert.AreEqual(42.0, suppliedDetail.Metrics["m1"], "Metric m1");
        }

        [Test]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndNoMetricsIsSentItShouldIncludeSource()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } }
            };
            SourceTaggingTestContext.ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.AreEqual(2, exDetail.AdditionalDetail?.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, exDetail.AdditionalDetail.Properties[this.Context.SourcePropertyName], "Source");
            Assert.AreEqual(ExistingDetailValue, exDetail.AdditionalDetail.Properties[ExistingDetailKey], "ExistingDetailKey");
        }

        [Test]
        public void WhenExceptionWithAdditionalInfoWithPropertiesAndNoMetricsIsSentMetricsShouldBeNull()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } }
            };
            SourceTaggingTestContext.ExceptionDetail exDetail = this.ThrowReportAndCatchException1(suppliedDetail);

            Assert.IsNull(exDetail.AdditionalDetail.MetricsIfPresent);
        }


        private SourceTaggingTestContext.ExceptionDetail ThrowReportAndCatchException1(
            AdditionalInstrumentationDetail additionalDetail = null)
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
            SourceTaggingTestContext.ExceptionDetail exDetail = this.Context.Exceptions[0];

            Assert.AreSame(ax, exDetail.Exception, "Exception");
            return exDetail;
        }
    }
}