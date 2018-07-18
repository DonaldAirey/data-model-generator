// <copyright file="CascadeRules.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    /// <summary>
    /// The cascade rules for relations.
    /// </summary>
    public enum CascadeRules
    {
        /// <summary>
        /// Child records are not adjusted when a parent record is modified.
        /// </summary>
        None,

        /// <summary>
        /// Child records are reconciled to the parent record when modified.
        /// </summary>
        Cascade,

        /// <summary>
        /// The reference to the parent record is set to null.
        /// </summary>
        SetNull,

        /// <summary>
        /// The reference to the parent record is set to a default.
        /// </summary>
        SetDefault
    }
}
