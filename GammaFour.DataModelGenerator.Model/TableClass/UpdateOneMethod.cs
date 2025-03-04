// <copyright file="UpdateOneMethod.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.TableClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to add a row to the set.
    /// </summary>
    public class UpdateOneMethod : SyntaxElement
    {
        /// <summary>
        /// The table schema.
        /// </summary>
        private readonly TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateOneMethod"/> class.
        /// </summary>
        /// <param name="tableElement">The unique constraint schema.</param>
        public UpdateOneMethod(TableElement tableElement)
        {
            // Initialize the object.
            this.tableElement = tableElement;
            this.Name = "UpdateAsync";

            //        /// <summary>
            //        /// Updates a <see cref="Thing"/> row in the table.
            //        /// </summary>
            //        /// <param name="thing">The thing row.</param>
            //        /// <returns>The updated The <see cref="Thing"/> row.</returns>
            //        public async Task<Thing> Update(Thing thing)
            //        {
            //            <Body>
            //        }
            this.Syntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("Task"))
                .WithTypeArgumentList(
                    SyntaxFactory.TypeArgumentList(
                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                            SyntaxFactory.IdentifierName(this.tableElement.Name)))),
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
                    SyntaxFactory.SingletonSeparatedList<ParameterSyntax>(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(this.tableElement.Name.ToVariableName()))
                        .WithType(
                            SyntaxFactory.IdentifierName(this.tableElement.Name)))))
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
                // The elements of the body are added to this collection as they are assembled.
                var statements = new List<StatementSyntax>
                {
                    //                using var lockingTransaction = new LockingTransaction(this.transactionTimeout);
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
                                    SyntaxFactory.Identifier("lockingTransaction"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("LockingTransaction"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName("timeout")))))))))))
                    .WithUsingKeyword(
                        SyntaxFactory.Token(SyntaxKind.UsingKeyword)),
                };

                // This is used to keep track of the parent rows that were locked for this operation.
                var parentTables = from parentIndex in this.tableElement.ParentIndices
                                   group parentIndex by parentIndex.UniqueIndex.Table into grouping
                                   select grouping.Key;
                foreach (var parentTable in parentTables)
                {
                    //            var parentThings = new HashSet<Thing>();
                    statements.Add(
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
                                        SyntaxFactory.Identifier($"parent{parentTable.Name.ToPlural()}"))
                                    .WithInitializer(
                                        SyntaxFactory.EqualsValueClause(
                                            SyntaxFactory.ObjectCreationExpression(
                                                SyntaxFactory.GenericName(
                                                    SyntaxFactory.Identifier("HashSet"))
                                                .WithTypeArgumentList(
                                                    SyntaxFactory.TypeArgumentList(
                                                        SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                            SyntaxFactory.IdentifierName(parentTable.Name)))))
                                            .WithArgumentList(
                                                SyntaxFactory.ArgumentList())))))));
                }

                // Find the row and delete it.
                statements.AddRange(
                    new StatementSyntax[]
                    {
                        //            if (this.dictionary.TryGetValue(thingId, out var foundRow))
                        //            {
                        //                <UpdateRow>
                        //            }
                        //            {
                        //                foundRow = thing;
                        //            }
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("dictionary")),
                                    SyntaxFactory.IdentifierName("TryGetValue")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            this.tableElement.PrimaryIndex.GetKeyAsArguments(this.tableElement.Name.ToVariableName()),
                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.DeclarationExpression(
                                                    SyntaxFactory.IdentifierName(
                                                        SyntaxFactory.Identifier(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.VarKeyword,
                                                            "var",
                                                            "var",
                                                            SyntaxFactory.TriviaList())),
                                                    SyntaxFactory.SingleVariableDesignation(
                                                        SyntaxFactory.Identifier("foundRow"))))
                                            .WithRefOrOutKeyword(
                                                SyntaxFactory.Token(SyntaxKind.OutKeyword)),
                                        }))),
                            SyntaxFactory.Block(this.UpdateRow))
                        .WithElse(
                            SyntaxFactory.ElseClause(
                                SyntaxFactory.Block(
                                    SyntaxFactory.ThrowStatement(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("KeyNotFoundException"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList()))))),

                        //            return foundRow;
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.IdentifierName("foundRow")),
                    });

                // This is the syntax for the body of the method.
                return SyntaxFactory.Block(SyntaxFactory.List<StatementSyntax>(statements));
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
                    //        /// Updates a <see cref="Order"/> row in the table.
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
                                                $" Updates a <see cref=\"{this.tableElement.Name}\"/> row in the table.",
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

                    //        /// <param name="order">The <see cref="Order"/> row.</param>
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
                                                    $" <param name=\"{this.tableElement.Name.ToCamelCase()}\">The {this.tableElement.Name.ToCamelCase()} row.</param>",
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
        /// Gets the statements that deletes a row.
        /// </summary>
        /// <returns>The statements to delete a row.</returns>
        private IEnumerable<StatementSyntax> UpdateRow
        {
            get
            {
                // Lock the row and perform optimistic concurrency check.
                var statements = new List<StatementSyntax>
                {
                        //                await lockingTransaction.WaitWriterAsync(foundRow).ConfigureAwait(false);
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AwaitExpression(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("lockingTransaction"),
                                                SyntaxFactory.IdentifierName("WaitWriterAsync")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))),

                        //                if (foundRow.RowVersion != thing.RowVersion)
                        //                {
                        //                    throw new DBConcurrencyException();
                        //                }
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                                    SyntaxFactory.IdentifierName("RowVersion")),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("foundRow"),
                                    SyntaxFactory.IdentifierName("RowVersion"))),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ThrowStatement(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName("DBConcurrencyException"))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList()))))),
                };

                // Detach ourselves from the old parent row, and attach ourselves to the new parent row.
                foreach (var foreignIndexElement in this.tableElement.ParentIndices)
                {
                    statements.AddRange(this.UpdateForeignIndex(foreignIndexElement));
                }

                // Update each of the columns.
                foreach (var columnElement in this.tableElement.Columns)
                {
                    if (columnElement.IsRowVersion)
                    {
                        //                foundRow.RowVersion = this.dataModel.IncrementRowVersion();
                        statements.Add(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("foundRow"),
                                        SyntaxFactory.IdentifierName("RowVersion")),
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(this.tableElement.Document.Name.ToCamelCase())),
                                            SyntaxFactory.IdentifierName("IncrementRowVersion"))))));
                    }
                    else
                    {
                        //                    foundRow.ModelId = account.ModelId;
                        statements.Add(SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("foundRow"),
                                    SyntaxFactory.IdentifierName(columnElement.Name)),
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName(this.tableElement.Name.ToVariableName()),
                                    SyntaxFactory.IdentifierName(columnElement.Name)))));
                    }
                }

                return statements;
            }
        }

        private IEnumerable<StatementSyntax> ChangeParentRow(ForeignIndexElement foreignIndexElement)
        {
            var statements = new List<StatementSyntax>();

            // If any of the index elements are nullable, we'll want a test to see if values are provided for all elements of the key.
            if (foreignIndexElement.Columns.Where(cre => cre.Column.ColumnType.IsNullable).Any())
            {
                //                    if (foundRow.ModelId != null)
                //                    {
                //                    }
                statements.Add(
                    SyntaxFactory.IfStatement(
                        foreignIndexElement.GetKeyAsInequalityConditional("foundRow", null),
                        SyntaxFactory.Block(RowUtilities.RemoveFromParent("foundRow", foreignIndexElement))));
            }
            else
            {
                statements.AddRange(RowUtilities.RemoveFromParent("foundRow", foreignIndexElement));
            }

            statements.AddRange(RowUtilities.AddToParent(foreignIndexElement));

            return statements;
        }

        private IEnumerable<StatementSyntax> UpdateForeignIndex(ForeignIndexElement foreignIndexElement)
        {
            var statements = new List<StatementSyntax>();

            //                if (account.ModelId != foundRow.ModelId)
            //                {
            //                }
            statements.Add(
                SyntaxFactory.IfStatement(
                    foreignIndexElement.GetKeyAsInequalityConditional(this.tableElement.Name.ToVariableName(), "foundRow"),
                    SyntaxFactory.Block(this.ChangeParentRow(foreignIndexElement))));

            return statements;
        }
    }
}