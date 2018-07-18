// <copyright file="DebugService.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.ServerGenerator
{
    using System;
    using System.Collections.Generic;
    using DarkBond.DataModelGenerator;

    /// <summary>
    /// 
    /// </summary>
    class ServerCompilerSite : IServerCompilerSite
    {
        private List<String> librariesField;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCompilerSite"/> class.
        /// </summary>
        /// <param name="libraries"></param>
        public ServerCompilerSite(List<String> libraries)
        {
            // Initialize the object.
            this.librariesField = libraries;
        }

        public List<String> Libraries
        {
            get
            {
                return this.librariesField;
            }
        }
    }
}
