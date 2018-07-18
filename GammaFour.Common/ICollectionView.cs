// <copyright file="ICollectionView.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Extends the WinRT ICollectionView to provide sorting and filtering.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "A view must be distinct from it's collection.")]
    public interface ICollectionView : IEnumerable
    {
        /// <summary>
        /// Gets a value indicating whether this view supports filtering via the <see cref="Filter"/> property.
        /// </summary>
        bool CanFilter
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this view supports grouping via the <see cref="GroupDescriptions"/> property.
        /// </summary>
        bool CanGroup
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this view supports sorting via the <see cref="SortDescriptions"/> property.
        /// </summary>
        bool CanSort
        {
            get;
        }

        /// <summary>
        /// Gets a collection of System.ComponentModel.GroupDescription objects that describe how the items in the collection are grouped in the
        /// view.
        /// </summary>
        IList<object> GroupDescriptions
        {
            get;
        }

        /// <summary>
        /// Gets or sets a callback used to determine if an item is suitable for inclusion in the view.
        /// </summary>
        Predicate<object> Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the underlying collection.
        /// </summary>
        IEnumerable SourceCollection
        {
            get;
        }

        /// <summary>
        /// Gets a collection of System.ComponentModel.SortDescription objects that describe how the items in the collection are sorted in the view.
        /// </summary>
        IList<SortDescription> SortDescriptions
        {
            get;
        }

        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.
        /// </summary>
        /// <returns>A disposable object.</returns>
        IDisposable DeferRefresh();

        /// <summary>
        /// Refreshes the view applying the current sort and filter conditions.
        /// </summary>
        void Refresh();
    }
}