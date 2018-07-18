// <copyright file="LotHandlingCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Describes the handling of the order.
    /// </summary>
    public enum LotHandlingCode
    {
        /// <summary>
        /// First In/First Out
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fifo", Justification = "Reviewed")]
        Fifo,

        /// <summary>
        /// Last In/First Out
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lifo", Justification = "Reviewed")]
        Lifo,

        /// <summary>
        /// Minimize the taxes
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mintax", Justification = "Reviewed")]
        Mintax
    }
}
