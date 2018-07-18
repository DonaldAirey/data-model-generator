// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common.RowDataClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates the data for a row.
    /// </summary>
    public class Class : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private TableSchema tableSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class.
        /// </summary>
        /// <param name="tableSchema">A description of a unique constraint.</param>
        public Class(TableSchema tableSchema)
        {
            // Initialize the object.
            this.tableSchema = tableSchema;

            // The name of this structure.
            this.Name = string.Format(CultureInfo.InvariantCulture, "{0}Data", tableSchema.Name);

            //        /// <summary>
            //        /// The fields of a ConfigurationRow.
            //        /// </summary>
            //        public sealed class ConfigurationData
            //        {
            //            <Members>
            //        }
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
                //        /// <summary>
                //        /// The fields of a ConfigurationRow.
                //        /// </summary>
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
                                                string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    " The fields of a {0}Row.",
                                                    this.tableSchema.Name),
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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.SealedKeyword)
                    });
            }
        }

        /// <summary>
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateInternalInstanceFields(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the private instance fields.
            List<SyntaxElement> fields = new List<SyntaxElement>();
            foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
            {
                fields.Add(new ColumnProperty(columnSchema));
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
        /// <returns>The structure members with the properties added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceProperties(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> properties = new List<SyntaxElement>();

            // Create a property for each column.
            foreach (ColumnSchema columnSchema in this.tableSchema.Columns)
            {
                properties.Add(new RowDataClass.ColumnProperty(columnSchema));
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
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInstanceMethods(SyntaxList<MemberDeclarationSyntax> members)
        {
            // This will create the public instance properties.
            List<SyntaxElement> methods = new List<SyntaxElement>();
            methods.Add(new CloneMethod(this.tableSchema));

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