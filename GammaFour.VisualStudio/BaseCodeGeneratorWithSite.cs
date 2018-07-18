// <copyright file="BaseCodeGeneratorWithSite.cs" company="Dark Bond, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;

    /// <summary>
    /// Base code generator with site implementation
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
        void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (this.site == null)
            {
                throw new COMException("object is not sited", VSConstants.E_FAIL);
            }

            var pUnknownPointer = Marshal.GetIUnknownForObject(this.site);
            IntPtr intPointer;
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero)
            {
                throw new COMException("site does not support requested interface", VSConstants.E_NOINTERFACE);
            }

            // The address of the caller's void* variable.
            ppvSite = intPointer;
        }

        /// <summary>
        /// Provides the site's IUnknown pointer to the object.
        /// </summary>
        /// <param name="pUnkSite">An interface pointer to the site managing this object. If null, the object should call Release to release the existing site</param>
        void IObjectWithSite.SetSite(object pUnkSite)
        {
            // Initialize the site.
            this.site = pUnkSite;
        }
    }
}