// <copyright file="StringExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Globalization;
    using Pluralize.NET;

    /// <summary>
    /// String Extension Methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Used to create plurals out of singles.
        /// </summary>
        private static Pluralizer pluralizer = new Pluralizer();

        /// <summary>
        /// Converts a string to have a lower case starting character.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The input string with a lower case starting letter.</returns>
        public static string ToCamelCase(this string text)
        {
            // Convert the variable to its camel case equivalent.
            return text[0].ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture) + text.Remove(0, 1);
        }

        /// <summary>
        /// Turns a singular noun into a plural noun.
        /// </summary>
        /// <param name="text">The text to be pluralized.</param>
        /// <returns>The pluralized text.</returns>
        public static string ToPlural(this string text)
        {
            return pluralizer.Pluralize(text);
        }
    }
}