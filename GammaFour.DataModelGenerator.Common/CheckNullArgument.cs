// <copyright file="CheckNullArgument.cs" company="Gamma Four, Inc.">
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
    public static class CheckNullArgument
    {
        /// <summary>
        /// Gets a block of code.
        /// </summary>
        /// <param name="argumentName">The argument name.</param>
        /// <returns>A block of code.</returns>
        public static StatementSyntax GetSyntax(string argumentName)
        {
            //            if (dataModelTransaction == null)
            //            {
            //                throw new ArgumentNullException("dataModelTransaction");
            //            }
            return SyntaxFactory.IfStatement(
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    SyntaxFactory.IdentifierName(argumentName),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.NullLiteralExpression)),
                SyntaxFactory.Block(
                    SyntaxFactory.SingletonList<StatementSyntax>(
                        SyntaxFactory.ThrowStatement(
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.IdentifierName("ArgumentNullException"))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(argumentName))))))))));
        }
    }
}