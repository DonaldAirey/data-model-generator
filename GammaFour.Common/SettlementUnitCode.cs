// <copyright file="SettlementUnitCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Units for setting an order.
    /// </summary>
    public enum SettlementUnitCode
    {
        /// <summary>
        /// Settlement is specified in hundredth of a percentage point (BIP).
        /// </summary>
        BasisPoint,

        /// <summary>
        /// Not defined.
        /// </summary>
        Empty,

        /// <summary>
        /// Settlement is in terms of market value.
        /// </summary>
        MarketValue,

        /// <summary>
        /// Settlement is specified in terms of percent.
        /// </summary>
        Percent
    }
}
