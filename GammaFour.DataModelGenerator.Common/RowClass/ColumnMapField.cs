// <copyright file="ColumnMapField.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common.RowClass
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
    public class ColumnMapField : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnMapField"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        public ColumnMapField(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "columnMap";

            //        /// <summary>
            //        /// Maps the column names to the index
            //        /// </summary>
            //        private static Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { { "RegionCode", 0 }, { "Name", 1 }, { "RegionId", 2 }, { "RowVersion", 3 } };
            this.Syntax = SyntaxFactory.FieldDeclaration(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("Dictionary"))
                    .WithTypeArgumentList(
                        SyntaxFactory.TypeArgumentList(
                            SyntaxFactory.SeparatedList<TypeSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                                }))))
                .WithVariables(this.Variables))
                .WithModifiers(ColumnMapField.Modifiers)
                .WithLeadingTrivia(ColumnMapField.DocumentationComment);
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
                    //        /// Maps the column names to the index
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
                                                " Maps the column names to the index",
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
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
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
                // Construct the map of the columns to their index in the object array.
                // { { "regionId", 0 }, { "code", 1 } }
                int index = 0;
                List<SyntaxNodeOrToken> syntaxNodeOrToken = new List<SyntaxNodeOrToken>();
                foreach (ColumnElement columnElement in this.tableElement.Columns)
                {
                    // ,
                    if (syntaxNodeOrToken.Count != 0)
                    {
                        syntaxNodeOrToken.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
                    }

                    // { "regionId", 0 }
                    syntaxNodeOrToken.Add(
                        SyntaxFactory.InitializerExpression(
                            SyntaxKind.ComplexElementInitializerExpression,
                            SyntaxFactory.SeparatedList<ExpressionSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        SyntaxFactory.Literal(columnElement.Name)),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        SyntaxFactory.Literal(index++)),
                                })));
                }

                // columnMap = new Dictionary<string, int>() { { "regionId", 0 }, { "code", 1 } }
                return SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                    SyntaxFactory.VariableDeclarator(
                        SyntaxFactory.Identifier(this.Name))
                    .WithInitializer(
                        SyntaxFactory.EqualsValueClause(
                            SyntaxFactory.ObjectCreationExpression(
                                SyntaxFactory.GenericName(
                                    SyntaxFactory.Identifier("Dictionary"))
                                .WithTypeArgumentList(
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SeparatedList<TypeSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                SyntaxFactory.PredefinedType(
                                                    SyntaxFactory.Token(SyntaxKind.StringKeyword)),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.PredefinedType(
                                                    SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                                            }))))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("StringComparer"),
                                                SyntaxFactory.IdentifierName("OrdinalIgnoreCase"))))))
                            .WithInitializer(
                                SyntaxFactory.InitializerExpression(
                                    SyntaxKind.CollectionInitializerExpression,
                                    SyntaxFactory.SeparatedList<ExpressionSyntax>(syntaxNodeOrToken))))));
            }
        }
    }
}