// <copyright file="DataAction.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// An action on a data model.
    /// </summary>
    public enum DataAction
    {
        /// <summary>
        /// Add an item.
        /// </summary>
        Add,

        /// <summary>
        /// Delete an item.
        /// </summary>
        Delete,

        /// <summary>
        /// Rollback an item.
        /// </summary>
        Rollback,

        /// <summary>
        /// Update an item.
        /// </summary>
        Update
    }
}
