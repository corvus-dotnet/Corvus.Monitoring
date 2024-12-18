// <copyright file="SourceTaggingOperationsInstrumentationSpecs.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System.Linq;
    using Corvus.Monitoring.Instrumentation.Abstractions.Specs.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SourceTaggingOperationsInstrumentationSpecs : SourceTaggingSpecsBase
    {
        private const string OpName1 = "Op1";
        private const string OpName2 = "Op2";
        private const string ExistingDetailKey = "Edk";
        private const string ExistingDetailValue = "Edv";

        [TestMethod]
        public void WhenOperationWithNoAdditionalInfoIsSentItShouldIncludeSource()
        {
            OperationDetail op1Detail = this.ReportOperation1();

            IOperationsInstrumentation<TestType2> opi2 = this.Context.GetOperationsInstrumentation<TestType2>();
            using (opi2.StartOperation(OpName2))
            {
            }

            Assert.AreEqual(2, this.Context.Operations.Count, "Operation count");
            OperationDetail op2Detail = this.Context.Operations[1];
            Assert.AreEqual(OpName2, op2Detail.Name, "Name (2)");

            Assert.AreEqual(1, op1Detail.AdditionalDetail!.Properties.Count, "Property count (1)");
            Assert.AreEqual(typeof(TestType1).FullName, op1Detail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source (1)");

            Assert.AreEqual(1, op2Detail.AdditionalDetail!.Properties.Count, "Property count (2)");
            Assert.AreEqual(typeof(TestType2).FullName, op2Detail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source (1)");
        }

        [TestMethod]
        public void WhenOperationWithAdditionalInfoWithNoPropertiesOrMetricsIsSentItShouldIncludeSource()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail();
            OperationDetail opDetail = this.ReportOperation1(suppliedDetail);

            Assert.AreEqual(1, opDetail.AdditionalDetail!.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, opDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source");
        }

        [TestMethod]
        public void WhenOperationWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldIncludeSourceAndSuppliedProperties()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } },
            };
            OperationDetail opDetail = this.ReportOperation1(suppliedDetail);

            Assert.AreEqual(2, opDetail.AdditionalDetail!.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, opDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source");
            Assert.AreEqual(ExistingDetailValue, opDetail.AdditionalDetail!.Properties[ExistingDetailKey], "ExistingDetailKey");
        }

        [TestMethod]
        public void WhenOperationWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldPassMetricsThrough()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } },
            };
            OperationDetail opDetail = this.ReportOperation1(suppliedDetail);

            Assert.AreSame(suppliedDetail.Metrics, opDetail.AdditionalDetail!.Metrics);
        }

        [TestMethod]
        public void WhenOperationWithAdditionalInfoWithPropertiesAndMetricsIsSentItShouldNotModifyInputDictionaries()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
                Metrics = { { "m1", 42.0 } },
            };
            this.ReportOperation1(suppliedDetail);

            Assert.AreEqual(1, suppliedDetail.Properties.Count);
            Assert.AreEqual(ExistingDetailValue, suppliedDetail.Properties[ExistingDetailKey], "ExistingDetailKey");

            Assert.AreEqual(1, suppliedDetail.Metrics.Count);
            Assert.AreEqual(42.0, suppliedDetail.Metrics["m1"], "Metric m1");
        }

        [TestMethod]
        public void WhenOperationWithAdditionalInfoWithPropertiesAndNoMetricsIsSentItShouldIncludeSource()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
            };
            OperationDetail opDetail = this.ReportOperation1(suppliedDetail);

            Assert.AreEqual(2, opDetail.AdditionalDetail!.Properties.Count, "Property count");
            Assert.AreEqual(typeof(TestType1).FullName, opDetail.AdditionalDetail!.Properties[this.Context.SourcePropertyName], "Source");
            Assert.AreEqual(ExistingDetailValue, opDetail.AdditionalDetail!.Properties[ExistingDetailKey], "ExistingDetailKey");
        }

        [TestMethod]
        public void WhenOperationWithAdditionalInfoWithPropertiesAndNoMetricsIsSentMetricsShouldBeNull()
        {
            var suppliedDetail = new AdditionalInstrumentationDetail
            {
                Properties = { { ExistingDetailKey, ExistingDetailValue } },
            };
            OperationDetail opDetail = this.ReportOperation1(suppliedDetail);

            Assert.IsNull(opDetail.AdditionalDetail!.MetricsIfPresent);
        }

        [TestMethod]
        public void WhenOperationHasNotYetFinishedUnderlyingOperationShouldNotBeDisposed()
        {
            IOperationsInstrumentation<TestType1> opi1 = this.Context.GetOperationsInstrumentation<TestType1>();

            using (opi1.StartOperation(OpName1))
            {
                OperationDetail opDetail = this.Context.Operations[0];
                Assert.IsFalse(opDetail.IsDisposed);
            }
        }

        [TestMethod]
        public void WhenOperationHasFinishedUnderlyingOperationShouldBeDisposed()
        {
            IOperationsInstrumentation<TestType1> opi1 = this.Context.GetOperationsInstrumentation<TestType1>();

            using (opi1.StartOperation(OpName1))
            {
            }

            OperationDetail opDetail = this.Context.Operations[0];
            Assert.IsTrue(opDetail.IsDisposed);
        }

        [TestMethod]
        public void WhenFurtherDetailsSuppliedTheyShouldBePassedStraightThrough()
        {
            IOperationsInstrumentation<TestType1> opi1 = this.Context.GetOperationsInstrumentation<TestType1>();

            var furtherDetail1 = new AdditionalInstrumentationDetail
            {
                Properties = { { "Edk1", "Edv1" } },
                Metrics = { { "m1", 42.0 } },
            };
            var furtherDetail2 = new AdditionalInstrumentationDetail
            {
                Properties = { { "Edk2", "Edv2" } },
                Metrics = { { "m2", 99.0 } },
            };

            using (IOperationInstance op = opi1.StartOperation(OpName1))
            {
                op.AddOperationDetail(furtherDetail1);
                op.AddOperationDetail(furtherDetail2);
            }

            OperationDetail opDetail = this.Context.Operations[0];

            // Check they've not changed
            Assert.AreEqual(2, opDetail.FurtherDetails.Properties.Count, "FurtherDetails.Properties Count");
            Assert.AreEqual(2, opDetail.FurtherDetails.Metrics.Count, "FurtherDetails.Metrics Count");

            Assert.AreEqual("Edv1", opDetail.FurtherDetails.Properties["Edk1"]);
            Assert.AreEqual("Edv2", opDetail.FurtherDetails.Properties["Edk2"]);
            Assert.AreEqual(42.0, opDetail.FurtherDetails.Metrics["m1"]);
            Assert.AreEqual(99.0, opDetail.FurtherDetails.Metrics["m2"]);
        }

        private OperationDetail ReportOperation1(AdditionalInstrumentationDetail? suppliedDetail = null)
        {
            IOperationsInstrumentation<TestType1> opi1 = this.Context.GetOperationsInstrumentation<TestType1>();

            using (opi1.StartOperation(OpName1, suppliedDetail))
            {
            }

            Assert.AreEqual(1, this.Context.Operations.Count, "Operation count");
            OperationDetail opDetail = this.Context.Operations[0];

            Assert.AreEqual(OpName1, opDetail.Name, "Name");
            return opDetail;
        }
    }
}