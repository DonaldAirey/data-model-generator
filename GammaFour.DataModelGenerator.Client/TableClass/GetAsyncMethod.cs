// <copyright file="GetAsyncMethod.cs" company="Gamma Four, Inc.">
//    Copyright � 2022 - Gamma Four, Inc.  All Rights Reserved.
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
    /// Creates a method to add a record to the set.
    /// </summary>
    public class GetAsyncMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAsyncMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public GetAsyncMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "GetAsync";

            //        /// <summary>
            //        /// Gets a <see cref="Fungible"/> record from the shared data model.
            //        /// </summary>
            //        /// <returns>The requested record.</returns>
            //        public async Task<Fungible> GetAsync(int fungibleId)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("Task"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName(this.tableElement.Name)))),
                SyntaxFactory.Identifier(this.Name))
                .WithModifiers(GetAsyncMethod.Modifiers)
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
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private SyntaxList<CatchClauseSyntax> CatchClauses
        {
            get
            {
                // The catch clauses are collected in this list.
                List<CatchClauseSyntax> clauses = new List<CatchClauseSyntax>();

                //            catch (Exception exception)
                //            {
                //                <HandleDbUpdateException>
                //            }
                clauses.Add(
                    SyntaxFactory.CatchClause()
                    .WithDeclaration(
                        SyntaxFactory.CatchDeclaration(
                            SyntaxFactory.IdentifierName("Exception"))
                        .WithIdentifier(
                            SyntaxFactory.Identifier("exception")))
                    .WithBlock(this.HandleException));

                // This is the collection of catch clauses.
                return SyntaxFactory.List<CatchClauseSyntax>(clauses);
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax HandleException
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                logger.LogError(exception, "Error while processing {table}.GetAsync() from {host}", "AccountCanonMap", this.httpClient.BaseAddress);
                statements.Add(
                     SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("logger"),
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
                                                SyntaxFactory.Literal("Error while processing {table}.GetAsync() from {host}"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(this.tableElement.Name))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("httpClient")),
                                                SyntaxFactory.IdentifierName("BaseAddress"))),
                                    })))));

                //                throw exception;
                statements.Add(
                    SyntaxFactory.ThrowStatement(
                        SyntaxFactory.IdentifierName("exception")));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(statements);
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

                //            try
                //            {
                //                <TryBlock>
                //            }
                //            catch
                //            {
                //                <CatchClauses>
                //            }
                statements.Add(
                    SyntaxFactory.TryStatement(this.CatchClauses)
                    .WithBlock(
                        SyntaxFactory.Block(this.TryBlock)));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
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
                //        /// Adds a <see cref="Buyer"/> to the set.
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
                                                $" Gets a <see cref=\"{this.tableElement.Name}\"/> record from the shared data model.",
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
                                                    $" <returns>The requested record.</returns>",
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
                List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

                // string countryCode
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    if (parameters.Count != 0)
                    {
                        parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    parameters.Add(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnReferenceElement.Column.Name.ToVariableName()))
                        .WithType(Conversions.FromType(columnReferenceElement.Column.ColumnType)));
                }

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> TryBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                // $"rest/accountGroups/{provinceId}/{regionId}"
                List<InterpolatedStringContentSyntax> interpolatedStringContentSyntax = new List<InterpolatedStringContentSyntax>();
                interpolatedStringContentSyntax.Add(
                    SyntaxFactory.InterpolatedStringText()
                    .WithTextToken(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.InterpolatedStringTextToken,
                            $"rest/{this.tableElement.Name.ToCamelCase().ToPlural()}/",
                            $"rest/{this.tableElement.Name.ToCamelCase().ToPlural()}/",
                            SyntaxFactory.TriviaList())));
                foreach (ColumnReferenceElement columnReferenceElement in this.tableElement.PrimaryKey.Columns)
                {
                    if (interpolatedStringContentSyntax.Count > 1)
                    {
                        interpolatedStringContentSyntax.Add(
                            SyntaxFactory.InterpolatedStringText()
                            .WithTextToken(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.InterpolatedStringTextToken,
                                    "/",
                                    "/",
                                    SyntaxFactory.TriviaList())));
                    }

                    interpolatedStringContentSyntax.Add(SyntaxFactory.Interpolation(SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name.ToVariableName())));
                }

                //                var requestUri = new Uri($"rest/accountGroups/{provinceId}/{regionId}", UriKind.Relative);
                statements.Add(
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(
                                SyntaxFactory.Identifier(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.VarKeyword,
                                    "var",
                                    "var",
                                    SyntaxFactory.TriviaList())))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("requestUri"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("Uri"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.InterpolatedStringExpression(
                                                                SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                                                            .WithContents(
                                                                SyntaxFactory.List<InterpolatedStringContentSyntax>(
                                                                    interpolatedStringContentSyntax.ToArray()))),
                                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("UriKind"),
                                                                SyntaxFactory.IdentifierName("Relative"))),
                                                    })))))))));

                //                using (HttpResponseMessage response = await this.httpClient.GetAsync(requestUri).ConfigureAwait(false))
                //                {
                //                    <UsingBlock>
                //                }
                statements.Add(
                    SyntaxFactory.UsingStatement(
                        SyntaxFactory.Block(this.UsingBlock))
                    .WithDeclaration(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName("HttpResponseMessage"))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier("response"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.AwaitExpression(
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName("httpClient")),
                                                            SyntaxFactory.IdentifierName("GetAsync")))
                                                    .WithArgumentList(
                                                        SyntaxFactory.ArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                SyntaxFactory.Argument(
                                                                    SyntaxFactory.IdentifierName("requestUri"))))),
                                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.FalseLiteralExpression))))))))))));

                // This is the complete block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        private List<StatementSyntax> UsingBlock
        {
            get
            {
                // This is used to collect the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    response.EnsureSuccessStatusCode();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("response"),
                                SyntaxFactory.IdentifierName("EnsureSuccessStatusCode")))));

                //                    return JsonConvert.DeserializeObject<Fungible>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("JsonConvert"),
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("DeserializeObject"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(this.tableElement.Name))))))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.AwaitExpression(
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.InvocationExpression(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName("response"),
                                                                SyntaxFactory.IdentifierName("Content")),
                                                            SyntaxFactory.IdentifierName("ReadAsStringAsync"))),
                                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.FalseLiteralExpression))))))))))));

                // This is the complete block.
                return statements;
            }
        }
    }
}