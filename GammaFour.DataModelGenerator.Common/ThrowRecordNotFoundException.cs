// <copyright file="ThrowRecordNotFoundException.cs" company="Gamma Four, Inc.">
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
    /// Creates a code block to throw an exception when a tenant isn't loaded.
    /// </summary>
    public static class ThrowRecordNotFoundException
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <param name="arguments">The parameters that cause the exception.</param>
        /// <returns>A block of code.</returns>
        public static SyntaxList<StatementSyntax> GetSyntax(UniqueConstraintSchema uniqueConstraintSchema, IEnumerable<ArgumentSyntax> arguments)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            // The list of arguments that couldn't be found are passed back as part of the fault data.
            List<ExpressionSyntax> expressions = new List<ExpressionSyntax>();
            foreach (ArgumentSyntax argument in arguments)
            {
                expressions.Add(argument.Expression);
            }

            //                    throw new RecordNotFoundException("Configuration", new object[] { configurationId, source });
            statements.Add(
                SyntaxFactory.ThrowStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.IdentifierName("RecordNotFoundException"))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(uniqueConstraintSchema.Name))),
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