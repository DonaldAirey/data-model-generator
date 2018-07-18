// <copyright file="DataRowVersion.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// The different versions of a row.
    /// </summary>
    public enum DataRowVersion
    {
        /// <summary>
        /// The current version of a row.
        /// </summary>
        Current,

        /// <summary>
        /// The original version of a row.
        /// </summary>
        Original,

        /// <summary>
        /// The previous version of a row.
        /// </summary>
        Previous
    }
}
