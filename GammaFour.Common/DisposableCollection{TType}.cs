// <copyright file="DisposableCollection{TType}.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a dynamic data collection that provides notifications of changes as well as disposing of the managed resources.
    /// </summary>
    /// <typeparam name="TType">The type of elements in the collection.</typeparam>
    public class DisposableCollection<TType> : ObservableCollection<TType>, IDisposable
    {
        /// <summary>
        /// A collection of handlers for property changed actions.
        /// </summary>
        private readonly Dictionary<string, Action> propertyChangedActions = new Dictionary<string, Action>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableCollection{TType}"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Reviewed")]
        public DisposableCollection()
        {
            // A quick-and-dirty switch for delegating property change events.
            this.PropertyChanged += (s, e) =>
        {
            Action action;
            if (this.propertyChangedActions.TryGetValue(e.PropertyName, out action))
            {
                action();
            }
        };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableCollection{TType}"/> class.
        /// </summary>
        ~DisposableCollection()
        {
            // Call the virtual method to dispose of the resources. This (recommended) design pattern gives any derived classes a chance to clean up
            // unmanaged resources even though this base class has none.
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a dictionary of actions that are executed when a property changes.
        /// </summary>
        public Dictionary<string, Action> PropertyChangedActions
        {
            get
            {
                return this.propertyChangedActions;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Call the virtual method to allow derived classes to clean up resources.
            this.Dispose(true);

            // Since we took care of cleaning up the resources, there is no need to call the finalizer.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Intended to override existing event.")]
        public void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
