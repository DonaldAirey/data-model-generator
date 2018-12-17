// <copyright file="OnRecordChangingMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common.RecordSet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method invoke the event handlers for a changing row.
    /// </summary>
    public class OnRecordChangingMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnRecordChangingMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public OnRecordChangingMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "OnRecordChanging";

            //        /// <summary>
            //        /// Handles a proposed change to the <see cref="Buyer"/> record.
            //        /// </summary>
            //        /// <param name="dataAction">The action taken.</param>
            //        /// <param name="previous">The previous state of the record.</param>
            //        /// <param name="current">The current state of the record.</param>
            //        internal void OnRecordChanging(DataAction dataAction, Buyer previous, Buyer current)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(this.Modifiers)
            .WithParameterList(this.Parameters)
            .WithBody(this.Body)
            .WithLeadingTrivia(this.DocumentationComment);
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

                //            this.RowChanging?.Invoke(this, new RecordChangeEventArgs<Buyer> { DataAction = dataAction, Previous = previous, Current = current });
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.ConditionalAccessExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("RowChanging")),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberBindingExpression(
                                    SyntaxFactory.IdentifierName("Invoke")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ThisExpression()),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.ObjectCreationExpression(
                                                    SyntaxFactory.GenericName(
                                                        SyntaxFactory.Identifier("RecordChangeEventArgs"))
                                                    .WithTypeArgumentList(
                                                        SyntaxFactory.TypeArgumentList(
                                                            SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                SyntaxFactory.IdentifierName(this.tableElement.Name)))))
                                                .WithInitializer(
                                                    SyntaxFactory.InitializerExpression(
                                                        SyntaxKind.ObjectInitializerExpression,
                                                        SyntaxFactory.SeparatedList<ExpressionSyntax>(
                                                            new SyntaxNodeOrToken[]
                                                            {
                                                                SyntaxFactory.AssignmentExpression(
                                                                    SyntaxKind.SimpleAssignmentExpression,
                                                                    SyntaxFactory.IdentifierName("DataAction"),
                                                                    SyntaxFactory.IdentifierName("dataAction")),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.AssignmentExpression(
                                                                    SyntaxKind.SimpleAssignmentExpression,
                                                                    SyntaxFactory.IdentifierName("Previous"),
                                                                    SyntaxFactory.IdentifierName("previous")),
                                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                SyntaxFactory.AssignmentExpression(
                                                                    SyntaxKind.SimpleAssignmentExpression,
                                                                    SyntaxFactory.IdentifierName("Current"),
                                                                    SyntaxFactory.IdentifierName("current"))
                                                            }))))
                                        }))))));

                // This is the syntax for the body of the method.
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
                //        /// Handles a proposed change to the <see cref="Buyer"/> record.
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
                                                $" Handles a proposed change to the <see cref=\"{this.tableElement.Name}\"/> record.",
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

                //        /// <param name="dataAction">The action taken.</param>
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
                                                    " <param name=\"dataAction\">The action taken.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList())
                                            }))))));

                //        /// <param name="previous">The previous state of the record.</param>
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
                                                    " <param name=\"previous\">The previous state of the record.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList())
                                            }))))));

                //        /// <param name="current">The current state of the record.</param>
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
                                                    " <param name=\"current\">The current state of the record.</param>",
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
        /// Gets the modifiers.
        /// </summary>
        private SyntaxTokenList Modifiers
        {
            get
            {
                // private
                return SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                        SyntaxFactory.Token(SyntaxKind.VirtualKeyword)
                   });
            }
        }

        /// <summary>
        /// Gets the list of parameters.
        /// </summary>
        private ParameterListSyntax Parameters
        {
            get
            {
                // Create a list of parameters.
                List<SyntaxNodeOrToken> parameters = new List<SyntaxNodeOrToken>();

                // DataAction dataAction
                parameters.Add(
                    SyntaxFactory.Parameter(
                        SyntaxFactory.Identifier("dataAction"))
                    .WithType(
                        SyntaxFactory.IdentifierName("DataAction")));

                // ,
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // Buyer previousBuyer
                parameters.Add(
                    SyntaxFactory.Parameter(
                         SyntaxFactory.Identifier("previous"))
                     .WithType(
                        SyntaxFactory.IdentifierName(this.tableElement.Name)));

                // ,
                parameters.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));

                // Buyer currentBuyer
                parameters.Add(
                    SyntaxFactory.Parameter(
                         SyntaxFactory.Identifier("current"))
                    .WithType(
                        SyntaxFactory.IdentifierName(this.tableElement.Name)));

                // This is the complete parameter specification for this constructor.
                return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(parameters));
            }
        }
    }
}