// <copyright file="IndexProperty.cs" company="Gamma Four, Inc.">
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
    /// Creates a property that returns the object at the given index.
    /// </summary>
    public class IndexProperty : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexProperty"/> class.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        public IndexProperty(TableElement tableElement)
        {
            //        /// <summary>
            //        /// Gets the <see cref="Allocation"/> at the given index.
            //        /// </summary>
            //        public Allocation this[int index];
            this.Syntax = SyntaxFactory.IndexerDeclaration(
                    SyntaxFactory.IdentifierName(tableElement.Name))
                .WithModifiers(IndexProperty.Modifiers)
                .WithParameterList(IndexProperty.Parameters)
                .WithAccessorList(IndexProperty.AccessorList)
                .WithLeadingTrivia(IndexProperty.DocumentationComment);
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
                            IndexProperty.GetAccessor,
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
                //            get
                //            {
                //                 return this.collection.Values[index];
                //            }
                return SyntaxFactory.AccessorDeclaration(
                    SyntaxKind.GetAccessorDeclaration)
                .WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList<StatementSyntax>(
                            SyntaxFactory.ReturnStatement(
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("collection")),
                                    SyntaxFactory.IdentifierName("Values")))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("index")))))))));
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
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Gets the Name.
                    //        /// </summary>
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
                                                " Gets the Name.",
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
                                        }))))),
                };

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

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private static BracketedParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters from the columns in the unique constraint.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>
                {
                    // DataModelContext dataModelContext
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("index"))
                    .WithType(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.IntKeyword))),
                };

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.BracketedParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}