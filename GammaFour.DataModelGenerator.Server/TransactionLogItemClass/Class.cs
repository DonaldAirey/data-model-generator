// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.TransactionLogItemClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a row.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">A description of a unique constraint.</param>
        public Class(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "TransactionLogItem";

            //    /// <summary>
            //    /// An item in the transaction log.
            //    /// </summary>
            //    public class TransactionLogItem
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
                .WithModifiers(Class.Modifiers)
                .WithMembers(Class.Members)
                .WithLeadingTrivia(Class.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                //    /// <summary>
                //    /// An item in the transaction log.
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
                                                " An item in the transaction log.",
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
            }
        }

        /// <summary>
        /// Gets the members syntax.
        /// </summary>
        private static SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = Class.CreateConstructors(members);
                members = Class.CreateInternalInstanceProperties(members);
                return members;
            }
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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    });
            }
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private static SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Add the constructors.
            members = members.Add(new Constructor().Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private static SyntaxList<MemberDeclarationSyntax> CreateInternalInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the internal instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new DataProperty());
            properties.Add(new SequenceProperty());
            properties.Add(new TimeStampProperty());

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in properties.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}