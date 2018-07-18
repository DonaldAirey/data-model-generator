// <copyright file="ThrowConstraintFault.cs" company="Gamma Four, Inc.">
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
    public static class ThrowConstraintFault
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="constraintException">The constraint that caused the exception.</param>
        /// <returns>A block of code.</returns>
        public static SyntaxList<StatementSyntax> GetSyntax(string constraintException)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                throw new FaultException<ConstraintFault>(new ConstraintFault(constraintException.Constraint));
            statements.Add(
                SyntaxFactory.ThrowStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("FaultException"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(nameof(ConstraintFault))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName(nameof(ConstraintFault)))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName(constraintException),
                                                            SyntaxFactory.IdentifierName("Operation"))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName(constraintException),
                                                            SyntaxFactory.IdentifierName("Constraint")))
                                                })))))))));

            // This is the complete block.
            return SyntaxFactory.List(statements);
        }
    }
}