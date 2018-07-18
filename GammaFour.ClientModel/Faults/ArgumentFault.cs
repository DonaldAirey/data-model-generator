// <copyright file="ArgumentFault.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// A fault that occurs when the arguments don't meet the expected values.
    /// </summary>
    [DataContract]
    public class ArgumentFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentFault"/> class.
        /// </summary>
        /// <param name="message">The message of the fault.</param>
        public ArgumentFault(string message)
        {
            // Initialize the object.
            this.Message = message;
        }

        /// <summary>
        /// Gets the message of the fault.
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            private set;
        }
    }
}
