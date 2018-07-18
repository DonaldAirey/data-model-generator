// <copyright file="UriParsingHelper.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;

    /// <summary>
    /// Helper class for parsing <see cref="Uri"/> instances.
    /// </summary>
    internal static class UriParsingHelper
    {
        /// <summary>
        /// Gets the query part of <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The Uri.</param>
        /// <returns>The query part of the URI.</returns>
        internal static string GetQuery(Uri uri)
        {
            return UriParsingHelper.EnsureAbsolute(uri).Query;
        }

        /// <summary>
        /// Parses the query of <paramref name="uri"/> into a dictionary.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A collection of values parsed out of the URI.</returns>
        internal static UriQueryCollection ParseQuery(Uri uri)
        {
            var query = UriParsingHelper.GetQuery(uri);
            return new UriQueryCollection(query);
        }

        /// <summary>
        /// Ensures that the URI is an well formed, absolute URI.
        /// </summary>
        /// <param name="uri">The URI that is the source of the query.</param>
        /// <returns>An absolute URI based on the given URI.</returns>
        private static Uri EnsureAbsolute(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri;
            }

            if (uri != null && !uri.OriginalString.StartsWith("/", StringComparison.Ordinal))
            {
                return new Uri("http://localhost/" + uri, UriKind.Absolute);
            }

            return new Uri("http://localhost" + uri, UriKind.Absolute);
        }
    }
}