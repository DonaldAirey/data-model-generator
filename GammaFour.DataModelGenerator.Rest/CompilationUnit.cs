// <copyright file="CompilationUnit.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A complete unit for the compiler.
    /// </summary>
    public class CompilationUnit
    {
        /// <summary>
        /// The namespace for the generated module.
        /// </summary>
        private readonly string customToolNamespace;

        /// <summary>
        /// The data model schema.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationUnit"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The data model schema.</param>
        /// <param name="customToolNamespace">The namespace of the generated code.</param>
        public CompilationUnit(XmlSchemaDocument xmlSchemaDocument, string customToolNamespace)
        {
            // Initialize the object.
            this.customToolNamespace = customToolNamespace;
            this.xmlSchemaDocument = xmlSchemaDocument;

            // This is the syntax for the compilation unit.
            this.Syntax = SyntaxFactory.CompilationUnit().WithMembers(this.Members);
        }

        /// <summary>
        /// Gets or sets the syntax.
        /// </summary>
        public CSharpSyntaxNode Syntax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // The compilation unit consists of a single namespace.
                Namespace @namespace = new Namespace(this.xmlSchemaDocument, this.customToolNamespace);
                return SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@namespace.Syntax);
            }
        }
    }
}