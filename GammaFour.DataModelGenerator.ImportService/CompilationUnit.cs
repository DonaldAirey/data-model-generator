﻿// <copyright file="CompilationUnit.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.ImportService
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
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilationUnit"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The data model schema.</param>
        public CompilationUnit(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;

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
                Namespace @namespace = new Namespace(this.dataModelSchema);
                return SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@namespace.Syntax);
            }
        }
    }
}