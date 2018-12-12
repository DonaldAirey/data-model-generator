// <copyright file="AcquireWriteLockExpression.cs" company="Gamma Four, Inc.">
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
    public static class AcquireWriteLockExpression
    {
        /// <summary>
        /// Gets the statement that acquires a read lock.
        /// </summary>
        /// <returns>A statement that acquires a read lock.</returns>
        public static StatementSyntax Statement
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
                SyntaxFactory.Block(AcquireWriteLockExpression.IfLockNotHeld));
            }
        }

        /// <summary>
        /// Gets the statements acquires the read lock.
        /// </summary>
        private static List<StatementSyntax> IfLockNotHeld
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
    }
}