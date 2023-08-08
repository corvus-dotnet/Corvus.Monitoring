// <copyright file="IHaveObservableActionMethods.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc
{
    using Corvus.Monitoring.Instrumentation;

    /// <summary>
    /// Interface for controller methods which are decorated with the <see cref="ObservableActionMethodsAttribute"/>
    /// to assist with adding observation data during action method execution.
    /// </summary>
    public interface IHaveObservableActionMethods
    {
        /// <summary>
        /// Gets or sets the <see cref="IOperationInstance"/> for execution of the current action method. This property
        /// will be set prior to action method execution and unset afterwards.
        /// </summary>
        IOperationInstance? CurrentOperation { get; set; }
    }
}