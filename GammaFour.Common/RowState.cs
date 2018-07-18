// <copyright file="RowState.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// The state of a row.
    /// </summary>
    public enum RowState
    {
        /// <summary>
        /// The row has been added.
        /// </summary>
        Added,

        /// <summary>
        /// The row has been deleted but the deletion has not been committed yet.
        /// </summary>
        Deleted,

        /// <summary>
        /// The row has been created but not added to a table, or deleted and removed from a table.
        /// </summary>
        Detached,

        /// <summary>
        /// An existing row has been modified.
        /// </summary>
        Modified,

        /// <summary>
        /// The row is unchanged from the last time AcceptChanges was called.
        /// </summary>
        Unchanged
    }
}