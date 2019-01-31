// <copyright file="LockTimeoutField.cs" company="Gamma Four, Inc.">
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
    public class LockTimeoutField : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LockTimeoutField"/> class.
        /// </summary>
        public LockTimeoutField()
        {
            // Initialize the object.
            this.Name = "lockTimeout";

            //        private int lockTimeout;
            this.Syntax = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.IntKeyword)))
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(this.Name)))))
                .WithModifiers(LockTimeoutField.Modifiers);
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