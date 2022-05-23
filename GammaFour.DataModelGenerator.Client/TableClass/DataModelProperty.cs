// <copyright file="DataModelProperty.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field that holds the column.
    /// </summary>
    public class DataModelProperty : SyntaxElement
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelProperty"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The column schema.</param>
        public DataModelProperty(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = this.xmlSchemaDocument.Name;

            //        /// <summary>
            //        /// Gets the DataModel.
            //        /// </summary>
            //        public DataModel DataModel { get; private set; }
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.IdentifierName(this.xmlSchemaDocument.Name),
                    SyntaxFactory.Identifier(this.Name))
                .WithAccessorList(DataModelProperty.AccessorList)
                .WithModifiers(DataModelProperty.Modifiers)
                .WithLeadingTrivia(DataModelProperty.DocumentationComment);
        }

        /// <summary>
        /// Gets the list of accessors.
        /// </summary>
        private static AccessorListSyntax AccessorList
        {
            get
            {
                return SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                        new AccessorDeclarationSyntax[]
                        {
                            DataModelProperty.GetAccessor,
                            DataModelProperty.SetAccessor,
                        }));
            }
        }

        /// <summary>
        /// Gets the 'Get' accessor.
        /// </summary>
        private static AccessorDeclarationSyntax GetAccessor
        {
            get
            {
                // get;
                return SyntaxFactory
                    .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(
                        SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }
        }

        /// <summary>
        /// Gets the 'Set' accessor.
        /// </summary>
        private static AccessorDeclarationSyntax SetAccessor
        {
            get
            {
                // internal set;
                return SyntaxFactory
                    .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(SyntaxKind.InternalKeyword)))
                    .WithSemicolonToken(
                        SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Gets the DataModel.
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
                                                " Gets the data model.",
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

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // internal
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    });
            }
        }
    }
}