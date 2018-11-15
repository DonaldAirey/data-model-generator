// <copyright file="ThrowDuplicateKeyException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class ThrowDuplicateKeyException
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema that was violated.</param>
        /// <param name="columns">The columns of the key that was violated.</param>
        /// <returns>A block of code.</returns>
        public static StatementSyntax GetSyntax(UniqueKeyElement uniqueKeyElement, List<ColumnReferenceElement> columns)
        {
            // This creates the comma-separated list of parameters that are used to create a key.
            List<ExpressionSyntax> expressions = new List<ExpressionSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in columns)
            {
                expressions.Add(SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name.ToCamelCase()));
            }

            //                throw new DuplicateKeyException("Configuration", new object[] { keyConfigurationId, keySource });
            return SyntaxFactory.ThrowStatement(
                SyntaxFactory.ObjectCreationExpression(
                    SyntaxFactory.IdentifierName("DuplicateKeyException"))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(uniqueKeyElement.Name))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ArrayCreationExpression(
                                        SyntaxFactory.ArrayType(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                        .WithRankSpecifiers(
                                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                SyntaxFactory.ArrayRankSpecifier(
                                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                        SyntaxFactory.OmittedArraySizeExpression())))))
                                    .WithInitializer(
                                        SyntaxFactory.InitializerExpression(
                                            SyntaxKind.ArrayInitializerExpression,
                                            SyntaxFactory.SeparatedList<ExpressionSyntax>(expressions))))
                            }))));
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueKeyElement">The unique constraint schema that was violated.</param>
        /// <param name="propertyOwner">The object that owns the columns in the specified key.</param>
        /// <param name="columns">The columns of the key that was violated.</param>
        /// <returns>A block of code.</returns>
        public static StatementSyntax GetSyntax(UniqueKeyElement uniqueKeyElement, ExpressionSyntax propertyOwner, List<ColumnElement> columns)
        {
            // This creates the comma-separated list of parameters that are used to create a key.
            List<ExpressionSyntax> expressions = new List<ExpressionSyntax>();
            foreach (ColumnElement columnElement in columns)
            {
                expressions.Add(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        propertyOwner,
                        SyntaxFactory.IdentifierName(columnElement.Name)));
            }

            //                throw new DuplicateKeyException("Configuration", new object[] { keyConfigurationId, keySource });
            return SyntaxFactory.ThrowStatement(
                SyntaxFactory.ObjectCreationExpression(
                    SyntaxFactory.IdentifierName("DuplicateKeyException"))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(uniqueKeyElement.Name))),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ArrayCreationExpression(
                                        SyntaxFactory.ArrayType(
                                            SyntaxFactory.PredefinedType(
                                                SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                        .WithRankSpecifiers(
                                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                SyntaxFactory.ArrayRankSpecifier(
                                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                        SyntaxFactory.OmittedArraySizeExpression())))))
                                    .WithInitializer(
                                        SyntaxFactory.InitializerExpression(
                                            SyntaxKind.ArrayInitializerExpression,
                                            SyntaxFactory.SeparatedList<ExpressionSyntax>(expressions))))
                            }))));
        }
    }
}