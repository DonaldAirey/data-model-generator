// <copyright file="ActiveViewChangedEventArgs.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;

    /// <summary>
    /// EventArgs used when the active view has change through a navigation event.
    /// </summary>
    public class ActiveViewChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The active view.
        /// </summary>
        private object activeView;

        /// <summary>
        /// The URI of the new view.
        /// </summary>
        private Uri source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveViewChangedEventArgs"/> class.
        /// </summary>
        /// <param name="activeView">The active view</param>
        /// <param name="source">The target URI.</param>
        public ActiveViewChangedEventArgs(object activeView, Uri source)
        {
            // Validate the activeView argument.
            if (activeView == null)
            {
                throw new ArgumentNullException(nameof(activeView));
            }

            // Validate the source argument.
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Initialize the object.
            this.activeView = activeView;
            this.source = source;
        }

        /// <summary>
        /// Gets the active view.
        /// </summary>
        public object ActiveView
        {
            get
            {
                return this.activeView;
            }
        }

        /// <summary>
        /// Gets the Source URI of the active view.
        /// </summary>
        public Uri Source
        {
            get
            {
                return this.source;
            }
        }
    }
}