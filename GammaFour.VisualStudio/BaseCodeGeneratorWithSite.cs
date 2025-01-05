// <copyright file="BaseCodeGeneratorWithSite.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;

    /// <summary>
    /// Base code generator with site implementation.
    /// </summary>
    [ComVisible(true)]
    public abstract class BaseCodeGeneratorWithSite : BaseCodeGenerator, IObjectWithSite
    {
        /// <summary>
        /// The site.
        /// </summary>
        private object site;

        /// <summary>
        /// Gets the last site set with <see cref="IObjectWithSite.SetSite"/>. If there is no known site, the object returns a failure code.
        /// </summary>
        /// <param name="riid">The IID of the interface pointer that should be returned in ppvSite.</param>
        /// <param name="ppvSite">The address of the caller's void* variable in which the object stores the interface pointer of the site last seen in <see cref="IObjectWithSite.SetSite"/>.</param>
        public void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (this.site == null)
            {
                throw new COMException(Strings.ObjectNotSitedError, VSConstants.EFAIL);
            }

            var pUnknownPointer = Marshal.GetIUnknownForObject(this.site);
            IntPtr intPointer;
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero)
            {
                throw new COMException(Strings.InterfaceNotSupportedError, VSConstants.ENOINTERFACE);
            }

            // The address of the caller's void* variable.
            ppvSite = intPointer;
        }

        /// <summary>
        /// Provides the site's IUnknown pointer to the object.
        /// </summary>
        /// <param name="pUnkSite">An interface pointer to the site managing this object. If null, the object should call Release to release the existing site.</param>
        public void SetSite(object pUnkSite)
        {
            // Initialize the site.
            this.site = pUnkSite;
        }
    }
}