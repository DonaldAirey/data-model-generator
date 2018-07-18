// <copyright file="ListCollectionView.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    /// Provides a view for a collection that can be sorted and filtered.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Views must have a distinct name from the collection")]
    public class ListCollectionView : Collection<object>, INotifyCollectionChanged, IComparer<object>
    {
        /// <summary>
        /// The filter.
        /// </summary>
        private Predicate<object> filter;

        /// <summary>
        /// The index to the current item.
        /// </summary>
        private int itemIndex = 0;

        /// <summary>
        /// The description of how the view is sorted.
        /// </summary>
        private ObservableCollection<SortDescription> sortDescriptionsField = new ObservableCollection<SortDescription>();

        /// <summary>
        /// The Reflection sorting properties.
        /// </summary>
        private Dictionary<TypeInfo, Dictionary<string, PropertyInfo>> sortProperties = new Dictionary<TypeInfo, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// The source.
        /// </summary>
        private object sourceCollectionField;

        /// <summary>
        /// The source list.
        /// </summary>
        private IList sourceList;

        /// <summary>
        /// Updating counter.
        /// </summary>
        private int updating;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCollectionView"/> class.
        /// </summary>
        /// <param name="source">The underlying collection for this view.</param>
        public ListCollectionView(object source)
        {
            // Initialize the sorting fields.
            this.sortDescriptionsField.CollectionChanged += this.SortCollectionChanged;

            // This provides access to the underlying collection and connects the source collection events to the view.
            this.Source = source;
        }

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets a value indicating whether there are more items in the view.
        /// </summary>
        public static bool HasMoreItems
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the current item in the view.
        /// </summary>
        public object CurrentItem
        {
            get
            {
                return this.itemIndex > -1 && this.itemIndex < this.Count ? this[this.itemIndex] : null;
            }

            set
            {
                this.MoveCurrentToIndex(this.IndexOf(value));
            }
        }

        /// <summary>
        /// Gets or sets the filter for the view.
        /// </summary>
        public Predicate<object> Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                if (this.filter != value)
                {
                    this.filter = value;
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current item is after the last item.
        /// </summary>
        public bool IsCurrentAfterLast
        {
            get
            {
                return this.itemIndex >= this.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current item is before the first item.
        /// </summary>
        public bool IsCurrentBeforeFirst
        {
            get
            {
                return this.itemIndex < 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the view is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.sourceList == null || this.sourceList.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets the collection of the sort fields.
        /// </summary>
        public IList<SortDescription> SortDescriptions
        {
            get
            {
                return this.sortDescriptionsField;
            }
        }

        /// <summary>
        /// Gets or sets the collection from which to create the view.
        /// </summary>
        public object Source
        {
            get
            {
                return this.sourceCollectionField;
            }

            set
            {
                if (this.sourceCollectionField != value)
                {
                    // save new source
                    this.sourceCollectionField = value;

                    // save new source as list (so we can add/remove etc)
                    this.sourceList = value as IList;

                    // refresh our view
                    this.HandleSourceChanged();
                }
            }
        }

        /// <summary>
        /// Gets the source collection.
        /// </summary>
        public IEnumerable SourceCollection
        {
            get
            {
                return this.sourceCollectionField as IEnumerable;
            }
        }

        /// <summary>
        /// Compares two items.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <returns>0 if the items are the same, -1 if the first item is less than the second, 1 otherwise.</returns>
        int IComparer<object>.Compare(object first, object second)
        {
            return this.Compare(first, second);
        }

        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.
        /// </summary>
        /// <returns>A disposable object used to undo the effects of deferring the refresh.</returns>
        public IDisposable DeferRefresh()
        {
            return new DeferNotifications(this);
        }

        /// <summary>
        /// Update the view from the current source, using the current filter and sort settings.
        /// </summary>
        public void Refresh()
        {
            this.HandleSourceChanged();
        }

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <returns>0 if the items are the same, -1 if the first item is less than the second, 1 otherwise.</returns>
        protected int Compare(object first, object second)
        {
            // Validate the first argument
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            // Validate the second argument
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            // This constructs a cache that maps the type of the first parameter to the property info.  The idea is to use reflection to find the
            // value based on the name of the fields used in the sort and do it as quickly as possible.  Note that we need to recurse into the type
            // in order to deal with generics.
            TypeInfo firstTypeInfo = first.GetType().GetTypeInfo();
            Dictionary<string, PropertyInfo> firstTypeDictionary;
            if (!this.sortProperties.TryGetValue(firstTypeInfo, out firstTypeDictionary))
            {
                firstTypeDictionary = new Dictionary<string, PropertyInfo>();
                this.sortProperties.Add(firstTypeInfo, firstTypeDictionary);
                foreach (SortDescription sortDescription in this.sortDescriptionsField)
                {
                    firstTypeDictionary[sortDescription.PropertyName] = ListCollectionView.GetDeclaredProperty(firstTypeInfo, sortDescription.PropertyName);
                }
            }

            // This constructs a cache that maps the type of the second parameter to the property info.
            TypeInfo secondTypeInfo = second.GetType().GetTypeInfo();
            Dictionary<string, PropertyInfo> secondTypeDictionary;
            if (!this.sortProperties.TryGetValue(secondTypeInfo, out secondTypeDictionary))
            {
                secondTypeDictionary = new Dictionary<string, PropertyInfo>();
                this.sortProperties.Add(secondTypeInfo, secondTypeDictionary);
                foreach (SortDescription sortDescription in this.sortDescriptionsField)
                {
                    secondTypeDictionary[sortDescription.PropertyName] = ListCollectionView.GetDeclaredProperty(secondTypeInfo, sortDescription.PropertyName);
                }
            }

            // compare two items
            foreach (SortDescription sortDescription in this.sortDescriptionsField)
            {
                // Use the dictionaries to get the PropertyInfo and then get the value.
                var cx = firstTypeDictionary[sortDescription.PropertyName].GetValue(first) as IComparable;
                var cy = secondTypeDictionary[sortDescription.PropertyName].GetValue(second) as IComparable;

                // Compare the two values extracted from the reflection operations above.
                var cmp =
                    cx == cy ? 0 :
                    cx == null ? -1 :
                    cy == null ? +1 :
                    cx.CompareTo(cy);

                if (cmp != 0)
                {
                    return sortDescription.Direction == SortDirection.Ascending ? +cmp : -cmp;
                }
            }

            return 0;
        }

        /// <summary>
        /// Uses reflection to get the property information for the property name.
        /// </summary>
        /// <param name="typeInfo">The type that contains the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The property information for the given type and property name.</returns>
        private static PropertyInfo GetDeclaredProperty(TypeInfo typeInfo, string propertyName)
        {
            PropertyInfo propertyInfo = typeInfo.GetDeclaredProperty(propertyName);
            if (propertyInfo == null && typeInfo.BaseType != null)
            {
                return GetDeclaredProperty(typeInfo.BaseType.GetTypeInfo(), propertyName);
            }

            return propertyInfo;
        }

        /// <summary>
        /// Performs a binary search on the view.
        /// </summary>
        /// <param name="item">The item to be found.</param>
        /// <returns>The index of the item if found or the one's complement of the index if not found.</returns>
        private int BinarySearch(object item)
        {
            // This is a standard binary search, ripped from the .NET code by Reflector.
            int low = 0;
            int high = this.Count - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) >> 1);
                object midItem = this[mid];
                int compare = this.Compare(midItem, item);
                if (compare == 0)
                {
                    return mid;
                }
                else
                {
                    if (compare < 0)
                    {
                        low = mid + 1;
                    }
                    else
                    {
                        high = mid - 1;
                    }
                }
            }

            // If the item isn't found using a binary search, then the complement of the 'low' variable indicates where in the list the item would go
            // if it were part of the sorted list.
            return ~low;
        }

        /// <summary>
        /// Updates the view after changes other than add/remove an item.
        /// </summary>
        private void HandleSourceChanged()
        {
            // release sort property PropertyInfo dictionary
            this.sortProperties.Clear();

            // keep selection if possible
            var currentItem = this.CurrentItem;

            // re-create view
            this.Clear();
            IEnumerable items = this.Source as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (this.filter == null || this.filter(item))
                    {
                        if (this.sortDescriptionsField.Count > 0)
                        {
                            var index = this.BinarySearch(item);
                            if (index < 0)
                            {
                                index = ~index;
                            }

                            this.Insert(index, item);
                        }
                        else
                        {
                            this.Add(item);
                        }
                    }
                }
            }

            // Reset the collection.
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            // release sort property PropertyInfo dictionary
            this.sortProperties.Clear();

            // Restore selection if possible.
            this.CurrentItem = currentItem;
        }

        /// <summary>
        /// Move the cursor to a new position.
        /// </summary>
        /// <param name="index">The new index position.</param>
        /// <returns>True if the operation was successful.</returns>
        private bool MoveCurrentToIndex(int index)
        {
            // invalid?
            if (index < -1 || index >= this.Count)
            {
                return false;
            }

            // no change?
            if (index == this.itemIndex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handles a change to the sorting parameters.
        /// </summary>
        /// <param name="sender">The object that originated the event.</param>
        /// <param name="notifyCollectionChangedEventArgs">The event arguments.</param>
        private void SortCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (this.updating <= 0)
            {
                this.HandleSourceChanged();
            }
        }

        /// <summary>
        /// Class that handles deferring notifications while the view is modified.
        /// </summary>
        private class DeferNotifications : IDisposable
        {
            /// <summary>
            /// The target view.
            /// </summary>
            private ListCollectionView view;

            /// <summary>
            /// The current item.
            /// </summary>
            private object currentItem;

            /// <summary>
            /// Initializes a new instance of the <see cref="DeferNotifications"/> class.
            /// </summary>
            /// <param name="view">The view where updates will be deferred.</param>
            internal DeferNotifications(ListCollectionView view)
            {
                this.view = view;
                this.currentItem = this.view.CurrentItem;
                this.view.updating++;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                this.view.CurrentItem = this.currentItem;
                this.view.updating--;
                this.view.HandleSourceChanged();
            }
        }
    }
}