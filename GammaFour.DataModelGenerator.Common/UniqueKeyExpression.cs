﻿// <copyright file="UniqueKeyExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates a unique key.
    /// </summary>
    public static class UniqueKeyExpression
    {
        /// <summary>
        /// Creates an argument that creates a lambda expression for extracting the key from a class.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique key element.</param>
        /// <param name="isAnonymous">Indicates we should create an anonymous key for Entity Framework.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetUniqueKey(UniqueElement uniqueKeyElement, bool isAnonymous = false)
        {
            // Validate the parameter
            if (uniqueKeyElement == null)
            {
                throw new ArgumentNullException(nameof(uniqueKeyElement));
            }

            // Used as a variable when constructing the lambda expression.
            string abbreviation = uniqueKeyElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // This will create an expression for extracting the key from record.
            CSharpSyntaxNode syntaxNode;
            if (uniqueKeyElement.Columns.Count == 1)
            {
                // A simple key is constructed a value type.
                syntaxNode = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(abbreviation),
                    SyntaxFactory.IdentifierName(uniqueKeyElement.Columns[0].Column.Name));
            }
            else
            {
                // A Compound key is constructed as a tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
                {
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    if (isAnonymous)
                    {
                        keyElements.Add(
                            SyntaxFactory.AnonymousObjectMemberDeclarator(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(abbreviation),
                                    SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
                    }
                    else
                    {
                        keyElements.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(abbreviation),
                                SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
                    }
                }

                if (isAnonymous)
                {
                    // b => b.BuyerId or b => new { b.BuyerId, b.ExternalId0 }
                    syntaxNode = SyntaxFactory.AnonymousObjectCreationExpression(
                        SyntaxFactory.SeparatedList<AnonymousObjectMemberDeclaratorSyntax>(keyElements.ToArray()));
                }
                else
                {
                    // p => (p.Name, p.CountryCode)
                    syntaxNode = SyntaxFactory.TupleExpression(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray()));
                }
            }

            //            this.BuyerKey = new UniqueIndex<Buyer>("BuyerKey").HasIndex(b => b.BuyerId);
            return SyntaxFactory.SimpleLambdaExpression(SyntaxFactory.Parameter(SyntaxFactory.Identifier(abbreviation)), syntaxNode);
        }
    }
}