// <copyright file="ApplyTableMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to acquire a reader lock.
    /// </summary>
    public class ApplyTableMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyTableMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        public ApplyTableMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = $"Apply{this.tableElement.Name}";

            //        /// <summary>
            //        /// Apply a <see cref="DataTransferObjects.Account"/> row.
            //        /// </summary>
            //        /// <param name="row">The row.</param>
            //        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            //        private async Task ApplyAccount(object row)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.IdentifierName("Task"),
                SyntaxFactory.Identifier(this.Name))
            .WithModifiers(
                SyntaxFactory.TokenList(
                    new[]
                    {
                        SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    }))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier("row"))
                        .WithType(
                            SyntaxFactory.PredefinedType(
                                SyntaxFactory.Token(SyntaxKind.ObjectKeyword))))))
            .WithBody(this.Body)
            .WithLeadingTrivia(this.LeadingTrivia);
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        private BlockSyntax Body
        {
            get
            {
                // This is used to collect the statements.
                return SyntaxFactory.Block(
                    new List<StatementSyntax>
                    {
                        //            var rowDto = (DataTransferObjects.Account)row;
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
                                        SyntaxFactory.Identifier("rowDto"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.CastExpression(
                                                SyntaxFactory.QualifiedName(
                                                    SyntaxFactory.IdentifierName("DataTransferObjects"),
                                                    SyntaxFactory.IdentifierName(this.tableElement.Name)),
                                                SyntaxFactory.IdentifierName("row"))))))),

                        //            switch (rowDto.DataAction)
                        //            {
                        //                case DataAction.Add:
                        //                    await this.Accounts.AddAsync(new Account(rowDto)).ConfigureAwait(false);
                        //                    break;
                        //                case DataAction.Remove:
                        //                    await this.Accounts.RemoveAsync(new Account(rowDto)).ConfigureAwait(false);
                        //                    break;
                        //                case DataAction.Update:
                        //                    await this.Accounts.UpdateAsync(new Account(rowDto)).ConfigureAwait(false);
                        //                    break;
                        //            }
                        SyntaxFactory.SwitchStatement(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("rowDto"),
                                SyntaxFactory.IdentifierName("DataAction")))
                        .WithSections(
                            SyntaxFactory.List<SwitchSectionSyntax>(
                                new SwitchSectionSyntax[]
                                {
                                    SyntaxFactory.SwitchSection()
                                    .WithLabels(
                                        SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                                            SyntaxFactory.CaseSwitchLabel(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("DataAction"),
                                                    SyntaxFactory.IdentifierName("Add")))))
                                    .WithStatements(
                                        SyntaxFactory.List<StatementSyntax>(
                                            new StatementSyntax[]
                                            {
                                                SyntaxFactory.ExpressionStatement(
                                                    SyntaxFactory.AwaitExpression(
                                                        SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.InvocationExpression(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.ThisExpression(),
                                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                                        SyntaxFactory.IdentifierName("AddAsync")))
                                                                .WithArgumentList(
                                                                    SyntaxFactory.ArgumentList(
                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.ObjectCreationExpression(
                                                                                    SyntaxFactory.IdentifierName(this.tableElement.Name))
                                                                                .WithArgumentList(
                                                                                    SyntaxFactory.ArgumentList(
                                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                                            SyntaxFactory.Argument(
                                                                                                SyntaxFactory.IdentifierName("rowDto"))))))))),
                                                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.LiteralExpression(
                                                                            SyntaxKind.FalseLiteralExpression))))))),
                                                SyntaxFactory.BreakStatement(),
                                            })),
                                    SyntaxFactory.SwitchSection()
                                    .WithLabels(
                                        SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                                            SyntaxFactory.CaseSwitchLabel(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("DataAction"),
                                                    SyntaxFactory.IdentifierName("Remove")))))
                                    .WithStatements(
                                        SyntaxFactory.List<StatementSyntax>(
                                            new StatementSyntax[]
                                            {
                                                SyntaxFactory.ExpressionStatement(
                                                    SyntaxFactory.AwaitExpression(
                                                        SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.InvocationExpression(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.ThisExpression(),
                                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                                        SyntaxFactory.IdentifierName("RemoveAsync")))
                                                                .WithArgumentList(
                                                                    SyntaxFactory.ArgumentList(
                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.ObjectCreationExpression(
                                                                                    SyntaxFactory.IdentifierName(this.tableElement.Name))
                                                                                .WithArgumentList(
                                                                                    SyntaxFactory.ArgumentList(
                                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                                            SyntaxFactory.Argument(
                                                                                                SyntaxFactory.IdentifierName("rowDto"))))))))),
                                                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.LiteralExpression(
                                                                            SyntaxKind.FalseLiteralExpression))))))),
                                                SyntaxFactory.BreakStatement(),
                                            })),
                                    SyntaxFactory.SwitchSection()
                                    .WithLabels(
                                        SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                                            SyntaxFactory.CaseSwitchLabel(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("DataAction"),
                                                    SyntaxFactory.IdentifierName("Update")))))
                                    .WithStatements(
                                        SyntaxFactory.List<StatementSyntax>(
                                            new StatementSyntax[]
                                            {
                                                SyntaxFactory.ExpressionStatement(
                                                    SyntaxFactory.AwaitExpression(
                                                        SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.InvocationExpression(
                                                                    SyntaxFactory.MemberAccessExpression(
                                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                                        SyntaxFactory.MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            SyntaxFactory.ThisExpression(),
                                                                            SyntaxFactory.IdentifierName(this.tableElement.Name.ToPlural())),
                                                                        SyntaxFactory.IdentifierName("UpdateAsync")))
                                                                .WithArgumentList(
                                                                    SyntaxFactory.ArgumentList(
                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                            SyntaxFactory.Argument(
                                                                                SyntaxFactory.ObjectCreationExpression(
                                                                                    SyntaxFactory.IdentifierName(this.tableElement.Name))
                                                                                .WithArgumentList(
                                                                                    SyntaxFactory.ArgumentList(
                                                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                                            SyntaxFactory.Argument(
                                                                                                SyntaxFactory.IdentifierName("rowDto"))))))))),
                                                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.LiteralExpression(
                                                                            SyntaxKind.FalseLiteralExpression))))))),
                                                SyntaxFactory.BreakStatement(),
                                            })),
                                })),
                    });
            }
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private IEnumerable<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> comments = new List<SyntaxTrivia>
                {
                    //        /// <summary>
                    //        /// Apply a <see cref="DataTransferObjects.Account"/> row.
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
                                                $" Apply a <see cref=\"DataTransferObjects.{this.tableElement.Name}\"/> row.",
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

                    //        /// <param name="row">The row.</param>
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
                                                        " <param name=\"row\">The row.</param>",
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                                    SyntaxFactory.XmlTextNewLine(
                                                        SyntaxFactory.TriviaList(),
                                                        Environment.NewLine,
                                                        string.Empty,
                                                        SyntaxFactory.TriviaList()),
                                            }))))),

                    //        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
                                                        " <returns>A <see cref=\"Task\"/> representing the asynchronous operation.</returns>",
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
    }
}