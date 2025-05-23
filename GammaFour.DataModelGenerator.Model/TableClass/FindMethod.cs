// <copyright file="FindMethod.cs" company="Gamma Four, Inc.">
//    Copyright � 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to find a row in the index.
    /// </summary>
    public class FindMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public FindMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Find";

            //        /// <summary>
            //        /// Finds the row with the given key.
            //        /// </summary>
            //        /// <param name="code">The code.</param>
            //        /// <param name="name">The name.</param>
            //        /// <returns>The row if found, null if the key doesn't exist.</returns>
            //        public Account? Find(string code, string name)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.NullableType(
                    SyntaxFactory.IdentifierName(this.tableElement.Name)),
                SyntaxFactory.Identifier("Find"))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        this.tableElement.PrimaryIndex.GetKeyAsParameters())))
            .WithBody(this.Body)
            .WithLeadingTrivia(this.LeadingTrivia);
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                var statements = new List<StatementSyntax>();

                //            return this.dictionary.TryGetValue(code, out var account) ? account : null;
                var variableName = this.tableElement.Name.ToVariableName();
                var findRowStatement = SyntaxFactory.ReturnStatement(
                    SyntaxFactory.ConditionalExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dictionary")),
                                SyntaxFactory.IdentifierName("TryGetValue")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                    this.tableElement.PrimaryIndex.GetKeyAsArguments(),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.DeclarationExpression(
                                            SyntaxFactory.IdentifierName(
                                                SyntaxFactory.Identifier(
                                                    SyntaxFactory.TriviaList(),
                                                    SyntaxKind.VarKeyword,
                                                    "var",
                                                    "var",
                                                    SyntaxFactory.TriviaList())),
                                            SyntaxFactory.SingleVariableDesignation(
                                                SyntaxFactory.Identifier(variableName))))
                                    .WithRefOrOutKeyword(
                                        SyntaxFactory.Token(SyntaxKind.OutKeyword)),
                                    }))),
                        SyntaxFactory.IdentifierName(variableName),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)));

                var condition = this.tableElement.PrimaryIndex.GetKeyAsEqualityConditional();
                if (condition == null)
                {
                    //            return this.dictionary.TryGetValue(code, out var account) ? account : null;
                    statements.Add(findRowStatement);
                }
                else
                {
                    //            if (symbol != null)
                    //            {
                    //              return this.dictionary.TryGetValue(code, out var account) ? account : null;
                    //            }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            condition,
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ReturnStatement(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)))))
                        .WithElse(
                            SyntaxFactory.ElseClause(
                                SyntaxFactory.Block(findRowStatement))));
                }

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private IEnumerable<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                var trivia = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Finds the row with the given key.
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
                                                $" Finds a <see cref=\"{this.tableElement.Name}\"/> row using the unique key.",
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

                foreach (var columnElementReference in this.tableElement.PrimaryIndex.Columns)
                {
                    //        /// <param name="code">The code.</param>
                    var columnElement = columnElementReference.Column;
                    trivia.Add(
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
                                                    $" <param name=\"{columnElement.Name.ToCamelCase()}\">The {columnElement.Name.ToCamelCase()}.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                            }))))));
                }

                //        /// <returns>The found <see cref="Thing"/> row, or null if not found.</returns>
                trivia.Add(
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
                                                $" <returns>The found <see cref=\"{this.tableElement.Name}\"/> row, or null if not found.</returns>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(trivia);
            }
        }
    }
}