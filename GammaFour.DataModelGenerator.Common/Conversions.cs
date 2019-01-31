// <copyright file="Conversions.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// A set of methods to assist in converting type information.
    /// </summary>
    public static class Conversions
    {
        /// <summary>
        /// Creates an expression that will cast a string expression to the given type.
        /// </summary>
        /// <param name="expression">An expression to cast.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>The expression cast to the string, or not if the source type is a string.</returns>
        public static ExpressionSyntax CreateParseExpression(ExpressionSyntax expression, Type sourceType)
        {
            // Enumerations must be parsed differently than other types.
            if (sourceType.GetTypeInfo().IsEnum)
            {
                return SyntaxFactory.CastExpression(
                    SyntaxFactory.IdentifierName(sourceType.Name),
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("Enum"),
                            SyntaxFactory.IdentifierName("Parse")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.IdentifierName(sourceType.Name))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(expression)
                                }))));
            }

            // Parse the string expression into the given type.
            switch (sourceType.FullName)
            {
                case "System.Int16":

                    return SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.SByteKeyword)),
                            SyntaxFactory.IdentifierName("Parse")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(expression))));

                case "System.Int32":

                    return SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                            SyntaxFactory.IdentifierName("Parse")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(expression))));

                case "System.Int64":

                    return SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.LongKeyword)),
                            SyntaxFactory.IdentifierName("Parse")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(expression))));

                case "System.DateTime":

                    return SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("DateTime"),
                            SyntaxFactory.IdentifierName("Parse")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(expression),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("CultureInfo"),
                                                SyntaxFactory.IdentifierName("CurrentCulture")))
                                    })));

                case "System.Guid":

                    return SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("Guid"),
                            SyntaxFactory.IdentifierName("Parse")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(expression))));
            }

            // Anything not recognized above is passed without parsing.
            return expression;
        }

        /// <summary>
        /// Creates an expression for parsing a parameter from a string into it's desired type.
        /// </summary>
        /// <param name="parameter">The parameter to be parsed.</param>
        /// <param name="desiredType">The desired type.</param>
        /// <returns>An expression that converts a string representation of a parameter into it's desired type.</returns>
        public static ExpressionSyntax CreateConditionalParseExpression(ExpressionSyntax parameter, Type desiredType)
        {
            // Enumerations are parsed differently than other types.
            if (desiredType.GetTypeInfo().IsEnum)
            {
                return SyntaxFactory.ConditionalExpression(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                            SyntaxFactory.IdentifierName("IsNullOrEmpty")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(parameter)))),
                    SyntaxFactory.DefaultExpression(
                        SyntaxFactory.IdentifierName(desiredType.Name)),
                    SyntaxFactory.CastExpression(
                        SyntaxFactory.IdentifierName(desiredType.Name),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("Enum"),
                                SyntaxFactory.IdentifierName("Parse")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.TypeOfExpression(
                                                SyntaxFactory.IdentifierName(desiredType.Name))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(parameter)
                                    })))));
            }

            switch (desiredType.FullName)
            {
                case "System.DateTime":

                    return SyntaxFactory.ConditionalExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                SyntaxFactory.IdentifierName("IsNullOrEmpty")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(parameter)))),
                        SyntaxFactory.DefaultExpression(
                            SyntaxFactory.IdentifierName("DateTime")),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("DateTime"),
                                SyntaxFactory.IdentifierName("Parse")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(parameter),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("CultureInfo"),
                                                SyntaxFactory.IdentifierName("CurrentCulture")))
                                    }))));

                case "System.Guid":

                    return SyntaxFactory.ConditionalExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                SyntaxFactory.IdentifierName("IsNullOrEmpty")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(parameter)))),
                        SyntaxFactory.DefaultExpression(
                            SyntaxFactory.IdentifierName("Guid")),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("Guid"),
                                SyntaxFactory.IdentifierName("Parse")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(parameter)))));
            }

            // All other types are handled generically.
            return SyntaxFactory.ConditionalExpression(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                        SyntaxFactory.IdentifierName("IsNullOrEmpty")))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(parameter)))),
                SyntaxFactory.DefaultExpression(Conversions.FromType(desiredType)),
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        Conversions.FromType(desiredType),
                        SyntaxFactory.IdentifierName("Parse")))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(parameter)))));
        }

        /// <summary>
        /// Creates a literal value syntax for a CLR value.
        /// </summary>
        /// <param name="value">The CLR value.</param>
        /// <returns>An expression that can be used as a literal.</returns>
        public static ExpressionSyntax ToLiteral(object value)
        {
            // Provide a constant expression for nulls.
            if (value == null)
            {
                return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                    .WithToken(SyntaxFactory.Token(SyntaxKind.NullKeyword));
            }

            // If the column provides a fixed value, then translate the default text into the proper datatype for the column.
            switch (value.GetType().ToString())
            {
                case "System.Boolean":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Double":

                    return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value.ToString()));

                case "System.String":

                    return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(value.ToString()));

                default:

                    if (value.GetType().GetTypeInfo().IsEnum)
                    {
                        return SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(value.GetType().Name),
                            SyntaxFactory.IdentifierName(value.ToString()));
                    }

                    break;
            }

            return SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value.ToString())));
        }

        /// <summary>
        /// Creates an expression of a CLR type as an SQL type.
        /// </summary>
        /// <param name="type">The source data type.</param>
        /// <returns>An argument that describes the parameter's type to SQL.</returns>
        public static ArgumentSyntax ToSqlType(Type type)
        {
            string sqlDataType = string.Empty;

            // Null-ables will resolve to the inner type.  The 'IsNullable' flag will instruct the compiler to allow nulls in these column types.
            if (type.FullName.StartsWith("System.Nullable", StringComparison.InvariantCulture))
            {
                type = type.GenericTypeArguments[0];
            }

            // This will convert the native CLR type to an SQL data type.
            switch (type.FullName)
            {
                case "System.Boolean":
                    sqlDataType = "Bit";
                    break;

                case "System.Byte":
                    sqlDataType = "TinyInt";
                    break;

                case "System.Byte[]":
                    sqlDataType = "Image";
                    break;

                case "System.DateTime":
                    sqlDataType = "DateTime";
                    break;

                case "System.Decimal":
                    sqlDataType = "Decimal";
                    break;

                case "System.Double":
                    sqlDataType = "Float";
                    break;

                case "System.Float":
                    sqlDataType = "Real";
                    break;

                case "System.Guid":
                    sqlDataType = "UniqueIdentifier";
                    break;

                case "System.Int16":
                    sqlDataType = "SmallInt";
                    break;

                case "System.Int32":
                    sqlDataType = "Int";
                    break;

                case "System.Int64":
                    sqlDataType = "BigInt";
                    break;

                case "System.Object":
                    sqlDataType = "Variant";
                    break;

                case "System.String":
                    sqlDataType = "NVarChar";
                    break;

                default:

                    // Enumerations are converted into a generic data type on the SQL Server.
                    sqlDataType = "Int";
                    break;
            }

            // This expression can be used when constructing parameters for an SQL query.
            return SyntaxFactory.Argument(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("SqlDbType"),
                    SyntaxFactory.IdentifierName(sqlDataType)));
        }

        /// <summary>
        /// Translates the given type into a type syntax.
        /// </summary>
        /// <param name="type">The CLR type.</param>
        /// <returns>A Roslyn type syntax corresponding to the CLR type.</returns>
        public static TypeSyntax FromType(Type type)
        {
            if (type.FullName.StartsWith("System.Nullable", StringComparison.InvariantCulture))
            {
                Type innerType = type.GenericTypeArguments[0];
                switch (innerType.FullName)
                {
                    case "System.Boolean":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)));

                    case "System.Double":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)));

                    case "System.Decimal":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DecimalKeyword)));

                    case "System.Int16":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ShortKeyword)));

                    case "System.Int32":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)));

                    case "System.Int64":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)));

                    case "System.Object":

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)));

                    default:

                        return SyntaxFactory.NullableType(
                            SyntaxFactory.IdentifierName(innerType.Name));
                }
            }

            switch (type.FullName)
            {
                case "System.Boolean":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));

                case "System.Double":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword));

                case "System.Decimal":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DecimalKeyword));

                case "System.Int16":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ShortKeyword));

                case "System.Int32":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));

                case "System.Int64":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword));

                case "System.Object":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword));

                case "System.String":

                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));

                case "System.Object[]":

                    return SyntaxFactory.ArrayType(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                        .WithRankSpecifiers(
                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                SyntaxFactory.ArrayRankSpecifier(
                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                        SyntaxFactory.OmittedArraySizeExpression()
                                        .WithOmittedArraySizeExpressionToken(SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken))))));

                case "System.String[]":

                    return SyntaxFactory.ArrayType(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)))
                        .WithRankSpecifiers(
                            SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                SyntaxFactory.ArrayRankSpecifier(
                                    SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                        SyntaxFactory.OmittedArraySizeExpression()
                                        .WithOmittedArraySizeExpressionToken(SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken))))));

                default:

                    return SyntaxFactory.IdentifierName(type.Name);
            }
        }
    }
}