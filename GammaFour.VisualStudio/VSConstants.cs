// <copyright file="VSConstants.cs" company="Theta Rex, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
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
        public const int S_OK = 0;

        /// <summary>
        /// Error HRESULT for a generic failure.
        /// </summary>
        public const int E_FAIL = -2147467259;

        /// <summary>
        /// Error HRESULT for the request of a not implemented interface.
        /// </summary>
        public const int E_NOINTERFACE = -2147467262;

    }
}
