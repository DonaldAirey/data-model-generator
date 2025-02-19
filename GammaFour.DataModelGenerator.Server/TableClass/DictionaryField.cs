// <copyright file="DictionaryField.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.TableClass
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
    public class DictionaryField : SyntaxElement
    {
        /// <summary>
        /// The description of the table.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryField"/> class.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        public DictionaryField(TableElement tableElement)
        {
            // Initialize the object.
            this.Name = "dictionary";
            this.tableElement = tableElement;

            //        /// <summary>
            //        /// The primary index.
            //        /// </summary>
            //        private readonly Dictionary<string, Asset> dictionary = new Dictionary<string, Asset>();
            this.Syntax = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("Dictionary"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(this.TypeArguments)))
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier("dictionary"))
                        .WithInitializer(
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("Dictionary"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(this.TypeArguments)))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList()))))))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword),
                    }))
            .WithLeadingTrivia(DictionaryField.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// The collection of records.
                    //        /// </summary>
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
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " <summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " The primary index.",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextLiteral(
                                                SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(Strings.CommentExterior)),
                                                " </summary>",
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                            SyntaxFactory.XmlTextNewLine(
                                                SyntaxFactory.TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                SyntaxFactory.TriviaList()),
                                        }))))),
                };

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the type arguments for the dictionary.
        /// </summary>
        /// <returns>The type arguments for the dictionary.</returns>
        private SeparatedSyntaxList<TypeSyntax> TypeArguments
        {
            get
            {
                // Create either a single element key, or a multiple element (tuple) key.
                if (this.tableElement.PrimaryIndex.Columns.Count == 1)
                {
                    // <string, Account>
                    return SyntaxFactory.SeparatedList<TypeSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.IdentifierName(this.tableElement.Name),
                        });
                }
                else
                {
                    // Gather up the elements of the tuple.
                    var tokens = new List<SyntaxNodeOrToken>();
                    foreach (var columnReferenceElement in this.tableElement.PrimaryIndex.Columns)
                    {
                        var columnElement = columnReferenceElement.Column;
                        if (tokens.Count != 0)
                        {
                            tokens.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                        }

                        tokens.Add(SyntaxFactory.TupleElement(columnElement.GetTypeSyntax()));
                    }

                    // <(string, string), Account>
                    return SyntaxFactory.SeparatedList<TypeSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            SyntaxFactory.TupleType(
                            SyntaxFactory.SeparatedList<TupleElementSyntax>(tokens)),
                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                            SyntaxFactory.IdentifierName(this.tableElement.Name),
                        });
                }
            }
        }
    }
}