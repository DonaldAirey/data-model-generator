// <copyright file="IsReadLockHeldProperty.cs" company="Gamma Four, Inc.">
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
    /// Creates a property that describes the state.
    /// </summary>
    public class IsReadLockHeldProperty : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsReadLockHeldProperty"/> class.
        /// </summary>
        public IsReadLockHeldProperty()
        {
            // Initialize the object.
            this.Name = "IsReadLockHeld";

            //        /// <inheritdoc/>
            //        public bool IsReadLockHeld => this.asyncReaderWriterLock.IsReadLockHeld;
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithModifiers(IsReadLockHeldProperty.Modifiers)
                .WithExpressionBody(IsReadLockHeldProperty.ExpressionBody)
                .WithLeadingTrivia(IsReadLockHeldProperty.DocumentationComment)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
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
                    //        /// <inheritdoc/>
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
                                                " <inheritdoc/>",
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
                // public
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the expression body of the property.
        /// </summary>
        private static ArrowExpressionClauseSyntax ExpressionBody
        {
            get
            {
                // => this.asyncReaderWriterLock.IsReadLockHeld
                return SyntaxFactory.ArrowExpressionClause(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("asyncReaderWriterLock")),
                        SyntaxFactory.IdentifierName("IsReadLockHeld")));
            }
        }
    }
}