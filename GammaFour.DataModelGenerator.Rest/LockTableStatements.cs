// <copyright file="LockTableStatements.cs" company="Theta Rex, Inc.">
//    Copyright © 2020 - Theta Rex, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class LockTableStatements
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <param name="bodyStatements">The statements that appear in the body of the using block.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static List<StatementSyntax> GetSyntax(TableElement tableElement, IEnumerable<StatementSyntax> bodyStatements)
        {
            // Validate the argument.
            if (tableElement == null)
            {
                throw new ArgumentNullException(nameof(tableElement));
            }

            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //            using (TransactionScope transactionScope = new TransactionScope())
            UsingStatementSyntax usingStatement = SyntaxFactory.UsingStatement(
                SyntaxFactory.Block(bodyStatements))
                .WithDeclaration(
                    SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName("var"))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                            SyntaxFactory.VariableDeclarator(
                                SyntaxFactory.Identifier("transactionScope"))
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.ObjectCreationExpression(
                                        SyntaxFactory.IdentifierName("TransactionScope"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("TransactionScopeOption"),
                                                            SyntaxFactory.IdentifierName("RequiresNew"))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName("transactionTimeout"))),
                                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.IdentifierName("TransactionScopeAsyncFlowOption"),
                                                            SyntaxFactory.IdentifierName("Enabled"))),
                                                }))))))));

            //             using (DisposableList disposables = new DisposableList())
            usingStatement = SyntaxFactory.UsingStatement(usingStatement)
            .WithDeclaration(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.IdentifierName("var"))
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier("disposables"))
                        .WithInitializer(
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.ObjectCreationExpression(
                                    SyntaxFactory.IdentifierName("DisposableList"))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList()))))));

            //            using (await this.domain.Accounts.Lock.WriteLockAsync())
            //            using (await this.domain.Accounts.AccountExternalKey.Lock.WriteLockAsync())
            //            using (await this.domain.Accounts.AccountKey.Lock.WriteLockAsync())
            //            using (await this.domain.Accounts.EntityAccountKey.Lock.WriteLockAsync())
            foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys.AsEnumerable().Reverse())
            {
                //            using (await this.Accounts.EntityAccountKey.Lock.WriteLockAsync())
                usingStatement = SyntaxFactory.UsingStatement(usingStatement)
                    .WithExpression(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                            SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name.ToPlural())),
                                        SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("WriteLockAsync")))));
            }

            // Lock each of the unique key indices
            foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys.AsEnumerable().Reverse())
            {
                //            using (await this.domain.Accounts.AccountKey.Lock.WriteLockAsync())
                usingStatement = SyntaxFactory.UsingStatement(usingStatement)
                    .WithExpression(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("WriteLockAsync")))));
            }

            // Finally, lock the table.
            //            using (await this.domain.Accounts.Lock.WriteLockAsync())
            usingStatement = SyntaxFactory.UsingStatement(usingStatement)
                .WithExpression(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("WriteLockAsync")))));

            // This puts all the other 'using' statements into a big block with the transaction as the first item.  This order also insures that
            // the transaction is also the last to be disposed.
            //            using (await this.Accounts.Lock.WriteLockAsync())
            //            using (await this.Accounts.AccountExternalKey.Lock.WriteLockAsync())
            //            using (await this.ManagedAccounts.AccountExternalKey.Lock.WriteLockAsync())
            //            using (DisposableList disposables = new DisposableList())
            //            using (TransactionScope transactionScope = new TransactionScope())
            //            {
            //                <LoadDomain>
            //            }
            statements.Add(usingStatement);

            // This set of statement will enlist the indices and tables in the current transaction and acquire an exclusive lock.
            return statements;
        }
    }
}