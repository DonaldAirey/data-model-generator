// <copyright file="PrepareMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordCollectionClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to prepare a resource for a transaction completion.
    /// </summary>
    public class PrepareMethod : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareMethod"/> class.
        /// </summary>
        public PrepareMethod()
        {
            // Initialize the object.
            this.Name = "Prepare";

            //        /// <inheritdoc/>
            //        public void Prepare(Enlistment enlistment)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(PrepareMethod.Modifiers)
            .WithParameterList(PrepareMethod.Parameters)
            .WithBody(PrepareMethod.Body)
            .WithLeadingTrivia(PrepareMethod.DocumentationComment);
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static BlockSyntax Body
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            if (this.undoStack.Count == 0)
                //            {
                //                <Enlistment is Done>
                //            }
                //            else
                //            {
                //                <Enlistment is Prepared>
                //            }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("undoStack")),
                                SyntaxFactory.IdentifierName("Count")),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(0))),
                        SyntaxFactory.Block(PrepareMethod.EnlistmentDone))
                    .WithElse(
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.Block(PrepareMethod.EnlistmentPrepared))));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <inheritdoc/>
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
                                                " <inheritdoc/>",
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
        /// Gets the block of code that says the transaction is done.
        /// </summary>
        private static IEnumerable<StatementSyntax> EnlistmentDone
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                preparingEnlistment.Done();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("preparingEnlistment"),
                                SyntaxFactory.IdentifierName("Done")))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the block of code that says the transaction is prepared.
        /// </summary>
        private static IEnumerable<StatementSyntax> EnlistmentPrepared
        {
            get
            {
                //                preparingEnlistment.Prepared();
                return SyntaxFactory.SingletonList<StatementSyntax>(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("preparingEnlistment"),
                                SyntaxFactory.IdentifierName("Prepared")))));
            }
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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private static ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters from the columns in the unique constraint.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // PreparingEnlistment preparingEnlistment
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("preparingEnlistment"))
                    .WithType(
                        SyntaxFactory.IdentifierName("PreparingEnlistment")));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}