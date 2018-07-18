// <copyright file="CommonConversion.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
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

        /// <summary>
        /// Converts a relative date into an actual date.
        /// </summary>
        /// <param name="length">An amount of time.</param>
        /// <param name="timeUnit">The units in which the time is measured.</param>
        /// <returns>The current time plus the relative time.</returns>
        public static DateTime ToDateTime(decimal length, TimeUnitCode timeUnit)
        {
            // Start with the current time.
            DateTime startDate = DateTime.Now;

            switch (timeUnit)
            {
                case TimeUnitCode.Days:

                    // When the time is measured in days.
                    startDate += TimeSpan.FromDays(Convert.ToInt32(length));
                    break;

                case TimeUnitCode.Weeks:

                    // When the time is measured in weeks.
                    startDate += TimeSpan.FromDays(Convert.ToInt32(length) * 7);
                    break;

                case TimeUnitCode.Months:

                    // When the time is measured in months.
                    startDate += TimeSpan.FromDays(Convert.ToInt32(length) * 30);
                    break;
            }

            // This value represents and absolute date from today.
            return startDate;
        }
    }
}