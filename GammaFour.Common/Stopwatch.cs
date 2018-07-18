// <copyright file="Stopwatch.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// Measures the amount of time between the construction of the object and its destruction.
    /// </summary>
    public class Stopwatch : IDisposable
    {
        /// <summary>
        /// The starting time of the measured event.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Analysis Bug - Field is used in Dispose")]
        private DateTime startTime = DateTime.Now;

        /// <summary>
        /// The format for the message that displays the elapsed time.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Analysis Bug - Field is used in Dispose")]
        private string format = "Elapsed time is {0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="Stopwatch"/> class.
        /// </summary>
        public Stopwatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stopwatch"/> class.
        /// </summary>
        /// <param name="format">The message that displays the elapsed time.</param>
        public Stopwatch(string format)
        {
            // Initialize the object.
            this.format = format;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Stopwatch"/> class.
        /// </summary>
        ~Stopwatch()
        {
            // Call the virtual method to dispose of the resources.  This (recommended) design pattern gives any derived classes a chance to clean up unmanaged
            // resources even though this base class has none.
            this.Dispose(false);
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">true to indicate that the object is being disposed, false to indicate that the object is being finalized.</param>
        protected virtual void Dispose(bool disposing)
        {
            // There are no managed resources here, but we'll use the Disposing event to display the elapsed time.
            if (disposing)
            {
                Debug.WriteLine(
                    string.Format(CultureInfo.InvariantCulture, this.format, DateTime.Now.Subtract(this.startTime).TotalMilliseconds));
            }
        }
    }
}
