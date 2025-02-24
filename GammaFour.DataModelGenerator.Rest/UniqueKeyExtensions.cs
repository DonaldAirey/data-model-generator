// <copyright file="UniqueKeyExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class UniqueKeyExtensions
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="uniqueKeyElement">The description of a unique key.</param>
        /// <param name="isDecorated">Indicates that the argument name should be prefixed with the index name.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static SeparatedSyntaxList<ArgumentSyntax> GetSyntax(UniqueIndexElement uniqueKeyElement, bool isDecorated = false)
        {
            //                    country = this.dataModel.Countries.CountryCountryCodeKey.Find(countryCountryCodeKeyCountryCode);
            //                    region = this.dataModel.Regions.RegionExternalKey.Find((regionExternalKeyName, regionExternalKeyCountryCode));
            SeparatedSyntaxList<ArgumentSyntax> findParameters;
            if (uniqueKeyElement.Columns.Count == 1)
            {
                ColumnElement columnElement = uniqueKeyElement.Columns[0].Column;
                string attributeName = isDecorated ? $"{uniqueKeyElement.Name.ToCamelCase()}{columnElement.Name}" : columnElement.Name.ToVariableName();
                findParameters = SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                    SyntaxFactory.Argument(
                        SyntaxFactory.IdentifierName(attributeName)));
            }
            else
            {
                List<SyntaxNodeOrToken> keys = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
                {
                    if (keys.Count != 0)
                    {
                        keys.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    ColumnElement columnElement = columnReferenceElement.Column;
                    string attributeName = isDecorated ? $"{uniqueKeyElement.Name.ToCamelCase()}{columnElement.Name}" : columnElement.Name.ToVariableName();
                    keys.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(attributeName)));
                }

                findParameters = SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                    SyntaxFactory.Argument(
                        SyntaxFactory.TupleExpression(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(keys))));
            }

            // This is either the parameter or a tuple that can be used as parameters to a 'Find' operation.
            return findParameters;
        }

        /// <summary>
        /// Gets the REST parameter list.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique index element.</param>
        /// <param name="httpVerb">The HTTP verb.</param>
        /// <returns>A list of HTTP attributes describing the key.</returns>
        public static SyntaxList<AttributeListSyntax> GetKeyAsHttpAttributes(this UniqueIndexElement uniqueIndexElement, string httpVerb)
        {
            // This collects all the attributes.
            List<AttributeListSyntax> attributes = new List<AttributeListSyntax>();

            string literal = string.Empty;
            foreach (ColumnReferenceElement columnReferenceElement in uniqueIndexElement.Columns)
            {
                // Separate the key elements.
                if (!string.IsNullOrEmpty(literal))
                {
                    literal += "/";
                }

                // A description of the key element.
                literal += $"{{{columnReferenceElement.Column.Name.ToCamelCase()}}}";
            }

            //        [HttpDelete("{name}/{countryCode}")]
            return SyntaxFactory.SingletonList<AttributeListSyntax>(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                        SyntaxFactory.Attribute(
                            SyntaxFactory.IdentifierName("HttpDelete"))
                    .WithArgumentList(
                        SyntaxFactory.AttributeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                SyntaxFactory.AttributeArgument(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(literal)))))))));
        }

        /// <summary>
        /// Gets the REST argument list.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique index element.</param>
        /// <returns>A list of HTTP arguments describing the key.</returns>
        public static IEnumerable<SyntaxNodeOrToken> GetKeyAsHttpArguments(this UniqueIndexElement uniqueIndexElement)
        {
            // Collect the parameters here.
            List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

            // [FromRoute] System.Guid accountId
            foreach (var columnElement in uniqueIndexElement.Columns)
            {
                if (parameters.Count != 0)
                {
                    parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                }

                // [FromRoute] accountId
                parameters.Add(
                    SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(columnElement.Column.Name.ToVariableName()))
                        .WithAttributeLists(
                            SyntaxFactory.SingletonList<AttributeListSyntax>(
                                SyntaxFactory.AttributeList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                                        SyntaxFactory.Attribute(
                                            SyntaxFactory.IdentifierName("FromRoute"))))))
                        .WithType(columnElement.Column.GetTypeSyntax()));
            }

            return parameters;
        }
    }
}