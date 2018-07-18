// <copyright file="ItemPropertyChangedEventArgs.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Contains the event data for a ItemPropertyChanged event.
    /// </summary>
    public class ItemPropertyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The item that has changed.
        /// </summary>
        private object itemField;

        /// <summary>
        /// The name of the property that has changed.
        /// </summary>
        private string propertyNameField;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item that has been changed.</param>
        /// <param name="propertyName">The property on the given item that has changed.</param>
        public ItemPropertyChangedEventArgs(object item, string propertyName)
        {
            // Initialize the object.
            this.itemField = item;
            this.propertyNameField = propertyName;
        }

        /// <summary>
        /// Gets the item that has changed.
        /// </summary>
        public object Item
        {
            get
            {
                return this.itemField;
            }
        }

        /// <summary>
        /// Gets the property name that has changed.
        /// </summary>
        public string PropertyName
        {
            get
            {
                return this.propertyNameField;
            }
        }
    }
}
