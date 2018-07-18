// <copyright file="KnownTypes.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates the 'KnownType' directives for a given interface.
    /// </summary>
    public class KnownTypes
    {
        /// <summary>
        /// These are the types that are already known to the WCF serializer.
        /// </summary>
        private static HashSet<string> knownTypes = new HashSet<string>()
        {
            typeof(bool).FullName,
            typeof(sbyte).FullName,
            typeof(byte).FullName,
            typeof(char).FullName,
            typeof(short).FullName,
            typeof(ushort).FullName,
            typeof(int).FullName,
            typeof(uint).FullName,
            typeof(long).FullName,
            typeof(ulong).FullName,
            typeof(float).FullName,
            typeof(double).FullName,
            typeof(decimal).FullName,
            typeof(string).FullName,
            typeof(DateTime).FullName,
            typeof(Guid).FullName,
            typeof(bool?).FullName,
            typeof(sbyte?).FullName,
            typeof(byte?).FullName,
            typeof(char?).FullName,
            typeof(short?).FullName,
            typeof(ushort?).FullName,
            typeof(int?).FullName,
            typeof(uint?).FullName,
            typeof(long?).FullName,
            typeof(ulong?).FullName,
            typeof(float?).FullName,
            typeof(double?).FullName,
            typeof(decimal?).FullName,
            typeof(DateTime?).FullName,
            typeof(Guid?).FullName
        };

        /// <summary>
        /// Emits a the 'KnowType' attribute for a given interface for the datatype that is not implicitly known to the serializer.
        /// </summary>
        /// <param name="type">The type to be emitted as a 'known type' to the service.</param>
        /// <returns>The syntax for an attribute that identifies a type to an interface.</returns>
        public static AttributeListSyntax Emit(Type type)
        {
            // Arrays are handled differently from non-array types.
            if (type.IsArray)
            {
                return SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ServiceKnownType"))
                        .WithArgumentList(
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.TypeOfExpression(
                                            SyntaxFactory.ArrayType(Conversions.FromType(type.GetElementType()))
                                            .WithRankSpecifiers(
                                                SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                    SyntaxFactory.ArrayRankSpecifier(
                                                        SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                            SyntaxFactory.OmittedArraySizeExpression()
                                                            .WithOmittedArraySizeExpressionToken(
                                                                SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken)))))))
                                        .WithKeyword(SyntaxFactory.Token(SyntaxKind.TypeOfKeyword))))))));
            }

            // Create a 'ServiceKnownType' attribute for the simple type.
            return SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                    SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName("ServiceKnownType"))
                    .WithArgumentList(
                        SyntaxFactory.AttributeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                SyntaxFactory.AttributeArgument(
                                    SyntaxFactory.TypeOfExpression(Conversions.FromType(type))
                                    .WithKeyword(SyntaxFactory.Token(SyntaxKind.TypeOfKeyword))))))));
        }

        /// <summary>
        /// Emits all the 'KnowType' attributes for a given interface for the datatypes that are not implicitly known to the serializer.
        /// </summary>
        /// <param name="dataModelSchema">The data model schema.</param>
        /// <returns>A list of attributes describing the data types that are not predefined..</returns>
        public static IEnumerable<AttributeListSyntax> Emit(DataModelSchema dataModelSchema)
        {
            // Once a 'KnownType' attribute has been emitted for a given data type, we don't need to emit it again.  This table keeps track of what
            // has been emitted for the current interface.
            HashSet<string> unknownTypes = new HashSet<string>();

            // The list of KnownAttributes attached to the method.
            List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

            // Run through every data type in the data model.  If a datatype is not implicit (or previously recognized 'KnownType'), then emit a
            // compiler directive to serialize the type should it appear in a generic object.
            foreach (TableSchema tableSchema in dataModelSchema.Tables)
            {
                foreach (ColumnSchema columnSchema in tableSchema.Columns)
                {
                    if (!KnownTypes.knownTypes.Contains(columnSchema.Type.FullName) && !unknownTypes.Contains(columnSchema.Type.FullName))
                    {
                        unknownTypes.Add(columnSchema.Type.FullName);
                        attributes.Add(KnownTypes.Emit(columnSchema.Type));
                    }
                }
            }

            // This array describes the known types for an interface.
            return attributes;
        }

        /// <summary>
        /// Emits all the 'KnowType' attributes for a given interface for the datatypes that are not implicitly known to the serializer.
        /// </summary>
        /// <param name="tableSchema">The table schema for which we're generating an interface.</param>
        /// <returns>A list of attributes describing the data types that are not predefined..</returns>
        public static SyntaxList<AttributeListSyntax> Emit(TableSchema tableSchema)
        {
            // Once a 'KnownType' attribute has been emitted for a given data type, we don't need to emit it again.  This table keeps track of what
            // has been emitted for the current interface.
            HashSet<string> unknownTypes = new HashSet<string>();
            unknownTypes.Add(typeof(Guid).FullName);

            // The list of KnownAttributes attached to the method.
            SyntaxList<AttributeListSyntax> attributes = default(SyntaxList<AttributeListSyntax>);

            // Run through every data type in the table.  If a datatype is not implicit (or previously recognized 'KnownType'), then emit a compiler
            // directive to serialize the type should it appear in a generic object.
            foreach (ColumnSchema columnSchema in tableSchema.Columns)
            {
                if (!KnownTypes.knownTypes.Contains(columnSchema.Type.FullName) && !unknownTypes.Contains(columnSchema.Type.FullName))
                {
                    unknownTypes.Add(columnSchema.Type.FullName);
                    attributes.Add(KnownTypes.Emit(columnSchema.Type));
                }
            }

            // This array describes the known types for an interface.
            return attributes;
        }
    }
}