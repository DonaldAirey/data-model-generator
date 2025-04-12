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
                new StatementSyntax[]
                {
                    //            this.CommitList.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("CommitActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.AddCommitBody(tableElement))
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

            // Remove this row from each of the parent rows.
            foreach (ForeignIndexElement foreignIndexElement in tableElement.ParentIndices)
            {
                // If any of the index elements are nullable, we'll want a test to see if values are provided for all elements of the key.
                if (foreignIndexElement.Columns.Where(cre => cre.Column.ColumnType.IsNullable).Any())
                {
                    //                    if (updatedRow.ModelId != null)
                    //                    {
                    //                        var removedAccount = this.fixture.Accounts.Find(foundRow.AccountId);
                    //                        ConstraintException.ThrowIfNull(removedAccount, "The action conflicted with the AccountPositionIndex constraint.");
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
                    //                    ConstraintException.ThrowIfNull(removedAccount, "The action conflicted with the AccountPositionIndex constraint.");
                    //                    await removedAccount.EnterWriteLockAsync().ConfigureAwait(false);
                    statements.AddRange(
                        RowUtilities.RemoveFromParent(
                            foreignIndexElement,
                            $"removed{foreignIndexElement.UniqueParentName}",
                            tableElement.Name.ToVariableName()));
                }
            }

            statements.AddRange(
                new StatementSyntax[]
                {
                    //            this.CommitList.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("CommitActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.RemoveCommitBody(tableElement))
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
                new StatementSyntax[]
                {
                    //            this.CommitList.Add(() =>
                    //            {
                    //                account.RowVersion = this.fixture.IncrementRowVersion();
                    //            });
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("CommitActions")),
                                SyntaxFactory.IdentifierName("Add")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithBlock(
                                            SyntaxFactory.Block(RowUtilities.UpdateCommitBody(tableElement))
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
            else
            {
                //            this.dataModel.RowVersion = account.RowVersion;
                expressionList.Add((
                    "RowVersion",
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(tableElement.Document.Name.ToCamelCase())),
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

            statements.AddRange(
                new StatementSyntax[]
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
                });

            return statements;
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
                //            this.dataModel.RowVersion = account.RowVersion;
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
                    //                    removedModel.Accounts.Remove(account);
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
                                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))))))));
                }
                else
                {
                    //                removedAccount.Positions.Remove(position);
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
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))));
                }
            }

            statements.AddRange(
                new StatementSyntax[]
                {
                    //                    this.dictionary.Remove(removedRow.AccountId);
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

                    //                this.DeletedRows.AddFirst(account);
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("DeletedRows")),
                                SyntaxFactory.IdentifierName("AddFirst")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName("foundRow")))))),

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
                                                SyntaxFactory.IdentifierName("Remove"))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.IdentifierName("foundRow")),
                                    })))),
                });

            return statements;
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
                //            this.dataModel.RowVersion = account.RowVersion;
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
                    //                    removedModel.Accounts.Remove(account);
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
                                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))))))));
                }
                else
                {
                    //                removedAccount.Positions.Remove(position);
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
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))));
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

            statements.AddRange(
                new StatementSyntax[]
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
                });

            return statements;
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

                //                ConstraintException.ThrowIfNull(newAccount, "The action conflicted with the AccountPositionIndex constraint.");
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
                                            SyntaxFactory.Literal($"The action conflicted with the {foreignIndexElement.Name} constraint."))),
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

                //                ConstraintException.ThrowIfNull(newAccount, "The action conflicted with the AccountPositionIndex constraint.");
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
                                            SyntaxFactory.Literal($"The action conflicted with the {foreignIndexElement.Name} constraint."))),
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

            return new StatementSyntax[]
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

            return new StatementSyntax[]
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