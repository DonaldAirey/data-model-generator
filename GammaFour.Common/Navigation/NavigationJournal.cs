// <copyright file="NavigationJournal.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides journaling of current, back, and forward navigation within regions.
    /// </summary>
    public class NavigationJournal
    {
        /// <summary>
        /// The backward navigation stack.
        /// </summary>
        private Stack<Uri> backStack = new Stack<Uri>();

        /// <summary>
        /// The forward navigation stack.
        /// </summary>
        private Stack<Uri> forwardStack = new Stack<Uri>();

        /// <summary>
        /// Gets the previous journal entry.
        /// </summary>
        public Uri BackEntry
        {
            get
            {
                return this.backStack.Peek();
            }
        }

        /// <summary>
        /// Gets the current journal entry.
        /// </summary>
        public Uri CurrentEntry { get; private set; }

        /// <summary>
        /// Gets the next journal entry.
        /// </summary>
        public Uri ForwardEntry
        {
            get
            {
                return this.forwardStack.Peek();
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is at least one entry in the back navigation history.
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return this.backStack.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is at least one entry in the forward navigation history.
        /// </summary>
        public bool CanGoForward
        {
            get
            {
                return this.forwardStack.Count > 0;
            }
        }

        /// <summary>
        /// Clears the journal of current, back, and forward navigation histories.
        /// </summary>
        public void Clear()
        {
            // Reset the stack.
            this.CurrentEntry = null;
            this.backStack.Clear();
            this.forwardStack.Clear();
        }

        /// <summary>
        /// Navigates to the most recent entry in the back navigation history, or does nothing if no entry exists in back navigation.
        /// </summary>
        public void MoveBack()
        {
            // If we can go backwards, then push the current entry on to the forward looking stack and pop the backward entry to be the current one.
            if (this.CanGoBack)
            {
                this.forwardStack.Push(this.CurrentEntry);
                this.CurrentEntry = this.backStack.Pop();
            }
        }

        /// <summary>
        /// Navigates to the most recent entry in the forward navigation history, or does nothing if no entry exists in forward navigation.
        /// </summary>
        public void MoveForward()
        {
            // If we can go forward, then push the current entry onto the backward looking stack and pop the forward entry to be the current one.
            if (this.CanGoForward)
            {
                this.backStack.Push(this.CurrentEntry);
                this.CurrentEntry = this.forwardStack.Pop();
            }
        }

        /// <summary>
        /// Records the navigation to the entry..
        /// </summary>
        /// <param name="entry">The entry to record.</param>
        public void MoveTo(Uri entry)
        {
            // If we've already navigated, then push the current journal entry onto the backward looking stack.
            if (this.CurrentEntry != null)
            {
                this.backStack.Push(this.CurrentEntry);
            }

            // When moving to an absolute location, there's nothing forward looking for the navigator to find.  Forward entries only exist when we've
            // moved backward on the stack.
            this.forwardStack.Clear();
            this.CurrentEntry = entry;
        }
    }
}