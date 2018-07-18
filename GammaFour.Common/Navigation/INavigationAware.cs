// <copyright file="INavigationAware.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    /// <summary>
    /// Used to communicate the navigation state to a target page.
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Raised when a target is no longer the active page.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        void OnNavigatedFrom(NavigationContext navigationContext);

        /// <summary>
        /// Raised when a target is becoming the active page.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        void OnNavigatedTo(NavigationContext navigationContext);
    }
}
