// <copyright file="NullableKeyFilterExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates a unique key.
    /// </summary>
    public static class NullableKeyFilterExpression
    {
        /// <summary>
        /// Creates an argument that creates a lambda expression for extracting the key from a class.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique key element.</param>
        /// <returns>An expression that creates a filter for an index.</returns>
        public static ExpressionSyntax GetNullableKeyFilter(UniqueKeyElement uniqueKeyElement)
        {
            // Validate the parameter
            if (uniqueKeyElement == null)
            {
                throw new ArgumentNullException(nameof(uniqueKeyElement));
            }

            // Used as a variable when constructing the lambda expression.
            string abbreviation = uniqueKeyElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // This will create an expression for extracting the key from record.
            CSharpSyntaxNode syntaxNode = null;
            if (uniqueKeyElement.Columns.Count == 1)
            {
                // A simple key can be used like a value type.
                syntaxNode = SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(abbreviation),
                        SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name)),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.NullLiteralExpression));
            }
            else
            {
                // A Compound key must be constructed from an anomymous type.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
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

                // b => b.BuyerId or p => ValueTuple.Create(p.Name, p.CountryCode)
                syntaxNode = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("ValueTuple"),
                        SyntaxFactory.IdentifierName("Create")))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray())));
            }

            //            this.BuyerKey = new UniqueKeyIndex<Buyer>("BuyerKey").HasIndex(b => b.BuyerId);
            return SyntaxFactory.SimpleLambdaExpression(SyntaxFactory.Parameter(SyntaxFactory.Identifier(abbreviation)), syntaxNode);
        }
    }
}