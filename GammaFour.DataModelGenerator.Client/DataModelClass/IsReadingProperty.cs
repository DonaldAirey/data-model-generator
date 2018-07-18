// <copyright file="IsReadingProperty.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a property to start the background thread that reconciles the data.
    /// </summary>
    public class IsReadingProperty : SyntaxElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsReadingProperty"/> class.
        /// </summary>
        public IsReadingProperty()
        {
            // Initialize the object.
            this.Name = "IsReading";

            //        /// <summary>
            //        /// Gets or sets an indication of whether the background thread that reconciles the client data model is running or not.
            //        /// </summary>
            //        public bool IsReading
            //        {
            //          <AccessorList>
            //        {
            this.Syntax = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.PredefinedType(
                        SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                    SyntaxFactory.Identifier(this.Name))
                .WithAccessorList(this.AccessorList)
                .WithModifiers(this.Modifiers)
                .WithLeadingTrivia(this.DocumentationComment);
        }

        /// <summary>
        /// Gets the list of accessors.
        /// </summary>
        private AccessorListSyntax AccessorList
        {
            get
            {
                // This collects the accessors.
                List<AccessorDeclarationSyntax> accessors = new List<AccessorDeclarationSyntax>();

                //            get
                //            {
                //                <GetAccessorBlock>
                //            }
                accessors.Add(
                    SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration,
                        SyntaxFactory.Block(SyntaxFactory.List(this.GetAccessorBlock))));

                //            get
                //            {
                //                <SetAccessorBlock>
                //            }
                accessors.Add(
                    SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.SetAccessorDeclaration,
                        SyntaxFactory.Block(SyntaxFactory.List(this.SetAccessorBlock))));

                // This is the complete list of accessors.
                return SyntaxFactory.AccessorList(SyntaxFactory.List(accessors));
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
                //        /// Gets or sets an indication of whether the background thread that reconciles the client data model is running or not.
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
                                                " Gets or sets an indication of whether the background thread that reconciles the client data model is running or not.",
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
        /// Gets the 'Get' accessor.
        /// </summary>
        private List<StatementSyntax> GetAccessorBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                return this.isReading;
                statements.Add(
                    SyntaxFactory.ReturnStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("isReading"))));

                // This is the complete statement block.
                return statements;
            }
        }

        /// <summary>
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
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
        /// Gets a block of code to start the background process to read the data from the service.
        /// </summary>
        private List<StatementSyntax> ReadDataBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                        this.synchronizationContext = SynchronizationContext.Current;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("synchronizationContext")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("SynchronizationContext"),
                                SyntaxFactory.IdentifierName("Current")))));

                //                        this.ReadTransactionsAsync(null);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("ReadTransactionsAsync")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)))))));

                // This is the complete statement block.
                return statements;
            }
        }

        /// <summary>
        /// Gets the 'Set' accessor.
        /// </summary>
        private List<StatementSyntax> SetAccessorBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                if (this.isReading != value)
                //                {
                //                    <ValueChangedBlock>
                //                }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("isReading")),
                            SyntaxFactory.IdentifierName("value")),
                        SyntaxFactory.Block(this.ValueChangedBlock)));

                // This is the complete statement block.
                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code to the handle a change in the value of the property.
        /// </summary>
        private List<StatementSyntax> ValueChangedBlock
        {
            get
            {
                // This list collects the statements.
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    if (this.isReading = value)
                //                    {
                //                        <ReadDataBlock>
                //                    }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("isReading")),
                            SyntaxFactory.IdentifierName("value")),
                        SyntaxFactory.Block(this.ReadDataBlock)));

                // This is the complete statement block.
                return statements;
            }
        }
    }
}