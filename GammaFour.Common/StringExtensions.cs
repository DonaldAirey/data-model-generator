// <copyright file="StringExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// String Extension Methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to have a lower case starting character.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The input string with a lower case starting letter.</returns>
        public static string ToCamelCase(this string text)
        {
            // Convert the variable to its camel case equivalent.
            return text[0].ToString().ToLower() + text.Remove(0, 1);
        }
    }
}