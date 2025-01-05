// <copyright file="VSConstants.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    /// <summary>
    /// A proxy for the VSConstants in the VS Shell library which is really, really bloated.
    /// </summary>
    public sealed class VSConstants
    {
        /// <summary>
        /// HRESULT for generic success.
        /// </summary>
        public const int SOK = 0;

        /// <summary>
        /// Error HRESULT for a generic failure.
        /// </summary>
        public const int EFAIL = -2147467259;

        /// <summary>
        /// Error HRESULT for the request of a not implemented interface.
        /// </summary>
        public const int ENOINTERFACE = -2147467262;
    }
}
