// <copyright file="ForeignKeyExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates a unique key.
    /// </summary>
    public static class ForeignKeyExpression
    {
        /// <summary>
        /// Creates an argument that creates a lambda expression for extracting the key from a class.
        /// </summary>
        /// <param name="foreignKeyElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetForeignKey(ForeignElement foreignKeyElement)
        {
            // Used as a variable when constructing the lambda expression.
            string abbreviation = foreignKeyElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // This will create an expression for extracting the key from record.
            CSharpSyntaxNode syntaxNode;
            if (foreignKeyElement.Columns.Count == 1)
            {
                // b.BuyerId
                syntaxNode = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(abbreviation),
                    SyntaxFactory.IdentifierName(foreignKeyElement.Columns[0].Column.Name));
            }
            else
            {
                // A Compound key is constructed as a value tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
                {
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    keyElements.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(abbreviation),
                                SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
                }

                // ValueTuple.Create(b.BuyerId, b.ExternalId0)
                syntaxNode = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("ValueTuple"),
                        SyntaxFactory.IdentifierName("Create")))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements)));
            }

            // b =>  b.BuyerId
            // b => ValueTuple.Create(b.BuyerId, b.ExternalId0);
            return SyntaxFactory.SimpleLambdaExpression(SyntaxFactory.Parameter(SyntaxFactory.Identifier(abbreviation)), syntaxNode);
        }
    }
}