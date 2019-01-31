// <copyright file="ConstructorDataModelName.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordSetClass
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a constructor.
    /// </summary>
    public class ConstructorDataModelName : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorDataModelName"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        public ConstructorDataModelName(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;

            // This is the name of the constructor.
            this.Name = $"{this.tableElement.Name}Set";

            //        /// <summary>
            //        /// Initializes a new instance of the <see cref="ConfigurationSet"/> class.
            //        /// </summary>
            //        internal ConfigurationSet(TenantDataModel tenantDataModel)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.ConstructorDeclaration(
                SyntaxFactory.Identifier(this.Name))
                .WithModifiers(ConstructorDataModelName.Modifiers)
                .WithParameterList(this.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    });
            }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // The elements of the body are added to this collection as they are assembled.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //            this.DataModel = dataModel;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                            SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Name.ToCamelCase()))));

                //            this.Name = name;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Name")),
                            SyntaxFactory.IdentifierName("name"))));

                // This is the syntax for the body of the constructor.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>();

                //        /// <summary>
                //        /// Initializes a new instance of the <see cref="ConfigurationSet"/> class.
                //        /// </summary>
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
                                                string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    " Initializes a new instance of the <see cref=\"{0}Set\"/> class.",
                                                    this.tableElement.Name),
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

                //        /// <param name="dataModel">The data model.</param>
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
                                                    $" <param name=\"{this.tableElement.XmlSchemaDocument.Name.ToCamelCase()}\">The data model.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList())
                                            }))))));

                //        /// <param name="name">The name of the set.</param>
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
                                                    $" <param name=\"name\">The name of the set.</param>",
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
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters from the columns in the unique constraint.
                List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

                // DataModel dataModel;
                parameters.Add(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(this.tableElement.XmlSchemaDocument.Name.ToCamelCase()))
                    .WithType(SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Name)));

                // ,
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // string Name
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("name"))
                    .WithType(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.StringKeyword))));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}