// <copyright file="UniqueKeyExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.RestClass
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
                            SyntaxFactory.IdentifierName(httpVerb))
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