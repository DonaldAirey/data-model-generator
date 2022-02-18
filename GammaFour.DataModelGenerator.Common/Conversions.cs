// <copyright file="Conversions.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A set of methods to assist in converting type information.
    /// </summary>
    public static class Conversions
    {
        /// <summary>
        /// Maps the CLR full type name to a predefined type syntax.
        /// </summary>
        private static readonly Dictionary<string, TypeSyntax> PredefinedTypes = new Dictionary<string, TypeSyntax>()
        {
            { "System.Boolean", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)) },
            { "System.Byte", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ByteKeyword)) },
            { "System.Double", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)) },
            { "System.Decimal", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DecimalKeyword)) },
            { "System.Int16", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ShortKeyword)) },
            { "System.Int32", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)) },
            { "System.Int64", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)) },
            { "System.Object", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)) },
            { "System.String", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)) },
            { "System.Float", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.FloatKeyword)) },
            { "System.UInt16", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UShortKeyword)) },
            { "System.UInt32", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UIntKeyword)) },
            { "System.UInt64", SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ULongKeyword)) },
        };

        /// <summary>
        /// Translates the given type into a type syntax.
        /// </summary>
        /// <param name="columnType">The column's type.</param>
        /// <returns>A Roslyn type syntax corresponding to the CLR type.</returns>
        public static TypeSyntax FromType(ColumnType columnType)
        {
            // Validate the parameter
            if (columnType == null)
            {
                throw new ArgumentNullException(nameof(columnType));
            }

            if (columnType.IsNullable && columnType.IsValueType)
            {
                TypeSyntax nullableTypeSyntax = null;
                if (!Conversions.PredefinedTypes.TryGetValue(columnType.FullName, out nullableTypeSyntax))
                {
                    nullableTypeSyntax = SyntaxFactory.IdentifierName(columnType.FullName);
                }

                return SyntaxFactory.NullableType(nullableTypeSyntax);
            }

            if (columnType.IsArray)
            {
                TypeSyntax arrayTypeSyntax = null;
                if (!Conversions.PredefinedTypes.TryGetValue(columnType.FullName, out arrayTypeSyntax))
                {
                    arrayTypeSyntax = SyntaxFactory.IdentifierName(columnType.FullName);
                }

                return SyntaxFactory.ArrayType(arrayTypeSyntax)
                    .WithRankSpecifiers(
                        SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                            SyntaxFactory.ArrayRankSpecifier(
                                SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                    SyntaxFactory.OmittedArraySizeExpression()
                                    .WithOmittedArraySizeExpressionToken(SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken))))));
            }

            TypeSyntax typeSyntax = null;
            if (!Conversions.PredefinedTypes.TryGetValue(columnType.FullName, out typeSyntax))
            {
                typeSyntax = SyntaxFactory.IdentifierName(columnType.FullName);
            }

            return typeSyntax;
        }
    }
}