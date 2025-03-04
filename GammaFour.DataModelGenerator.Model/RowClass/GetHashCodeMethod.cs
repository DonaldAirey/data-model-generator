// <copyright file="GetHashCodeMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.RowClass
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
    public class GetHashCodeMethod : SyntaxElement
    {
        /// <summary>
        /// The table element.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHashCodeMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        public GetHashCodeMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "GetHashCode";

            //        /// <inheritdoc/>
            //        public override bool GetHashCode()
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.OverrideKeyword),
                    }))
            .WithBody(this.Body)
            .WithLeadingTrivia(GetHashCodeMethod.LeadingTrivia);
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
                        previousExpressions = SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.IdentifierName("GetHashCode")));
                    }
                    else
                    {
                        previousExpressions = SyntaxFactory.BinaryExpression(
                            SyntaxKind.AddExpression,
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(columnElement.Name)),
                                    SyntaxFactory.IdentifierName("GetHashCode"))),
                            previousExpressions);
                    }
                }

                //            return this.AccountCode.GetHashCode() + this.AssetCode.GetHashCode() + this.Date.GetHashCode();
                statements.Add(SyntaxFactory.ReturnStatement(previousExpressions));

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }
    }
}