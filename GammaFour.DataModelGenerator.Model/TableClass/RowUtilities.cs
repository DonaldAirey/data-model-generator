// <copyright file="RowUtilities.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Model.TableClass
{
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a method to add a row to the set.
    /// </summary>
    public static class RowUtilities
    {
        /// <summary>
        /// A range of statements to add one row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>A collection of statements that add a row.</returns>
        public static IEnumerable<StatementSyntax> AddRow(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>();

            // Check the constraints and update foreign keys.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                //            var thing = this.ledger.Things.Find(order.ThingCode);
                var uniqueIndexElement = foreignIndexElement.UniqueIndex;
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    statements.AddRange(
                        RowUtilities.GetNullableConstraintCheck(
                            foreignIndexElement,
                            $"added{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
                else
                {
                    statements.AddRange(
                        RowUtilities.GetNonNullableConstraintCheck(
                            foreignIndexElement,
                            $"added{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            var originalRow = new Account(account);
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
                                    SyntaxFactory.Identifier("originalRow"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(tableElement.Name))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))))))),
                });
            statements.AddRange(RowUtilities.AddCommitBody(tableElement));
            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            enlistment.RollbackActions.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("enlistmentState"),
                                    SyntaxFactory.IdentifierName("RollbackActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.AddRollbackBody(tableElement))
                                            .NormalizeWhitespace())))))),

                    //            enlistmentState.CommitActions.Add(() =>
                    //            {
                    //                this.OnRowChanged(DataAction.Add, account);
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("enlistmentState"),
                                    SyntaxFactory.IdentifierName("CommitActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.AddFinalizeBody(tableElement))
                                            .NormalizeWhitespace())))))),
                });

            return statements;
        }

        /// <summary>
        /// Gets the statements that removes a row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements to remove a row.</returns>
        public static IEnumerable<StatementSyntax> RemoveRow(TableElement tableElement)
        {
            // Lock the row.
            var statements = new List<StatementSyntax>
            {
                //            await removedRow.EnterWriteLockAsync().ConfigureAwait(false);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("foundRow"),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                                 tableElement.Document.IsMaster ?
                                                 SyntaxKind.FalseLiteralExpression :
                                                 SyntaxKind.TrueLiteralExpression))))))),
            };

            // Only the master needs to check for concurrency.
            if (tableElement.Document.IsMaster)
            {
                //                    ConcurrencyException.ThrowIfNotEqual(account.RowVersion, foundRow.RowVersion);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("ConcurrencyException"),
                                SyntaxFactory.IdentifierName("ThrowIfNotEqual")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                                SyntaxFactory.IdentifierName("RowVersion"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("foundRow"),
                                                SyntaxFactory.IdentifierName("RowVersion"))),
                                    })))));
            }

            // Make sure there are no children.
            foreach (ForeignIndexElement foreignIndexElement in tableElement.ChildIndices)
            {
                //                ConstraintException.ThrowIfTrue(foundRow.Positions, "AssetPositionIndex");
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("ConstraintException"),
                                SyntaxFactory.IdentifierName("ThrowIfTrue")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.BinaryExpression(
                                                SyntaxKind.NotEqualsExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("foundRow"),
                                                        SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                    SyntaxFactory.IdentifierName("Count")),
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(0)))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                SyntaxFactory.Literal(foreignIndexElement.Name))),
                                    })))));
            }

            // Lock each of the parent rows.
            foreach (ForeignIndexElement foreignIndexElement in tableElement.ParentIndices)
            {
                // If any of the index elements are nullable, we'll want a test to see if values are provided for all elements of the key.
                if (foreignIndexElement.Columns.Where(cre => cre.Column.ColumnType.IsNullable).Any())
                {
                    //                    if (updatedRow.ModelId != null)
                    //                    {
                    //                        var removedAccount = this.fixture.Accounts.Find(foundRow.AccountId);
                    //                        ConstraintException.ThrowIfNull(removedAccount, "AccountPositionIndex");
                    //                        await removedAccount.EnterWriteLockAsync().ConfigureAwait(false);
                    //                    }
                    statements.AddRange(
                        RowUtilities.RemoveFromNullableParent(
                            foreignIndexElement,
                            $"removed{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
                else
                {
                    //                    var removedAccount = this.fixture.Accounts.Find(foundRow.AccountId);
                    //                    ConstraintException.ThrowIfNull(removedAccount, "AccountPositionIndex");
                    //                    await removedAccount.EnterWriteLockAsync().ConfigureAwait(false);
                    statements.AddRange(
                        RowUtilities.RemoveFromParent(
                            foreignIndexElement,
                            $"removed{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            var originalRow = new Account(foundRow);
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
                                    SyntaxFactory.Identifier("originalRow"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(tableElement.Name))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow")))))))))),
                });
            statements.AddRange(RowUtilities.RemoveCommitBody(tableElement));
            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            this.RollbackActions.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("enlistmentState"),
                                    SyntaxFactory.IdentifierName("RollbackActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.RemoveRollbackBody(tableElement))
                                            .NormalizeWhitespace())))))),
                });
            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            this.CommitActions.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("enlistmentState"),
                                    SyntaxFactory.IdentifierName("CommitActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.RemoveFinalizeBody(tableElement))
                                            .NormalizeWhitespace())))))),
                });

            return statements;
        }

        /// <summary>
        /// Gets the statements that updates a row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements to update a row.</returns>
        public static IEnumerable<StatementSyntax> UpdateRow(TableElement tableElement)
        {
            // Lock the row and perform optimistic concurrency check.
            var statements = new List<StatementSyntax>
            {
                //            await updatedRow.EnterWriteLockAsync().ConfigureAwait(false);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName("foundRow"),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                                 tableElement.Document.IsMaster ?
                                                 SyntaxKind.FalseLiteralExpression :
                                                 SyntaxKind.TrueLiteralExpression))))))),
            };

            // Only the master needs to check for concurrency.
            if (tableElement.Document.IsMaster)
            {
                //                    ConcurrencyException.ThrowIfNotEqual(account.RowVersion, foundRow.RowVersion);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("ConcurrencyException"),
                                SyntaxFactory.IdentifierName("ThrowIfNotEqual")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                                SyntaxFactory.IdentifierName("RowVersion"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("foundRow"),
                                                SyntaxFactory.IdentifierName("RowVersion"))),
                                    })))));
            }

            // Remove from the parent.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                //            var thing = this.ledger.Things.Find(order.ThingCode);
                var uniqueIndexElement = foreignIndexElement.UniqueIndex;
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    statements.AddRange(
                        RowUtilities.RemoveFromNullableParent(
                            foreignIndexElement,
                            $"removed{foreignIndexElement.UniqueParentName}",
                            "foundRow"));
                }
                else
                {
                    statements.AddRange(
                        RowUtilities.RemoveFromParent(
                            foreignIndexElement,
                            $"removed{foreignIndexElement.UniqueParentName}",
                            "foundRow"));
                }
            }

            // Check the constraints and update foreign keys.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                //            var thing = this.ledger.Things.Find(order.ThingCode);
                var uniqueIndexElement = foreignIndexElement.UniqueIndex;
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    statements.AddRange(
                        RowUtilities.GetNullableConstraintCheck(
                            foreignIndexElement,
                            $"added{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
                else
                {
                    statements.AddRange(
                        RowUtilities.GetNonNullableConstraintCheck(
                            foreignIndexElement,
                            $"added{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            var originalRow = new Account(foundRow);
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
                                    SyntaxFactory.Identifier("originalRow"))
                                .WithInitializer(
                                    SyntaxFactory.EqualsValueClause(
                                        SyntaxFactory.ObjectCreationExpression(
                                            SyntaxFactory.IdentifierName(tableElement.Name))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow")))))))))),
                });
            statements.AddRange(RowUtilities.UpdateCommitBody(tableElement));
            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            enlistment.RollbackActions.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("enlistmentState"),
                                    SyntaxFactory.IdentifierName("RollbackActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.UpdateRollbackBody(tableElement))
                                            .NormalizeWhitespace())))))),

                    //            enlistmentState.CommitActions.Add(() =>
                    //            {
                    //                this.OnRowChanged(DataAction.Add, account);
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("enlistmentState"),
                                    SyntaxFactory.IdentifierName("CommitActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.UpdateFinalizeBody(tableElement))
                                            .NormalizeWhitespace())))))),
                });

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> AddCommitBody(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>();

            // The expressions and their property names are collected here.
            var expressionList = new List<(string, ExpressionStatementSyntax)>();

            // Copy each of the parent values.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                expressionList.Add((
                    foreignIndexElement.UniqueParentName,
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName)),
                            SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}")))));
            }

            if (tableElement.Document.IsMaster)
            {
                //            thing.RowVersion = this.ledger.IncrementRowVersion();
                expressionList.Add((
                    "RowVersion",
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(tableElement.Document.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName("IncrementRowVersion")))))));
            }

            //            this.AccountCode = position.AccountCode;
            //            this.AssetCode = position.AssetCode;
            //            this.Date = position.Date;
            //            this.Quantity = position.Quantity;
            foreach ((var key, var expressionStatement) in expressionList.OrderBy(tuple => tuple.Item1))
            {
                statements.Add(expressionStatement);
            }

            // Add this row to each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (addedModel != null)
                    //                {
                    //                    addedModel.Accounts.Add(account);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Add")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))))))));
                }
                else
                {
                    //                addedAccount.Positions.Add(position);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                        SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                    SyntaxFactory.IdentifierName("Add")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))));
                }
            }

            // Add the row to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                //            this.AccountNameIndex.Add(account);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))));
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            this.dictionary.Add(thing.Code, thing);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dictionary")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        tableElement.PrimaryIndex.GetKeyAsArguments(tableElement.Name.ToVariableName()),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())),
                                    })))),
                });

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> AddRollbackBody(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>
            {
                //                account.CopyFrom(originalRow);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                            SyntaxFactory.IdentifierName("CopyFrom")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("originalRow")))))),
            };

            // Add this row to each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (addedModel != null)
                    //                {
                    //                    addedModel.Accounts.Remove(account);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Remove")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))))))));
                }
                else
                {
                    //                addedAccount.Positions.Remove(position);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                        SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))));
                }
            }

            // Remove the row to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                //            this.AccountNameIndex.Remove(account);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))));
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            this.dictionary.Remove(thing.Code);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dictionary")),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        tableElement.PrimaryIndex.GetKeyAsArguments(tableElement.Name.ToVariableName()),
                                    })))),
                });

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> AddFinalizeBody(TableElement tableElement)
        {
            return new List<StatementSyntax>
            {
                //                this.OnRowChanged(DataAction.Add, account);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("OnRowChanged")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("DataAction"),
                                            SyntaxFactory.IdentifierName("Add"))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())),
                                })))),
            };
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> RemoveCommitBody(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>();

            // The expressions and their property names are collected here.
            var expressionList = new List<(string, ExpressionStatementSyntax)>();

            // Clear each of the parent values.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                //                        foundRow.Account = null;
                expressionList.Add((
                    foreignIndexElement.UniqueParentName,
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("foundRow"),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName)),
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.NullLiteralExpression)))));
            }

            if (tableElement.Document.IsMaster)
            {
                //            thing.RowVersion = this.ledger.IncrementRowVersion();
                expressionList.Add((
                    "RowVersion",
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
                                        SyntaxFactory.IdentifierName(tableElement.Document.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName("IncrementRowVersion")))))));
            }
            else
            {
                //            foundRow.RowVersion = account.RowVersion;
                expressionList.Add((
                    "RowVersion",
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("foundRow"),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName("RowVersion"))))));
            }

            //            this.AccountCode = position.AccountCode;
            //            this.AssetCode = position.AssetCode;
            //            this.Date = position.Date;
            //            this.Quantity = position.Quantity;
            foreach ((var key, var expressionStatement) in expressionList.OrderBy(tuple => tuple.Item1))
            {
                statements.Add(expressionStatement);
            }

            // Remove this row from each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (removedModel != null)
                    //                {
                    //                    removedModel.Accounts.Remove(foundRow);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Remove")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))))))));
                }
                else
                {
                    //                removedAccount.Positions.Remove(foundRow);
                    statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))));
                }
            }

            // Remove the row to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                //            this.AccountNameIndex.Remove(account);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))));
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //                    this.dictionary.Remove(foundRow.AccountId);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dictionary")),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    tableElement.PrimaryIndex.GetKeyAsArguments("foundRow"))))),
                });

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> RemoveRollbackBody(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>
            {
                //                        foundRow.CopyFrom(originalRow);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("foundRow"),
                            SyntaxFactory.IdentifierName("CopyFrom")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("originalRow")))))),
            };

            // Add this row back to each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (removedModel != null)
                    //                {
                    //                    removedModel.Accounts.Add(foundRow);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Add")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))))))));
                }
                else
                {
                    //                removedAccount.Positions.Add(foundRow);
                    statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))));
                }
            }

            // Add the row back to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                //            this.AccountNameIndex.Add(foundRow);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))));
            }

            statements.AddRange(
                new List<StatementSyntax>
                {
                    //            this.dictionary.Add(thing.Code, thing);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("dictionary")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        tableElement.PrimaryIndex.GetKeyAsArguments("foundRow"),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("foundRow")),
                                    })))),
                });

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> RemoveFinalizeBody(TableElement tableElement)
        {
            return new List<StatementSyntax>
            {
                //                this.OnRowChanged(DataAction.Remove, foundRow);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("OnRowChanged")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("DataAction"),
                                            SyntaxFactory.IdentifierName("Remove"))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")),
                                })))),
            };
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> UpdateCommitBody(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>();

            // The expressions and their property names are collected here.
            var expressionList = new List<(string, ExpressionStatementSyntax)>();

            // Copy each of the parent values.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                //                        foundRow.Account = addedAccount;
                expressionList.Add((
                    foreignIndexElement.UniqueParentName,
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("foundRow"),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName)),
                            SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}")))));
            }

            // Copy the value properties from the source row.
            foreach (var columnElement in tableElement.Columns.Where(ce => !ce.IsRowVersion))
            {
                //                        foundRow.AccountId = position.AccountId;
                expressionList.Add((
                    columnElement.Name,
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("foundRow"),
                                SyntaxFactory.IdentifierName(columnElement.Name)),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName(columnElement.Name))))));
            }

            if (tableElement.Document.IsMaster)
            {
                //            thing.RowVersion = this.ledger.IncrementRowVersion();
                expressionList.Add((
                    "RowVersion",
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
                                        SyntaxFactory.IdentifierName(tableElement.Document.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName("IncrementRowVersion")))))));
            }
            else
            {
                //            foundRow.RowVersion = account.RowVersion;
                expressionList.Add((
                    "RowVersion",
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("foundRow"),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                                SyntaxFactory.IdentifierName("RowVersion"))))));
            }

            //            this.AccountCode = position.AccountCode;
            //            this.AssetCode = position.AssetCode;
            //            this.Date = position.Date;
            //            this.Quantity = position.Quantity;
            foreach ((var key, var expressionStatement) in expressionList.OrderBy(tuple => tuple.Item1))
            {
                statements.Add(expressionStatement);
            }

            // Remove this row from each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (removedModel != null)
                    //                {
                    //                    removedModel.Accounts.Remove(foundRow);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Remove")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))))))));
                }
                else
                {
                    //                removedAccount.Positions.Remove(foundRow);
                    statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                SyntaxFactory.IdentifierName("Remove")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))));
                }
            }

            // Add this row to each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (addedModel != null)
                    //                {
                    //                    addedModel.Accounts.Add(foundRow);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Add")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))))))));
                }
                else
                {
                    //                addedAccount.Positions.Add(foundRow);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                        SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                    SyntaxFactory.IdentifierName("Add")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("foundRow")))))));
                }
            }

            // Remove the row to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                statements.Add(
                    SyntaxFactory.IfStatement(
                        uniqueKeyElement.GetKeyAsInequalityConditional("originalRow", "foundRow"),
                        SyntaxFactory.Block(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Remove")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("originalRow")))))),
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Add")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("foundRow")))))))));
            }

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> UpdateRollbackBody(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>
            {
                //                        foundRow.CopyFrom(originalRow);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("foundRow"),
                            SyntaxFactory.IdentifierName("CopyFrom")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName("originalRow")))))),
            };

            // Add this row back to each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (removedModel != null)
                    //                {
                    //                    removedModel.Accounts.Add(foundROw);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Add")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))))))));
                }
                else
                {
                    //                removedAccount.Positions.Add(position);
                    statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName($"removed{foreignIndexElement.UniqueParentName}"),
                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))));
                }
            }

            // Remove this row from each of the parent rows.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                // We need to check for a null parent before updating nullable parents.
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    //                if (addedModel != null)
                    //                {
                    //                    addedModel.Accounts.Remove(account);
                    //                }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            SyntaxFactory.Block(
                                SyntaxFactory.SingletonList<StatementSyntax>(
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Remove")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName("foundRow"))))))))));
                }
                else
                {
                    //                addedAccount.Positions.Remove(position);
                    statements.Add(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName($"added{foreignIndexElement.UniqueParentName}"),
                                        SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                    SyntaxFactory.IdentifierName("Remove")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("foundRow")))))));
                }
            }

            // Remove the row to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                statements.Add(
                    SyntaxFactory.IfStatement(
                        uniqueKeyElement.GetKeyAsInequalityConditional("originalRow", "foundRow"),
                        SyntaxFactory.Block(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Add")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("originalRow")))))),
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Remove")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.IdentifierName("foundRow")))))))));
            }

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        private static IEnumerable<StatementSyntax> UpdateFinalizeBody(TableElement tableElement)
        {
            return new List<StatementSyntax>
            {
                //                this.OnRowChanged(DataAction.Update, account);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("OnRowChanged")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("DataAction"),
                                            SyntaxFactory.IdentifierName("Update"))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")),
                                })))),
            };
        }

        /// <summary>
        /// Gets the constraint check for a non-nullable index.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="parentRowName">The parent row name.</param>
        /// <param name="rowName">The row name.</param>
        /// <returns>The code for a constraint check.</returns>
        private static IEnumerable<StatementSyntax> GetNonNullableConstraintCheck(
            ForeignIndexElement foreignIndexElement,
            string parentRowName,
            string rowName)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var tableElement = foreignIndexElement.Table;
            return new List<StatementSyntax>
            {
                //            var thing = this.ledger.Things.Find(order.ThingCode);
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
                                SyntaxFactory.Identifier(parentRowName))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Document.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("Find")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                foreignIndexElement.GetKeyAsArguments(
                                                    rowName))))))))),

                //                ConstraintException.ThrowIfNull(newAccount, "AccountPositionIndex");
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("ConstraintException"),
                            SyntaxFactory.IdentifierName("ThrowIfNull")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(parentRowName)),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(foreignIndexElement.Name))),
                                })))),

                //            await account.EnterWriteLockAsync().ConfigureAwait(false);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(parentRowName),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            foreignIndexElement.Table.Document.IsMaster ?
                                            SyntaxKind.FalseLiteralExpression :
                                            SyntaxKind.TrueLiteralExpression))))))),
            };
        }

        /// <summary>
        /// Gets the constraint check for a nullable index.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="parentRowName">The parent row name.</param>
        /// <param name="rowName">The row name.</param>
        /// <returns>The code for a constraint check.</returns>
        private static IEnumerable<StatementSyntax> GetNullableConstraintCheck(
            ForeignIndexElement foreignIndexElement,
            string parentRowName,
            string rowName)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var tableElement = foreignIndexElement.Table;
            var statements = new List<StatementSyntax>
            {
                //            thing = this.ledger.Things.Find(order.ThingCode);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(parentRowName),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Document.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Find")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    foreignIndexElement.GetKeyAsArguments(rowName)))))),

                //                ConstraintException.ThrowIfNull(newAccount, "AccountPositionIndex");
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("ConstraintException"),
                            SyntaxFactory.IdentifierName("ThrowIfNull")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(parentRowName)),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(foreignIndexElement.Name))),
                                })))),

                //            await account.EnterWriteLockAsync().ConfigureAwait(false);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(parentRowName),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            foreignIndexElement.Table.Document.IsMaster ?
                                            SyntaxKind.FalseLiteralExpression :
                                            SyntaxKind.TrueLiteralExpression))))))),
            };

            return new List<StatementSyntax>
            {
                //            Model? addedModel = null;
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.NullableType(
                            SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name)))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(parentRowName))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)))))),

                //            if (thing.BrokerFeedId != null)
                //            {
                //                  <statements>
                //            }
                SyntaxFactory.IfStatement(
                    foreignIndexElement.GetKeyAsInequalityConditional(rowName, null),
                    SyntaxFactory.Block(statements)),
            };
        }

        /// <summary>
        /// Removes the row from the parent row.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="parentRowName">The parent row name.</param>
        /// <param name="rowName">The row name.</param>
        /// <returns>The statements to remove a row from the parent row.</returns>
        private static IEnumerable<StatementSyntax> RemoveFromNullableParent(
            ForeignIndexElement foreignIndexElement,
            string parentRowName,
            string rowName)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var statements = new List<StatementSyntax>
            {
                //            thing = this.ledger.Things.Find(order.ThingCode);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(parentRowName),
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Document.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name.ToPlural())),
                                SyntaxFactory.IdentifierName("Find")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    foreignIndexElement.GetKeyAsArguments(rowName)))))),

                //            ArgumentNullException.ThrowIfNull(thing);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("ArgumentNullException"),
                            SyntaxFactory.IdentifierName("ThrowIfNull")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(parentRowName)))))),

                //            await account.EnterWriteLockAsync().ConfigureAwait(false);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(parentRowName),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            foreignIndexElement.Table.Document.IsMaster ?
                                            SyntaxKind.FalseLiteralExpression :
                                            SyntaxKind.TrueLiteralExpression))))))),
            };

            return new List<StatementSyntax>
            {
                //            Model? addedModel = null;
                SyntaxFactory.LocalDeclarationStatement(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.NullableType(
                            SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name)))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier(parentRowName))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NullLiteralExpression)))))),

                //            if (thing.BrokerFeedId != null)
                //            {
                //                  <statements>
                //            }
                SyntaxFactory.IfStatement(
                    foreignIndexElement.GetKeyAsInequalityConditional(rowName, null),
                    SyntaxFactory.Block(statements)),
            };
        }

        /// <summary>
        /// Removes the row from the parent row.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="parentRowName">The parent row name.</param>
        /// <param name="rowName">The row name.</param>
        /// <returns>The statements to remove a row from the parent row.</returns>
        private static IEnumerable<StatementSyntax> RemoveFromParent(ForeignIndexElement foreignIndexElement, string parentRowName, string rowName)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var statements = new List<StatementSyntax>
            {
                //            var thing = this.ledger.Things.Find(order.ThingCode);
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
                                SyntaxFactory.Identifier(parentRowName))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Document.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("Find")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                foreignIndexElement.GetKeyAsArguments(rowName))))))))),

                //            ArgumentNullException.ThrowIfNull(thing);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("ArgumentNullException"),
                            SyntaxFactory.IdentifierName("ThrowIfNull")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(parentRowName)))))),

                //            await account.EnterWriteLockAsync().ConfigureAwait(false);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(parentRowName),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync"))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            foreignIndexElement.Table.Document.IsMaster ?
                                            SyntaxKind.FalseLiteralExpression :
                                            SyntaxKind.TrueLiteralExpression))))))),
            };

            return statements;
        }
    }
}