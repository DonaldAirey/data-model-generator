// <copyright file="SyntaxElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Used for naming syntax elements and generating code.
    /// </summary>
    public class SyntaxElement
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the syntax for the method.
        /// </summary>
        public MemberDeclarationSyntax Syntax
        {
            get;
            protected set;
        }
    }
}