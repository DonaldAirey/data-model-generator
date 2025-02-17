// <copyright file="ColumnField.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RowClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field that holds the column.
    /// </summary>
    public class ColumnField : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private readonly ColumnElement columnElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnField"/> class.
        /// </summary>
        /// <param name="columnElement">The column schema.</param>
        public ColumnField(ColumnElement columnElement)
        {
            // Initialize the object.
            this.columnElement = columnElement;

            // This is the name of the field.
            this.Name = this.columnElement.Name.ToCamelCase();

            //        private double quantity;
            //        private string code = string.Empty;
            var columnType = this.columnElement.ColumnType;
            var variableDeclarator = SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(this.Name));
            if (!columnType.IsValueType && !columnType.IsNullable)
            {
                variableDeclarator = SyntaxFactory.VariableDeclarator(
                    SyntaxFactory.Identifier(this.Name))
                .WithInitializer(
                    SyntaxFactory.EqualsValueClause(
                        Defaults.FromType(columnType)));
            }

            //        /// <summary>
            //        /// The code.
            //        /// </summary>
            //        private string code = string.Empty;
            this.Syntax = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    Conversions.FromType(columnElement.ColumnType))
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(variableDeclarator)))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
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
                //        /// Gets or sets Address1.
                //        /// </summary>
                string description = $" The {this.Name.ToCamelCase()}";
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
                                                description,
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
    }
}