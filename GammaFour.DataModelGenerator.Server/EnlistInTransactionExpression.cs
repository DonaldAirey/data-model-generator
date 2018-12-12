// <copyright file="EnlistInTransactionExpression.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a statement to enlist in a transcation.
    /// </summary>
    public static class EnlistInTransactionExpression
    {
        /// <summary>
        /// Gets the statement that enlists a resource in a transaction.
        /// </summary>
        /// <returns>A statement that enlists a resource in a transaction.</returns>
        public static StatementSyntax Statement
        {
            get
            {
                //            if (Transaction.Current != null)
                //            {
                //                <Enlist in Transaction>
                //            }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.NotEqualsExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("Transaction"),
                            SyntaxFactory.IdentifierName("Current")),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    SyntaxFactory.Block(EnlistInTransactionExpression.IfTransactionNull));
            }
        }

        /// <summary>
        /// Gets the statements that enlist a resource in a transaction.
        /// </summary>
        private static List<StatementSyntax> IfTransactionNull
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    lock (this)
                //                    {
                statements.Add(
                    SyntaxFactory.LockStatement(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("syncRoot")),
                        SyntaxFactory.Block(EnlistInTransactionExpression.IfNewTransaction)));

                return statements;
            }
        }

        /// <summary>
        /// Gets a statement that checks to see if this resource is already part of a transaction.
        /// </summary>
        private static StatementSyntax IfNewTransaction
        {
            get
            {
                //                if (!this.transactions.Contains(Transaction.Current.TransactionInformation.LocalIdentifier))
                //                {
                //                    <Enlist In Transaction>
                //                }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("transactions")),
                                SyntaxFactory.IdentifierName("Contains")))
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
                                                    SyntaxFactory.IdentifierName("Transaction"),
                                                    SyntaxFactory.IdentifierName("Current")),
                                                SyntaxFactory.IdentifierName("TransactionInformation")),
                                            SyntaxFactory.IdentifierName("LocalIdentifier"))))))),
                    SyntaxFactory.Block(EnlistInTransactionExpression.EnlistInTransaction));
            }
        }

        /// <summary>
        /// Gets the statements that enlist a resource in a transaction.
        /// </summary>
        private static List<StatementSyntax> EnlistInTransaction
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                            this.transactions.Add(Transaction.Current.TransactionInformation.LocalIdentifier);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("transactions")),
                                SyntaxFactory.IdentifierName("Add")))
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
                                                    SyntaxFactory.IdentifierName("Transaction"),
                                                    SyntaxFactory.IdentifierName("Current")),
                                                SyntaxFactory.IdentifierName("TransactionInformation")),
                                            SyntaxFactory.IdentifierName("LocalIdentifier"))))))));

                //                            Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("Transaction"),
                                    SyntaxFactory.IdentifierName("Current")),
                                SyntaxFactory.IdentifierName("EnlistVolatile")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.ThisExpression()),
                                        SyntaxFactory.Token(SyntaxKind.CommaToken),
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName("EnlistmentOptions"),
                                                SyntaxFactory.IdentifierName("None")))
                                    })))));

                //                            Transaction.Current.TransactionCompleted += this.OnTransactionCompleted;
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(
                            SyntaxKind.AddAssignmentExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("Transaction"),
                                    SyntaxFactory.IdentifierName("Current")),
                                SyntaxFactory.IdentifierName("TransactionCompleted")),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("OnTransactionCompleted")))));

                return statements;
            }
        }
    }
}