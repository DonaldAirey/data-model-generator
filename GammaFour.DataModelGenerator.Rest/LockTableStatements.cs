// <copyright file="LockTableStatements.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class LockTableStatements
    {
        /// <summary>
        /// Gets the syntax for the locking the resources of a table for a write operation.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <returns>An expression that locks all the resources related to the given table element.</returns>
        public static List<StatementSyntax> GetSyntax(TableElement tableElement)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //            using var lockingTransaction = new LockingTransaction(this.transactionTimeout);
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
                                                        SyntaxFactory.IdentifierName("transactionTimeout")))))))))))
                .WithUsingKeyword(
                    SyntaxFactory.Token(SyntaxKind.UsingKeyword)),

                //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts).ConfigureAwait(false);
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
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))),
            };

            //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts.AccountKey).ConfigureAwait(false);
            //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts.AccountSymbolKey).ConfigureAwait(false);
            foreach (UniqueElement uniqueElement in tableElement.UniqueKeys.OrderBy(ue => ue.Name))
            {
                statements.Add(
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
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                            SyntaxFactory.IdentifierName(uniqueElement.Table.Name.ToPlural())),
                                                        SyntaxFactory.IdentifierName(uniqueElement.Name)))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts.ItemAccountKey).ConfigureAwait(false);
            foreach (ForeignElement foreignElement in tableElement.ParentKeys.OrderBy(fe => fe.Name))
            {
                statements.Add(
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
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                            SyntaxFactory.IdentifierName(foreignElement.Table.Name.ToPlural())),
                                                        SyntaxFactory.IdentifierName(foreignElement.Name)))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            // This set of statement will enlist the indices and tables in the current transaction and acquire an exclusive lock.
            return statements;
        }

        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <param name="usingBlock">The block of code inside the using statement.</param>
        /// <returns>An expression that locks all the resources related to the given table element.</returns>
        public static List<StatementSyntax> GetUsingSyntax(TableElement tableElement, List<StatementSyntax> usingBlock)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //                using (var lockingTransaction = new LockingTransaction(this.transactionTimeout))
                //                {
                //                    <UsingBody>
                //                }
                SyntaxFactory.UsingStatement(
                    SyntaxFactory.Block(LockTableStatements.UsingBody(tableElement, usingBlock)))
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
                                                             SyntaxFactory.IdentifierName("transactionTimeout"))))))))))),
            };

            // This set of statement will enlist the indices and tables in the current transaction and acquire an exclusive lock.
            return statements;
        }

        /// <summary>
        /// Gets the body of the using statement.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <param name="usingBlock">The block of code inside the using statement.</param>
        private static List<StatementSyntax> UsingBody(TableElement tableElement, List<StatementSyntax> usingBlock)
        {
            // The elements of the body are added to this collection as they are assembled.
            List<StatementSyntax> statements = new List<StatementSyntax>
            {
                //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts).ConfigureAwait(false);
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
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                    SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))),
            };

            // Alphabetize the unique and foreign indices so we can order them alphabetically.
            var names = new List<(string, object)>();
            foreach (UniqueElement uniqueElement in tableElement.UniqueKeys.OrderBy(ue => ue.Name))
            {
                names.Add((uniqueElement.Name, uniqueElement));
            }

            foreach (ForeignElement foreignElement in tableElement.ParentKeys.OrderBy(fe => fe.Name))
            {
                names.Add((foreignElement.Name, foreignElement));
            }

            //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts.AccountKey).ConfigureAwait(false);
            //            await lockingTransaction.WaitWriterAsync(this.dataModel.Accounts.AccountSymbolKey).ConfigureAwait(false);
            foreach (var name in names.OrderBy(n => n.Item1))
            {
                //                     await lockingTransaction.WaitWriterAsync(this.dataModel.Positions.PositionIndex).ConfigureAwait(false);
                if (name.Item2 is UniqueElement uniqueElement)
                {
                    statements.Add(
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
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.ThisExpression(),
                                                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                                SyntaxFactory.IdentifierName(uniqueElement.Table.Name.ToPlural())),
                                                            SyntaxFactory.IdentifierName(uniqueElement.Name)))))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))));
                }

                //                    await lockingTransaction.WaitWriterAsync(this.dataModel.Positions.AccountPositionIndex).ConfigureAwait(false);
                //                    await lockingTransaction.WaitWriterAsync(this.dataModel.Positions.AssetPositionIndex).ConfigureAwait(false);
                if (name.Item2 is ForeignElement foreignElement)
                {
                    statements.Add(
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
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.ThisExpression(),
                                                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.DataModel.ToVariableName())),
                                                                SyntaxFactory.IdentifierName(foreignElement.Table.Name.ToPlural())),
                                                            SyntaxFactory.IdentifierName(foreignElement.Name)))))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))));
                }
            }

            // Add the reset of the using body.
            statements.AddRange(usingBlock);

            // This is the syntax for the body of the method.
            return statements;
        }
    }
}