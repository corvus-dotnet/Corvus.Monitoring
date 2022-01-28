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
        private ServiceCollection? services;
        private FakeInstrumentationSinks? fakeInstrumentationSinks;
        private IExceptionsInstrumentation<TestSource>? exceptionsInstrumentation;
        private IOperationsInstrumentation<TestSource>? operationsInstrumentation;

        private ServiceCollection Services
        {
            get => this.services ?? throw new InvalidOperationException($"The property {nameof(this.Services)} has not been set.");
            set => this.services = value ?? throw new ArgumentNullException();
        }

        private FakeInstrumentationSinks FakeInstrumentationSinks
        {
            get => this.fakeInstrumentationSinks ?? throw new InvalidOperationException($"The property {nameof(this.FakeInstrumentationSinks)} has not been set.");
            set => this.fakeInstrumentationSinks = value ?? throw new ArgumentNullException();
        }

        [SetUp]
        public void Setup()
        {
            this.Services = new ServiceCollection();
            this.FakeInstrumentationSinks = new FakeInstrumentationSinks();
        }

        [Test]
        public void WhenAddInstrumentationCalledAloneGenericImplementationsCanBeResolvedAndUsedWithoutError()
        {
            this.Services.AddInstrumentation();
            this.ResolveAndUseAll();

            // In this scenario, we expect everything to go into the bit bucket, which is why there
            // are no tests to check that the information went anywhere in this case.
        }

        [Test]
        public void WhenAddInstrumentationCalledAfterNonGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.FakeInstrumentationSinks.AddNonGenericImplementationsToServices(this.Services);
            this.Services.AddInstrumentation();

            this.ResolveAndCheckInstrumentationReachesSuppliedNonGenericImplementation();
        }

        [Test]
        public void WhenAddInstrumentationCalledBeforeNonGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.Services.AddInstrumentation();
            this.FakeInstrumentationSinks.AddNonGenericImplementationsToServices(this.Services);

            this.ResolveAndCheckInstrumentationReachesSuppliedNonGenericImplementation();
        }

        [Test]
        public void WhenAddInstrumentationCalledAfterGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.FakeInstrumentationSinks.AddGenericImplementationsToServices(this.Services);
            this.Services.AddInstrumentation();

            this.ResolveAndCheckInstrumentationReachesSuppliedGenericImplementation();
        }

        [Test]
        public void WhenAddInstrumentationCalledBeforeGenericImplementationsAddedPresuppliedImplementationsAreUsed()
        {
            this.Services.AddInstrumentation();
            this.FakeInstrumentationSinks.AddGenericImplementationsToServices(this.Services);

            this.ResolveAndCheckInstrumentationReachesSuppliedGenericImplementation();
        }

        private void ResolveAndCheckInstrumentationReachesSuppliedNonGenericImplementation()
        {
            this.ResolveAndUseAll();

            Assert.AreEqual(1, this.FakeInstrumentationSinks.Operations.Count, "Operations.Count");
            Assert.AreEqual(1, this.FakeInstrumentationSinks.Exceptions.Count, "Exceptions.Count");
        }

        private void ResolveAndCheckInstrumentationReachesSuppliedGenericImplementation()
        {
            this.ResolveAndUseAll();

            Assert.AreEqual(1, this.FakeInstrumentationSinks.GenericOperations.Count, "GenericOperations.Count");
            Assert.AreEqual(1, this.FakeInstrumentationSinks.GenericExceptions.Count, "GenericExceptions.Count");

            Assert.AreEqual(0, this.FakeInstrumentationSinks.Operations.Count, "Operations.Count");
            Assert.AreEqual(0, this.FakeInstrumentationSinks.Exceptions.Count, "Exceptions.Count");
        }

        private void ResolveAndUseAll()
        {
            this.ResolveAll();

            using (this.operationsInstrumentation!.StartOperation("Foo"))
            {
            }

            this.exceptionsInstrumentation!.ReportException(new Exception("Uh oh"));
        }

        private void ResolveAll()
        {
            IServiceProvider sp = this.Services.BuildServiceProvider();

            Get(ref this.exceptionsInstrumentation);
            Get(ref this.operationsInstrumentation);

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