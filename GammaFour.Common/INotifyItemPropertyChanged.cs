// <copyright file="INotifyItemPropertyChanged.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Notifies clients that a property value has changed on an item in the collection.
    /// </summary>
    public interface INotifyItemPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes on an item in the collection.
        /// </summary>
        event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;
    }
}
