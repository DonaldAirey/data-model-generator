// <copyright file="UniqueKeyExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class UniqueKeyExpression
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="uniqueKeyElement">The description of a unique key.</param>
        /// <param name="isDecorated">Indicates that the argument name should be prefixed with the index name.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static SeparatedSyntaxList<ArgumentSyntax> GetSyntax(UniqueElement uniqueKeyElement, bool isDecorated = false)
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
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="uniqueKeyElement">The description of a unique key.</param>
        /// <param name="variableName">The name of the variable holding the keys.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static SeparatedSyntaxList<ArgumentSyntax> GetMemberSyntax(UniqueElement uniqueKeyElement, string variableName)
        {
            SeparatedSyntaxList<ArgumentSyntax> findParameters;
            if (uniqueKeyElement.Columns.Count == 1)
            {
                //                    country = this.dataModel.Countries.CountryCountryCodeKey.Find(countryCountryCodeKeyCountryCode);
                ColumnElement columnElement = uniqueKeyElement.Columns[0].Column;
                string propertyName = columnElement.Name;
                findParameters = SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(propertyName))));
            }
            else
            {
                //                    region = this.dataModel.Regions.RegionExternalKey.Find((regionExternalKeyName, regionExternalKeyCountryCode));
                List<SyntaxNodeOrToken> keys = new List<SyntaxNodeOrToken>();
                foreach (ColumnReferenceElement columnReferenceElement in uniqueKeyElement.Columns)
                {
                    if (keys.Count != 0)
                    {
                        keys.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    ColumnElement columnElement = columnReferenceElement.Column;
                    string attributeName = columnElement.Name.ToVariableName();
                    keys.Add(
                        SyntaxFactory.Argument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(variableName),
                                SyntaxFactory.IdentifierName(attributeName))));
                }

                findParameters = SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                    SyntaxFactory.Argument(
                        SyntaxFactory.TupleExpression(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(keys))));
            }

            // This is either the parameter or a tuple that can be used as parameters to a 'Find' operation.
            return findParameters;
        }
    }
}