// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.RecordCollectionClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using GammaFour.DataModelGenerator.Common.RecordSet;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a row.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The type of the row.
        /// </summary>
        private readonly string rowType;

        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// The XML Schema document.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="tableElement">A description of a unique constraint.</param>
        public Class(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement ?? throw new ArgumentNullException(nameof(tableElement));
            this.xmlSchemaDocument = this.tableElement.XmlSchemaDocument;
            this.Name = tableElement.Name + "Collection";
            this.rowType = this.tableElement.Name;

            //    /// <summary>
            //    /// The Configuration table.
            //    /// </summary>
            //    public partial class ConfigurationTable : IEnumerable<ConfigurationRow>
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

                // IEnumerable<Buyer>
                baseList.Add(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("IEnumerable"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(this.tableElement.Name))))));

                // ,
                baseList.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // IMergable
                baseList.Add(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.IdentifierName("IMergable")));

                // ,
                baseList.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // IMergable
                baseList.Add(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.IdentifierName("IPurgable")));

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
                //    /// A collection of <see cref="Buyer"/> records.
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
                                                string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    $" A collection of <see cref=\"{this.tableElement.Name}\"/> records.",
                                                    this.tableElement.Name),
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
                members = this.CreatePrivateInstanceFields(members);
                members = this.CreateConstructors(members);
                members = this.CreatePublicEvents(members);
                members = this.CreatePublicInstanceProperties(members);
                members = this.CreatePublicInstanceMethods(members);
                members = this.CreateInternalInstanceMethods(members);
                members = Class.CreatePrivateInstanceMethods(members);
                return members;
            }
        }

        /// <summary>
        /// Create the public instance methods.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the methods added.</returns>
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
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // Add the constructors.
            members = members.Add(new ConstructorDataModelName(this.tableElement).Syntax);

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
            fields.Add(new PrimaryKeyFunctionField(this.tableElement.PrimaryKey));
            fields.Add(new CollectionField(this.tableElement));

            // Add a field for each of the foreign indices.
            foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
            {
                fields.Add(new ForeignKeyIndexField(foreignKeyElement));
            }

            // Alphabetize and add the fields as members of the class.
            foreach (SyntaxElement syntaxElement in fields.OrderBy(f => f.Name))
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
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();
            properties.Add(new DataModelProperty(this.tableElement.XmlSchemaDocument));
            properties.Add(new IndexProperty(this.tableElement));
            properties.Add(new NameProperty());

            // Add a property for each of the unique keys indices.
            foreach (UniqueKeyElement uniqueKeyElement in this.tableElement.UniqueKeys)
            {
                properties.Add(new UniqueKeyIndexProperty(uniqueKeyElement));
            }

            // Add a property for each of the foreign key indices.
            foreach (ForeignKeyElement foreignKeyElement in this.tableElement.ParentKeys)
            {
                properties.Add(new ForeignKeyIndexProperty(foreignKeyElement));
            }

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
        private SyntaxList<MemberDeclarationSyntax> CreateInternalInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new OnRecordChangingMethod(this.tableElement));

            // Alphabetize and add the methods as members of the class.
            foreach (SyntaxElement syntaxElement in methods.OrderBy(m => m.Name))
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
        /// <returns>The structure members with the methods added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicEvents(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> events = new List<SyntaxElement>();
            events.Add(new RecordChangingEvent(this.tableElement));

            // Alphabetize and add the events as members of the class.
            foreach (SyntaxElement syntaxElement in events.OrderBy(m => m.Name))
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
            methods.Add(new AddMethod(this.tableElement));
            methods.Add(new GetEnumeratorMethod(this.tableElement));
            methods.Add(new GenericGetEnumeratorMethod(this.tableElement));
            methods.Add(new MergeMethod(this.tableElement));
            methods.Add(new PurgeMethod(this.tableElement));
            methods.Add(new RemoveMethod(this.tableElement));
            methods.Add(new UpdateMethod(this.tableElement));

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