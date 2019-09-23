// <copyright file="EnlistAndLockStatements.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
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
    public static class EnlistAndLockStatements
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <param name="lockParentKeys">Indicates that the unique, non-primary keys of the parent should be locked also.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static List<StatementSyntax> GetSyntax(TableElement tableElement, bool lockParentKeys = false)
        {
            // Validate the argument.
            if (tableElement == null)
            {
                throw new ArgumentNullException(nameof(tableElement));
            }

            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                    this.domain.Provinces.Enlist();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToVariableName())),
                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                            SyntaxFactory.IdentifierName("Enlist")))));

            //                    this.domain.Provinces.ProvinceExternalKey.Enlist();
            //                    this.domain.Provinces.ProvinceKey.Enlist();
            foreach (UniqueKeyElement innerUniqueKeyElement in tableElement.UniqueKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
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
                                            SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToVariableName())),
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName(innerUniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Enlist")))));
            }

            //                    this.domain.Provinces.CountryProvinceKey.Enlist();
            //                    this.domain.Provinces.RegionProvinceRegionIdKey.Enlist();
            foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
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
                                            SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToVariableName())),
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Enlist")))));
            }

            //                    await this.domain.Provinces.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
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
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToVariableName())),
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("Lock")),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("lockTimeout")))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))));

            //                    await this.domain.Provinces.ProvinceExternalKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            //                    await this.domain.Provinces.ProvinceKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            foreach (UniqueKeyElement innerUniqueKeyElement in tableElement.UniqueKeys)
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
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(innerUniqueKeyElement.XmlSchemaDocument.Name.ToVariableName())),
                                                        SyntaxFactory.IdentifierName(innerUniqueKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(innerUniqueKeyElement.Name)),
                                                SyntaxFactory.IdentifierName("Lock")),
                                            SyntaxFactory.IdentifierName("EnterWriteLockAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("lockTimeout")))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            //                    await this.domain.Provinces.CountryProvinceKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            //                    await this.domain.Provinces.RegionProvinceRegionIdKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
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
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(foreignKeyElement.XmlSchemaDocument.Name.ToVariableName())),
                                                        SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                                SyntaxFactory.IdentifierName("Lock")),
                                            SyntaxFactory.IdentifierName("EnterWriteLockAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("lockTimeout")))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            // This is a distinct list of all the parent tables.
            var parentTables = from pk in tableElement.ParentKeys
                               group pk.UniqueKey.Table by pk.UniqueKey.Table into g
                               select g.First();

            //                    await this.domain.Countries.CountryKey.Lock.EnterReadLockAsync(this.lockTimeout).ConfigureAwait(false);
            //                    await this.domain.Regions.RegionKey.Lock.EnterReadLockAsync(this.lockTimeout).ConfigureAwait(false);
            foreach (TableElement parentTable in parentTables)
            {
                foreach (UniqueKeyElement uniqueKeyElement in parentTable.UniqueKeys)
                {
                    if (!uniqueKeyElement.IsPrimaryKey && !lockParentKeys)
                    {
                        continue;
                    }

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
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.ThisExpression(),
                                                                SyntaxFactory.IdentifierName(uniqueKeyElement.XmlSchemaDocument.Name.ToVariableName())),
                                                            SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                                        SyntaxFactory.IdentifierName(uniqueKeyElement.Name)),
                                                    SyntaxFactory.IdentifierName("Lock")),
                                                SyntaxFactory.IdentifierName("EnterReadLockAsync")))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName("lockTimeout")))))),
                                        SyntaxFactory.IdentifierName("ConfigureAwait")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.FalseLiteralExpression))))))));
                }
            }

            // This set of statement will enlist the indices and tables in the current transaction and acquire an exclusive lock.
            return statements;
        }
    }
}