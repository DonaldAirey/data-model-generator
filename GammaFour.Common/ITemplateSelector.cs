// <copyright file="ITemplateSelector.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Used by a template selector to connect a view model to the template used to display it.
    /// </summary>
    public interface ITemplateSelector
    {
        /// <summary>
        /// Gets the name of the data template used to present the data.
        /// </summary>
        string Key { get; }
    }
}
