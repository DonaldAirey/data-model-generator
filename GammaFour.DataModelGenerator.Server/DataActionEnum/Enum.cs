// <copyright file="Enum.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.DataActionEnum
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a row.
    /// </summary>
    public class Enum : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enum"/> class.
        /// </summary>
        public Enum()
        {
            // Initialize the object.
            this.Name = "DataAction";

            //    /// <summary>
            //    /// The Configuration table.
            //    /// </summary>
            //    public partial class AchEvents : ITable, IEnumerable<AchEvent>
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.EnumDeclaration(this.Name)
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithMembers(
                SyntaxFactory.SeparatedList<EnumMemberDeclarationSyntax>(
                    new SyntaxNodeOrToken[]
                    {
                        SyntaxFactory.EnumMemberDeclaration(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Trivia(
                                        SyntaxFactory.DocumentationCommentTrivia(
                                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                                            SyntaxFactory.List<XmlNodeSyntax>(
                                                new XmlNodeSyntax[]
                                                {
                                                    SyntaxFactory.XmlText()
                                                    .WithTextTokens(
                                                        SyntaxFactory.TokenList(
                                                            SyntaxFactory.XmlTextLiteral(
                                                                SyntaxFactory.TriviaList(
                                                                    SyntaxFactory.DocumentationCommentExterior("///")),
                                                                " ",
                                                                " ",
                                                                SyntaxFactory.TriviaList()))),
                                                    SyntaxFactory.XmlExampleElement(
                                                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                                            SyntaxFactory.XmlText()
                                                            .WithTextTokens(
                                                                SyntaxFactory.TokenList(
                                                                    new[]
                                                                    {
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " Add an item.",
                                                                            " Add an item.",
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " ",
                                                                            " ",
                                                                            SyntaxFactory.TriviaList()),
                                                                    }))))
                                                    .WithStartTag(
                                                        SyntaxFactory.XmlElementStartTag(
                                                            SyntaxFactory.XmlName(
                                                                SyntaxFactory.Identifier("summary"))))
                                                    .WithEndTag(
                                                        SyntaxFactory.XmlElementEndTag(
                                                            SyntaxFactory.XmlName(
                                                                SyntaxFactory.Identifier("summary")))),
                                                    SyntaxFactory.XmlText()
                                                    .WithTextTokens(
                                                        SyntaxFactory.TokenList(
                                                            SyntaxFactory.XmlTextNewLine(
                                                                SyntaxFactory.TriviaList(),
                                                                Environment.NewLine,
                                                                Environment.NewLine,
                                                                SyntaxFactory.TriviaList()))),
                                                })))),
                                "Add",
                                SyntaxFactory.TriviaList())),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.EnumMemberDeclaration(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Trivia(
                                        SyntaxFactory.DocumentationCommentTrivia(
                                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                                            SyntaxFactory.List<XmlNodeSyntax>(
                                                new XmlNodeSyntax[]
                                                {
                                                    SyntaxFactory.XmlText()
                                                    .WithTextTokens(
                                                        SyntaxFactory.TokenList(
                                                            SyntaxFactory.XmlTextLiteral(
                                                                SyntaxFactory.TriviaList(
                                                                    SyntaxFactory.DocumentationCommentExterior("///")),
                                                                " ",
                                                                " ",
                                                                SyntaxFactory.TriviaList()))),
                                                    SyntaxFactory.XmlExampleElement(
                                                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                                            SyntaxFactory.XmlText()
                                                            .WithTextTokens(
                                                                SyntaxFactory.TokenList(
                                                                    new[]
                                                                    {
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " Delete an item.",
                                                                            " Delete an item.",
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " ",
                                                                            " ",
                                                                            SyntaxFactory.TriviaList()),
                                                                    }))))
                                                    .WithStartTag(
                                                        SyntaxFactory.XmlElementStartTag(
                                                            SyntaxFactory.XmlName(
                                                                SyntaxFactory.Identifier("summary"))))
                                                    .WithEndTag(
                                                        SyntaxFactory.XmlElementEndTag(
                                                            SyntaxFactory.XmlName(
                                                                SyntaxFactory.Identifier("summary")))),
                                                    SyntaxFactory.XmlText()
                                                    .WithTextTokens(
                                                        SyntaxFactory.TokenList(
                                                            SyntaxFactory.XmlTextNewLine(
                                                                SyntaxFactory.TriviaList(),
                                                                Environment.NewLine,
                                                                Environment.NewLine,
                                                                SyntaxFactory.TriviaList()))),
                                                })))),
                                "Delete",
                                SyntaxFactory.TriviaList())),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                        SyntaxFactory.EnumMemberDeclaration(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Trivia(
                                        SyntaxFactory.DocumentationCommentTrivia(
                                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                                            SyntaxFactory.List<XmlNodeSyntax>(
                                                new XmlNodeSyntax[]
                                                {
                                                    SyntaxFactory.XmlText()
                                                    .WithTextTokens(
                                                        SyntaxFactory.TokenList(
                                                            SyntaxFactory.XmlTextLiteral(
                                                                SyntaxFactory.TriviaList(
                                                                    SyntaxFactory.DocumentationCommentExterior("///")),
                                                                " ",
                                                                " ",
                                                                SyntaxFactory.TriviaList()))),
                                                    SyntaxFactory.XmlExampleElement(
                                                        SyntaxFactory.SingletonList<XmlNodeSyntax>(
                                                            SyntaxFactory.XmlText()
                                                            .WithTextTokens(
                                                                SyntaxFactory.TokenList(
                                                                    new[]
                                                                    {
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " Update an item.",
                                                                            " Update an item.",
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " ",
                                                                            " ",
                                                                            SyntaxFactory.TriviaList()),
                                                                    }))))
                                                    .WithStartTag(
                                                        SyntaxFactory.XmlElementStartTag(
                                                            SyntaxFactory.XmlName(
                                                                SyntaxFactory.Identifier("summary"))))
                                                    .WithEndTag(
                                                        SyntaxFactory.XmlElementEndTag(
                                                            SyntaxFactory.XmlName(
                                                                SyntaxFactory.Identifier("summary")))),
                                                    SyntaxFactory.XmlText()
                                                    .WithTextTokens(
                                                        SyntaxFactory.TokenList(
                                                            SyntaxFactory.XmlTextNewLine(
                                                                SyntaxFactory.TriviaList(),
                                                                Environment.NewLine,
                                                                Environment.NewLine,
                                                                SyntaxFactory.TriviaList()))),
                                                })))),
                                "Update",
                                SyntaxFactory.TriviaList())),
                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                    }))
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
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //    /// <summary>
                    //    /// A table of <see cref="Account"/> rows.
                    //    /// </summary>
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
                                                $" An action on a row.",
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