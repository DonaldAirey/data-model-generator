// <copyright file="DictionaryField.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common.ForeignKeyIndexClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field to hold the parent table.
    /// </summary>
    public class DictionaryField : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private ForeignKeyElement foreignKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryField"/> class.
        /// </summary>
        /// <param name="foreignKeyElement">The table schema.</param>
        public DictionaryField(ForeignKeyElement foreignKeyElement)
        {
            // Initialize the object.
            this.foreignKeyElement = foreignKeyElement;

            // This is the name of the field.
            this.Name = "dictionary";

            //        /// <summary>
            //        /// The dictionary containing the index.
            //        /// </summary>
            //        private Dictionary<Guid, ProvinceRow> dictionary = new Dictionary<Guid, ProvinceRow>();
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(this.Type)
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.Name))
                            .WithInitializer(this.Initializer))))
                .WithModifiers(DictionaryField.Modifiers)
                .WithLeadingTrivia(DictionaryField.DocumentationComment);
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
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                    });
            }
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
                //        /// The dictionary containing the index.
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
                                                " The dictionary containing the index.",
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
                //  = new Dictionary<ProvinceExternalId0Key, ProvinceRow>();
                return SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.ObjectCreationExpression(this.Type)
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList()));
            }
        }

        /// <summary>
        /// Gets the syntax for the field's type.
        /// </summary>
        private TypeSyntax Type
        {
            get
            {
                // Collect the datatypes used to create the dictionary.
                List<TypeSyntax> types = new List<TypeSyntax>();

                // The key of the dictionary is a simple or compound key that can uniquely identify the parent row.
                if (this.foreignKeyElement.UniqueKey.Columns.Count == 1)
                {
                    types.Add(Conversions.FromType(this.foreignKeyElement.UniqueKey.Columns.Single().Column.ColumnType));
                }
                else
                {
                    types.Add(SyntaxFactory.IdentifierName(this.foreignKeyElement.Refer + "Set"));
                }

                // This HashSet holds the child rows.
                types.Add(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("HashSet"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name)))));

                // Dictionary<Guid, HashSet<ProvinceRow>>
                //                 or
                // Dictionary<CustomerLastNameDateOfBirthKeySet, HashSet<MemberRow>>
                return SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("Dictionary"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SeparatedList<TypeSyntax>(types)));
            }
        }
    }
}