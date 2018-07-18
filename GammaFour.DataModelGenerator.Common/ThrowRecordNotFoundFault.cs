// <copyright file="ThrowRecordNotFoundFault.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Collections.Generic;
    using GammaFour.ClientModel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw an exception when a tenant isn't loaded.
    /// </summary>
    public static class ThrowRecordNotFoundFault
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The unique constraint schema.</param>
        /// <param name="arguments">The parameters that caused the exception.</param>
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

            // The fault takes as arguments the name of the index plus the values that caused the violation.
            List<ArgumentSyntax> faultArguments = new List<ArgumentSyntax>();
            faultArguments.Add(
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(uniqueConstraintSchema.Name))));
            faultArguments.Add(
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
                                SyntaxFactory.SeparatedList<ExpressionSyntax>(expressions)))));

            //                                throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault("CountryExternalId0KeyIndex", new object[] { countryKey[0] }));
            statements.Add(
                SyntaxFactory.ThrowStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("FaultException"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault)))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(faultArguments)))))))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }

        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="recordNotFoundException">he optimisticConcurrency that caused the exception.</param>
        /// <returns>A block of code.</returns>
        public static SyntaxList<StatementSyntax> GetSyntax(string recordNotFoundException)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                throw new FaultException<RecordNotFoundFault>(new RecordNotFoundFault(recordNotFoundException.Table, recordNotFoundException.Key));
            statements.Add(
                SyntaxFactory.ThrowStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("FaultException"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName(nameof(RecordNotFoundFault)))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(recordNotFoundException),
                                                SyntaxFactory.IdentifierName("Table"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(recordNotFoundException),
                                                SyntaxFactory.IdentifierName("Key")))
                                                })))))))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}