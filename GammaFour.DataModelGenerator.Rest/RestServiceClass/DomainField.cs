// <copyright file="DomainField.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using System;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field to hold a buffer for creating transaction log items.
    /// </summary>
    public class DomainField : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainField"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The XML Schema document.</param>
        public DomainField(XmlSchemaDocument xmlSchemaDocument)
        {
            // Validate the argument.
            if (xmlSchemaDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlSchemaDocument));
            }

            // Initialize the object.
            this.Name = xmlSchemaDocument.Domain.ToVariableName();

            //        private Domain domain;
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(xmlSchemaDocument.Domain))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.Name)))))
                .WithModifiers(DomainField.Modifiers);
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // internal
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                    });
            }
        }
    }
}