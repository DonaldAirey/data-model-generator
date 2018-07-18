// <copyright file="ConstraintException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;

    /// <summary>
    /// Represents errors that occur when locking records for a transaction.
    /// </summary>
    public class ConstraintException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        public ConstraintException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        /// <param name="operation">the operation where the constraint violation occurred.</param>
        public ConstraintException(string operation)
        {
            // Initialize the object.
            this.Operation = operation;
            this.Constraint = "Not specified";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        /// <param name="operation">the operation where the constraint violation occurred.</param>
        /// <param name="constraint">The constraint that was violated.</param>
        public ConstraintException(string operation, string constraint)
        {
            // Initialize the object.
            this.Operation = operation;
            this.Constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        /// <param name="message">The message that gives more information about the Win32 error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.</param>
        public ConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the constraint that was violated.
        /// </summary>
        public string Constraint { get; private set; }

        /// <summary>
        /// Gets the operation where the constraint violation occurred.
        /// </summary>
        public string Operation { get; private set; }
    }
}