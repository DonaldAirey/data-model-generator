// <copyright file="FindMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.UniqueIndexClass
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
        private readonly UniqueIndexElement uniqueIndexElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindMethod"/> class.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique constraint schema.</param>
        public FindMethod(UniqueIndexElement uniqueIndexElement)
        {
            // Initialize the object.
            this.uniqueIndexElement = uniqueIndexElement;
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
                    SyntaxFactory.IdentifierName(this.uniqueIndexElement.Table.Name)),
                SyntaxFactory.Identifier("Find"))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(this.uniqueIndexElement.GetKeyAsParameters())
            .WithBody(this.Body)
            .WithLeadingTrivia(this.DocumentationComment);
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
                var variableName = this.uniqueIndexElement.Table.Name.ToVariableName();
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
                                    this.uniqueIndexElement.GetKeyAsArguments(),
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

                var condition = this.uniqueIndexElement.GetKeyAsEqualityConditional();
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
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Finds the row with the given key.
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
                                            $" Finds a <see cref=\"{this.uniqueIndexElement.Table.Name}\"/> row using the unique key.",
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

                foreach (var columnElementReference in this.uniqueIndexElement.Columns)
                {
                    //        /// <param name="code">The code.</param>
                    var columnElement = columnElementReference.Column;
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

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }
    }
}