// <copyright file="ForeignKeyIndexProperty.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a property gives access to the foreign index.
    /// </summary>
    public class ForeignKeyIndexProperty : SyntaxElement
    {
        /// <summary>
        /// The foreign key description.
        /// </summary>
        private readonly ForeignKeyElement foreignKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyIndexProperty"/> class.
        /// </summary>
        /// <param name="foreignKeyElement">The column schema.</param>
        public ForeignKeyIndexProperty(ForeignKeyElement foreignKeyElement)
        {
            // Initialize the object.
            this.foreignKeyElement = foreignKeyElement;
            this.Name = this.foreignKeyElement.Name;

            //        /// <summary>
            //        /// Gets the CountryBuyerCountryIdKey foreign index.
            //        /// </summary>
            //        public ForeignKeyIndex<Buyer, Country> CountryBuyerCountryIdKey
            //        {
            //            <Get>
            //        }
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("ForeignKeyIndex"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList<TypeSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.UniqueKey.Table.Name),
                            }))),
                SyntaxFactory.Identifier(this.foreignKeyElement.Name))
            .WithModifiers(ForeignKeyIndexProperty.Modifiers)
            .WithAccessorList(ForeignKeyIndexProperty.AccessorList)
            .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the list of accessors.
        /// </summary>
        private static AccessorListSyntax AccessorList
        {
            get
            {
                return SyntaxFactory.AccessorList(
                    SyntaxFactory.List<AccessorDeclarationSyntax>(
                        new AccessorDeclarationSyntax[]
                        {
                            SyntaxFactory.AccessorDeclaration(
                                SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(
                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(
                                SyntaxKind.SetAccessorDeclaration)
                            .WithModifiers(
                                SyntaxFactory.TokenList(
                                    SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                            .WithSemicolonToken(
                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        }));
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // public
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Gets the CountryBuyerCountryIdKey foregn index.
                //        /// </summary>
                comments.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                SyntaxFactory.XmlText()
                                .WithTextTokens(
                                    SyntaxFactory.TokenList(
                                        new[]
                                        {
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                $" Gets the {this.foreignKeyElement.Name} foreign index.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }
    }
}