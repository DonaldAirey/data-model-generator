// <copyright file="Class.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.DataServiceClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// The data model class.
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
            this.Name = "DataServiceClient";

            //    /// <summary>
            //    /// Network client connection to a shared data model.
            //    /// </summary>
            //    public class DataModelServiceClient : ClientBase<IDataService>, IDataService
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
                // Collect the base types here.
                List<BaseTypeSyntax> baseTypes = new List<BaseTypeSyntax>();

                // ClientBase<IDataService>
                baseTypes.Add(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("ClientBase"))
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName("IDataService"))))));

                // IDataService
                baseTypes.Add(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.IdentifierName("IDataService")));

                // This is the list of base classes.
                return SyntaxFactory.BaseList(SyntaxFactory.SeparatedList<BaseTypeSyntax>(baseTypes));
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
                //    /// Network client connection to a shared data model.
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
                                                " Network client connection to a shared data model.",
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
                members = this.CreateConstructors(members);
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
        /// Create the private instance fields.
        /// </summary>
        /// <param name="members">The structure members.</param>
        /// <returns>The structure members with the fields added.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreateConstructors(SyntaxList<MemberDeclarationSyntax> members)
        {
            // These are the constructors.
            members = members.Add(new ConstructorEtc(this.xmlSchemaDocument).Syntax);

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
            methods.Add(new ReadAsyncMethod(this.xmlSchemaDocument));

            // Each of the tables has a method to create, update and delete the records.
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                methods.Add(new CreateAsyncMethod(tableElement));
                methods.Add(new DeleteAsyncMethod(tableElement));
                methods.Add(new UpdateAsyncMethod(tableElement));
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