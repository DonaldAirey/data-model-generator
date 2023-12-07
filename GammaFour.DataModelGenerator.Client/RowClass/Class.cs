// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.RowClass
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
            //    public class Account : IRow
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
                .WithModifiers(Class.Modifiers)
                .WithBaseList(this.BaseList)
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
                    });
            }
        }

        /// <summary>
        /// Gets the base class syntax.
        /// </summary>
        private BaseListSyntax BaseList
        {
            get
            {
                // A list of base classes or interfaces.
                List<SyntaxNodeOrToken> baseList = new List<SyntaxNodeOrToken>();

                // IVersionable
                baseList.Add(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.IdentifierName("IRow")));

                return SyntaxFactory.BaseList(
                      SyntaxFactory.SeparatedList<BaseTypeSyntax>(baseList.ToArray()));
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
                                                $" A {this.tableElement.Name} record.",
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
                members = this.CreatePrivateStaticReadonlyFields(members);
                members = this.CreatePrivateStaticFields(members);
                members = this.CreatePrivateInstanceFields(members);
                members = this.CreateConstructors(members);
                members = this.CreatePublicInstanceProperties(members);
                members = Class.CreatePublicInstanceMethods(members);
                members = this.CreateInternalInstanceMethods(members);
                members = Class.CreatePrivateInstanceMethods(members);
                return members;
            }
        }

        /// <summary>
        /// Create the internal instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the internal instance methods.</returns>
        private static SyntaxList<MemberDeclarationSyntax> CreatePrivateInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();

            // Alphabetize and add the methods as members of the class.
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
        /// <returns>The syntax for creating the public instance methods.</returns>
        private static SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();

            // Alphabetize and add the methods as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the constructors.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the constructors.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Add the constructors.
            members = members.Add(new Constructor(this.tableElement).Syntax);
            members = members.Add(new ConstructorRecordVersion(this.tableElement).Syntax);

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the internal instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the internal instance methods.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateInternalInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new GetVersionMethod(this.tableElement));
            methods.Add(new MarkMethod());
            methods.Add(new SetOwnerMethod(this.tableElement));

            // Alphabetize and add the methods as members of the class.
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
        /// <returns>The syntax for creating the internal instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateInstanceFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new CurrentDataField());
            fields.Add(new OriginalDataField());
            fields.Add(new PreviousDataField());
            fields.Add(new TableField(this.tableElement));

            // Create these fields for each child table.
            foreach (ForeignElement foreignKeyElement in this.tableElement.ChildKeys)
            {
                fields.Add(new GetChildrenFunctionField(foreignKeyElement));
            }

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
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
        /// <returns>The syntax for creating the internal instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateStaticFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new CloneVersionField(this.tableElement));
            fields.Add(new ColumnMapField(this.tableElement));

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
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
        /// <returns>The syntax for creating the internal instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePrivateStaticReadonlyFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> fields = new List<SyntaxElement>();

            // Create these fields for each child table.
            foreach (ForeignElement foreignKeyElement in this.tableElement.ChildKeys)
            {
                fields.Add(new DefaultChildrenField(foreignKeyElement));
            }

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
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
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>
            {
                new TableProperty(this.tableElement),
                new RecordStateProperty(),
                new IndexProperty(this.tableElement),
            };

            // Create a navigation property to each of the parent collections.
            foreach (ForeignElement foreignKeyElement in this.tableElement.ParentKeys)
            {
                properties.Add(new ParentProperty(foreignKeyElement));
            }

            // Create a navigation property to each of the child records.
            foreach (ForeignElement foreignKeyElement in this.tableElement.ChildKeys)
            {
                properties.Add(new ChildrenProperty(foreignKeyElement));
            }

            // Create a property for each column.
            foreach (ColumnElement columnElement in this.tableElement.Columns)
            {
                properties.Add(new ColumnProperty(columnElement));
            }

            // Alphabetize and add the properties as members of the class.
            foreach (SyntaxElement syntaxElement in properties.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }
    }
}