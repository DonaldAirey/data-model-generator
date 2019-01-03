﻿// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// The root namespace.
    /// </summary>
    public class Namespace
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The name of the namespace.</param>
        public Namespace(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;

            // This is the syntax of the namespace.
            this.Syntax = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(xmlSchemaDocument.TargetNamespace))
                .WithUsings(SyntaxFactory.List<UsingDirectiveSyntax>(this.UsingStatements))
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.LeadingTrivia);
        }

        /// <summary>
        /// Gets or sets gets the syntax.
        /// </summary>
        public NamespaceDeclarationSyntax Syntax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList LeadingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>();

                // // <auto-generated />
                trivia.Add(
                    SyntaxFactory.Comment("// <auto-generated />"));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(trivia).NormalizeWhitespace();
            }
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreatePublicClasses(members);
                return members;
            }
        }

        /// <summary>
        /// Gets the 'using' statements.
        /// </summary>
        private List<UsingDirectiveSyntax> UsingStatements
        {
            get
            {
                // Create the 'using' statements.
                // [TODO] Make the addition of user namespaces part of the initialization.  Run through the tables and extract the
                // namespaces from the types found therein.
                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>();
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections.Generic")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Linq")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Transactions")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("GammaFour.Common")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.AspNetCore.Mvc")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.EntityFrameworkCore")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.Extensions.Configuration")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Newtonsoft.Json.Linq")));
                return usingStatements;
            }
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicClasses(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Create a controller for each table in the schema.
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                members = members.Add(new RestServiceClass.Class(tableElement).Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}