// <copyright file="OrderStatusCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Describes whether an asset is acquired or disposed.
    /// </summary>
    public enum OrderStatusCode
    {
        /// <summary>
        /// New Order
        /// </summary>
        New,

        /// <summary>
        /// Partially Filled Order
        /// </summary>
        PartiallyFilled,

        /// <summary>
        /// Filled Order
        /// </summary>
        Filled,

        /// <summary>
        /// Done for the day Order
        /// </summary>
        DoneForDay,

        /// <summary>
        /// Canceled Order
        /// </summary>
        Canceled,

        /// <summary>
        /// Replaced Order
        /// </summary>
        Replaced,

        /// <summary>
        /// Waiting for a cancel confirmation Order
        /// </summary>
        PendingCancel,

        /// <summary>
        /// Stopped Order
        /// </summary>
        Stopped,

        /// <summary>
        /// Rejected Order
        /// </summary>
        Rejected,

        /// <summary>
        /// Suspended Order
        /// </summary>
        Suspended,

        /// <summary>
        /// Waiting for a confirmation on a new order
        /// </summary>
        PendingNew,

        /// <summary>
        /// Calculated Order
        /// </summary>
        Calculated,

        /// <summary>
        /// Expired Order
        /// </summary>
        Expired,

        /// <summary>
        /// Accepted for bidding Order
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ForBidding", Justification = "Reviewed")]
        AcceptedForBidding,

        /// <summary>
        /// Waiting for confirmation that the order has been replaced.
        /// </summary>
        PendingReplace
    }
}
