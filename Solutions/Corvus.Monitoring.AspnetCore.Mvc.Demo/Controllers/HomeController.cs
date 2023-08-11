// <copyright file="HomeController.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Demo.Controllers
{
    using Corvus.Monitoring.AspnetCore.Mvc.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for the home page.
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// Action method for the home page.
        /// </summary>
        /// <returns>A view result for the home page.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new();

            // Since this.CurrentOperation is nullable, we use the null-conditional operator to avoid compiler warnings/runtime exceptions
            // if it's not set for some reason.
            this.CurrentOperation?.AddOperationProperty("HomeController.Index:CustomProperty1", "ExampleValue1");

            // Simulated work time to demonstrate how this appears in App Insights.
            await Task.Delay(500);

            this.CurrentOperation?.AddOperationProperty("HomeController.Index:CustomProperty2", "ExampleValue2");

            return this.View(model);
        }
    }
}