// <copyright file="TimeInForceCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Specifies how long the order remains in effect.
    /// </summary>
    public enum TimeInForceCode
    {
        /// <summary>
        /// Good at Crossing.
        /// </summary>
        AtCrossing,

        /// <summary>
        /// Good at the close of the market.
        /// </summary>
        AtTheClose,

        /// <summary>
        /// Good at the opening of the market.
        /// </summary>
        AtTheOpening,

        /// <summary>
        /// Good for the day.
        /// </summary>
        Day,

        /// <summary>
        /// Good until cancelled.
        /// </summary>
        GoodTillCancel,

        /// <summary>
        /// Good until crossed with another order.
        /// </summary>
        GoodTillCrossing,

        /// <summary>
        /// Good until a specific date.
        /// </summary>
        GoodTillDate,

        /// <summary>
        /// Good through crossing.
        /// </summary>
        GoodThroughCrossing,

        /// <summary>
        /// Fill the order completely or cancel.
        /// </summary>
        FillOrKill,

        /// <summary>
        /// Execute immediately or cancel.
        /// </summary>
        ImmediateOrCancel
    }
}
