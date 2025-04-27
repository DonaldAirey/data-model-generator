// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.DtoClass
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
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="tableElement">A description of a unique constraint.</param>
        public Class(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = tableElement.Name;

            //    /// <summary>
            //    /// A row of data in the Configuration table.
            //    /// </summary>
            //    public class Account
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.tableElement.Name)
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithMembers(this.Members)
            .WithLeadingTrivia(this.LeadingTrivia);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private IEnumerable<SyntaxTrivia> LeadingTrivia
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
                                                $" A {this.tableElement.Name} row.",
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
                members = this.CreateConstructors(members);
                members = this.CreatePublicProperties(members);
                members = this.CreatePublicMethods(members);
                return members;
            }
        }

        /// <summary>
        /// Create the constructors.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the constructors.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Add the constructors.
            members = members.Add(new ConstructorMaster(this.tableElement).Syntax);
            members = members.Add(new ConstructorSlave(this.tableElement).Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the public instance methods.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>
            {
                new EqualsMethod(this.tableElement),
                new GetHashCodeMethod(this.tableElement),
            };

            // Alphabetize and add the methods as members of the class.
            foreach (var syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance properties.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the public instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new DataActionProperty());

            // Create a property for each column.
            foreach (ColumnElement columnElement in this.tableElement.Columns)
            {
                properties.Add(new PropertyProperty(columnElement));
            }

            // Alphabetize and add the properties as members of the class.
            foreach (var syntaxElement in properties.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}