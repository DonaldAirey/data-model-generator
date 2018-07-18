// <copyright file="ConditionCode.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Conditions for the execution of an order.
    /// </summary>
    public enum ConditionCode
    {
        /// <summary>
        /// Execute all or none of the order.
        /// </summary>
        AllOrNone,

        /// <summary>
        /// Execute all or none and do not reduce the order.
        /// </summary>
        AllOrNoneDoNotReduce,

        /// <summary>
        /// Do not reduce the order.
        /// </summary>
        DoNotReduce
    }
}
