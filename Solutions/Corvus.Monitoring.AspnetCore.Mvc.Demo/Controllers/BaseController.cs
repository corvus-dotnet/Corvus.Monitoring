// <copyright file="BaseController.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Demo.Controllers
{
    using Corvus.Monitoring.AspNetCore.Mvc;
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Base class for controllers in this application.
    /// </summary>
    /// <remarks>
    /// Having a base class for your controllers that has the <see cref="ObservableActionMethodsAttribute"/>
    /// action filter is the simplest way of adding observability to all of your controllers.
    /// </remarks>
    [ObservableActionMethods]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Gets the current <see cref="IOperationInstance"/>.
        /// </summary>
        /// <remarks>
        /// This property is provided to simplify access to the current operation stored on the HttpContext.
        /// </remarks>
        public IOperationInstance CurrentOperation
        {
            get => this.HttpContext.GetCurrentOperationInstance();
        }
    }
}