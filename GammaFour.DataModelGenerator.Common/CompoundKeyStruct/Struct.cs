// <copyright file="Struct.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common.CompoundKeyStruct
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a CodeDOM description of a strongly typed index.
    /// </summary>
    public class Struct : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Struct"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">A description of a unique constraint.</param>
        public Struct(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.
            this.uniqueKeyElement = uniqueKeyElement;

            // The name of this structure.
            this.Name = uniqueKeyElement.Name + "Set";

            // Create the syntax of the structure.
            this.Syntax = SyntaxFactory.StructDeclaration(this.Name)
                .WithModifiers(this.Modifiers)
                .WithBaseList(this.BaseList)
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.DocumentionComment);
        }

        /// <summary>
        /// Gets the base class syntax.
        /// </summary>
        private BaseListSyntax BaseList
        {
            get
            {
                // This is the list of base classes.
                return SyntaxFactory.BaseList(
                    SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                        SyntaxFactory.SimpleBaseType(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("IComparable"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(this.Name)))))));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentionComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //    /// <summary>
                //    /// A key for finding objects in the Configuration table.
                //    /// </summary>
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
                                                " A compound key for finding objects in the " + this.uniqueKeyElement.Name + " index.",
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
        /// Gets the members syntax.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreateConstructors(members);
                members = this.CreatePublicInstanceProperties(members);
                members = this.CreatePublicStaticOperators(members);
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
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.InternalKeyword)
                    });
            }
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Each field is added to the collection of structure members.
            members = members.Add(new Constructor(this.uniqueKeyElement).Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will alphabetize the methods and add them to the collection of members in this structure.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            foreach (ColumnReferenceElement columnReferenceElement in this.uniqueKeyElement.Columns)
            {
                // Add each of the fields with a trailing space.
                fields.Add(new ColumnProperty(columnReferenceElement.Column));
            }

            // Sort the fields before adding them to the members.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
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
            // This will alphabetize the methods and add them to the collection of members in this structure.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new CompareToMethod(this.uniqueKeyElement));
            methods.Add(new EqualsMethod(this.uniqueKeyElement));
            methods.Add(new HashMethod(this.uniqueKeyElement));

            // This will sort the elements.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
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
        private SyntaxList<MemberDeclarationSyntax> CreatePublicStaticOperators(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Add the operators (not that sorting is not an option on a math symbol).
            members = members.Add(new EqualityOperator(this.uniqueKeyElement).Syntax);
            members = members.Add(new InequalityOperator(this.uniqueKeyElement).Syntax);
            members = members.Add(new LessThanOperator(this.uniqueKeyElement).Syntax);
            members = members.Add(new GreaterThanOperator(this.uniqueKeyElement).Syntax);

            // Return the new collection of members.
            return members;
        }
    }
}