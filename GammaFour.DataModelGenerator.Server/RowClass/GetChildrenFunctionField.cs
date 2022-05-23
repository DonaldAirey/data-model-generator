// <copyright file="GetChildrenFunctionField.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RowClass
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
    public class GetChildrenFunctionField : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly ForeignKeyElement foreignKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChildrenFunctionField"/> class.
        /// </summary>
        /// <param name="foreignKeyElement">A description of a foreign key.</param>
        public GetChildrenFunctionField(ForeignKeyElement foreignKeyElement)
        {
            // Initialize the object.
            this.foreignKeyElement = foreignKeyElement;
            this.Name = $"get{this.foreignKeyElement.UniqueChildName}";

            //        /// <summary>
            //        /// Function to get child Buyers.
            //        /// </summary>
            //        private Func<IEnumerable<Buyer>> getBuyers;
            this.Syntax = SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("Func"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.GenericName(
                                        SyntaxFactory.Identifier("IEnumerable"))
                                    .WithTypeArgumentList(
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                SyntaxFactory.IdentifierName(this.foreignKeyElement.Table.Name))))))))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(this.Name)))))
                .WithModifiers(GetChildrenFunctionField.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
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
                                                                SyntaxFactory.OmittedArraySizeExpression())))),
                                            }))),
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
                //        /// Function to get child buyers.
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
                                                $" Function to get child {this.foreignKeyElement.Table.Name.ToCamelCase().ToPlural()}.",
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
    }
}