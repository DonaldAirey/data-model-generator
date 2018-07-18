// <copyright file="TimeUnitCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Units of time.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed")]
    public enum TimeUnitCode
    {
        /// <summary>
        /// Days
        /// </summary>
        Days,

        /// <summary>
        /// Hours
        /// </summary>
        Hours,

        /// <summary>
        /// Minutes
        /// </summary>
        Minutes,

        /// <summary>
        /// Months
        /// </summary>
        Months,

        /// <summary>
        /// Seconds
        /// </summary>
        Seconds,

        /// <summary>
        /// Weeks
        /// </summary>
        Weeks,

        /// <summary>
        /// Years
        /// </summary>
        Years
    }
}
