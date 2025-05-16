// <copyright file="MergeAsyncMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.DataModelClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to find a row in the index.
    /// </summary>
    public class MergeAsyncMethod : SyntaxElement
    {
        /// <summary>
        /// The unique constraint schema.
        /// </summary>
        private readonly XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="MergeAsyncMethod"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">A description of a unique constraint.</param>
        public MergeAsyncMethod(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;
            this.Name = "MergeAsync";

            //        /// <summary>
            //        /// Merges the <see cref="Ledger"/> data transfer object.
            //        /// </summary>
            //        /// <param name="ledgerDto">The ledger data transfer object.</param>
            //        /// <param name="cancellationToken">The cancellation token.</param>
            //        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
            //        public async Task MergeAsync(LedgerDto ledgerDto, CancellationToken cancellationToken = default)
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
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.AsyncKeyword),
                    }))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier($"{this.xmlSchemaDocument.Name.ToCamelCase()}Dto"))
                            .WithType(
                                SyntaxFactory.IdentifierName($"{this.xmlSchemaDocument.Name}Dto")),
                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                            SyntaxFactory.Parameter(
                                SyntaxFactory.Identifier("cancellationToken"))
                            .WithType(
                                SyntaxFactory.IdentifierName("CancellationToken"))
                            .WithDefault(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.DefaultLiteralExpression,
                                        SyntaxFactory.Token(SyntaxKind.DefaultKeyword)))),
                        })))
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
                return SyntaxFactory.Block(
                    new StatementSyntax[]
                    {
                        //            using (var asyncTransaction = new AsyncTransaction(cancellationToken))
                        //            {
                        //                 <MergeTables>
                        //            }
                        SyntaxFactory.UsingStatement(
                            SyntaxFactory.Block(this.MergeTables))
                        .WithDeclaration(
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
                                        SyntaxFactory.Identifier("asyncTransaction"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.IdentifierName("AsyncTransaction"))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.IdentifierName("cancellationToken")))))))))),
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
                return SyntaxFactory.TriviaList(
                    new List<SyntaxTrivia>
                    {
                        //        /// <summary>
                        //        /// Merges the <see cref="Ledger"/> data transfer object.
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
                                                    $" Merges the <see cref=\"{this.xmlSchemaDocument.Name}\"/> data transfer object.",
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

                        //        /// <param name="ledgerDto">The ledger data transfer object.</param>
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
                                                    $" <param name=\"{this.xmlSchemaDocument.Name.ToCamelCase()}Dto\">The data transfer object.</param>",
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                                SyntaxFactory.XmlTextNewLine(
                                                    SyntaxFactory.TriviaList(),
                                                    Environment.NewLine,
                                                    string.Empty,
                                                    SyntaxFactory.TriviaList()),
                                            }))))),

                        //        /// <param name="cancellationToken">The cancellation token.</param>
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
                                                    " <param name=\"cancellationToken\">The cancellation token.</param>",
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
                    });
            }
        }

        /// <summary>
        /// Gets the code to initialize the data model.
        /// </summary>
        private IEnumerable<StatementSyntax> InitializeDataModel
        {
            get
            {
                // This is used to collect the statements.
                var statements = new List<StatementSyntax>();

                // Create a list of tables ordered by dependencies on other tables.
                var orderedTables = new List<TableElement>();
                var resolvedTables = new HashSet<TableElement>();
                var unresolvedTables = new HashSet<TableElement>(this.xmlSchemaDocument.Tables);
                while (unresolvedTables.Count != 0)
                {
                    foreach (var tableElement in unresolvedTables.OrderBy(te => te.Name))
                    {
                        var isDependent = false;
                        foreach (var foreignIndexElement in tableElement.ParentIndices)
                        {
                            var parentTable = foreignIndexElement.UniqueIndex.Table;
                            if (!resolvedTables.Contains(parentTable))
                            {
                                isDependent = true;
                                break;
                            }
                        }

                        if (!isDependent)
                        {
                            orderedTables.Add(tableElement);
                            resolvedTables.Add(tableElement);
                            unresolvedTables.Remove(tableElement);
                        }
                    }
                }

                // Apply each of the tables in dependency order.
                foreach (TableElement tableElement in orderedTables)
                {
                    //                    await this.ApplyAllocationOrders(ledgerDto.AllocationOrders).ConfigureAwait(false);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AwaitExpression(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName($"Apply{tableElement.Name.ToPlural()}")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName($"{this.xmlSchemaDocument.Name.ToCamelCase()}Dto"),
                                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))));
                }

                return statements;
            }
        }

        /// <summary>
        /// Gets the code to merge the tables.
        /// </summary>
        private IEnumerable<StatementSyntax> MergeTables
        {
            get
            {
                var statements = new List<StatementSyntax>();

                // Lock each of the tables for writing.
                foreach (var tableElement in this.xmlSchemaDocument.Tables)
                {
                    //            await this.Accounts.EnterReadLockAsync().ConfigureAwait(false);
                    statements.Add(
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
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                                SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))));
                }

                statements.AddRange(
                    new StatementSyntax[]
                    {
                        //              if (this.RowVersion == 0L)
                        //              {
                        //                  <InitializeDataModel>
                        //              }
                        //              else
                        //              {
                        //                  <UpdateDataModel>
                        //              }
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("RowVersion")),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal(0L))),
                            SyntaxFactory.Block(this.InitializeDataModel))
                        .WithElse(
                            SyntaxFactory.ElseClause(
                                SyntaxFactory.Block(this.UpdateDataModel))),

                        //              asyncTransaction.Commit();
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("asyncTransaction"),
                                    SyntaxFactory.IdentifierName("Commit")))),
                    });

                return statements;
            }
        }

        /// <summary>
        /// Gets the code to update the data model.
        /// </summary>
        private IEnumerable<StatementSyntax> UpdateDataModel
        {
            get
            {
                var statements = new List<StatementSyntax>
                {
                    //                  var allRecords = new List<DataTransferObjects.IRowVersioned>();
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
                                    SyntaxFactory.Identifier("allRecords"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.GenericName(
                                                SyntaxFactory.Identifier("List"))
                                            .WithTypeArgumentList(
                                                SyntaxFactory.TypeArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                        SyntaxFactory.QualifiedName(
                                                            SyntaxFactory.IdentifierName("DataTransferObjects"),
                                                            SyntaxFactory.IdentifierName("IRowVersioned"))))))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList())))))),
                };

                // Add the table DTOs to the unified collection.
                foreach (var tableElement in this.xmlSchemaDocument.Tables.OrderBy(te => te.Name))
                {
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("allRecords"),
                                    SyntaxFactory.IdentifierName("AddRange")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName($"{this.xmlSchemaDocument.Name.ToCamelCase()}Dto"),
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural()))))))));
                }

                // Apply each row in row version order.
                statements.Add(
                    SyntaxFactory.ForEachStatement(
                        SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.VarKeyword,
                                "var",
                                "var",
                                SyntaxFactory.TriviaList())),
                        SyntaxFactory.Identifier("row"),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("allRecords"),
                                SyntaxFactory.IdentifierName("OrderBy")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.SimpleLambdaExpression(
                                            SyntaxFactory.Parameter(
                                                SyntaxFactory.Identifier("r")))
                                        .WithExpressionBody(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("r"),
                                                SyntaxFactory.IdentifierName("RowVersion"))))))),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.AwaitExpression(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.ElementAccessExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("rowVector")))
                                            .WithArgumentList(
                                                SyntaxFactory.BracketedArgumentList(
                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                        SyntaxFactory.Argument(
                                                            SyntaxFactory.InvocationExpression(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName("row"),
                                                                    SyntaxFactory.IdentifierName("GetType"))))))))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("row")))))))))));

                return statements;
            }
        }
    }
}