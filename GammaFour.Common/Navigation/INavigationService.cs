// <copyright file="INavigationService.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;

    /// <summary>
    /// Provides navigation services.
    /// </summary>
    public interface INavigationService : INavigate
    {
        /// <summary>
        /// Occurs when the active view has changed.
        /// </summary>
        event EventHandler<ActiveViewChangedEventArgs> ActiveViewChanged;

        /// <summary>
        /// Occurs when a target has been successfully navigated.
        /// </summary>
        event EventHandler<NavigationEventArgs> Navigated;

        /// <summary>
        /// Occurs when navigation to a target has failed.
        /// </summary>
        event EventHandler<NavigationFailedEventArgs> NavigationFailed;

        /// <summary>
        /// Gets a value indicating whether there is at least one entry in the back navigation history.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether there is at least one entry in the forward navigation history.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Clears the journal of current, back, and forward navigation histories.
        /// </summary>
        void Clear();

        /// <summary>
        /// Navigates to the previous location in the journal.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to the next location in the journal.
        /// </summary>
        void GoForward();
    }
}