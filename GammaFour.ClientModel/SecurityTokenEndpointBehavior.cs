// <copyright file="SecurityTokenEndpointBehavior.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// The endpoint behavior for handling security tokens.
    /// </summary>
    public class SecurityTokenEndpointBehavior : IEndpointBehavior
    {
        /// <summary>
        /// The security token.
        /// </summary>
        private ClientSecurityToken clientSecurityToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenEndpointBehavior"/> class.
        /// </summary>
        /// <param name="clientSecurityToken">The security token.</param>
        public SecurityTokenEndpointBehavior(ClientSecurityToken clientSecurityToken)
        {
            // Initialize the object.
            this.clientSecurityToken = clientSecurityToken;
        }

        /// <inheritdoc/>
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <inheritdoc/>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // Validate the parameter.
            if (clientRuntime == null)
            {
                throw new ArgumentNullException(nameof(clientRuntime));
            }

            // Add the security token message inspector to the list of inspectors.
            clientRuntime.ClientMessageInspectors.Add(new ClientMessageInspector(this.clientSecurityToken));
        }

        /// <inheritdoc/>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <inheritdoc/>
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}