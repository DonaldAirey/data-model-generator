// <copyright file="Verb.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
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
        /// Delete a resource from the dataModel.
        /// </summary>
        Delete,

        /// <summary>
        /// Get one or more resources from the dataModel.
        /// </summary>
        Get,

        /// <summary>
        /// Put a resource in the dataModel.
        /// </summary>
        Put,

        /// <summary>
        /// Post a resource to the dataModel.
        /// </summary>
        Post,
    }
}