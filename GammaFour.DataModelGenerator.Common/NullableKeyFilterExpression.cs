// <copyright file="NullableKeyFilterExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Globalization;
    using GammaFour.DataModelGenerator.Common;
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
        public static ExpressionSyntax GetNullableKeyFilter(UniqueIndexElement uniqueKeyElement)
        {
            // Used as a variable when constructing the lambda expression.
            string abbreviation = uniqueKeyElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // .HasFilter(s => s.Figi != null)
            // .HasFilter(p => p.BaseCurrencyCode != null && p.CurrencyCode != null)
            BinaryExpressionSyntax syntaxNode = default;
            for (int index = 0; index < uniqueKeyElement.Columns.Count; index++)
            {
                // We can only filter nullable items.
                if (uniqueKeyElement.Columns[index].Column.ColumnType.IsNullable)
                {
                    if (index == 0)
                    {
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
                        BinaryExpressionSyntax oldNode = syntaxNode;
                        syntaxNode = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            oldNode,
                            SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(abbreviation),
                                SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[index].Column.Name)),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)));
                    }
                }
            }

            //            this.BuyerKey = new UniqueIndex("BuyerKey").HasIndex(b => b.BuyerId);
            return SyntaxFactory.SimpleLambdaExpression(SyntaxFactory.Parameter(SyntaxFactory.Identifier(abbreviation)), syntaxNode);
        }

        /// <summary>
        /// Creates an argument that creates a lambda expression for extracting the key from a class.
        /// </summary>
        /// <param name="foreignIndexElement">The unique key element.</param>
        /// <returns>An expression that creates a filter for an index.</returns>
        public static ExpressionSyntax GetNullableKeyFilter(ForeignIndexElement foreignIndexElement)
        {
            // Used as a variable when constructing the lambda expression.
            string abbreviation = foreignIndexElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // .HasFilter(s => s.Figi != null)
            // .HasFilter(p => p.BaseCurrencyCode != null && p.CurrencyCode != null)
            BinaryExpressionSyntax syntaxNode = default;
            for (int index = 0; index < foreignIndexElement.Columns.Count; index++)
            {
                // We can only filter nullable items.
                if (foreignIndexElement.Columns[0].Column.ColumnType.IsNullable)
                {
                    if (index == 0)
                    {
                        syntaxNode = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(abbreviation),
                                SyntaxFactory.IdentifierName(foreignIndexElement.Columns[0].Column.Name)),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression));
                    }
                    else
                    {
                        BinaryExpressionSyntax oldNode = syntaxNode;
                        syntaxNode = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            oldNode,
                            SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(abbreviation),
                                SyntaxFactory.IdentifierName(foreignIndexElement.Columns[index].Column.Name)),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)));
                    }
                }
            }

            //            this.BuyerKey = new UniqueIndex<Buyer>("BuyerKey").HasIndex(b => b.BuyerId);
            return SyntaxFactory.SimpleLambdaExpression(SyntaxFactory.Parameter(SyntaxFactory.Identifier(abbreviation)), syntaxNode);
        }
    }
}