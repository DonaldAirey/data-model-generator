// <copyright file="CommonStatements.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A method to create a row.
    /// </summary>
    public static class CommonStatements
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        internal static SyntaxList<CatchClauseSyntax> CommonCatchClauses
        {
            get
            {
                // The catch clauses are collected in this list.
                List<CatchClauseSyntax> clauses = new List<CatchClauseSyntax>
                {
                    //            catch (ConcurrencyException concurrencyException)
                    //            {
                    //                <HandleConcurrencyException>
                    //            }
                    SyntaxFactory.CatchClause()
                        .WithDeclaration(
                            SyntaxFactory.CatchDeclaration(
                                SyntaxFactory.IdentifierName("ConcurrencyException"))
                            .WithIdentifier(
                                SyntaxFactory.Identifier("concurrencyException")))
                        .WithBlock(
                            SyntaxFactory.Block(CommonStatements.HandleConcurrencyException)),

                    //            catch (ConstraintException constraintException)
                    //            {
                    //                <HandleOperationCancelledException>
                    //            }
                    SyntaxFactory.CatchClause()
                        .WithDeclaration(
                            SyntaxFactory.CatchDeclaration(
                                SyntaxFactory.IdentifierName("ConstraintException"))
                            .WithIdentifier(
                                SyntaxFactory.Identifier("constraintException")))
                        .WithBlock(
                            SyntaxFactory.Block(CommonStatements.HandleConstraintException)),

                    //            catch (OperationCanceledException operationCanceledException)
                    //            {
                    //                <HandleOperationCancelledException>
                    //            }
                    SyntaxFactory.CatchClause()
                        .WithDeclaration(
                            SyntaxFactory.CatchDeclaration(
                                SyntaxFactory.IdentifierName("OperationCanceledException"))
                            .WithIdentifier(
                                SyntaxFactory.Identifier("operationCanceledException")))
                        .WithBlock(
                            SyntaxFactory.Block(CommonStatements.HandleOperationCanceledException)),

                    //            catch (Exception exception)
                    //            {
                    //                <HandleException>
                    //            }
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("Exception"))
                        .WithIdentifier(
                            SyntaxFactory.Identifier("exception")))
                    .WithBlock(
                        SyntaxFactory.Block(CommonStatements.HandleException)),
                };

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> HandleConstraintException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
                {
                    //                this.logger.LogError(constraintException, "{message}", constraintException.Message);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("logger")),
                                SyntaxFactory.IdentifierName("LogError")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("constraintException")),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal("{message}"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("constraintException"),
                                                SyntaxFactory.IdentifierName("Message"))),
                                    })))),

                    ////                return this.StatusCode(StatusCodes.Status422UnprocessableEntity);
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("StatusCode")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("StatusCodes"),
                                            SyntaxFactory.IdentifierName("Status422UnprocessableEntity"))))))),
                };

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> HandleConcurrencyException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
                {
                    //                this.logger.LogError(concurrencyException, "{message}", concurrencyException.Message);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("logger")),
                                SyntaxFactory.IdentifierName("LogError")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("concurrencyException")),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal("{message}"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("concurrencyException"),
                                                SyntaxFactory.IdentifierName("Message"))),
                                    })))),

                    ////                return this.StatusCode(StatusCodes.Status412PreconditionFailed);
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("StatusCode")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("StatusCodes"),
                                            SyntaxFactory.IdentifierName("Status412PreconditionFailed"))))))),
                };

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> HandleException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
                {
                    //                this.logger.LogError(exception, "{message}", exception.Message);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("logger")),
                                SyntaxFactory.IdentifierName("LogError")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("exception")),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal("{message}"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("exception"),
                                                SyntaxFactory.IdentifierName("Message"))),
                                    })))),

                    //                return this.BadRequest($"{exception.GetType()}: {exception.Message}");
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("BadRequest")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.InterpolatedStringExpression(
                                            SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                        .WithContents(
                                            SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                new InterpolatedStringContentSyntax[]
                                                {
                                                    SyntaxFactory.Interpolation(
                                                        SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("exception"),
                                                                SyntaxFactory.IdentifierName("GetType")))),
                                                    SyntaxFactory.InterpolatedStringText()
                                                    .WithTextToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.InterpolatedStringTextToken,
                                                            ": ",
                                                            ": ",
                                                            SyntaxFactory.TriviaList())),
                                                    SyntaxFactory.Interpolation(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("exception"),
                                                            SyntaxFactory.IdentifierName("Message"))),
                                                }))))))),
                };

                // This is the syntax for the body of the method.
                return statements;
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private static List<StatementSyntax> HandleOperationCanceledException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
                {
                    //                this.logger.LogError(operationCanceledException, "{message}", operationCanceledException.Message);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("logger")),
                                SyntaxFactory.IdentifierName("LogError")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("operationCanceledException")),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal("{message}"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("operationCanceledException"),
                                                SyntaxFactory.IdentifierName("Message"))),
                                    })))),

                    ////                return this.StatusCode(StatusCodes.Status408RequestTimeout);
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("StatusCode")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("StatusCodes"),
                                            SyntaxFactory.IdentifierName("Status408RequestTimeout"))))))),
                };

                // This is the syntax for the body of the method.
                return statements;
            }
        }
    }
}