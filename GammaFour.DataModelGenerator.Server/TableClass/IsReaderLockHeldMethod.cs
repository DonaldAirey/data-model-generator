// <copyright file="IsReaderLockHeldMethod.cs" company="Dark Bond, Inc.">
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
    public class IsReaderLockHeldMethod : CodeMemberMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsReaderLockHeldMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The table schema.</param>
        public IsReaderLockHeldMethod(TableSchema tableSchema)
        {
            //        /// <summary>
            //        /// Gets a value indicating whether the owner of a token holds a reader lock.
            //        /// </summary>
            //        /// <param name="transactionId">The unique identifier of the transaction.</param>
            //        /// <returns>true if the current token owner holds a reader lock.</returns>
            //        public Boolean IsReaderLockHeld(Guid transactionId)
            //        {
            this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Gets a value indicating whether the owner of a token holds a reader lock.", true));
            this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.Comments.Add(new CodeCommentStatement("<param name=\"transactionId\">The unique identifier of the transaction.</param>", true));
            this.Comments.Add(new CodeCommentStatement("<returns>true if the current token owner holds a reader lock.</returns>", true));
            this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            this.ReturnType = new CodeTypeReference(typeof(Boolean));
            this.Name = "IsReaderLockHeld";
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

            //                return this.readers.Contains(transactionId);
            tryLock.TryStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readers"),
                        "Contains",
                        transactionId)));

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