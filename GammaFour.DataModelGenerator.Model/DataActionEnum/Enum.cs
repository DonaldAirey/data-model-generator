// <copyright file="Enum.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.DataActionEnum
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
            //    /// An action on a row.
            //    /// </summary>
            //    [JsonConverter(typeof(JsonStringEnumConverter))]
            //    public enum DataAction
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.EnumDeclaration(this.Name)
            .WithAttributeLists(
                SyntaxFactory.SingletonList<AttributeListSyntax>(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("JsonConverter"))
                            .WithArgumentList(
                                SyntaxFactory.AttributeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<AttributeArgumentSyntax>(
                                        SyntaxFactory.AttributeArgument(
                                            SyntaxFactory.TypeOfExpression(
                                                SyntaxFactory.IdentifierName("JsonStringEnumConverter"))))))))))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithLeadingTrivia(this.LeadingTrivia)
            .WithMembers(Enum.Members);
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        private static SeparatedSyntaxList<EnumMemberDeclarationSyntax> Members
        {
            get
            {
                return SyntaxFactory.SeparatedList<EnumMemberDeclarationSyntax>(
                    new SyntaxNodeOrToken[]
                    {
                        //        /// <summary>
                        //        /// Add an item.
                        //        /// </summary>
                        //        Add,
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
                                                                    new SyntaxToken[]
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

                        //        /// <summary>
                        //        /// Remove an item.
                        //        /// </summary>
                        //        Remove,
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
                                                                    new SyntaxToken[]
                                                                    {
                                                                        SyntaxFactory.XmlTextNewLine(
                                                                            SyntaxFactory.TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            SyntaxFactory.TriviaList()),
                                                                        SyntaxFactory.XmlTextLiteral(
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.DocumentationCommentExterior("        ///")),
                                                                            " Remove an item.",
                                                                            " Remove an item.",
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
                                "Remove",
                                SyntaxFactory.TriviaList())),

                        SyntaxFactory.Token(SyntaxKind.CommaToken),

                        //        /// <summary>
                        //        /// Update an item.
                        //        /// </summary>
                        //        Update,
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
                                                                    new SyntaxToken[]
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
                    });
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private IEnumerable<SyntaxTrivia> LeadingTrivia
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