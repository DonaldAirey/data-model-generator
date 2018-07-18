// <copyright file="ThrowArgumentNullException.cs" company="Gamma Four, Inc.">
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
    public static class ThrowArgumentNullException
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="argument">The name of the argument.</param>
        /// <returns>A block of code.</returns>
        public static StatementSyntax GetSyntax(string argument)
        {
            //                    throw new ArgumentNullException("configurationId");
            return SyntaxFactory.ThrowStatement(
                SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName("ArgumentNullException"))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal(argument)))))));
        }
    }
}