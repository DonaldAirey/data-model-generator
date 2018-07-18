// <copyright file="NavigationContext.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;

    /// <summary>
    /// Encapsulates information about a navigation request.
    /// </summary>
    public class NavigationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationContext"/> class for a region name.
        /// <see cref="Uri"/>.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="uri">The Uri.</param>
        public NavigationContext(INavigationService navigationService, Uri uri)
        {
            this.NavigationService = navigationService;
            this.Uri = uri;
            this.Parameters = uri != null ? UriParsingHelper.ParseQuery(uri) : null;
        }

        /// <summary>
        /// Gets the region navigation service.
        /// </summary>
        public INavigationService NavigationService { get; private set; }

        /// <summary>
        /// Gets the navigation URI.
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the <see cref="UriQueryCollection"/> extracted from the URI.
        /// </summary>
        public UriQueryCollection Parameters { get; private set; }
    }
}
