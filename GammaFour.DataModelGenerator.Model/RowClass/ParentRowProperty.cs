// <copyright file="ParentRowProperty.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.RowClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a property that navigates to the parent record.
    /// </summary>
    public class ParentRowProperty : SyntaxElement
    {
        /// <summary>
        /// The foreign key description.
        /// </summary>
        private readonly ForeignIndexElement foreignIndexElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentRowProperty"/> class.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        public ParentRowProperty(ForeignIndexElement foreignIndexElement)
        {
            // Initialize the object.
            this.foreignIndexElement = foreignIndexElement;
            this.Name = this.foreignIndexElement.UniqueParentName;

            //        /// <summary>
            //        /// Gets or sets the parent <see cref="Account"/> row.
            //        /// </summary>
            //        [JsonIgnore]
            //        public Account? Account
            //        {
            //            get
            //            {
            //                <GetAccessor>;
            //            }
            //
            //            set
            //            {
            //                <SetAccessor>
            //            }
            //        }
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.NullableType(
                    SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueIndex.Table.Name)),
                SyntaxFactory.Identifier(this.foreignIndexElement.UniqueParentName))
            .WithAttributeLists(
                SyntaxFactory.SingletonList<AttributeListSyntax>(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                SyntaxFactory.IdentifierName("JsonIgnore"))))))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithAccessorList(
                SyntaxFactory.AccessorList(
                    SyntaxFactory.List<AccessorDeclarationSyntax>(
                        new AccessorDeclarationSyntax[]
                        {
                            this.GetAccessor,
                            this.SetAccessor,
                        })))
            .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList DocumentationComment
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Gets or sets the parent <see cref="Account"/> table.
                    //        /// </summary>
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
                                                $" Gets or sets the parent <see cref=\"{this.foreignIndexElement.UniqueIndex.Table.Name}\"/> row.",
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
                                        }))))),
                };

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the 'Get' accessor.
        /// </summary>
        private AccessorDeclarationSyntax GetAccessor
        {
            get
            {
                // This list collects the statements.
                var statements = new List<StatementSyntax>
                {
                    //                return this.account;
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueParentName.ToCamelCase()))),
                };

                //            get
                //            {
                //                <statements>
                //            }
                return SyntaxFactory.AccessorDeclaration(
                    SyntaxKind.GetAccessorDeclaration,
                    SyntaxFactory.Block(
                        SyntaxFactory.List(statements)))
                .WithKeyword(SyntaxFactory.Token(SyntaxKind.GetKeyword));
            }
        }

        /// <summary>
        /// Gets the 'Set' accessor.
        /// </summary>
        private AccessorDeclarationSyntax SetAccessor
        {
            get
            {
                // This list collects the statements.
                var statements = new List<StatementSyntax>
                {
                    //                if (this.account != value)
                    //                {
                    //                    var account = this.Account;
                    //                    this.undoStack.Push(() => this.account = account);
                    //                    this.account = value;
                    //                }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueParentName.ToCamelCase())),
                            SyntaxFactory.IdentifierName("value")),
                        SyntaxFactory.Block(
                            SyntaxFactory.LocalDeclarationStatement(
                                SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.IdentifierName(
                                        SyntaxFactory.Identifier(
                                            SyntaxFactory.TriviaList(),
                                            SyntaxKind.VarKeyword,
                                            "var",
                                            "var",
                                            SyntaxFactory.TriviaList())))
                                .WithVariables(
                                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                        SyntaxFactory.VariableDeclarator(
                                            SyntaxFactory.Identifier(this.foreignIndexElement.UniqueParentName.ToCamelCase()))
                                        .WithInitializer(
                                            SyntaxFactory.EqualsValueClause(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueParentName.ToCamelCase()))))))),
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("undoStack")),
                                        SyntaxFactory.IdentifierName("Push")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ParenthesizedLambdaExpression()
                                                .WithExpressionBody(
                                                    SyntaxFactory.AssignmentExpression(
                                                        SyntaxKind.SimpleAssignmentExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueParentName.ToCamelCase())),
                                                        SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueParentName.ToVariableName())))))))),
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(this.foreignIndexElement.UniqueParentName.ToCamelCase())),
                                    SyntaxFactory.IdentifierName("value"))))),
                };

                //            set
                //            {
                //                <statements>
                //            }
                return SyntaxFactory.AccessorDeclaration(
                    SyntaxKind.SetAccessorDeclaration,
                    SyntaxFactory.Block(
                        SyntaxFactory.List(statements)))
                .WithKeyword(SyntaxFactory.Token(SyntaxKind.SetKeyword));
            }
        }
    }
}