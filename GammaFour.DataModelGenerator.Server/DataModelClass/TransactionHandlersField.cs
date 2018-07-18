// <copyright file="TransactionHandlersField.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a collection of readers (transactions) waiting for a read lock.
    /// </summary>
    public class TransactionHandlersField : SyntaxElement
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHandlersField"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The data model schema.</param>
        public TransactionHandlersField(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;
            this.Name = "transactionHandlers";

            //        /// <summary>
            //        /// The vector to the transaction handlers for each of the tables.
            //        /// </summary>
            //        private Func<object[], bool>[] transactionHandlers = new Func<object[], bool>[8];
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.ArrayType(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("Func"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.ArrayType(
                                                SyntaxFactory.PredefinedType(
                                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                            .WithRankSpecifiers(
                                                SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                    SyntaxFactory.ArrayRankSpecifier(
                                                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                            SyntaxFactory.OmittedArraySizeExpression())))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.BoolKeyword))
                                        }))))
                        .WithRankSpecifiers(
                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                SyntaxFactory.ArrayRankSpecifier(
                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                        SyntaxFactory.OmittedArraySizeExpression())))))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.Name))
                            .WithInitializer(this.Initializer))))
                .WithModifiers(this.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
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
                //        /// The vector to the transaction handlers for each of the tables.
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
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " The vector to the transaction handlers for each of the tables.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList())
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
                // = new Func<object[], bool>[8];
                return SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.ArrayCreationExpression(
                        SyntaxFactory.ArrayType(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("Func"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SeparatedList<TypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.ArrayType(
                                                SyntaxFactory.PredefinedType(
                                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                            .WithRankSpecifiers(
                                                SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                    SyntaxFactory.ArrayRankSpecifier(
                                                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                            SyntaxFactory.OmittedArraySizeExpression())))),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.BoolKeyword))
                                        }))))
                        .WithRankSpecifiers(
                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                SyntaxFactory.ArrayRankSpecifier(
                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            SyntaxFactory.Literal(this.dataModelSchema.Tables.Count))))))));
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
                    });
            }
        }
    }
}