// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.UniqueKeyIndexClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Common.UniqueKeyIndexClass;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates an event that indicates a change to a row.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="uniqueKeyElement">A description of a unique constraint.</param>
        public Class(UniqueKeyElement uniqueKeyElement)
        {
            // Initialize the object.
            this.uniqueKeyElement = uniqueKeyElement;
            this.Name = uniqueKeyElement.Name;

            //    /// <summary>
            //    /// Unique key index for the Configuration table.
            //    /// </summary>
            //    internal class ConfigurationKeyIndex
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
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
                //    /// Unique key index for the Configuration table.
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
                                                " Unique key index for the " + this.uniqueKeyElement.Table.Name + " table.",
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
                members = this.CreatePrivateInstanceFields(members);
                members = this.CreateConstructors(members);
                members = this.CreatePublicInstanceProperties(members);
                members = this.CreatePublicInstanceMethods(members);
                members = this.CreateInternalInstanceMethods(members);
                members = this.CreatePrivateStructs(members);
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
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // These are the constructors.
            members = members.Add(new ConstructorDataModel(this.uniqueKeyElement).Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance properties.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the properties added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new FindMethod(this.uniqueKeyElement));

            // Alphabetize and add the properties as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

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
            // This will create the internal instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new DataModelProperty(this.uniqueKeyElement.Table.XmlSchemaDocument));

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in properties.OrderBy(m => m.Name))
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
        /// <returns>The structure members with the properties added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateInternalInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new AddMethod(this.uniqueKeyElement));
            methods.Add(new ClearMethod(this.uniqueKeyElement));
            methods.Add(new RemoveMethod(this.uniqueKeyElement));

            // Determine if this unique constraint is the parent for some other key.  If so, we'll need to add a 'ContainsKey' method.
            bool isParent = false;
            foreach (TableElement tableElement in this.uniqueKeyElement.Table.XmlSchemaDocument.Tables)
            {
                foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
                {
                    if (foreignKeyElement.UniqueKey == this.uniqueKeyElement)
                    {
                        isParent = true;
                    }
                }
            }

            if (isParent)
            {
                methods.Add(new ContainsKeyMethod(this.uniqueKeyElement));
            }

            // Non-nullable unique keys can use the 'Update' method which does the work of an 'Add' and a 'Remove' in a single call.
            if (!this.uniqueKeyElement.IsNullable)
            {
                methods.Add(new UpdateMethod(this.uniqueKeyElement));
            }

            // Alphabetize and add the properties as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateInstanceFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new DictionaryField(this.uniqueKeyElement));

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the private structures.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateStructs(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> structs = new List<SyntaxElement>();

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in structs.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}