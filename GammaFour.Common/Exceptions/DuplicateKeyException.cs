// <copyright file="DuplicateKeyException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents errors that occur when a duplicate key is added to a unique index.
    /// </summary>
    public class DuplicateKeyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        public DuplicateKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateKeyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="index">The unique key index where the exception occurred.</param>
        /// <param name="key">The key that caused the exception.</param>
        public DuplicateKeyException(string index, object[] key)
            : base("Duplicate Record.")
        {
            // Initialize the object.
            this.Index = index;
            this.Key = new ReadOnlyCollection<object>(key);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is
        /// specified.
        /// </param>
        public DuplicateKeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the index where the exception occurred.
        /// </summary>
        public string Index { get; private set; }

        /// <summary>
        /// Gets the key that caused the exception.
        /// </summary>
        public ReadOnlyCollection<object> Key { get; private set; }
    }
}