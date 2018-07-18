// <copyright file="IsLockHeldMethod.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.TableClass
{
    using System;
    using System.CodeDom;
    using System.Threading;

    /// <summary>
    /// Creates a method determines if the given lock is still held.
    /// </summary>
    public class IsLockHeldMethod : CodeMemberMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsLockHeldMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The table schema.</param>
        public IsLockHeldMethod(TableSchema tableSchema)
        {
            //        /// <summary>
            //        /// Gets a value indicating whether the owner of a token holds a any locks.
            //        /// </summary>
            //        /// <param name="transactionId">The transaction unique identifier.</param>
            //        /// <returns>true if the current token owner holds a reader lock.</returns>
            //        public Boolean IsLockHeld(Guid transactionId)
            //        {
            this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Gets a value indicating whether the owner of a token holds any locks.", true));
            this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.Comments.Add(new CodeCommentStatement("<param name=\"transactionId\">The unique identifier of the transaction.</param>", true));
            this.Comments.Add(new CodeCommentStatement("<returns>true if the current token owner holds any locks.</returns>", true));
            this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            this.ReturnType = new CodeTypeReference(typeof(Boolean));
            this.Name = "IsLockHeld";
            CodeArgumentReferenceExpression transactionId = new CodeArgumentReferenceExpression("transactionId");
            this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Guid)), transactionId.ParameterName));

            //            try
            //            {
            CodeTryCatchFinallyStatement tryLock = new CodeTryCatchFinallyStatement();

            //                Monitor.Enter(this.syncRoot);
            tryLock.TryStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Enter",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "syncRoot")));

            //                return this.writer == transactionId || this.readers.Contains(transactionId);
            tryLock.TryStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeBinaryOperatorExpression(
                            new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writer"),
                            CodeBinaryOperatorType.IdentityEquality,
                            transactionId),
                    CodeBinaryOperatorType.BooleanOr,
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readers"),
                        "Contains",
                        transactionId))));

            //            }
            //            finally
            //            {
            //                Monitor.Exit(this.syncRoot);
            tryLock.FinallyStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Exit",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "syncRoot")));

            //            }
            this.Statements.Add(tryLock);

            //        }
        }
    }
}