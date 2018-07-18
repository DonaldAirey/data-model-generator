// <copyright file="NavigationEventArgs.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;

    /// <summary>
    /// EventArgs used with the Navigated event.
    /// </summary>
    public class NavigationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationEventArgs"/> class.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public NavigationEventArgs(NavigationContext navigationContext)
        {
            if (navigationContext == null)
            {
                throw new ArgumentNullException(nameof(navigationContext));
            }

            this.NavigationContext = navigationContext;
        }

        /// <summary>
        /// Gets the navigation context.
        /// </summary>
        public NavigationContext NavigationContext { get; private set; }

        /// <summary>
        /// Gets the navigation URI
        /// </summary>
        /// <remarks>
        /// This is a convenience accessor around NavigationContext.Uri.
        /// </remarks>
        public Uri Uri
        {
            get
            {
                if (this.NavigationContext != null)
                {
                    return this.NavigationContext.Uri;
                }

                return null;
            }
        }
    }
}