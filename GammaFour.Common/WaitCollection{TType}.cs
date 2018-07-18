// <copyright file="WaitCollection{TType}.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    /// <summary>
    /// A thread-safe queue of objects of TType that will cause the consumer to block until an item is available.
    /// </summary>
    /// <typeparam name="TType">The base type that is stored in the collection.</typeparam>
    public class WaitCollection<TType> : Queue<TType>, IDisposable
    {
        /// <summary>
        /// Wakes up the consumer when an object of TType is available.
        /// </summary>
        private ManualResetEvent queueEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitCollection{TType}"/> class.
        /// </summary>
        public WaitCollection()
        {
            // This event is used to signal a waiting thread that new data is available in the ticker.
            this.queueEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Gets a value indicating whether the queue is empty or not.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                bool isEmpty = true;
                try
                {
                    Monitor.Enter(this);
                    isEmpty = this.Count == 0;
                }
                finally
                {
                    Monitor.Exit(this);
                }

                return isEmpty;
            }
        }

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        public new int Count
        {
            get
            {
                int count;
                try
                {
                    Monitor.Enter(this);
                    count = base.Count;
                }
                finally
                {
                    Monitor.Exit(this);
                }

                return count;
            }
        }

        /// <summary>
        /// Returns the element at the beginning of the <see cref="WaitCollection{TType}"/> without removing it.
        /// </summary>
        /// <returns>The next price record on the queue.</returns>
        public new TType Peek()
        {
            try
            {
                // Insure thread safety.
                Monitor.Enter(this);

                // If there is nothing in the queue, wait until something is put in the other end.
                if (this.Count == 0)
                {
                    Monitor.Exit(this);
                    this.queueEvent.WaitOne();
                    Monitor.Enter(this);
                }

                // Remove the first item placed in the queue.
                return base.Peek();
            }
            finally
            {
                // The queue doesn't need to be blocked any longer.
                Monitor.Exit(this);
            }
        }

        /// <summary>
        /// Place an object in the queue.
        /// </summary>
        /// <param name="queueObject">The object to be placed in the queue.</param>
        public new void Enqueue(TType queueObject)
        {
            try
            {
                // Make sure we release the lock when we exit.
                Monitor.Enter(this);

                // Place the object in the queue.
                base.Enqueue(queueObject);

                // Signal anyone waiting on a tick that one is ready in the queue.
                if (this.Count == 1)
                {
                    this.queueEvent.Set();
                }
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        /// <summary>
        /// Remove an item from the queue.
        /// </summary>
        /// <returns>The next object of TType in the queue.</returns>
        public new TType Dequeue()
        {
            bool timedOut;
            return this.Dequeue(-1, out timedOut);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Call our virtual method for disposing of resources and then inhibit the native garbage collection.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Remove an item of TType from the queue.
        /// </summary>
        /// <param name="timeout">The time to wait for an object.</param>
        /// <param name="isTimedOut">Indicates that the thread timed out waiting for an object.</param>
        /// <returns>The most first item placed in the queue.</returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "Reviewed")]
        public TType Dequeue(int timeout, out bool isTimedOut)
        {
            isTimedOut = false;
            try
            {
                // Insure thread safety.
                Monitor.Enter(this);

                // Wait for an item of TType to be placed in the queue.
                if (this.Count == 0)
                {
                    Monitor.Exit(this);
                    try
                    {
                        if (this.queueEvent.WaitOne(timeout) == false)
                        {
                            isTimedOut = true;
                            return default(TType);
                        }
                    }
                    finally
                    {
                        Monitor.Enter(this);
                    }
                }

                // Remove the first item placed in the queue.
                TType queueObject = base.Dequeue();

                // If there is nothing left in the queue, then clear the event.  This will block any calls to 'Dequeue' until there is something to extract from the
                // queue.
                if (this.Count == 0)
                {
                    this.queueEvent.Reset();
                }

                // This is the first item placed in the queue.
                return queueObject;
            }
            finally
            {
                // The queue can be accessed by other threads now.
                Monitor.Exit(this);
            }
        }

        /// <summary>
        /// Dispose of managed resources.
        /// </summary>
        /// <param name="disposing">true to indicate that the managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            // This will release the managed resources.
            if (disposing)
            {
                if (this.queueEvent != null)
                {
                    this.queueEvent.Dispose();
                    this.queueEvent = null;
                }
            }
        }
    }
}
