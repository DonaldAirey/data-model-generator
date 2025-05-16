// <copyright file="ApplyManyMethod.cs" company="Gamma Four, Inc.">
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
    public class ApplyManyMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyManyMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The table schema.</param>
        public ApplyManyMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = $"Apply{this.tableElement.Name.ToPlural()}";

            //        /// <summary>
            //        /// Apply a series of <see cref="DataTransferObjects.Account"/> rows.
            //        /// </summary>
            //        /// <param name="rows">The data transfer object.</param>
            //        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            //        private async Task ApplyAllocationOrders(IEnumerable<DataTransferObjects.AllocationOrder> rows)
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
                            SyntaxFactory.Identifier("rows"))
                        .WithType(
                            SyntaxFactory.GenericName(
                                SyntaxFactory.Identifier("IEnumerable"))
                            .WithTypeArgumentList(
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                        SyntaxFactory.QualifiedName(
                                            SyntaxFactory.IdentifierName("DataTransferObjects"),
                                            SyntaxFactory.IdentifierName(this.tableElement.Name)))))))))
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
                        //            var uniqueObjects = new HashSet<DataTransferObjects.AllocationOrder>();
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
                                        SyntaxFactory.Identifier("uniqueObjects"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("HashSet"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                            SyntaxFactory.QualifiedName(
                                                                SyntaxFactory.IdentifierName("DataTransferObjects"),
                                                                SyntaxFactory.IdentifierName(this.tableElement.Name))))))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList())))))),

                        //            foreach (var row in rows)
                        //            {
                        //                <ApplyRow>
                        //            }
                        SyntaxFactory.ForEachStatement(
                            SyntaxFactory.IdentifierName(
                                SyntaxFactory.Identifier(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.VarKeyword,
                                    "var",
                                    "var",
                                    SyntaxFactory.TriviaList())),
                            SyntaxFactory.Identifier("row"),
                            SyntaxFactory.IdentifierName("rows"),
                            SyntaxFactory.Block(this.ConsolidateRow)),

                        //            foreach (var row in uniqueObjects)
                        //            {
                        //                await this.AllocationOrders.AddAsync(new AllocationOrder(row)).ConfigureAwait(false);
                        //            }
                        SyntaxFactory.ForEachStatement(
                            SyntaxFactory.IdentifierName(
                                SyntaxFactory.Identifier(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.VarKeyword,
                                    "var",
                                    "var",
                                    SyntaxFactory.TriviaList())),
                            SyntaxFactory.Identifier("row"),
                            SyntaxFactory.IdentifierName("uniqueObjects"),
                            SyntaxFactory.Block(this.ApplyRow)),
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
                    //        /// Apply a series of <see cref="DataTransferObjects.Account"/> rows.
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
                                                $" Apply a series of <see cref=\"DataTransferObjects.{this.tableElement.Name}\"/> rows.",
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

                    //        /// <param name="rows">The time series of data transfer objects.</param>
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
                                                        " <param name=\"rows\">The time series of data transfer objects.</param>",
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

        /// <summary>
        /// Gets the statements to apply the row.
        /// </summary>
        private IEnumerable<StatementSyntax> ApplyRow
        {
            get
            {
                return new StatementSyntax[]
                {
                    //                await this.AllocationOrders.AddAsync(new AllocationOrder(row)).ConfigureAwait(false);
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
                                                                    SyntaxFactory.IdentifierName("row"))))))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))),
                };
            }
        }

        /// <summary>
        /// Gets the statements to consolidate the rows.
        /// </summary>
        private IEnumerable<StatementSyntax> ConsolidateRow
        {
            get
            {
                return new StatementSyntax[]
                {
                    //                switch (row.DataAction)
                    //                {
                    //                    case DataAction.Add:
                    //                        uniqueObjects.Add(row);
                    //                        break;
                    //                    case DataAction.Remove:
                    //                        uniqueObjects.Remove(row);
                    //                        break;
                    //                    case DataAction.Update:
                    //                        uniqueObjects.Remove(row);
                    //                        uniqueObjects.Add(row);
                    //                        break;
                    //                }
                    SyntaxFactory.SwitchStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("row"),
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
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("uniqueObjects"),
                                                        SyntaxFactory.IdentifierName("Add")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("row")))))),
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
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("uniqueObjects"),
                                                        SyntaxFactory.IdentifierName("Remove")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("row")))))),
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
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("uniqueObjects"),
                                                        SyntaxFactory.IdentifierName("Remove")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("row")))))),
                                            SyntaxFactory.ExpressionStatement(
                                                SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("uniqueObjects"),
                                                        SyntaxFactory.IdentifierName("Add")))
                                                .WithArgumentList(
                                                    SyntaxFactory.ArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("row")))))),
                                            SyntaxFactory.BreakStatement(),
                                        })),
                            })),
                };
            }
        }
    }
}