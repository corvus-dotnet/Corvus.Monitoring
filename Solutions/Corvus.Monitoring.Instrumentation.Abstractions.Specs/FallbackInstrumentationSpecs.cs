// <copyright file="FallbackInstrumentationSpecs.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.Instrumentation.Abstractions.Specs
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    /// <summary>
    /// Ensure that fallback 'do nothing' instrumentation implementations are available by default,
    /// but that they don't interfere when apps decide to take control of their own configuration.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There are essentially two goals here.
    /// </para>
    /// <para>
    /// First, we want to ensure that components are free to take a dependency on Corvus.Monitoring
    /// without imposing a burden on applications that use those components. For example, we want
    /// Menes to be able to use it, and for that decision not to mean that anyone using Menes is
    /// forced to make a bunch of decisions about how to configure instrumentation and telemetry
    /// before they can get started. Menes should be able to call
    /// <see cref="InstrumentationServiceCollectionExtensions.AddInstrumentation"/> in its own DI
    /// initialization, with the effect that applications using Menes can be written in blissful
    /// ignorance of the very existence of Corvus.Monitoring.
    /// </para>
    /// <para>
    /// Second, we want applications that are fully aware of Corvus.Monitoring to be able to fully
    /// control it even when they are using some other framework that called
    /// <see cref="InstrumentationServiceCollectionExtensions.AddInstrumentation"/>. The fact that
    /// a component that depends on Corvus.Monitoring has ensured that a bunch of default
    /// implementations are automatically in place must not get in the way of an application's
    /// decision to configure Corvus.Monitoring in any way they please. The fallbacks need to
    /// ensure that they get well out of the way when they are not needed.
    /// </para>
    /// </remarks>
    public class FallbackInstrumentationSpecs
    {
        private ServiceCollection services;
        private IExceptionsInstrumentation<TestSource> exceptionsInstrumentation;
        private IOperationsInstrumentation<TestSource> operationsInstrumentation;
        private FakeInstrumentationSinks fakeInstrumentationSinks;

        [SetUp]
        public void Setup()
        {
            this.services = new ServiceCollection();
            this.fakeInstrumentationSinks = new FakeInstrumentationSinks();
        }

        [Test]
        public void WhenAddInstrumentationCalledAloneGenericImplementationsCanBeResolvedAndUsedWithoutError()
        {
            this.services.AddInstrumentation();
            this.ResolveAndUseAll();

            // In this scenario, we expect everything to go into the bit bucket, which is why there
            // are no tests to check that the information went anywhere in this case.
        }

        [Test]
        public void WhenAddInstrumentationCalledAfterNonGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.fakeInstrumentationSinks.AddNonGenericImplementationsToServices(this.services);
            this.services.AddInstrumentation();

            this.ResolveAndCheckInstrumentationReachesSuppliedNonGenericImplementation();
        }

        [Test]
        public void WhenAddInstrumentationCalledBeforeNonGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.services.AddInstrumentation();
            this.fakeInstrumentationSinks.AddNonGenericImplementationsToServices(this.services);

            this.ResolveAndCheckInstrumentationReachesSuppliedNonGenericImplementation();
        }

        [Test]
        public void WhenAddInstrumentationCalledAfterGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.fakeInstrumentationSinks.AddGenericImplementationsToServices(this.services);
            this.services.AddInstrumentation();

            this.ResolveAndCheckInstrumentationReachesSuppliedGenericImplementation();
        }

        [Test]
        public void WhenAddInstrumentationCalledBeforeGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.services.AddInstrumentation();
            this.fakeInstrumentationSinks.AddGenericImplementationsToServices(this.services);

            this.ResolveAndCheckInstrumentationReachesSuppliedGenericImplementation();
        }

        private void ResolveAndCheckInstrumentationReachesSuppliedNonGenericImplementation()
        {
            this.ResolveAndUseAll();

            Assert.AreEqual(1, this.fakeInstrumentationSinks.Operations.Count, "Operations.Count");
            Assert.AreEqual(1, this.fakeInstrumentationSinks.Exceptions.Count, "Exceptions.Count");
        }

        private void ResolveAndCheckInstrumentationReachesSuppliedGenericImplementation()
        {
            this.ResolveAndUseAll();

            Assert.AreEqual(1, this.fakeInstrumentationSinks.GenericOperations.Count, "GenericOperations.Count");
            Assert.AreEqual(1, this.fakeInstrumentationSinks.GenericExceptions.Count, "GenericExceptions.Count");

            Assert.AreEqual(0, this.fakeInstrumentationSinks.Operations.Count, "Operations.Count");
            Assert.AreEqual(0, this.fakeInstrumentationSinks.Exceptions.Count, "Exceptions.Count");
        }

        private void ResolveAndUseAll()
        {
            this.ResolveAll();

            using (this.operationsInstrumentation.StartOperation("Foo"))
            {
            }

            this.exceptionsInstrumentation.ReportException(new Exception("Uh oh"));
        }

        private void ResolveAll()
        {
            IServiceProvider sp = this.services.BuildServiceProvider();

            // These two calls are to the method defined below. Roslynator gets confused and thinks they are local calls.
#pragma warning disable SA1101 // Prefix local calls with this
            Get(ref this.exceptionsInstrumentation);
            Get(ref this.operationsInstrumentation);
#pragma warning restore SA1101 // Prefix local calls with this

            void Get<T>(ref T target)
            {
                target = sp.GetRequiredService<T>();
            }
        }

        // This class exists purely to be the type argument for the various instrumentation
        // interfaces.
        private class TestSource
        {
        }
    }
}
