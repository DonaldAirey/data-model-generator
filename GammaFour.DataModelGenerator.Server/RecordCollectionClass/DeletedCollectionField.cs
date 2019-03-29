// <copyright file="DeletedCollectionField.cs" company="Gamma Four, Inc.">
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
    public class DeletedCollectionField : SyntaxElement
    {
        /// <summary>
        /// The description of the table.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletedCollectionField"/> class.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        public DeletedCollectionField(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "deletedCollection";

            //        /// <summary>
            //        /// The collection of deleted records.
            //        /// </summary>
            //        private List<ValueTuple<DateTime, Alert>> deletedCollection = new List<ValueTuple<DateTime, Alert>>();
            this.Syntax = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("List"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("ValueTuple"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SeparatedList<TypeSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                SyntaxFactory.IdentifierName("DateTime"),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.IdentifierName(this.tableElement.Name)
                                            })))))))
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(this.Name))
                            .WithInitializer(this.Initializer))))
                .WithModifiers(DeletedCollectionField.Modifiers)
                .WithLeadingTrivia(DeletedCollectionField.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// The collection of deleted records.
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
                                                " The collection of deleted records.",
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
        private static SyntaxTokenList Modifiers
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

        /// <summary>
        /// Gets the initializer.
        /// </summary>
        private EqualsValueClauseSyntax Initializer
        {
            get
            {
                // = new List<ValueTuple<DateTime, Alert>>();
                return SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("List"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("ValueTuple"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SeparatedList<TypeSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    SyntaxFactory.IdentifierName("DateTime"),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name)
                                                })))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList()));
            }
        }
    }
}