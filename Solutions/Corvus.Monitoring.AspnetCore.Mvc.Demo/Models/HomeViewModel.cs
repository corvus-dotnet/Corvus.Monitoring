// <copyright file="HomeViewModel.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Monitoring.AspnetCore.Mvc.Demo.Models
{
    using System;

    /// <summary>
    /// View model for the home page.
    /// </summary>
    public class HomeViewModel
    {
        private DateTimeOffset requestTimeUtc = DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets or sets the date and time of the request.
        /// </summary>
        /// <remarks>
        /// The <c>Thread.Sleep</c> in the getter is to make it easier to see the span which times the view rendering.
        /// </remarks>
        public DateTimeOffset RequestTimeUtc
        {
            get
            {
                Thread.Sleep(500);
                return this.requestTimeUtc;
            }

            set
            {
                this.requestTimeUtc = value;
            }
        }
    }
}