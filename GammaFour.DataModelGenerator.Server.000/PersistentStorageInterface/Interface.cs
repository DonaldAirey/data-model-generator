// <copyright file="Interface.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.PersistentStoreageInterface
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
    public class Interface : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interface"/> class.
        /// </summary>
        /// <param name="dataModelSchema">A description of a unique constraint.</param>
        public Interface(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;
            this.Name = "IPersistentStore";

            //    /// <summary>
            //    /// Interface for a strongly typed persistent store.
            //    /// </summary>
            //    public interface IPersistentStore
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.InterfaceDeclaration(this.Name)
                .WithModifiers(this.Modifiers)
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                //    /// <summary>
                //    /// Interface for a strongly typed persistent store.
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
                                                " Interface for a strongly typed persistent store.",
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
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreatePublicInstanceProperties(members);
                members = this.CreatePublicInstanceMethods(members);
                return members;
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
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
        /// Create the public instance properties.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the properties added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();

            // Alphabetize and add the properties as members of the class.
            foreach (SyntaxElement syntaxElement in properties.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the methods added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new ReadMethod());

            // Generate a "load row" event for each of the tables.
            foreach (TableSchema tableSchema in this.dataModelSchema.Tables)
            {
                methods.Add(new CreateMethod(tableSchema));
                methods.Add(new DeleteMethod(tableSchema));
                methods.Add(new UpdateMethod(tableSchema));
            }

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