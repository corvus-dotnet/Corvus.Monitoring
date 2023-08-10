// <copyright file="IHaveObservableActionMethods.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc
{
    using Corvus.Monitoring.Instrumentation;

    /// <summary>
    /// Interface for controllers which are decorated with the <see cref="ObservableActionMethodsAttribute"/>
    /// to assist with adding observation data during action method execution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is intended to be used in conjunction with the <see cref="ObservableActionMethodsAttribute"/>.
    /// action filter. If your controller implements this interface, the action filter will set the <see cref="CurrentOperation"/>
    /// property prior to the action method being executed.
    /// </para>
    /// <para>
    /// In most cases, the simplest approach to adding the interface and action filter is to create a bespoke base controller
    /// class for all of your controllers and to add the interface implementation and action filter to this base class. This
    /// is demonstrated in the demo project (Corvus.Monitoring.AspnetCore.Mvc.Demo).
    /// </para>
    /// </remarks>
    public interface IHaveObservableActionMethods
    {
        /// <summary>
        /// Gets or sets the <see cref="IOperationInstance"/> for execution of the current action method. This property
        /// will be set prior to action method execution and unset afterwards.
        /// </summary>
        IOperationInstance? CurrentOperation { get; set; }
    }
}