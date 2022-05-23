// <copyright file="PrimaryKeyFunctionField.cs" company="Gamma Four, Inc.">
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
    /// Creates a field to hold the current contents of the record.
    /// </summary>
    public class PrimaryKeyFunctionField : SyntaxElement
    {
        /// <summary>
        /// The description of the table.
        /// </summary>
        private readonly UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyFunctionField"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">The table element.</param>
        public PrimaryKeyFunctionField(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.
            this.Name = "primaryKeyFunction";
            this.uniqueKeyElement = uniqueKeyElement;

            //        /// <summary>
            //        /// Used to get the primary key from the record.
            //        /// </summary>
            //        private Func<Buyer, object> primaryKeyFunction = ((Expression<Func<Buyer, object>>)(b => b.BuyerId)).Compile();
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("Func"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SeparatedList<TypeSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.PredefinedType(
                                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
                                    }))))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier("primaryKeyFunction"))
                            .WithInitializer(this.Initializer))))
                .WithModifiers(PrimaryKeyFunctionField.Modifiers)
                .WithLeadingTrivia(PrimaryKeyFunctionField.DocumentationComment);
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                    });
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
                //        /// Used to get the primary key from the record.
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
                                                " Used to get the primary key from the record.",
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
                // = ((Expression<Func<Buyer, object>>)(b => b.BuyerId)).Compile();
                return SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ParenthesizedExpression(
                                SyntaxFactory.CastExpression(
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Expression"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("Func"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SeparatedList<TypeSyntax>(
                                                            new SyntaxNodeOrToken[]
                                                            {
                                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.PredefinedType(
                                                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
                                                            })))))),
                                    SyntaxFactory.ParenthesizedExpression(
                                        UniqueKeyExpression.GetUniqueKey(this.uniqueKeyElement)))),
                            SyntaxFactory.IdentifierName("Compile"))));
            }
        }
    }
}