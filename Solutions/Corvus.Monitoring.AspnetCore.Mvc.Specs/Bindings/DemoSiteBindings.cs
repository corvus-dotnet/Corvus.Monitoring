// <copyright file="DemoSiteBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Specs.Bindings
{
    using TechTalk.SpecFlow;

    /// <summary>
    /// Bindings that start and stop the test server hosting the demo site.
    /// </summary>
    [Binding]
    public static class DemoSiteBindings
    {
#nullable disable
        public static DemoWebApplicationFactory Factory { get; private set; }

        public static HttpClient Client { get; private set; }
#nullable enable

        [BeforeFeature]
        public static void StartDemoSite()
        {
            Factory = new();
            Client = Factory.CreateClient();
        }

        [AfterFeature]
        public static void StopDemoSite()
        {
            Factory?.Dispose();
        }
    }
}