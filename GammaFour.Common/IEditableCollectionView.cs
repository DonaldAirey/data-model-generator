// <copyright file="IEditableCollectionView.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Implements a WinRT version of the IEditableCollectionView interface.
    /// </summary>
    public interface IEditableCollectionView
    {
        /// <summary>
        /// Gets a value indicating whether a new item can be added.
        /// </summary>
        bool CanAddNew
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether editing can be canceled.
        /// </summary>
        bool CanCancelEdit
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether an item can be removed.
        /// </summary>
        bool CanRemove
        {
            get;
        }

        /// <summary>
        /// Gets the item currently being added.
        /// </summary>
        object CurrentAddItem
        {
            get;
        }

        /// <summary>
        /// Gets the current edited item.
        /// </summary>
        object CurrentEditItem
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether a new item is being added.
        /// </summary>
        bool IsAddingNew
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether an item is being edited.
        /// </summary>
        bool IsEditingItem
        {
            get;
        }

        /// <summary>
        /// Adds a new object.
        /// </summary>
        /// <returns>The newly added object.</returns>
        object AddNew();

        /// <summary>
        /// Cancels editing an item.
        /// </summary>
        void CancelEdit();

        /// <summary>
        /// Cancels adding a new object.
        /// </summary>
        void CancelNew();

        /// <summary>
        /// Commits an edited item to the view.
        /// </summary>
        void CommitEdit();

        /// <summary>
        /// Commits the new object to the view.
        /// </summary>
        void CommitNew();

        /// <summary>
        /// Edits the given item.
        /// </summary>
        /// <param name="item">The item to be edited.</param>
        void EditItem(object item);
    }
}
