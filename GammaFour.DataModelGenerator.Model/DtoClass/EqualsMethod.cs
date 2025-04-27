// <copyright file="EqualsMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.DtoClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to acquire a reader lock.
    /// </summary>
    public class EqualsMethod : SyntaxElement
    {
        /// <summary>
        /// The table element.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        public EqualsMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Equals";

            //        /// <inheritdoc/>
            //        public override bool Equals(object? obj)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.OverrideKeyword),
                    }))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier("obj"))
                        .WithType(
                            SyntaxFactory.NullableType(
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))))))
            .WithBody(this.Body)
            .WithLeadingTrivia(EqualsMethod.LeadingTrivia);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static IEnumerable<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <inheritdoc/>
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
                                        }))))),
                };

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>();

                // this.AccountCode == other.AccountCode && this.AssetCode == other.AssetCode && this.Date == other.Date;
                ExpressionSyntax previousExpressions = null;
                foreach (var colmnReferenceElement in this.tableElement.PrimaryIndex.Columns.Reverse<ColumnReferenceElement>())
                {
                    var columnElement = colmnReferenceElement.Column;
                    if (previousExpressions == null)
                    {
                        previousExpressions = SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("other"),
                                SyntaxFactory.IdentifierName(columnElement.Name)));
                    }
                    else
                    {
                        previousExpressions = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("other"),
                                    SyntaxFactory.IdentifierName(columnElement.Name))),
                            previousExpressions);
                    }
                }

                //            return obj is Order other && this.AccountCode == other.AccountCode && this.AssetCode == other.AssetCode && this.Date == other.Date;
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            SyntaxFactory.IsPatternExpression(
                                SyntaxFactory.IdentifierName("obj"),
                                SyntaxFactory.DeclarationPattern(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name),
                                    SyntaxFactory.SingleVariableDesignation(
                                        SyntaxFactory.Identifier("other")))),
                            previousExpressions)));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }
    }
}