// <copyright file="DbContextField.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService.RestServiceClass
{
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field to hold a buffer for creating transaction log items.
    /// </summary>
    public class DbContextField : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextField"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The XML Schema document.</param>
        public DbContextField(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.Name = $"{xmlSchemaDocument.Name.ToCamelCase()}Context";

            //        private DomainContext domainContext;
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName($"{xmlSchemaDocument.Name}Context"))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.Name)))))
                .WithModifiers(DbContextField.Modifiers);
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
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
                    });
            }
        }
    }
}