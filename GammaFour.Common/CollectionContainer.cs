// <copyright file="CollectionContainer.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;

    /// <summary>
    /// A container for a collection.
    /// </summary>
    public class CollectionContainer : INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// The collection.
        /// </summary>
        private IEnumerable collectionField;

        /// <summary>
        /// Invoked when the collection has changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Invoked when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        public IEnumerable Collection
        {
            get
            {
                return this.collectionField;
            }

            set
            {
                if (this.collectionField != value)
                {
                    INotifyCollectionChanged notifyCollectionChanged = this.collectionField as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged -= this.OnCollectionChanged;
                    }

                    this.collectionField = value;

                    notifyCollectionChanged = this.collectionField as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                    {
                        notifyCollectionChanged.CollectionChanged += this.OnCollectionChanged;
                    }

                    this.OnPropertyChanged("Collection");
                }
            }
        }

        /// <summary>
        /// Handles a change to the collection.
        /// </summary>
        /// <param name="sender">The object where the event is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }

        /// <summary>
        /// Handles a change to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}