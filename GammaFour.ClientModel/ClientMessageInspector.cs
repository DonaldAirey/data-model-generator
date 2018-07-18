// <copyright file="ClientMessageInspector.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Inspects the incoming and outgoing messages.
    /// </summary>
    public class ClientMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// The security token.
        /// </summary>
        private ClientSecurityToken clientSecurityToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMessageInspector"/> class.
        /// </summary>
        /// <param name="clientSecurityToken">The security token used to authenticate the client.</param>
        public ClientMessageInspector(ClientSecurityToken clientSecurityToken)
        {
            // Initialize the object.
            this.clientSecurityToken = clientSecurityToken;
        }

        /// <inheritdoc/>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        /// <inheritdoc/>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Validate the parameter.
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // This places the security token in the header of the message.
            MessageHeader<string> tokenHeader = new MessageHeader<string>(this.clientSecurityToken.ToString());
            MessageHeader messageHeader = tokenHeader.GetUntypedHeader("securityToken", "https://www.darkbond.com/security");
            request.Headers.Add(messageHeader);
            return null;
        }
    }
}