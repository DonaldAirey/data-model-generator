﻿// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model
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

                    // #nullable true
                    SyntaxFactory.Trivia(
                        SyntaxFactory.NullableDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.EnableKeyword),
                            true)),
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
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Data")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Linq")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Text.Json.Serialization")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Transactions")),
                };

                // System.Linq is only needed with the persistent model.
                if (!this.xmlSchemaDocument.IsVolatile)
                {
                    systemUsingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")));
                }

                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>();
                if (!this.xmlSchemaDocument.IsVolatile)
                {
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
                members = this.CreatePublicEnums(members);
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
            List<SyntaxElement> syntaxElements = new List<SyntaxElement>();

            // The data model class.
            syntaxElements.Add(new DataModelClass.Class(this.xmlSchemaDocument));
            syntaxElements.Add(new RowChangedEventArgsClass.Class());

            // Create the row classes.
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                syntaxElements.Add(new RowClass.Class(tableElement));
            }

            // Create the table classes and non-primary unique indexes.
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                syntaxElements.Add(new TableClass.Class(tableElement));
                foreach (var uniqueIndexElement in tableElement.UniqueIndexes.Where(uie => !uie.IsPrimaryIndex))
                {
                    syntaxElements.Add(new UniqueIndexClass.Class(uniqueIndexElement));
                }
            }

            // The non-volatile data model doesn't need the ORM infrastructure.
            if (!this.xmlSchemaDocument.IsVolatile)
            {
                // The DbContext class that provides access to the persistent store.
                members = members.Add(new DbContextClass.Class(this.xmlSchemaDocument).Syntax);
            }

            // Alphabetize the list of classes and add them to the structure.
            foreach (var syntaxElement in syntaxElements.OrderBy(se => se.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }

        /// <summary>
        /// Creates the enums.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the enums.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicEnums(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Create the row classes.
            List<SyntaxElement> syntaxElements = new List<SyntaxElement>();
            syntaxElements.Add(new DataActionEnum.Enum());

            // Alphabetize the list of classes and add them to the structure.
            foreach (var syntaxElement in syntaxElements.OrderBy(se => se.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}