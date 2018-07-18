// <copyright file="IsWriterLockHeldMethod.cs" company="Dark Bond, Inc.">
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
    public class IsWriterLockHeldMethod : CodeMemberMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsWriterLockHeldMethod"/> class.
        /// </summary>
        /// <param name="tableSchema">The table schema.</param>
        public IsWriterLockHeldMethod(TableSchema tableSchema)
        {
            //        /// <summary>
            //        /// Gets a value indicating whether the owner of a token holds a writer lock.
            //        /// </summary>
            //        /// <param name="transactionId">The unique identifier of the transaction.</param>
            //        /// <returns>true if the current token owner holds a writer lock.</returns>
            //        public Boolean IsWriterLockHeld(Guid transactionId)
            //        {
            this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Gets a value indicating whether the owner of a token holds any locks.", true));
            this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.Comments.Add(new CodeCommentStatement("<param name=\"transactionId\">The unique identifier of the transaction.</param>", true));
            this.Comments.Add(new CodeCommentStatement("<returns>true if the current token owner holds any locks.</returns>", true));
            this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            this.ReturnType = new CodeTypeReference(typeof(Boolean));
            this.Name = "IsWriterLockHeld";
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

            //                return this.writer == transactionId;
            tryLock.TryStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writer"),
                        CodeBinaryOperatorType.IdentityEquality,
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