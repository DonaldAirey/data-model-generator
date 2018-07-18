// <copyright file="ThrowOptimisticConcurrencyFault.cs" company="Gamma Four, Inc.">
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
    public static class ThrowOptimisticConcurrencyFault
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="optimisticConcurrencyException">The optimisticConcurrency that caused the exception.</param>
        /// <returns>A block of code.</returns>
        public static SyntaxList<StatementSyntax> GetSyntax(string optimisticConcurrencyException)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                throw new FaultException<OptimisticConcurrencyFault>(new OptimisticConcurrencyFault(optimisticConcurrencyException.Table, optimisticConcurrencyException.Key));
            statements.Add(
                SyntaxFactory.ThrowStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("FaultException"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(nameof(OptimisticConcurrencyFault))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName(nameof(OptimisticConcurrencyFault)))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName(optimisticConcurrencyException),
                                                            SyntaxFactory.IdentifierName("Table"))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName(optimisticConcurrencyException),
                                                            SyntaxFactory.IdentifierName("Key")))
                                                })))))))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}