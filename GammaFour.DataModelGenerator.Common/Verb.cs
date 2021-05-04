// <copyright file="Verb.cs" company="Gamma Four, Inc.">
//    Copyright © 2021 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    /// <summary>
    /// The HTTP verbs.
    /// </summary>
    public enum Verb
    {
        /// <summary>
        /// Delete a resource from the domain.
        /// </summary>
        Delete,

        /// <summary>
        /// Get one or more resources from the domain.
        /// </summary>
        Get,

        /// <summary>
        /// Put a resource in the domain.
        /// </summary>
        Put,

        /// <summary>
        /// Post a resource to the domain.
        /// </summary>
        Post,
    }
}