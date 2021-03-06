// <copyright file="UniqueKeyIndexProperty.cs" company="Gamma Four, Inc.">
//    Copyright � 2021 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordSetClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a unique key for the set.
    /// </summary>
    public class UniqueKeyIndexProperty : SyntaxElement
    {
        /// <summary>
        /// The unique key element.
        /// </summary>
        private readonly UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyIndexProperty"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique key element.</param>
        public UniqueKeyIndexProperty(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.
            this.uniqueKeyElement = uniqueKeyElement ?? throw new ArgumentNullException(nameof(uniqueKeyElement));
            this.Name = this.uniqueKeyElement.Name;

            //        /// <summary>
            //        /// Gets the BuyerExternalId0Key unique index.
            //        /// </summary>
            //        public UniqueKeyIndex<Buyer> BuyerExternalId0Key { get; } = new UniqueKeyIndex<Buyer>("BuyerExternalId0Key").HasIndex(b => b.ExternalId0);
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("UniqueKeyIndex"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name)))),
                    SyntaxFactory.Identifier(this.uniqueKeyElement.Name))
                .WithModifiers(UniqueKeyIndexProperty.Modifiers)
                .WithAccessorList(UniqueKeyIndexProperty.AccessorList)
                .WithInitializer(this.Initializer)
                .WithLeadingTrivia(this.DocumentationComment)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        /// <summary>
        /// Gets the list of accessors.
        /// </summary>
        private static AccessorListSyntax AccessorList
        {
            get
            {
                return SyntaxFactory.AccessorList(
                    SyntaxFactory.SingletonList<AccessorDeclarationSyntax>(
                        SyntaxFactory.AccessorDeclaration(
                            SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(
                            SyntaxFactory.Token(SyntaxKind.SemicolonToken))));
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
                //        /// Gets the BuyerExternalId0Key unique index.
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
                                                $" Gets the {this.uniqueKeyElement.Name} unique index.",
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
        /// Gets the initializer.
        /// </summary>
        private EqualsValueClauseSyntax Initializer
        {
            get
            {
                // .HasIndex(b => b.ExternalId0)
                // .HasIndex(p => ValueTuple.Create(p.Name, p.CountryCode))
                InvocationExpressionSyntax invocationExpressionSyntax = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("UniqueKeyIndex"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name)))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(this.uniqueKeyElement.Name)))))),
                            SyntaxFactory.IdentifierName("HasIndex")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(UniqueKeyExpression.GetUniqueKey(this.uniqueKeyElement)))));

                // This allows for unique indices that also allow nulls.
                // [TODO] make this work on compound nullable indices.
                if (this.uniqueKeyElement.IsNullable && this.uniqueKeyElement.Columns.Count == 1)
                {
                    // .HasFilter(s => s.Figi != null)
                    invocationExpressionSyntax = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            invocationExpressionSyntax,
                            SyntaxFactory.IdentifierName("HasFilter")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(NullableKeyFilterExpression.GetNullableKeyFilter(this.uniqueKeyElement)))));
                }

                // = new UniqueKeyIndex<Security>("SecurityFigiKey").HasIndex(s => s.Figi).HasFilter(s => s.Figi != null);
                return SyntaxFactory.EqualsValueClause(invocationExpressionSyntax);
            }
        }
    }
}