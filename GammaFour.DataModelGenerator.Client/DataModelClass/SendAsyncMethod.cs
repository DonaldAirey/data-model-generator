// <copyright file="SendAsyncMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    /// <summary>
    /// Creates a method to merge a record.
    /// </summary>
    public class SendAsyncMethod : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendAsyncMethod"/> class.
        /// </summary>
        public SendAsyncMethod()
        {
            // Initialize the object.
            this.Name = "SendAsync";

            //        /// <summary>
            //        /// Send the request.
            //        /// </summary>
            //        /// <param name="httpRequestMessage">The HTTP request message.</param>
            //        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            //        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
            //        {
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("Task"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName("HttpResponseMessage")))),
                SyntaxFactory.Identifier("SendAsync"))
            .WithModifiers(SendAsyncMethod.Modifiers)
            .WithParameterList(SendAsyncMethod.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(SendAsyncMethod.DocumentationComment);
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
                    //        /// Send the request.
                    //        /// </summary>
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SingletonList<XmlNodeSyntax>(
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        new[]
                                        {
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " <summary>",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " Send the request.",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " </summary>",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                        }))))),

                    //        /// <param name="jsonObject">The JSON object containg the incremental data.</param>
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SingletonList<XmlNodeSyntax>(
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            new[]
                                            {
                                                XmlTextLiteral(
                                                    TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                    $" <param name=\"httpRequestMessage\">The HTTP request message.</param>",
                                                    string.Empty,
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    TriviaList()),
                                            }))))),

                    //        /// <returns>The set of fungibles.</returns>
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
                                                    $" <returns>A <see cref=\"Task\"/> representing the asynchronous operation.</returns>",
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
                return TriviaList(comments);
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
                return TokenList(
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
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
                // Create a list of parameters.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>
                {
                    // HttpRequestMessage httpRequestMessage
                    Parameter(
                        Identifier("httpRequestMessage"))
                    .WithType(
                        IdentifierName("HttpRequestMessage")),
                };

                // This is the complete parameter specification for this constructor.
                return ParameterList(SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
                {
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("httpClient")),
                                SyntaxFactory.IdentifierName("SendAsync")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("httpRequestMessage")))))),
                };

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }
    }
}