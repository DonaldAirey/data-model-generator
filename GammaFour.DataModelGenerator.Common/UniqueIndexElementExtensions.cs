// <copyright file="UniqueIndexElementExtensions.cs" company="Gamma Four, Inc.">
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
    public static class UniqueIndexElementExtensions
    {
        /// <summary>
        /// Creates an argument that creates a lambda expression for extracting the key from a class.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <param name="isAnonymous">Indicates we should create an anonymous key for Entity Framework.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetUniqueKey(this UniqueIndexElement uniqueIndexElement, bool isAnonymous)
        {
            // Used as a variable when constructing the lambda expression.
            string abbreviation = uniqueIndexElement.Table.Name[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            // This will create an expression for extracting the key from row.
            CSharpSyntaxNode syntaxNode;
            if (uniqueIndexElement.Columns.Count == 1)
            {
                // A simple key is constructed a value type.
                syntaxNode = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(abbreviation),
                    SyntaxFactory.IdentifierName(uniqueIndexElement.Columns[0].Column.Name));
            }
            else
            {
                // A Compound key is constructed as a tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
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

        /// <summary>
        /// Creates an argument for a unique key.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ArgumentSyntax GetKeyAsArguments(this UniqueIndexElement uniqueIndexElement)
        {
            // This will create an expression for extracting the key from row.
            if (uniqueIndexElement.Columns.Count == 1)
            {
                // code
                return SyntaxFactory.Argument(
                    SyntaxFactory.IdentifierName(uniqueIndexElement.Columns[0].Column.Name.ToVariableName()));
            }
            else
            {
                // A Compound key is constructed as a tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
                {
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    keyElements.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name.ToVariableName())));
                }

                // (code, name)
                return SyntaxFactory.Argument(
                    SyntaxFactory.TupleExpression(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray())));
            }
        }

        /// <summary>
        /// Creates an argument for a unique key using the members of a value.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <param name="variableName">The name of the variable used in the expression.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ArgumentSyntax GetKeyAsArguments(this UniqueIndexElement uniqueIndexElement, string variableName)
        {
            // This will create an expression for extracting the key from row.
            if (uniqueIndexElement.Columns.Count == 1)
            {
                var columnElement = uniqueIndexElement.Columns[0].Column;
                if (columnElement.ColumnType.IsNullable && columnElement.ColumnType.IsValueType)
                {
                    // account.Account.Value
                    return SyntaxFactory.Argument(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(columnElement.Name)),
                        SyntaxFactory.IdentifierName("Value")));
                }
                else
                {
                    // account.Account
                    return SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(columnElement.Name)));
                }
            }
            else
            {
                // A Compound key is constructed as a tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
                {
                    var columnElement = columnReferenceElement.Column;
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    if (columnElement.ColumnType.IsNullable && columnElement.ColumnType.IsValueType)
                    {
                        keyElements.Add(
                            SyntaxFactory.Argument(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(variableName),
                                        SyntaxFactory.IdentifierName(columnElement.Name)),
                                    SyntaxFactory.IdentifierName("Value"))));
                    }
                    else
                    {
                        keyElements.Add(
                            SyntaxFactory.Argument(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(variableName),
                                    SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
                    }
                }

                // (account.Code, account.Name)
                return SyntaxFactory.Argument(
                    SyntaxFactory.TupleExpression(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray())));
            }
        }

        /// <summary>
        /// Creates an argument for a unique key.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static SeparatedSyntaxList<ArgumentSyntax> GetKeyAsFindArguments(this UniqueIndexElement uniqueIndexElement)
        {
            // This will create an expression for extracting the key from row.
                // A Compound key is constructed as a tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
                {
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    keyElements.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name.ToVariableName())));
                }

                // code, name
                return SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray());
        }

        /// <summary>
        /// Creates an argument for a unique key.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <param name="variableName">The name of the variable used in the expression.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static SeparatedSyntaxList<ArgumentSyntax> GetKeyAsFindArguments(this UniqueIndexElement uniqueIndexElement, string variableName)
        {
            // This will create an expression for extracting the key from row.
            // A Compound key is constructed as a tuple.
            List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
            foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
            {
                if (keyElements.Count != 0)
                {
                    keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                }

                keyElements.Add(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
            }

            // code, name
            return SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray());
        }

        /// <summary>
        /// Creates a conditional statement for a unique key using the members of a value.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetKeyAsEqualityConditional(this UniqueIndexElement uniqueIndexElement)
        {
            ExpressionSyntax expressionSyntax = null;
            foreach (var columnReferenceElement in uniqueIndexElement.Columns)
            {
                var columnElement = columnReferenceElement.Column;
                if (columnElement.ColumnType.IsNullable)
                {
                    if (expressionSyntax == null)
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.IdentifierName(columnElement.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression));
                    }
                    else
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.IdentifierName(columnElement.Name.ToVariableName()),
                                SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                            expressionSyntax);
                    }
                }
            }

            return expressionSyntax;
        }

        /// <summary>
        /// Creates a conditional statement for a unique key using the members of a value.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetKeyAsInequalityConditional(this UniqueIndexElement uniqueIndexElement)
        {
            ExpressionSyntax expressionSyntax = null;
            foreach (var columnReferenceElement in uniqueIndexElement.Columns)
            {
                var columnElement = columnReferenceElement.Column;
                if (columnElement.ColumnType.IsNullable)
                {
                    if (expressionSyntax == null)
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(columnElement.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression));
                    }
                    else
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName(columnElement.Name.ToVariableName()),
                                SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                            expressionSyntax);
                    }
                }
            }

            return expressionSyntax;
        }

        /// <summary>
        /// Creates a conditional statement for a unique key using the members of a value.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <param name="variableName">The name of the variable used in the expression.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetKeyAsInequalityConditional(this UniqueIndexElement uniqueIndexElement, string variableName)
        {
            ExpressionSyntax expressionSyntax = null;
            foreach (var columnReferenceElement in uniqueIndexElement.Columns)
            {
                var columnElement = columnReferenceElement.Column;
                if (columnElement.ColumnType.IsNullable)
                {
                    if (expressionSyntax == null)
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(variableName),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression));
                    }
                    else
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(variableName),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            expressionSyntax);
                    }
                }
            }

            return expressionSyntax;
        }

        /// <summary>
        /// Creates a parameter list for a unique key.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ParameterListSyntax GetKeyAsParameters(this UniqueIndexElement uniqueIndexElement)
        {
            // This will create an expression for extracting the key from row.
            if (uniqueIndexElement.Columns.Count == 1)
            {
                // string code
                var columnElement = uniqueIndexElement.Columns[0].Column;
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnElement.Name.ToVariableName()))
                        .WithType(columnElement.GetTypeSyntax())));
            }
            else
            {
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
                {
                    var columnElement = columnReferenceElement.Column;
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    keyElements.Add(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnReferenceElement.Column.Name.ToVariableName()))
                        .WithType(columnElement.GetTypeSyntax()));
                }

                // string code, string name
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(keyElements.ToArray()));
            }
        }
    }
}