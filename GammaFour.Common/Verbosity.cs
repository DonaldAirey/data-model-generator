// <copyright file="Verbosity.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    /// <summary>
    /// Describes how much information is emitted during the processing of the scripts.
    /// </summary>
    public enum Verbosity
    {
        /// <summary>
        /// No progress messages.
        /// </summary>
        Quiet,

        /// <summary>
        /// Minimal progress messages.
        /// </summary>
        Minimal,

        /// <summary>
        /// Descriptive progress message.
        /// </summary>
        Verbose
    }
}
