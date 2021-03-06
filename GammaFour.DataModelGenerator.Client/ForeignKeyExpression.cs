﻿// <copyright file="ForeignKeyExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2021 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client
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
    public static class ForeignKeyExpression
    {
        /// <summary>
        /// Creates an argument that creates a lambda expression for extracting the key from a class.
        /// </summary>
        /// <param name="foreignKeyElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetForeignKey(ForeignKeyElement foreignKeyElement)
        {
            // Validate the parameter
            if (foreignKeyElement == null)
            {
                throw new ArgumentNullException(nameof(foreignKeyElement));
            }

            // Used as a variable when constructing the lambda expression.
            string abbreviation = foreignKeyElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // This will create an expression for extracting the key from record.
            CSharpSyntaxNode syntaxNode = null;
            if (foreignKeyElement.Columns.Count == 1)
            {
                // A simple key can be used like a value type.
                syntaxNode = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(abbreviation),
                    SyntaxFactory.IdentifierName(foreignKeyElement.Columns[0].Column.Name));
            }
            else
            {
                // A Compound key must be constructed from an anomymous type.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in foreignKeyElement.Columns)
                {
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    keyElements.Add(
                        SyntaxFactory.AnonymousObjectMemberDeclarator(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(abbreviation),
                                SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
                }

                // b => b.BuyerId or b => new { b.BuyerId, b.ExternalId0 }
                syntaxNode = SyntaxFactory.AnonymousObjectCreationExpression(
                        SyntaxFactory.SeparatedList<AnonymousObjectMemberDeclaratorSyntax>(keyElements.ToArray()));
            }

            //            this.BuyerKey = new SimpleForeignKeyIndex<Buyer>("BuyerKey").HasIndex(b => b.BuyerId);
            return SyntaxFactory.SimpleLambdaExpression(SyntaxFactory.Parameter(SyntaxFactory.Identifier(abbreviation)), syntaxNode);
        }
    }
}