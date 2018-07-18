// <copyright file="DictionaryField.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common.UniqueKeyIndexClass
{
    using System;
    using System.Collections.Generic;
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
        private UniqueConstraintSchema uniqueConstraintSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryField"/> class.
        /// </summary>
        /// <param name="uniqueConstraintSchema">The table schema.</param>
        public DictionaryField(UniqueConstraintSchema uniqueConstraintSchema)
        {
            // Initialize the object.
            this.uniqueConstraintSchema = uniqueConstraintSchema;

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
                                                " The dictionary containing the index.",
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
                //  = new Dictionary<ProvinceExternalId0Key, ProvinceRow>();
                return SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.ObjectCreationExpression(this.Type)
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList()));
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

        /// <summary>
        /// Gets the syntax for the field's type.
        /// </summary>
        private TypeSyntax Type
        {
            get
            {
                // Collect the datatypes used to create the dictionary.
                List<TypeSyntax> types = new List<TypeSyntax>();

                // Keys with a single element don't require a compound key in order to access the dictionary.
                if (this.uniqueConstraintSchema.Columns.Count == 1)
                {
                    types.Add(Conversions.FromType(this.uniqueConstraintSchema.Columns[0].Type));
                }
                else
                {
                    types.Add(SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Name + "Set"));
                }

                // And finally the type of row found in this dictionary.
                types.Add(SyntaxFactory.IdentifierName(this.uniqueConstraintSchema.Table.Name + "Row"));

                // Dictionary<Guid, ProvinceRow>
                return SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("Dictionary"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SeparatedList<TypeSyntax>(types)));
            }
        }
    }
}
