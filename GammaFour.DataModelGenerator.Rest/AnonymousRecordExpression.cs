// <copyright file="AnonymousRecordExpression.cs" company="Gamma Four, Inc.">
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
    public static class AnonymousRecordExpression
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static ExpressionSyntax GetSyntax(TableElement tableElement)
        {
            // new { country.CountryId, country.CountryCode, country.Name, country.RowVersion };
            List<SyntaxNodeOrToken> properties = new List<SyntaxNodeOrToken>();
            foreach (ColumnElement columnElement in tableElement.Columns)
            {
                if (properties.Count != 0)
                {
                    properties.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                }

                properties.Add(
                    SyntaxFactory.AnonymousObjectMemberDeclarator(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                            SyntaxFactory.IdentifierName(columnElement.Name))));
            }

            return SyntaxFactory.AnonymousObjectCreationExpression(
                SyntaxFactory.SeparatedList<AnonymousObjectMemberDeclaratorSyntax>(properties));
        }
    }
}