// <copyright file="ControllerOperationsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Specs.Steps
{
    using System.Net;
    using System.Security.Cryptography;
    using Corvus.Monitoring.AspnetCore.Mvc.Demo.Controllers;
    using Corvus.Monitoring.AspnetCore.Mvc.Specs.Bindings;
    using Corvus.Monitoring.AspnetCore.Mvc.Specs.Fakes;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ControllerOperationsSteps : Steps
    {
        private const string ExpectedActionExecutionOperationName 

        [Given("my controller implements the IHaveObservableActionMethods interface and has the ObservableActionMethodsAttribute applied to it")]
        public void GivenMyControllerImplementsTheIHaveObservableActionMethodsInterfaceAndHasTheObservableActionMethodsAttributeAppliedToIt()
        {
            // Nothing to do here.
        }

        [When("I make a web request that is fulfilled by that controller")]
        public async Task WhenIMakeAWebRequestThatIsFulfilledByThatController()
        {
            HttpResponseMessage result = await DemoSiteBindings.Client.GetAsync("/Home/Index/this-is-the-id");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Then("an IOperationInstance should have been created for the action execution")]
        public void ThenAnIOperationInstanceShouldHaveBeenCreatedForTheActionExecution()
        {
            TestOperationInstance? operation = this.GetOperationInstance($"{typeof(HomeController).FullName}.{nameof(HomeController.Index)}");
            Assert.IsNotNull(operation);
        }

        [Then("route data should have been added to the IOperationInstance for the action execution")]
        public void ThenRouteDataShouldHaveBeenAddedToTheIOperationInstanceForTheActionExecution()
        {
            TestOperationInstance operation = this.GetOperationInstance($"{typeof(HomeController).FullName}.{nameof(HomeController.Index)}")!;

            Assert.IsTrue(operation.Properties.ContainsKey("RouteData[controller]"));
            Assert.AreEqual("Home", operation.Properties["RouteData[controller]"]);

            Assert.IsTrue(operation.Properties.ContainsKey("RouteData[action]"));
            Assert.AreEqual("Index", operation.Properties["RouteData[action]"]);

            Assert.IsTrue(operation.Properties.ContainsKey("RouteData[id]"));
            Assert.AreEqual("this-is-the-id", operation.Properties["RouteData[id]"]);
        }

        [Then("operation properties added in the action method should have been added to the IOperationInstance for the action execution")]
        public void ThenOperationPropertiesAddedInTheActionMethodShouldHaveBeenAddedToTheIOperationInstanceForTheActionExecution()
        {
            TestOperationInstance operation = this.GetOperationInstance($"{typeof(HomeController).FullName}.{nameof(HomeController.Index)}")!;
        }

        [Then("an IOperationInstance should have been created for the result execution")]
        public void ThenAnIOperationInstanceShouldHaveBeenCreatedForTheResultExecution()
        {
            TestOperationInstance? operation = this.GetOperationInstance($"{typeof(HomeController).FullName}.{nameof(HomeController.Index)}::ResultExecution");
            Assert.IsNotNull(operation);
        }

        [Then("all IOperationInstances have been disposed")]
        public void ThenAllIOperationInstancesHaveBeenDisposed()
        {
            foreach (TestOperationInstance current in TestOperationsInstrumentation.Instance.StartedOperations)
            {
                Assert.IsTrue(current.IsDisposed);
            }
        }

        private TestOperationInstance? GetOperationInstance(string expectedName)
        {
            return TestOperationsInstrumentation.Instance.StartedOperations.FirstOrDefault(op => op.Name == expectedName);
        }
    }
}