// <copyright file="ConstraintFault.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A fault that occurs when an update is made to an already updated record.
    /// </summary>
    [DataContract]
    public class ConstraintFault
    {
        /// <summary>
        /// The constraint that was violated.
        /// </summary>
        [DataMember]
        private string constraintField;

        /// <summary>
        /// The operation that caused the violation.
        /// </summary>
        [DataMember]
        private string operationField;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintFault"/> class.
        /// </summary>
        /// <param name="operation">The operation that caused the violation.</param>
        /// <param name="constraint">The constraint that was violated.</param>
        public ConstraintFault(string operation, string constraint)
        {
            // Initialize the object
            this.operationField = operation;
            this.constraintField = constraint;
        }

        /// <summary>
        /// Gets the constraint that was violated.
        /// </summary>
        public string Constraint
        {
            get
            {
                return this.constraintField;
            }
        }

        /// <summary>
        /// Gets the operation that caused the violation.
        /// </summary>
        public string Operation
        {
            get
            {
                return this.operationField;
            }
        }
    }
}
