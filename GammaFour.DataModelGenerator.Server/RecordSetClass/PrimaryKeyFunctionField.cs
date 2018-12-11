// <copyright file="PrimaryKeyFunctionField.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordSetClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field to hold the current contents of the record.
    /// </summary>
    public class PrimaryKeyFunctionField : SyntaxElement
    {
        /// <summary>
        /// The description of the table.
        /// </summary>
        private UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryKeyFunctionField"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">The table element.</param>
        public PrimaryKeyFunctionField(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.
            this.Name = "primaryKeyFunction";
            this.uniqueKeyElement = uniqueKeyElement;

            //        /// <summary>
            //        /// Used to get the primary key from the record.
            //        /// </summary>
            //        private Func<Buyer, object> primaryKeyFunction = ((Expression<Func<Buyer, object>>)(b => b.BuyerId)).Compile();
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("Func"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SeparatedList<TypeSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.PredefinedType(
                                            SyntaxFactory.Token(SyntaxKind.ObjectKeyword))
                                    }))))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier("primaryKeyFunction"))
                            .WithInitializer(this.Initializer))))
                .WithModifiers(this.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
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
                //        /// Used to get the primary key from the record.
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
                                                " Used to get the primary key from the record.",
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
        /// Gets the initializer.
        /// </summary>
        private EqualsValueClauseSyntax Initializer
        {
            get
            {
                // Used as a variable when constructing the lambda expression.
                string abbreviation = this.uniqueKeyElement.Name[0].ToString().ToLower();

                // This will create an expression for extracting the key from record.
                CSharpSyntaxNode syntaxNode = null;
                if (this.uniqueKeyElement.Columns.Count == 1)
                {
                    // A simple key can be used like a value type.
                    syntaxNode = SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(abbreviation),
                        SyntaxFactory.IdentifierName(this.uniqueKeyElement.Columns[0].Column.Name));
                }
                else
                {
                    // A Compound key must be constructed from an anomymous type.
                    List<SyntaxNodeOrToken> keyElements = new List<SyntaxNodeOrToken>();
                    foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
                    {
                        if (keyElements.Count != 0)
                        {
                            SyntaxFactory.Token(SyntaxKind.CommaToken);
                        }

                        keyElements.Add(
                            SyntaxFactory.AnonymousObjectMemberDeclarator(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(abbreviation),
                                    SyntaxFactory.IdentifierName(columnReferenceElement.Column.Name))));
                    }

                    syntaxNode = SyntaxFactory.AnonymousObjectCreationExpression(
                            SyntaxFactory.SeparatedList<AnonymousObjectMemberDeclaratorSyntax>(keyElements.ToArray()));
                }

                // = ((Expression<Func<Buyer, object>>)(b => b.BuyerId)).Compile();
                return SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ParenthesizedExpression(
                                SyntaxFactory.CastExpression(
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Expression"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("Func"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SeparatedList<TypeSyntax>(
                                                            new SyntaxNodeOrToken[]
                                                            {
                                                                SyntaxFactory.IdentifierName(this.uniqueKeyElement.Table.Name),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.PredefinedType(
                                                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword))
                                                            })))))),
                                    SyntaxFactory.ParenthesizedExpression(
                                        SyntaxFactory.SimpleLambdaExpression(
                                            SyntaxFactory.Parameter(SyntaxFactory.Identifier("b")), syntaxNode)))),
                            SyntaxFactory.IdentifierName("Compile"))));
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
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword)
                    });
            }
        }
    }
}