﻿// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server
{
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
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>
                {
                    // // <auto-generated />
                    SyntaxFactory.Comment("// <auto-generated />"),

                    // #pragma warning disable SA1402
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1402")))),

                    // #pragma warning disable SA1649
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1649")))),
                };

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
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>
                {
                    // #pragma warning restore SA1402
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1402")))),

                    // #pragma warning restore SA1649
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1649")))),
                };

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
                List<UsingDirectiveSyntax> systemUsingStatements = new List<UsingDirectiveSyntax>
                {
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections.Generic")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Text.Json.Serialization")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Transactions")),
                };

                // System.Linq is only needed with the persistent model.
                if (!this.xmlSchemaDocument.IsVolatile)
                {
                    systemUsingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Linq")));
                }

                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>
                {
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("DotNext.Threading")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("GammaFour.Data.Server")),
                };

                if (!this.xmlSchemaDocument.IsVolatile)
                {
                    usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.Extensions.Configuration")));
                    usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("Microsoft.EntityFrameworkCore")));
                }

                // This sorts and combines the two lists. The 'System' namespace comes before the rest.
                return systemUsingStatements.OrderBy(ud => ud.Name.ToString()).Concat(usingStatements.OrderBy(ud => ud.Name.ToString())).ToList();
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
            List<RowClass.Class> recordClasses = new List<RowClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                recordClasses.Add(new RowClass.Class(tableElement));
            }

            // Alphabetize the list of record classes and add them to the structure.
            foreach (RowClass.Class recordClass in recordClasses.OrderBy(c => c.Name))
            {
                members = members.Add(recordClass.Syntax);
            }

            // Create the record set classes.
            List<TableClass.Class> recordCollectionClasses = new List<TableClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                recordCollectionClasses.Add(new TableClass.Class(tableElement));
            }

            // Alphabetize the list of record sets and add them to the structure.
            foreach (TableClass.Class @class in recordCollectionClasses.OrderBy(c => c.Name))
            {
                members = members.Add(@class.Syntax);
            }

            // The actual data model class.
            members = members.Add(new DataModelClass.Class(this.xmlSchemaDocument).Syntax);

            // The non-volatile data model doesn't need the ORM infrastructure.
            if (!this.xmlSchemaDocument.IsVolatile)
            {
                // The DbContext class that provides access to the persistent store.
                members = members.Add(new DbContextClass.Class(this.xmlSchemaDocument).Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}