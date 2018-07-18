// <copyright file="INavigate.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;

    /// <summary>
    /// Provides a method to navigate to a URI.
    /// </summary>
    public interface INavigate
    {
        /// <summary>
        /// Navigate to the given URI.
        /// </summary>
        /// <param name="target">The target URI.</param>
        void Navigate(Uri target);
    }
}
