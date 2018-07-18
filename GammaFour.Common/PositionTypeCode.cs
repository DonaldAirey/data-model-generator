// <copyright file="PositionTypeCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Indicates whether a position is owned or borrowed.
    /// </summary>
    public enum PositionTypeCode
    {
        /// <summary>
        /// The position is owned.
        /// </summary>
        Long,

        /// <summary>
        /// The position is borrowed.
        /// </summary>
        Short
    }
}
