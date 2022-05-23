// <copyright file="CreateContentMethod.cs" company="Gamma Four, Inc.">
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
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    /// <summary>
    /// Creates a method to merge a record.
    /// </summary>
    public class CreateContentMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContentMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public CreateContentMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "CreateContent";

            //        /// <summary>
            //        /// Creates JSON content.
            //        /// </summary>
            //        /// <param name="accounts">The collection of accounts.</param>
            //        /// <returns>The serialized content in a JSON format.</returns>
            //        private static HttpContent CreateContent(IEnumerable<Account> accounts)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.IdentifierName("HttpContent"),
                SyntaxFactory.Identifier("CreateContent"))
            .WithModifiers(CreateContentMethod.Modifiers)
            .WithParameterList(this.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // private static
                return TokenList(
                    new[]
                    {
                        Token(SyntaxKind.PrivateKeyword),
                        Token(SyntaxKind.StaticKeyword),
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
                //        /// Creates JSON content.
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
                                                " Creates JSON content.",
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

                //        /// <param name="accounts">The collection of accounts.</param>
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
                                                    $" <param name=\"{this.tableElement.Name.ToCamelCase().ToPlural()}\">The collection of {this.tableElement.Name.ToCamelCase().ToPlural()}.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                            }))))));

                //        /// <returns>The set of fungibles.</returns>
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
                                                    $" <returns>The serialized content in a JSON format.</returns>",
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
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>();

                // IEnumerable<Account> accounts
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier(this.tableElement.Name.ToCamelCase().ToPlural()))
                    .WithType(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("IEnumerable"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name))))));

                // This is the complete parameter specification for this constructor.
                return ParameterList(SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets the statements add the record to the residuals.
        /// </summary>
        private List<StatementSyntax> AddRecordToResiduals
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    residuals.Add(newBuyer);
                statements.Add(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("residuals"),
                                IdentifierName("Add")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList<ArgumentSyntax>(
                                    Argument(
                                        IdentifierName($"new{this.tableElement.Name}")))))));

                //                    continue;
                statements.Add(
                    ContinueStatement());

                return statements;
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
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(accounts));
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("HttpContent"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("httpContent"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("StringContent"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("JsonConvert"),
                                                                SyntaxFactory.IdentifierName("SerializeObject")))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToCamelCase().ToPlural())))))))))))))));

                //            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("httpContent"),
                                    SyntaxFactory.IdentifierName("Headers")),
                                SyntaxFactory.IdentifierName("ContentType")),
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.IdentifierName("MediaTypeHeaderValue"))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal("application/json")))))))));

                //            return httpContent;
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.IdentifierName("httpContent")));

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }
    }
}