﻿// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2021 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The name of the namespace.</param>
        /// <param name="customToolNamespace">The namespace of the generated module.</param>
        public Namespace(XmlSchemaDocument xmlSchemaDocument, string customToolNamespace)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;

            // This is the syntax of the namespace.
            this.Syntax = SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.IdentifierName(customToolNamespace))
                .WithUsings(SyntaxFactory.List<UsingDirectiveSyntax>(this.UsingStatements))
                .WithMembers(this.Members)
                .WithLeadingTrivia(Namespace.LeadingTrivia)
                .WithTrailingTrivia(Namespace.TrailingTrivia);
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
        private static SyntaxTriviaList LeadingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>();

                // // <auto-generated />
                trivia.Add(
                    SyntaxFactory.Comment("// <auto-generated />"));

                // #pragma warning disable SA1402
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1402")))));

                // #pragma warning disable SA1649
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1649")))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(trivia).NormalizeWhitespace();
            }
        }

        /// <summary>
        /// Gets the trailing trivia.
        /// </summary>
        private static SyntaxTriviaList TrailingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>();

                // #pragma warning restore SA1402
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1402")))));

                // #pragma warning restore SA1649
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1649")))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(trivia).NormalizeWhitespace();
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
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections.Generic")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Linq")));

                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Linq.Expressions")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("GammaFour.Data")));
                if (!this.xmlSchemaDocument.IsVolatile.HasValue || !this.xmlSchemaDocument.IsVolatile.Value)
                {
                    usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.EntityFrameworkCore")));
                }

                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Newtonsoft.Json")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Newtonsoft.Json.Converters")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Newtonsoft.Json.Linq")));
                return usingStatements;
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
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicClasses(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Create the record classes.
            List<RecordClass.Class> recordClasses = new List<RecordClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                recordClasses.Add(new RecordClass.Class(tableElement));
            }

            // Alphabetize the list of record classes and add them to the structure.
            foreach (RecordClass.Class recordClass in recordClasses.OrderBy(c => c.Name))
            {
                members = members.Add(recordClass.Syntax);
            }

            // Create the record set classes.
            List<RecordSetClass.Class> recordSetClasses = new List<RecordSetClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                recordSetClasses.Add(new RecordSetClass.Class(tableElement));
            }

            // Alphabetize the list of record sets and add them to the structure.
            foreach (RecordSetClass.Class @class in recordSetClasses.OrderBy(c => c.Name))
            {
                members = members.Add(@class.Syntax);
            }

            // The actual data model class.
            members = members.Add(new DataModelClass.Class(this.xmlSchemaDocument).Syntax);

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}