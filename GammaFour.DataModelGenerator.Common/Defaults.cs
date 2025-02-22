// <copyright file="Defaults.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A set of methods to assist in converting type information.
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// Translates the given type into a type syntax.
        /// </summary>
        /// <param name="columnType">The column's type.</param>
        /// <returns>A Roslyn type syntax corresponding to the CLR type.</returns>
        public static ExpressionSyntax FromType(ColumnType columnType)
        {
            if (columnType.FullName == "System.String")
            {
                return SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                    SyntaxFactory.IdentifierName("Empty"));
            }

            // Not handled.
            throw new NotImplementedException();
        }
    }
}