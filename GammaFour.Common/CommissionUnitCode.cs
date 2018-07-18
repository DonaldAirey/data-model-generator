// <copyright file="CommissionUnitCode.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// The units of elements in a commission schedule.
    /// </summary>
    public enum CommissionUnitCode
    {
        /// <summary>
        /// No units.
        /// </summary>
        Empty,

        /// <summary>
        /// In terms of face value.
        /// </summary>
        Face,

        /// <summary>
        /// In terms of market value.
        /// </summary>
        MarketValue,

        /// <summary>
        /// In terms of shares.
        /// </summary>
        Shares
    }
}
