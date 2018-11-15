// <copyright file="InvalidOperationFault.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A general fault indication.
    /// </summary>
    [DataContract]
    public class InvalidOperationFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationFault"/> class.
        /// </summary>
        /// <param name="message">The constraint violation message.</param>
        public InvalidOperationFault(string message)
        {
            // Initialize the object
            this.Message = message;
        }

        /// <summary>
        /// Gets the message of the fault.
        /// </summary>
        [DataMember]
        public string Message { get; private set; }
    }
}