// <copyright file="ICommunicationExceptionHandler.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Provides a handler for communication exceptions.
    /// </summary>
    public interface ICommunicationExceptionHandler
    {
        /// <summary>
        /// Handler for communication exceptions.
        /// </summary>
        /// <param name="exception">The exception that occurred during the operation.</param>
        /// <param name="operation">The operation where the exception occurred.</param>
        /// <returns>True indicates the operation should not be retried, false indicates it should.</returns>
        bool HandleException(Exception exception, string operation);
    }
}