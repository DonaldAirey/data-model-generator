// <copyright file="ColumnProperty.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server.RecordClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a field that holds the column.
    /// </summary>
    public class ColumnProperty : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private ColumnElement columnElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnProperty"/> class.
        /// </summary>
        /// <param name="columnElement">The column schema.</param>
        public ColumnProperty(ColumnElement columnElement)
        {
            // Initialize the object.
            this.columnElement = columnElement;

            // This is the name of the property.
            this.Name = this.columnElement.Name;

            //        /// <summary>
            //        /// Gets or sets ConfigurationId.
            //        /// </summary>
            //        public string ConfigurationId { get; set; }
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                    Conversions.FromType(columnElement.Type),
                    SyntaxFactory.Identifier(this.Name))
                .WithAccessorList(this.AccessorList)
                .WithModifiers(ColumnProperty.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the statements that sets the underlying field to the new value.
        /// </summary>
        private static List<StatementSyntax> InitializeState
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.RecordState = this.RecordState == RecordState.Added ? RecordState.Added : RecordState.Modified;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("State")),
                            SyntaxFactory.ConditionalExpression(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("State")),
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("RecordState"),
                                        SyntaxFactory.IdentifierName("Added"))),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("RecordState"),
                                    SyntaxFactory.IdentifierName("Added")),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("RecordState"),
                                    SyntaxFactory.IdentifierName("Modified"))))));

                //                    this.originalData = (object[])this.currentData.Clone();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("originalData")),
                            SyntaxFactory.CastExpression(
                                SyntaxFactory.ArrayType(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                .WithRankSpecifiers(
                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                        SyntaxFactory.ArrayRankSpecifier(
                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                SyntaxFactory.OmittedArraySizeExpression())))),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName("Clone")))))));

                //                    this.previousData = (object[])this.currentData.Clone();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("previousData")),
                            SyntaxFactory.CastExpression(
                                SyntaxFactory.ArrayType(
                                    SyntaxFactory.PredefinedType(
                                        SyntaxFactory.Token(SyntaxKind.ObjectKeyword)))
                                .WithRankSpecifiers(
                                    SyntaxFactory.SingletonList<ArrayRankSpecifierSyntax>(
                                        SyntaxFactory.ArrayRankSpecifier(
                                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                                SyntaxFactory.OmittedArraySizeExpression())))),
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName("currentData")),
                                        SyntaxFactory.IdentifierName("Clone")))))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // internal
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                    });
            }
        }

        /// <summary>
        /// Gets the list of accessors (get, set).
        /// </summary>
        private AccessorListSyntax AccessorList
        {
            get
            {
                return SyntaxFactory.AccessorList(
                    SyntaxFactory.List(
                        new AccessorDeclarationSyntax[] { this.GetAccessor, this.SetAccessor }));
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
                List<StatementSyntax> statements = new List<StatementSyntax>();
                string property = this.columnElement.Name;

                //                return this.currentData[0] as string;
                if (this.columnElement.Type.IsValueType)
                {
                    statements.Add(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.CastExpression(
                                Conversions.FromType(this.columnElement.Type),
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("currentData")))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(this.columnElement.Index)))))))));
                }
                else
                {
                    statements.Add(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.AsExpression,
                                SyntaxFactory.ElementAccessExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("currentData")))
                                .WithArgumentList(
                                    SyntaxFactory.BracketedArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(this.columnElement.Index)))))),
                                Conversions.FromType(this.columnElement.Type))));
                }

                //            get
                //            {
                //            }
                return SyntaxFactory.AccessorDeclaration(
                    SyntaxKind.GetAccessorDeclaration,
                    SyntaxFactory.Block(
                        SyntaxFactory.List(statements)))
                    .WithKeyword(SyntaxFactory.Token(SyntaxKind.GetKeyword));
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
                //        /// Gets or sets Address1.
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
                                                $" Gets or sets {this.Name}.",
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
        /// Gets the 'Set' accessor.
        /// </summary>
        private AccessorDeclarationSyntax SetAccessor
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();
                string property = this.columnElement.Name;

                //                if (this.State == RecordState.Unchanged)
                //                {
                //                    <InitializeState>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("State")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("RecordState"),
                                SyntaxFactory.IdentifierName("Unchanged"))),
                                    SyntaxFactory.Block(ColumnProperty.InitializeState)));

                //                    this.currentData[1] = value;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.ElementAccessExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("currentData")))
                            .WithArgumentList(
                                SyntaxFactory.BracketedArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                SyntaxFactory.Literal(this.columnElement.Index)))))),
                            SyntaxFactory.IdentifierName("value"))));

                //            set
                //            {
                //            }
                return SyntaxFactory.AccessorDeclaration(
                    SyntaxKind.SetAccessorDeclaration,
                    SyntaxFactory.Block(
                        SyntaxFactory.List<StatementSyntax>(statements)));
            }
        }
    }
}