// <copyright file="LockException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents errors that occur when locking records for a transaction.
    /// </summary>
    public class LockException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LockException"/> class.
        /// </summary>
        public LockException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LockException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockException"/> class.
        /// </summary>
        /// <param name="message">The message that gives more information about the Win32 error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.</param>
        public LockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}