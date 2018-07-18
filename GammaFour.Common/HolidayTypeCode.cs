// <copyright file="HolidayTypeCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Holiday types for trading and settlement.
    /// </summary>
    public enum HolidayTypeCode
    {
        /// <summary>
        /// No trading or settlements on this day.
        /// </summary>
        Both,

        /// <summary>
        /// No trading on this day.
        /// </summary>
        Trading,

        /// <summary>
        /// No settlements on this day.
        /// </summary>
        Settlement
    }
}
