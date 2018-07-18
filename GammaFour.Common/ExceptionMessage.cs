// <copyright file="ExceptionMessage.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Formats messages that appear in exceptions.
    /// </summary>
    public sealed class ExceptionMessage
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="ExceptionMessage" /> class from being created.
        /// </summary>
        private ExceptionMessage()
        {
        }

        /// <summary>
        /// Formats an exception message.
        /// </summary>
        /// <param name="message">The format of the message.</param>
        /// <param name="arguments">An optional list of parameters.</param>
        /// <returns>The formatted string targeting the invariant culture.</returns>
        public static string Format(string message, params object[] arguments)
        {
            // This insures that the rules for formatting a string for a common culture is applied. Exception messages are intended to be universal and should not
            // be formatted for a local culture.
            return string.Format(CultureInfo.InvariantCulture, message, arguments);
        }
    }
}
