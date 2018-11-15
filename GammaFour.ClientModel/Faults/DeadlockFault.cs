// <copyright file="DeadlockFault.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A fault that occurs when the data model is deadlocked.
    /// </summary>
    [DataContract]
    public class DeadlockFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeadlockFault"/> class.
        /// </summary>
        /// <param name="message">The message of the fault.</param>
        public DeadlockFault(string message)
        {
            // Initialize the object.
            this.Message = message;
        }

        /// <summary>
        /// Gets the message of the fault.
        /// </summary>
        [DataMember]
        public string Message { get; private set; }
    }
}