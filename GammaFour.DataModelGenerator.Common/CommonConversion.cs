// <copyright file="CommonConversion.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Common Conversions
    /// </summary>
    public static class CommonConversion
    {
        /// <summary>
        /// Converts an array of objects into the equivalent text.
        /// </summary>
        /// <param name="collection">An array of objects to be converted.</param>
        /// <returns>The text equivalent of the array.</returns>
        public static string FromArray(ReadOnlyCollection<object> collection)
        {
            // Validate the parameters before using.
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            // Create a text string containing all the elements separated by commas.
            string keyText = string.Empty;
            foreach (object element in collection)
            {
                if (string.IsNullOrEmpty(keyText))
                {
                    keyText += ", ";
                }

                keyText += element == null ? "{NULL}" : element.ToString();
            }

            return keyText;
        }

        /// <summary>
        /// Converts a string to have a lower case starting character.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The input string with a lower case starting letter.</returns>
        public static string ToCamelCase(string text)
        {
            // Validate the argument before using it.
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // Convert the variable to its camel case equivalent.
            return text[0].ToString().ToLower() + text.Remove(0, 1);
        }
    }
}