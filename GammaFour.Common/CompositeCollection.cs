// <copyright file="CompositeCollection.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    /// <summary>
    /// A composite collection.
    /// </summary>
    public class CompositeCollection : ObservableCollection<object>, IList<object>
    {
        /// <summary>
        /// Counts the objects in the combined collection.
        /// </summary>
        private readonly Func<object, int> sumFunction = x =>
        {
            CollectionContainer collectionContainer = x as CollectionContainer;
            if (collectionContainer != null)
            {
                if (collectionContainer.Collection == null)
                {
                    return 0;
                }

                return collectionContainer.Collection.Cast<object>().Count();
            }
            else
            {
                return 1;
            }
        };

        /// <summary>
        /// The composite collection.
        /// </summary>
        private ObservableCollection<object> composition;

        /// <summary>
        /// Used to map the collection change action to a handler for that action.
        /// </summary>
        private Dictionary<NotifyCollectionChangedAction, Action<NotifyCollectionChangedEventArgs>> changedMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCollection"/> class.
        /// </summary>
        public CompositeCollection()
        {
            // Initialize the object.
            this.composition = new ObservableCollection<object>();
            this.composition.CollectionChanged += (s, e) => this.changedMap[e.Action](e);

            // This map is used to direct the action to a handler for that action.
            this.changedMap = new Dictionary<NotifyCollectionChangedAction, Action<NotifyCollectionChangedEventArgs>>
            {
                { NotifyCollectionChangedAction.Add,  this.OnCompositionItemAdded },
                { NotifyCollectionChangedAction.Move, this.OnCompositionItemReset },
                { NotifyCollectionChangedAction.Remove,   this.OnCompositionItemRemove },
                { NotifyCollectionChangedAction.Replace,  this.OnCompositionItemReplaced },
                { NotifyCollectionChangedAction.Reset,    this.OnCompositionItemReset }
            };
        }

        /// <summary>
        /// Gets the composite collection.
        /// </summary>
        public ObservableCollection<object> Composition
        {
            get
            {
                return this.composition;
            }
        }

        /// <summary>
        /// Handles a change to one of the composite collections.
        /// </summary>
        /// <param name="sender">The object where the event is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int index = this.composition.IndexOf(sender);
            int startIndex = this.composition.Take(index).Sum(this.sumFunction);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    startIndex += e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        this.Insert(startIndex + i, e.NewItems[i]);
                    }

                    break;

                case NotifyCollectionChangedAction.Move:

                    this.Reset();
                    break;

                case NotifyCollectionChangedAction.Remove:

                    startIndex += e.OldStartingIndex;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        this.RemoveAt(startIndex + i);
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:

                    startIndex += e.OldStartingIndex;
                    for (int i = e.OldItems.Count - 1; i >= 0; i--)
                    {
                        this.RemoveAt(startIndex + i);
                    }

                    startIndex -= e.OldStartingIndex;
                    startIndex += e.NewStartingIndex;
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        this.Insert(startIndex + i, e.NewItems[i]);
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:

                    this.Reset();
                    break;

                default:

                    this.Reset();
                    break;
            }
        }

        /// <summary>
        /// Handles an item added to the composite collection.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnCompositionItemAdded(NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                var collectionContainer = item as CollectionContainer;
                if (collectionContainer != null)
                {
                    collectionContainer.CollectionChanged += this.OnContainedCollectionChanged;
                    if (collectionContainer.Collection != null)
                    {
                        foreach (IEnumerable collection in collectionContainer.Collection)
                        {
                            this.Add(collection);
                        }
                    }
                }
                else
                {
                    this.Add(item);
                }
            }
        }

        /// <summary>
        /// Handles an item moved in the composite collection.
        /// </summary>
        /// <param name="notifyCollectionChangedEventArgs">The event data.</param>
        private void OnCompositionItemReset(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.Reset();
        }

        /// <summary>
        /// Handles an item being removed from the composite collection.
        /// </summary>
        /// <param name="notifyCollectionChangedEventArgs">The event data.</param>
        private void OnCompositionItemRemove(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            foreach (object item in notifyCollectionChangedEventArgs.OldItems)
            {
                CollectionContainer collectionContainer = item as CollectionContainer;
                if (collectionContainer != null)
                {
                    collectionContainer.CollectionChanged -= this.OnContainedCollectionChanged;
                }
            }

            int startIndex = this.Composition.Cast<object>().Take(notifyCollectionChangedEventArgs.OldStartingIndex).Sum(this.sumFunction);
            int count = this.Composition.Cast<object>().Skip(
            notifyCollectionChangedEventArgs.OldStartingIndex).Take(notifyCollectionChangedEventArgs.OldItems.Count).Sum(this.sumFunction);
            for (int index = startIndex + count - 1; index >= startIndex; index--)
            {
                this.RemoveAt(index);
            }
        }

        /// <summary>
        /// Handles an item replaced in the composite collection.
        /// </summary>
        /// <param name="notifyCollectionChangedEventArgs">The event data.</param>
        private void OnCompositionItemReplaced(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            foreach (var item in notifyCollectionChangedEventArgs.OldItems)
            {
                var collectionContainer = item as CollectionContainer;
                if (collectionContainer != null)
                {
                    collectionContainer.CollectionChanged -= this.OnContainedCollectionChanged;
                }
            }

            foreach (object item in notifyCollectionChangedEventArgs.NewItems)
            {
                CollectionContainer collectionContainer = item as CollectionContainer;
                if (collectionContainer != null)
                {
                    collectionContainer.CollectionChanged += this.OnContainedCollectionChanged;
                }
            }

            int startIndex = this.Composition.Cast<object>().Take(notifyCollectionChangedEventArgs.OldStartingIndex).Sum(this.sumFunction);
            int count = this.Composition.Cast<object>().Skip(
            notifyCollectionChangedEventArgs.OldStartingIndex).Take(notifyCollectionChangedEventArgs.OldItems.Count).Sum(this.sumFunction);
            for (int index = startIndex + count - 1; index >= startIndex; index--)
            {
                this.RemoveAt(index);
            }

            startIndex = this.Composition.Cast<object>().Take(notifyCollectionChangedEventArgs.NewStartingIndex).Sum(this.sumFunction);
            foreach (object item in notifyCollectionChangedEventArgs.NewItems)
            {
                var collectionContainer = item as CollectionContainer;
                if (collectionContainer != null)
                {
                    foreach (object collection in collectionContainer.Collection)
                    {
                        this.Add(collection);
                    }
                }
                else
                {
                    this.Add(item);
                }
            }
        }

        /// <summary>
        /// Resets the composite collection.
        /// </summary>
        private void Reset()
        {
            foreach (var item in this)
            {
                var cc = item as CollectionContainer;
                if (cc != null)
                {
                    cc.CollectionChanged -= this.OnContainedCollectionChanged;
                }
            }

            this.Clear();
            if (this.Composition != null)
            {
                foreach (var item in this.Composition)
                {
                    var cc = item as CollectionContainer;
                    if (cc != null)
                    {
                        cc.CollectionChanged += this.OnContainedCollectionChanged;
                    }

                    this.Add(item);
                }
            }
        }
    }
}