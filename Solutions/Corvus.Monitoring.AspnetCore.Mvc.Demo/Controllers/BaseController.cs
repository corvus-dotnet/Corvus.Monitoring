// <copyright file="BaseController.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Demo.Controllers
{
    using Corvus.Monitoring.Instrumentation;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Base class for controllers in this application.
    /// </summary>
    /// <remarks>
    /// Having a base class for your controllers that implements <see cref="IHaveObservableActionMethods "/>
    /// and has the <see cref="ObservableActionMethodsAttribute"/> action filter is the simplest way of
    /// adding observability to your controllers.
    /// </remarks>
    [ObservableActionMethods]
    public abstract class BaseController : Controller, IHaveObservableActionMethods
    {
        /// <inheritdoc/>
        public IOperationInstance? CurrentOperation
        {
            get;
            set;
        }
    }
}