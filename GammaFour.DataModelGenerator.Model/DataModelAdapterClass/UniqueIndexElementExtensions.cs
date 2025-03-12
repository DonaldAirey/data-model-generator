// <copyright file="UniqueIndexElementExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.DataModelAdapterClass
{
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates a unique key.
    /// </summary>
    public static class UniqueIndexElementExtensions
    {
        /// <summary>
        /// Creates an argument for a unique key.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ArgumentSyntax GetKeyAsRestArguments(this UniqueIndexElement uniqueIndexElement)
        {
            // This collects all the elements of the key in an REST endpoint description.
            List<InterpolatedStringContentSyntax> interpolatedStringContentSyntax = new List<InterpolatedStringContentSyntax>
            {
                SyntaxFactory.InterpolatedStringText()
                .WithTextToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.InterpolatedStringTextToken,
                        $"{uniqueIndexElement.Table.Document.Name.ToCamelCase()}/{uniqueIndexElement.Table.Name.ToCamelCase().ToPlural()}/",
                        $"{uniqueIndexElement.Table.Document.Name.ToCamelCase()}/{uniqueIndexElement.Table.Name.ToCamelCase().ToPlural()}/",
                        SyntaxFactory.TriviaList())),
            };

            // Create an interpolated string for each element of the key.
            foreach (var columnReferenceElement in uniqueIndexElement.Columns)
            {
                var column = columnReferenceElement.Column;

                // Place a separator between key elements.
                if (interpolatedStringContentSyntax.Count > 1)
                {
                    interpolatedStringContentSyntax.Add(
                        SyntaxFactory.InterpolatedStringText()
                        .WithTextToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.InterpolatedStringTextToken,
                                "/",
                                "/",
                                SyntaxFactory.TriviaList())));
                }

                // provinceId
                interpolatedStringContentSyntax.Add(
                    SyntaxFactory.Interpolation(
                        SyntaxFactory.IdentifierName(column.Name.ToVariableName())));
            }

            // $"dataModel/accountGroups/{province.ProvinceId}/{province.RegionId}"
            return SyntaxFactory.Argument(
                SyntaxFactory.InterpolatedStringExpression(
                    SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                .WithContents(
                    SyntaxFactory.List(interpolatedStringContentSyntax)));
        }

        /// <summary>
        /// Creates an argument for a unique key.
        /// </summary>
        /// <param name="uniqueIndexElement">The unique key element.</param>
        /// <param name="variableName">The varioable name.</param>
        /// <returns>An argument that extracts a key from an object.</returns>
        public static ArgumentSyntax GetKeyAsRestArguments(this UniqueIndexElement uniqueIndexElement, string variableName)
        {
            // This collects all the elements of the key in an REST endpoint description.
            List<InterpolatedStringContentSyntax> interpolatedStringContentSyntax = new List<InterpolatedStringContentSyntax>
            {
                SyntaxFactory.InterpolatedStringText()
                .WithTextToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.InterpolatedStringTextToken,
                        $"{uniqueIndexElement.Table.Document.Name.ToCamelCase()}/{uniqueIndexElement.Table.Name.ToCamelCase().ToPlural()}/",
                        $"{uniqueIndexElement.Table.Document.Name.ToCamelCase()}/{uniqueIndexElement.Table.Name.ToCamelCase().ToPlural()}/",
                        SyntaxFactory.TriviaList())),
            };

            // Create an interpolated string for each element of the key.
            foreach (var columnReferenceElement in uniqueIndexElement.Columns)
            {
                var column = columnReferenceElement.Column;

                // Place a separator between key elements.
                if (interpolatedStringContentSyntax.Count > 1)
                {
                    interpolatedStringContentSyntax.Add(
                        SyntaxFactory.InterpolatedStringText()
                        .WithTextToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.InterpolatedStringTextToken,
                                "/",
                                "/",
                                SyntaxFactory.TriviaList())));
                }

                // province.ProvinceId
                interpolatedStringContentSyntax.Add(
                    SyntaxFactory.Interpolation(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(column.Name))));
            }

            // $"dataModel/accountGroups/{province.ProvinceId}/{province.RegionId}"
            return SyntaxFactory.Argument(
                SyntaxFactory.InterpolatedStringExpression(
                    SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken))
                .WithContents(
                    SyntaxFactory.List(interpolatedStringContentSyntax)));
        }
    }
}