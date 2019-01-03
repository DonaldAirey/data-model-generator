// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.RowClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Common.RecordClass;
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
        private TableElement tableElement;

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
            //    public partial class ConfigurationRow
            //    {
            //        <Members>
            //    }
            this.Syntax = SyntaxFactory.ClassDeclaration(this.Name)
                .WithModifiers(this.Modifiers)
                .WithBaseList(this.BaseList)
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.DocumentationComment);
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
                            SyntaxFactory.IdentifierName("INotifyPropertyChanged"))));
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
                                                " A row of data in the " + this.tableElement.Name + " table.",
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
                members = this.CreatePrivateStaticFields(members);
                members = this.CreatePrivateInstanceFields(members);
                members = this.CreatePublicEvents(members);
                members = this.CreateConstructors(members);
                members = this.CreatePublicInstanceProperties(members);
                members = this.CreatePublicInstanceIndexers(members);
                members = this.CreateInternalInstanceMethods(members);
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
        /// Create the constructors.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the constructors.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Add the constructors.
            members = members.Add(new ConstructorTableTableData(this.tableElement).Syntax);

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
            methods.Add(new AddMethod(this.tableElement));
            methods.Add(new DeleteMethod(this.tableElement));
            methods.Add(new UpdateMethod(this.tableElement));

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
            fields.Add(new CurrentDataField(this.tableElement));
            fields.Add(new PreviousDataField(this.tableElement));

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
            fields.Add(new ColumnNamesField(this.tableElement));

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public events.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the internal instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicEvents(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            fields.Add(new PropertyChangedEvent(this.tableElement));

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(m => m.Name))
            {
                members = members.Add(syntaxElement.Syntax);
            }

            // Return the new collection of members.
            return members;
        }

        /// <summary>
        /// Create the public instance indexers.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The syntax for creating the public instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceIndexers(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new ItemProperty(this.tableElement));

            // Alphabetize and add the properties as members of the class.
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
        /// <returns>The syntax for creating the public instance properties.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new RowStateProperty(this.tableElement));
            properties.Add(new RecordSetProperty(this.tableElement));

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