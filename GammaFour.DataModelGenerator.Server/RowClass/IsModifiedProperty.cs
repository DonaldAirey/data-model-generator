// <copyright file="IsModifiedProperty.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
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
    /// Creates a property that navigates to the parent record.
    /// </summary>
    public class IsModifiedProperty : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsModifiedProperty"/> class.
        /// </summary>
        public IsModifiedProperty()
        {
            // Initialize the object.
            this.Name = "IsModified";

            //        /// <summary>
            //        /// Gets the modified state.
            //        /// </summary>
            //        [JsonIgnore]
            //        public bool IsModified { get; private set; }
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithAttributeLists(
                SyntaxFactory.SingletonList<AttributeListSyntax>(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("JsonIgnore"))))))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithAccessorList(
                SyntaxFactory.AccessorList(
                    SyntaxFactory.List<AccessorDeclarationSyntax>(
                        new AccessorDeclarationSyntax[]
                        {
                            SyntaxFactory.AccessorDeclaration(
                                SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(
                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(
                                SyntaxKind.SetAccessorDeclaration)
                            .WithModifiers(
                                SyntaxFactory.TokenList(
                                    SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                            .WithSemicolonToken(
                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        })))
            .WithLeadingTrivia(IsModifiedProperty.DocumentationComment);
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
                    //        /// Gets the modified state.
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
                                                " Gets the modified state.",
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
    }
}