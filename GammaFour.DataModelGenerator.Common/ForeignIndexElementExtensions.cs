// <copyright file="ForeignIndexElementExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates a foreign key.
    /// </summary>
    public static class ForeignIndexElementExtensions
    {
        /// <summary>
        /// Creates an argument for a foreign key.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ArgumentSyntax GetKeyAsArguments(this ForeignIndexElement foreignIndexElement)
        {
            // This will create an expression for extracting the key from row.
            if (foreignIndexElement.Columns.Count == 1)
            {
                // code
                return SyntaxFactory.Argument(
                    SyntaxFactory.IdentifierName(foreignIndexElement.Columns[0].Column.Name.ToVariableName()));
            }
            else
            {
                // A Compound key is constructed as a tuple.
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in foreignIndexElement.Columns)
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
        /// Creates an argument for a foreign key using the members of a value.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign key element.</param>
        /// <param name="variableName">The name of the variable used in the expression.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ArgumentSyntax GetKeyAsArguments(this ForeignIndexElement foreignIndexElement, string variableName)
        {
            // This will create an expression for extracting the key from row.
            if (foreignIndexElement.Columns.Count == 1)
            {
                var columnElement = foreignIndexElement.Columns[0].Column;
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
                foreach (ColumnReferenceElement columnReferenceElement in foreignIndexElement.Columns)
                {
                    // Add a comma between key elements.
                    if (keyElements.Count != 0)
                    {
                        keyElements.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    var columnElement = columnReferenceElement.Column;
                    if (columnElement.ColumnType.IsNullable && columnElement.ColumnType.IsValueType)
                    {
                        // account.Account.Value
                        keyElements.Add(
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
                        keyElements.Add(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(variableName),
                                SyntaxFactory.IdentifierName(columnElement.Name)));
                    }
                }

                // (account.Code, account.Name)
                return SyntaxFactory.Argument(
                    SyntaxFactory.TupleExpression(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(keyElements.ToArray())));
            }
        }

        /// <summary>
        /// Creates an argument for a foreign key using the members of a value.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign key element.</param>
        /// <param name="variableName">The name of the variable used in the expression.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetKeyAsInequalityConditional(this ForeignIndexElement foreignIndexElement, string variableName)
        {
            ExpressionSyntax expressionSyntax = null;
            foreach (var columnReferenceElement in foreignIndexElement.Columns)
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
        /// Creates an argument for a foreign key using the members of a value.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign key element.</param>
        /// <param name="variableName">The name of the variable used in the expression.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ExpressionSyntax GetKeyAsEqualityConditional(this ForeignIndexElement foreignIndexElement, string variableName)
        {
            ExpressionSyntax expressionSyntax = null;
            foreach (var columnReferenceElement in foreignIndexElement.Columns)
            {
                var columnElement = columnReferenceElement.Column;
                if (columnElement.ColumnType.IsNullable)
                {
                    if (expressionSyntax == null)
                    {
                        expressionSyntax = SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
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
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
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
        /// Creates a parameter list for a foreign key.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ParameterListSyntax GetKeyAsParameters(this ForeignIndexElement foreignIndexElement)
        {
            // This will create an expression for extracting the key from row.
            if (foreignIndexElement.Columns.Count == 1)
            {
                // string code
                var columnElement = foreignIndexElement.Columns[0].Column;
                return SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnElement.Name.ToVariableName()))
                        .WithType(columnElement.GetTypeSyntax())));
            }
            else
            {
                List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in foreignIndexElement.Columns)
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