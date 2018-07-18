// <copyright file="MessageEventArgs.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Event data for progress messages.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs"/> class.
        /// </summary>
        /// <param name="message">The event message.</param>
        /// <param name="arguments">The arguments for constructing the message.</param>
        public MessageEventArgs(string message, params object[] arguments)
        {
            // Initialize the object.
            this.IsProgressTick = false;
            this.Message = string.Format(CultureInfo.CurrentCulture, message, arguments);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs"/> class.
        /// </summary>
        /// <param name="isProgressTick">Indicates the event is a tick.</param>
        /// <param name="message">The event message.</param>
        /// <param name="arguments">The arguments for constructing the message.</param>
        public MessageEventArgs(bool isProgressTick, string message, params object[] arguments)
        {
            // Initialize the object.
            this.IsProgressTick = isProgressTick;
            this.Message = string.Format(CultureInfo.CurrentCulture, message, arguments);
        }

        /// <summary>
        /// Gets the event message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets a value indicating whether progress was made since the last message.
        /// </summary>
        public bool IsProgressTick { get; private set; }
    }
}
