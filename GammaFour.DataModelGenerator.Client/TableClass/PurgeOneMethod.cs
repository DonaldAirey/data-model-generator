// <copyright file="PurgeOneMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.TableClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    /// <summary>
    /// Creates a method to merge a record.
    /// </summary>
    public class PurgeOneMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeOneMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public PurgeOneMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "Purge";

            //        /// <inheritdoc/>
            //        public void Purge(IRow row)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
                .WithModifiers(PurgeOneMethod.Modifiers)
                .WithParameterList(PurgeOneMethod.Parameters)
                .WithBody(this.Body)
                .WithLeadingTrivia(PurgeOneMethod.DocumentationComment);
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private static SyntaxTriviaList DocumentationComment
        {
            get
            {
                // This is used to collect the trivia.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <inheritdoc/>
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            SingletonList<XmlNodeSyntax>(
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        new[]
                                        {
                                            XmlTextLiteral(
                                                TriviaList(DocumentationCommentExterior(Strings.CommentExterior)),
                                                " <inheritdoc/>",
                                                string.Empty,
                                                TriviaList()),
                                            XmlTextNewLine(
                                                TriviaList(),
                                                Environment.NewLine,
                                                string.Empty,
                                                TriviaList()),
                                        }))))),
                };

                // This is the complete document comment.
                return TriviaList(comments);
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private static SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return TokenList(
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                    });
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private static ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<ParameterSyntax> parameters = new List<ParameterSyntax>
                {
                    // IRow row
                    Parameter(
                        Identifier("row"))
                    .WithType(
                        SyntaxFactory.IdentifierName("IRow")),
                };

                // This is the complete parameter specification for this constructor.
                return ParameterList(SeparatedList<ParameterSyntax>(parameters));
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
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //            ListMap oldListMap = row as ListMap;
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier($"old{this.tableElement.Name}"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.BinaryExpression(
                                            SyntaxKind.AsExpression,
                                            SyntaxFactory.IdentifierName("row"),
                                            SyntaxFactory.IdentifierName(this.tableElement.Name))))))),

                    //                ProposedOrder proposedOrder = this.PortfolioDataModel.ProposedOrders.ProposedOrderKey.Find(oldProposedOrder.ProposedOrderId);
                    SyntaxFactory.LocalDeclarationStatement(
                        SyntaxFactory.VariableDeclaration(
                            SyntaxFactory.IdentifierName(this.tableElement.Name))
                        .WithVariables(
                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                SyntaxFactory.VariableDeclarator(
                                    SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(this.tableElement.PrimaryKey.Name)),
                                                SyntaxFactory.IdentifierName("Find")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName($"old{this.tableElement.Name}"),
                                                            SyntaxFactory.IdentifierName(this.tableElement.PrimaryKey.Columns[0].Column.Name))))))))))),

                    //                if (proposedOrder != null)
                    //                {
                    //                    <CheckAndRemove>
                    //                }
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)),
                        SyntaxFactory.Block(this.CheckAndRemove)),

                    //                if (entity.RowVersion > this.AlertDataModel.RowVersion)
                    //                {
                    //                    <UpdateRowVersion>
                    //                }
                    IfStatement(
                        BinaryExpression(
                            SyntaxKind.GreaterThanExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName($"old{this.tableElement.Name}"),
                                IdentifierName("RowVersion")),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                IdentifierName("RowVersion"))),
                        Block(this.UpdateRowVersion)),
                };

                // This is the syntax for the body of the method.
                return Block(List<StatementSyntax>(statements));
            }
        }

        /// <summary>
        /// Gets the statements checks to check to see if there are child records and remove if not.
        /// </summary>
        private List<StatementSyntax> CheckAndRemove
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                this.Remove(proposedOrder);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName())))))),
                };

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements checks to see if the parent record exists.
        /// </summary>
        private List<StatementSyntax> UpdateRowVersion
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>
                {
                    //                    this.AlertDataModel.RowVersion = entity.RowVersion;
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName(this.tableElement.XmlSchemaDocument.Name)),
                                IdentifierName("RowVersion")),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName($"old{this.tableElement.Name}"),
                                IdentifierName("RowVersion")))),
                };

                return statements;
            }
        }
    }
}