// <copyright file="ThrowOptimisticConcurrencyException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server
{
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw an exception when a tenant isn't loaded.
    /// </summary>
    public static class ThrowOptimisticConcurrencyException
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        /// <returns>A block of code.</returns>
        public static SyntaxList<StatementSyntax> GetSyntax(TableElement tableElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // Create a list of parameters that comprise the primary key.
            List<ExpressionSyntax> expressions = new List<ExpressionSyntax>();
            foreach (ColumnReferenceElement columnReferenceElement in tableElement.PrimaryKey.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                expressions.Add(SyntaxFactory.IdentifierName(columnElement.Name.ToCamelCase()));
            }

            //                    throw new ConcurrencyException("Configuration", configurationKey.Elements);
            statements.Add(
                SyntaxFactory.ThrowStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.IdentifierName("OptimisticConcurrencyException"))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(tableElement.Name))),
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
                                })))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}