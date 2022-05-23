// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.ServiceCollectionExtensionClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Common.RowClass;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a row.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The document describing the data model.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The document describing the data model.</param>
        public Class(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "ServiceCollectionExtension";

            //    /// <summary>
            //    /// Used to add the services found in this library.
            //    /// </summary>
            //    public static class ServiceCollectionExtensions
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
                .WithModifiers(Class.Modifiers)
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                //    /// <summary>
                //    /// A key for finding objects in the Configuration table.
                //    /// </summary>
                return SyntaxFactory.TriviaList(
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
                                                " Used to add the services found in this library.",
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
            }
        }

        /// <summary>
        /// Gets the members syntax.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreatePublicStaticMethods(members);
                return members;
            }
        }

        /// <summary>
        /// Create the public instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the public instance methods.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicStaticMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new UseDataModelMethod(this.xmlSchemaDocument));

            // Alphabetize and add the methods as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}