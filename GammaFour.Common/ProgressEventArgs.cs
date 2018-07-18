// <copyright file="ProgressEventArgs.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Contains event data for indicating progress.
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// The current count.
        /// </summary>
        private double current;

        /// <summary>
        /// The end of the progress range.
        /// </summary>
        private double maximum;

        /// <summary>
        /// The start of the progress range.
        /// </summary>
        private double minimum;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
        /// </summary>
        /// <param name="minimum">The minimum number of items.</param>
        /// <param name="maximum">The maximum number of items.</param>
        /// <param name="current">The current number of items.</param>
        public ProgressEventArgs(double minimum, double maximum, double current)
        {
            // Initialize the object.
            this.maximum = maximum;
            this.minimum = minimum;
            this.current = current;
        }

        /// <summary>
        /// Gets the current progress.
        /// </summary>
        public double Current
        {
            get
            {
                return this.current;
            }
        }

        /// <summary>
        /// Gets the maximum of the progress range.
        /// </summary>
        public double Maximum
        {
            get
            {
                return this.maximum;
            }
        }

        /// <summary>
        /// Gets the minimum of the progress range.
        /// </summary>
        public double Minimum
        {
            get
            {
                return this.minimum;
            }
        }
    }
}
