// <copyright file="EnlistTableStatements.cs" company="Gamma Four, Inc.">
//    Copyright © 2021 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System;
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class EnlistTableStatements
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <returns>A set of expression that enlists the table in a transaction.</returns>
        public static List<StatementSyntax> GetSyntax(TableElement tableElement)
        {
            // Validate the argument.
            if (tableElement == null)
            {
                throw new ArgumentNullException(nameof(tableElement));
            }

            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                transaction.EnlistVolatile(this.domain.Accounts, EnlistmentOptions.None);
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("transaction"),
                            SyntaxFactory.IdentifierName("EnlistVolatile")))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.ThisExpression(),
                                                SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                            SyntaxFactory.IdentifierName(tableElement.Name.ToPlural()))),
                                    SyntaxFactory.Token(SyntaxKind.CommaToken),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("EnlistmentOptions"),
                                            SyntaxFactory.IdentifierName("None"))),
                                })))));

            // Enlist each of the unique key indices in this transaction.
            foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
            {
                //                transaction.EnlistVolatile(this.domain.Accounts.AccountExternalKey, EnlistmentOptions.None);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("transaction"),
                                SyntaxFactory.IdentifierName("EnlistVolatile")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                                    SyntaxFactory.IdentifierName(uniqueKeyElement.Table.Name.ToPlural())),
                                                SyntaxFactory.IdentifierName(uniqueKeyElement.Name))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("EnlistmentOptions"),
                                                SyntaxFactory.IdentifierName("None"))),
                                    })))));
            }

            // Enlist each of the foreign key indices in this transaction.
            foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
            {
                //                transaction.EnlistVolatile(this.domain.Accounts.EntityAccountKey, EnlistmentOptions.None);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("transaction"),
                                SyntaxFactory.IdentifierName("EnlistVolatile")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Domain.ToVariableName())),
                                                    SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name.ToPlural())),
                                                SyntaxFactory.IdentifierName(foreignKeyElement.Name))),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("EnlistmentOptions"),
                                                SyntaxFactory.IdentifierName("None"))),
                                    })))));
            }

            // This set of statement will enlist the indices and tables in the current transaction and acquire an exclusive lock.
            return statements;
        }
    }
}