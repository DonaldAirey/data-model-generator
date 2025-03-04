// <copyright file="RowUtilities.cs" company="Gamma Four, Inc.">
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
    public static class RowUtilities
    {
        /// <summary>
        /// A range of statements to add one row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>A collection of statements that add a row.</returns>
        public static IEnumerable<StatementSyntax> AddRowToDataModel(TableElement tableElement)
        {
            var statements = new List<StatementSyntax>
            {
                //            await lockingTransaction.WaitWriterAsync(thing).ConfigureAwait(false);
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
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))),
            };

            // Check the constraints and update foreign keys.
            foreach (var foreignIndexElement in tableElement.ParentIndices)
            {
                //            var thing = this.DataModel.Things.Find(order.ThingCode);
                var uniqueIndexElement = foreignIndexElement.UniqueIndex;
                if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
                {
                    statements.AddRange(
                        RowUtilities.GetNullableConstraintCheck(foreignIndexElement, tableElement.Name.ToVariableName()));
                }
                else
                {
                    statements.AddRange(
                        RowUtilities.GetNonNullableConstraintCheck(foreignIndexElement, tableElement.Name.ToVariableName()));
                }
            }

            statements.AddRange(
                new StatementSyntax[]
                {
                    //            thing.RowVersion = this.DataModel.IncrementRowVersion();
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
                                    SyntaxFactory.IdentifierName("IncrementRowVersion"))))),

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

                    //            this.rollbackStack.Push(() => this.dictionary.Remove(thing.Code));
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("rollbackStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithExpressionBody(
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
                                                        tableElement.PrimaryIndex.GetKeyAsArguments(
                                                            tableElement.Name.ToVariableName())))))))))),
                });

            // Add the row to each of the unique key indices on this set.
            foreach (var uniqueKeyElement in tableElement.UniqueIndexes.Where(ui => !ui.IsPrimaryIndex))
            {
                //            this.ThingNameIndex.Add(thing);
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

                //            this.rollbackStack.Push(() => this.ThingNameIndex.Remove(thing));
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("rollbackStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithExpressionBody(
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
                                                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()))))))))))));
            }

            statements.AddRange(
                new StatementSyntax[]
                {
                    //            this.commitStack.Push(() => this.RowChanged?.Invoke(this, new RowChangedEventArgs<Thing>(DataAction.Add, thing)));
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("commitStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithExpressionBody(
                                            SyntaxFactory.ConditionalAccessExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("RowChanged")),
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
                                                                            SyntaxFactory.Identifier("RowChangedEventArgs"))
                                                                        .WithTypeArgumentList(
                                                                            SyntaxFactory.TypeArgumentList(
                                                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                                    SyntaxFactory.IdentifierName(tableElement.Name)))))
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
                                                            })))))))))),
                });

            return statements;
        }

        /// <summary>
        /// Adds a parent row to the child row.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <returns>The statements that add a parent row to a child row.</returns>
        public static IEnumerable<StatementSyntax> AddToParent(ForeignIndexElement foreignIndexElement)
        {
            var statements = new List<StatementSyntax>();

            //            var thing = this.DataModel.Things.Find(order.ThingCode);
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            if (foreignIndexElement.Columns.Where(ce => ce.Column.ColumnType.IsNullable).Any())
            {
                statements.AddRange(RowUtilities.GetNullableConstraintCheck(foreignIndexElement, "foundRow"));
            }
            else
            {
                statements.AddRange(RowUtilities.GetNonNullableConstraintCheck(foreignIndexElement, "foundRow"));
            }

            return statements;
        }

        /// <summary>
        /// A range of statements to add one row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>A collection of statements that add a row.</returns>
        public static IEnumerable<StatementSyntax> DeleteRowFromDataModel(TableElement tableElement)
        {
            // Find the row and delete it.
            return new List<StatementSyntax>
            {
                //            if (this.dictionary.TryGetValue(thingId, out var deletedRow))
                //            {
                //                <DeleteRow>
                //            }
                //            {
                //                deletedRow = thing;
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
                                    tableElement.PrimaryIndex.GetKeyAsArguments(tableElement.Name.ToVariableName()),
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
                                                SyntaxFactory.Identifier("deletedRow"))))
                                    .WithRefOrOutKeyword(
                                        SyntaxFactory.Token(SyntaxKind.OutKeyword)),
                                }))),
                    SyntaxFactory.Block(RowUtilities.DeleteRow(tableElement)))
                .WithElse(
                    SyntaxFactory.ElseClause(
                        SyntaxFactory.Block(
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.AssignmentExpression(
                                    SyntaxKind.SimpleAssignmentExpression,
                                    SyntaxFactory.IdentifierName("deletedRow"),
                                    SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName())))))),
            };
        }

        /// <summary>
        /// A range of statements to add one row.
        /// </summary>
        /// <param name="tableElement">The table element.</param>
        /// <returns>A collection of statements that add a row.</returns>
        public static IEnumerable<StatementSyntax> GetParentRowCache(TableElement tableElement)
        {
            // Find the row and delete it.
            var statements = new List<StatementSyntax>();

            //            var parentThings = new HashSet<Thing>();
            //            var parentAccounts = new HashSet<Account>();
            //            var parentAssets = new HashSet<Asset>();
            var parentTables = from parentIndex in tableElement.ParentIndices
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

            return statements;
        }

        /// <summary>
        /// Deletes the row from the parent row.
        /// </summary>
        /// <param name="variableName">The variable name.</param>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <returns>The statements to delete a row from the parent row.</returns>
        public static IEnumerable<StatementSyntax> RemoveFromParent(string variableName, ForeignIndexElement foreignIndexElement)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var parentRowName = $"old{foreignIndexElement.UniqueParentName}";
            var statements = new List<StatementSyntax>
            {
                //            var thing = this.DataModel.Things.Find(order.ThingCode);
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
                                                foreignIndexElement.GetKeyAsArguments(variableName))))))))),

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

                //            if (!parentThings.Contains(thingByChildId))
                //            {
                //                <LockParent>
                //            }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName($"parent{foreignIndexElement.UniqueIndex.Table.Name.ToPlural()}"),
                                SyntaxFactory.IdentifierName("Contains")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(parentRowName)))))),
                    SyntaxFactory.Block(RowUtilities.LockParent(foreignIndexElement, parentRowName))),

                //                thing.Orders.Remove(order);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(parentRowName),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                            SyntaxFactory.IdentifierName("Remove")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(variableName)))))),

                //            this.rollbackStack.Push(() => thing.Orders.Add(order));
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("rollbackStack")),
                            SyntaxFactory.IdentifierName("Push")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ParenthesizedLambdaExpression()
                                    .WithExpressionBody(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName(parentRowName),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Add")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(variableName))))))))))),

                //            order.Thing = null;
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName)),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression))),
            };

            return statements;
        }

        /// <summary>
        /// Gets the statements that deletes a row.
        /// </summary>
        /// <returns>The statements to delete a row.</returns>
        private static IEnumerable<StatementSyntax> DeleteRow(TableElement tableElement)
        {
            // Lock the row and perform optimistic concurrency check.
            var statements = new List<StatementSyntax>
            {
                //                await lockingTransaction.WaitWriterAsync(deletedRow).ConfigureAwait(false);
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
                                                SyntaxFactory.IdentifierName("deletedRow"))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))),

                //                if (deletedRow.RowVersion != thing.RowVersion)
                //                {
                //                    throw new DBConcurrencyException();
                //                }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("deletedRow"),
                            SyntaxFactory.IdentifierName("RowVersion")),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                            SyntaxFactory.IdentifierName("RowVersion"))),
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList<StatementSyntax>(
                            SyntaxFactory.ThrowStatement(
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName("DBConcurrencyException"))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList()))))),
            };

            // Enforce referential integrity constraints.
            foreach (ForeignIndexElement foreignIndexElement in tableElement.ChildIndices)
            {
                statements.Add(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("deletedRow"),
                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                SyntaxFactory.IdentifierName("Any"))),
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ThrowStatement(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName("ConstraintException"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        SyntaxFactory.Literal(
                                                            $"The delete action conflicted with the constraint {foreignIndexElement.Name}")))))))))));
            }

            // Delete this row from each of the parent rows.
            foreach (ForeignIndexElement foreignIndexElement in tableElement.ParentIndices)
            {
                // If any of the index elements are nullable, we'll want a test to see if values are provided for all elements of the key.
                if (foreignIndexElement.Columns.Where(cre => cre.Column.ColumnType.IsNullable).Any())
                {
                    //                    if (foundRow.ModelId != null)
                    //                    {
                    //                    }
                    statements.Add(
                        SyntaxFactory.IfStatement(
                            foreignIndexElement.GetKeyAsInequalityConditional(tableElement.Name.ToVariableName(), null),
                            SyntaxFactory.Block(RowUtilities.RemoveFromParent(tableElement.Name.ToVariableName(), foreignIndexElement))));
                }
                else
                {
                    statements.AddRange(RowUtilities.RemoveFromParent(tableElement.Name.ToVariableName(), foreignIndexElement));
                }
            }

            // Delete the row from the table.
            statements.AddRange(
                new StatementSyntax[]
                {
                    //            this.dictionary.Remove((order.ThingCode, order.AssetCode, order.Date));
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
                                    tableElement.PrimaryIndex.GetKeyAsArguments("deletedRow"))))),

                    //            this.rollbackStack.Push(() => this.dictionary.Add((order.ThingCode, order.AssetCode, order.Date), order));
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("rollbackStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithExpressionBody(
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
                                                            tableElement.PrimaryIndex.GetKeyAsArguments("deletedRow"),
                                                            SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                            SyntaxFactory.Argument(
                                                                SyntaxFactory.IdentifierName("deletedRow")),
                                                        }))))))))),

                    //            deletedRow.RowVersion = this.DataModel.IncrementRowVersion();
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("deletedRow"),
                                SyntaxFactory.IdentifierName("RowVersion")),
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName(tableElement.Document.Name.ToCamelCase())),
                                    SyntaxFactory.IdentifierName("IncrementRowVersion"))))),

                    //                this.commitStack.Push(() => this.DeletedRows.AddFirst(allocation));
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("commitStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithExpressionBody(
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
                                                            SyntaxFactory.IdentifierName("deletedRow"))))))))))),

                    //            this.commitStack.Push(() => this.RowChanged?.Invoke(this, new RowChangedEventArgs<Thing>(DataAction.Add, thing)));
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("commitStack")),
                                SyntaxFactory.IdentifierName("Push")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.ParenthesizedLambdaExpression()
                                        .WithExpressionBody(
                                            SyntaxFactory.ConditionalAccessExpression(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("RowChanged")),
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
                                                                            SyntaxFactory.Identifier("RowChangedEventArgs"))
                                                                        .WithTypeArgumentList(
                                                                            SyntaxFactory.TypeArgumentList(
                                                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                                                    SyntaxFactory.IdentifierName(tableElement.Name)))))
                                                                    .WithArgumentList(
                                                                        SyntaxFactory.ArgumentList(
                                                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                                                new SyntaxNodeOrToken[]
                                                                                {
                                                                                    SyntaxFactory.Argument(
                                                                                        SyntaxFactory.MemberAccessExpression(
                                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                                            SyntaxFactory.IdentifierName("DataAction"),
                                                                                            SyntaxFactory.IdentifierName("Delete"))),
                                                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                                                    SyntaxFactory.Argument(
                                                                                        SyntaxFactory.IdentifierName("deletedRow")),
                                                                                })))),
                                                            })))))))))),
                });

            return statements;
        }

        /// <summary>
        /// Gets the constraint check for a non-nullable index.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="variableName">The variable name.</param>
        /// <returns>The code for a constraint check.</returns>
        private static IEnumerable<StatementSyntax> GetNonNullableConstraintCheck(ForeignIndexElement foreignIndexElement, string variableName)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var tableElement = foreignIndexElement.Table;
            var parentRowName = $"new{foreignIndexElement.UniqueParentName}";
            return new List<StatementSyntax>
            {
                //            var thing = this.DataModel.Things.Find(order.ThingCode);
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
                                                    tableElement.Name.ToVariableName()))))))))),

                //            if (thing == null)
                //            {
                //                throw new ConstraintException("The add action conflicted with the constraint ThingIndex.");
                //            }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.IdentifierName(parentRowName),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList<StatementSyntax>(
                            SyntaxFactory.ThrowStatement(
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName("ConstraintException"))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(
                                                        $"The add action conflicted with the constraint {foreignIndexElement.Name}.")))))))))),

                //            if (!parentThings.Contains(thingByChildId))
                //            {
                //                <LockParent>
                //            }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName($"parent{foreignIndexElement.UniqueIndex.Table.Name.ToPlural()}"),
                                SyntaxFactory.IdentifierName("Contains")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(parentRowName)))))),
                    SyntaxFactory.Block(
                        RowUtilities.LockParent(foreignIndexElement, parentRowName))),

                //            thing.Orders.Add(order);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(parentRowName),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                            SyntaxFactory.IdentifierName("Add")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(variableName)))))),

                //            this.rollbackStack.Push(() => thing.Orders.Remove(order));
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("rollbackStack")),
                            SyntaxFactory.IdentifierName("Push")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ParenthesizedLambdaExpression()
                                    .WithExpressionBody(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName(parentRowName),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Remove")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(variableName))))))))))),

                //            order.Thing = thing;
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(foreignIndexElement.UniqueParentName)),
                        SyntaxFactory.IdentifierName(parentRowName))),
            };
        }

        /// <summary>
        /// Gets the constraint check for a nullable index.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="variableName">The variable name.</param>
        /// <returns>The code for a constraint check.</returns>
        private static IEnumerable<StatementSyntax> GetNullableConstraintCheck(ForeignIndexElement foreignIndexElement, string variableName)
        {
            var uniqueIndexElement = foreignIndexElement.UniqueIndex;
            var tableElement = foreignIndexElement.Table;
            var parentRowName = $"new{foreignIndexElement.UniqueParentName}";
            var statements = new List<StatementSyntax>
            {
                //            var thing = this.DataModel.Things.Find(order.ThingCode);
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
                                                    tableElement.Name.ToVariableName()))))))))),

                //            if (thing == null)
                //            {
                //                throw new ConstraintException("The add action conflicted with the constraint ThingIndex.");
                //            }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.IdentifierName(parentRowName),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList<StatementSyntax>(
                            SyntaxFactory.ThrowStatement(
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName("ConstraintException"))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    SyntaxFactory.Literal(
                                                        $"The add action conflicted with the constraint {foreignIndexElement.Name}.")))))))))),

                //            if (!parentThings.Contains(thingByChildId))
                //            {
                //                <LockParent>
                //            }
                SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName($"parent{foreignIndexElement.UniqueIndex.Table.Name.ToPlural()}"),
                                SyntaxFactory.IdentifierName("Contains")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.IdentifierName(parentRowName)))))),
                    SyntaxFactory.Block(
                        RowUtilities.LockParent(foreignIndexElement, parentRowName))),

                //            thing.Orders.Add(order);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(parentRowName),
                                SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                            SyntaxFactory.IdentifierName("Add")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(variableName)))))),

                //            this.rollbackStack.Push(() => thing.Orders.Remove(order));
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("rollbackStack")),
                            SyntaxFactory.IdentifierName("Push")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.ParenthesizedLambdaExpression()
                                    .WithExpressionBody(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName(parentRowName),
                                                    SyntaxFactory.IdentifierName(foreignIndexElement.UniqueChildName)),
                                                SyntaxFactory.IdentifierName("Remove")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.IdentifierName(variableName))))))))))),

                //            order.Thing = thing;
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(variableName),
                            SyntaxFactory.IdentifierName(uniqueIndexElement.Table.Name)),
                        SyntaxFactory.IdentifierName(parentRowName))),
            };

            //            if (thing.BrokerFeedId != null)
            //            {
            //                  <Nullable Constraint Check>
            //            }
            return new StatementSyntax[]
            {
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName(tableElement.Name.ToVariableName()),
                            SyntaxFactory.IdentifierName(foreignIndexElement.Columns[0].Column.Name)),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(statements)),
            };
        }

        /// <summary>
        /// Locks the parent table.
        /// </summary>
        /// <param name="foreignIndexElement">The foreign index element.</param>
        /// <param name="variableName">The variable name.</param>
        /// <returns>The statements to lock the parent.</returns>
        private static IEnumerable<StatementSyntax> LockParent(ForeignIndexElement foreignIndexElement, string variableName)
        {
            return new StatementSyntax[]
            {
                //                parentThings.Add(thingByChildId);
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName($"parent{foreignIndexElement.UniqueIndex.Table.Name.ToPlural()}"),
                            SyntaxFactory.IdentifierName("Add")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.IdentifierName(variableName)))))),

                //                await lockingTransaction.WaitWriterAsync(thingByChildId).ConfigureAwait(false);
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
                                                SyntaxFactory.IdentifierName(variableName))))),
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
}