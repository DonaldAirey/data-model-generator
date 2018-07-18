// <copyright file="CollectionChange.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Describes the type of change on the collection.
    /// </summary>
    public enum CollectionChange
    {
        /// <summary>
        /// The collection has been reset.
        /// </summary>
        Reset = 0,

        /// <summary>
        /// An item has been inserted.
        /// </summary>
        ItemInserted = 1,

        /// <summary>
        /// An item has been removed.
        /// </summary>
        ItemRemoved = 2,

        /// <summary>
        /// An item has been changed.
        /// </summary>
        ItemChanged = 3
    }
}