// <copyright file="RecordNotFoundException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents errors that occur calling the unmanaged Win32 libraries.
    /// </summary>
    public class RecordNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        public RecordNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RecordNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        /// <param name="table">The table where the exception occurred.</param>
        /// <param name="key">The key that caused the exception.</param>
        public RecordNotFoundException(string table, object[] key)
            : base("Record not found.")
        {
            // Initialize the object.
            this.Table = table;
            this.Key = new ReadOnlyCollection<object>(key);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that gives more information about the Win32 error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.</param>
        public RecordNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the table where the exception occurred.
        /// </summary>
        public string Table { get; private set; }

        /// <summary>
        /// Gets the key that caused the exception.
        /// </summary>
        public ReadOnlyCollection<object> Key { get; private set; }
    }
}