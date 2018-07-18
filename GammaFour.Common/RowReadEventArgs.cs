// <copyright file="RowReadEventArgs.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Event arguments for the reading a row from a data source.
    /// </summary>
    public class RowReadEventArgs : EventArgs
    {
        /// <summary>
        /// The raw row data.
        /// </summary>
        private object[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowReadEventArgs"/> class.
        /// </summary>
        /// <param name="data">The raw row data.</param>
        public RowReadEventArgs(object[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Gets the raw row data.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>The data at the given index.</returns>
        public object GetData(int index)
        {
            return this.data[index];
        }
    }
}