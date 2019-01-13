// <copyright file="DefaultChildrenField.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field to hold the current contents of the row.
    /// </summary>
    public class DefaultChildrenField : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private ForeignKeyElement foreignKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultChildrenField"/> class.
        /// </summary>
        /// <param name="foreignKeyElement">A description of a foreign key.</param>
        public DefaultChildrenField(ForeignKeyElement foreignKeyElement)
        {
            // Initialize the object.
            this.foreignKeyElement = foreignKeyElement;
            this.Name = $"default{this.foreignKeyElement.Table.Name.ToPlural()}";

            //        /// <summary>
            //        /// Default Buyers.
            //        /// </summary>
            //        private static readonly List<Buyer> defaultBuyers = new List<Buyer>();
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("List"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name)))))
                    .WithVariables(this.Variables))
                .WithModifiers(this.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the generic type declaration.
        /// </summary>
        private TypeArgumentListSyntax ArgumentListSyntax
        {
            get
            {
                return SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SeparatedList<TypeSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                SyntaxFactory.IdentifierName("RecordVersion"),
                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("Func"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SeparatedList<TypeSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Name),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.ArrayType(
                                                    SyntaxFactory.PredefinedType(
                                                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                                .WithRankSpecifiers(
                                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                                        SyntaxFactory.ArrayRankSpecifier(
                                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                                SyntaxFactory.OmittedArraySizeExpression()))))
                                            })))
                            }));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Default Buyers.
                //        /// </summary>
                comments.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                SyntaxFactory.XmlText()
                                .WithTextTokens(
                                    SyntaxFactory.TokenList(
                                        new[]
                                        {
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///")),
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                $" Default {this.foreignKeyElement.Table.Name}.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("         ///")),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList())
                                        }))))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                        SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)
                    });
            }
        }

        /// <summary>
        /// Gets the initialization list.
        /// </summary>
        private SeparatedSyntaxList<VariableDeclaratorSyntax> Variables
        {
            get
            {
                // defaultBuyers = new List<Buyer>()
                return SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier(this.Name))
                    .WithInitializer(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("List"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                            SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name)))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList()))));
            }
        }
    }
}