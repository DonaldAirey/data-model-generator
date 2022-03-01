// <copyright file="DataModelField.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
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
    public class DataModelField : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelField"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The XML Schema document.</param>
        public DataModelField(XmlSchemaDocument xmlSchemaDocument)
        {
            // Validate the argument.
            if (xmlSchemaDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlSchemaDocument));
            }

            // Initialize the object.
            this.Name = xmlSchemaDocument.DataModel.ToVariableName();

            //        private DataModel dataModel;
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(xmlSchemaDocument.DataModel))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.Name)))))
                .WithModifiers(DataModelField.Modifiers);
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
                        SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword),
                    });
            }
        }
    }
}