// <copyright file="LockExpressions.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Server
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a statement to enlist in a transcation.
    /// </summary>
    public static class LockExpressions
    {
        /// <summary>
        /// Gets the statement that acquires a read lock.
        /// </summary>
        /// <returns>A statement that acquires a read lock.</returns>
        public static StatementSyntax AcquireReadLockStatement
        {
            get
            {
                //                if (!this.Lock.IsReadLockHeld && !this.Lock.IsUpgradeableReadLockHeld && !this.Lock.IsWriteLockHeld)
                //                {
                //                    <Acquire Read Lock>
                //                }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            SyntaxFactory.PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("IsReadLockHeld"))),
                            SyntaxFactory.PrefixUnaryExpression(
                                SyntaxKind.LogicalNotExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.ThisExpression(),
                                        SyntaxFactory.IdentifierName("Lock")),
                                    SyntaxFactory.IdentifierName("IsUpgradeableReadLockHeld")))),
                        SyntaxFactory.PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("IsWriteLockHeld")))),
                    SyntaxFactory.Block(LockExpressions.EnterReadLock));
            }
        }

        /// <summary>
        /// Gets the statement that acquires a read lock.
        /// </summary>
        /// <returns>A statement that acquires a read lock.</returns>
        public static StatementSyntax AcquireWriteLockStatement
        {
            get
            {
                //                if (!this.Lock.IsWriteLockHeld)
                //                {
                //                    <Acquire Write Lock>
                //                }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Lock")),
                            SyntaxFactory.IdentifierName("IsWriteLockHeld"))),
                SyntaxFactory.Block(LockExpressions.EnterWriteLock));
            }
        }

        /// <summary>
        /// Gets the statement that checks for and releases a read lock.
        /// </summary>
        /// <returns>A statement that acquires a read lock.</returns>
        public static StatementSyntax ReleaseReadLockStatement
        {
            get
            {
                //            if (this.Lock.IsReadLockHeld)
                //            {
                //                <ExitReadLock>
                //            }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("Lock")),
                        SyntaxFactory.IdentifierName("IsReadLockHeld")),
                    SyntaxFactory.Block(LockExpressions.ExitReadLock));
            }
        }

        /// <summary>
        /// Gets the statement that checks for and releases an upgradeable read lock.
        /// </summary>
        /// <returns>A statement that acquires a read lock.</returns>
        public static StatementSyntax ReleaseUpgradeableReadLockStatement
        {
            get
            {
                //            if (this.Lock.IsReadLockHeld)
                //            {
                //                <ExitReadLock>
                //            }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("Lock")),
                        SyntaxFactory.IdentifierName("IsUpgradeableReadLockHeld")),
                    SyntaxFactory.Block(LockExpressions.ExitUpgradeableReadLock));
            }
        }

        /// <summary>
        /// Gets the statement that checks for and releases an write lock.
        /// </summary>
        /// <returns>A statement that acquires a read lock.</returns>
        public static StatementSyntax ReleaseWriteLockStatement
        {
            get
            {
                //            if (this.Lock.IsReadLockHeld)
                //            {
                //                <ExitReadLock>
                //            }
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.IdentifierName("Lock")),
                        SyntaxFactory.IdentifierName("IsWriteLockHeld")),
                    SyntaxFactory.Block(LockExpressions.ExitWriteLock));
            }
        }

        /// <summary>
        /// Gets the statements acquires the read lock.
        /// </summary>
        private static List<StatementSyntax> EnterReadLock
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.Lock.EnterReadLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterReadLock")))));

                return statements;
            }
        }

        /// <summary>
        /// Gets the statements acquires the write lock.
        /// </summary>
        private static List<StatementSyntax> EnterWriteLock
        {
            get
            {
                List<StatementSyntax> statements = new List<StatementSyntax>();

                //                    this.Lock.EnterWriteLock();
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("EnterWriteLock")))));

                return statements;
            }
        }

        /// <summary>
        /// Gets a block of code that exits from a read lock.
        /// </summary>
        private static IEnumerable<StatementSyntax> ExitReadLock
        {
            get
            {
                //                this.Lock.ExitReadLock();
                return SyntaxFactory.SingletonList<StatementSyntax>(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("ExitReadLock")))));
            }
        }

        /// <summary>
        /// Gets a block of code that exits from an upgradable read lock.
        /// </summary>
        private static IEnumerable<StatementSyntax> ExitUpgradeableReadLock
        {
            get
            {
                //                this.Lock.ExitReadLock();
                return SyntaxFactory.SingletonList<StatementSyntax>(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("ExitUpgradeableReadLock")))));
            }
        }

        /// <summary>
        /// Gets a block of code exits from a write lock.
        /// </summary>
        private static IEnumerable<StatementSyntax> ExitWriteLock
        {
            get
            {
                //                this.Lock.ExitReadLock();
                return SyntaxFactory.SingletonList<StatementSyntax>(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName("Lock")),
                                SyntaxFactory.IdentifierName("ExitWriteLock")))));
            }
        }
    }
}