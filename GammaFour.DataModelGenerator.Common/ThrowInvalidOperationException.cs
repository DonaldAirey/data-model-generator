// <copyright file="ThrowInvalidOperationException.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class ThrowInvalidOperationException
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <returns>A block of code.</returns>
        public static StatementSyntax GetSyntax(string message)
        {
            //                    throw new InvalidOperationException("Configuration row not locked");
            return SyntaxFactory.ThrowStatement(
                SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("InvalidOperationException"))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(message)))))));
        }
    }
}