// <copyright file="ClientSecurityToken.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    /// <summary>
    /// The security token from the Azure Active Directory.
    /// </summary>
    public class ClientSecurityToken
    {
        /// <summary>
        /// Gets or sets the security token.
        /// </summary>
        public object Value { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}