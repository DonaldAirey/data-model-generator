// <copyright file="CompilationUnit.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client
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
        /// The data model schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationUnit"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The data model schema.</param>
        public CompilationUnit(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
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
                Namespace @namespace = new Namespace(this.xmlSchemaDocument);
                return SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@namespace.Syntax);
            }
        }
    }
}