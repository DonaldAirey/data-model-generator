// <copyright file="ReleaseWriterLockMethod.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.RowClass
{
    using System;
    using System.CodeDom;
    using System.Threading;

    /// <summary>
    /// Creates a method that releases all locks.
    /// </summary>
    public class ReleaseWriterLockMethod : CodeMemberMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseWriterLockMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The table schema.</param>
        public ReleaseWriterLockMethod(TableSchema tableSchema)
        {
            //        /// <summary>
            //        /// Releases the writer lock on this record.
            //        /// </summary>
            //        /// <param name="transactionId">The unique identifier of the transaction.</param>
            //        public virtual void ReleaseWriterLock(Guid transactionId)
            //        {
            this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Releases the writer lock on this record.", true));
            this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.Comments.Add(new CodeCommentStatement("<param name=\"transactionId\">The unique transaction identifier.</param>", true));
            this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            this.Name = "ReleaseWriterLock";
            CodeArgumentReferenceExpression transactionId = new CodeArgumentReferenceExpression("transactionId");
            this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Guid)), transactionId.ParameterName));

            //            try
            //            {
            CodeTryCatchFinallyStatement trySyncRootLock = new CodeTryCatchFinallyStatement();

            //                Monitor.Enter(this.syncRoot);
            trySyncRootLock.TryStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Enter",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "syncRoot")));

            //                if (this.writer != transactionId)
            //                {
            CodeConditionStatement ifWriterRemoved = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writer"),
                    CodeBinaryOperatorType.IdentityInequality,
                    transactionId));
            
            //                    throw new FaultException<SynchronizationLockFault>(new SynchronizationLockFault(this.Table.TableName));
            ifWriterRemoved.TrueStatements.Add(new CodeThrowSynchronizationExceptionStatement(tableSchema));

            //                }
            trySyncRootLock.TryStatements.Add(ifWriterRemoved);
            
            //                    this.writer = Guid.Empty;
            trySyncRootLock.TryStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writer"),
                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(Guid)), "Empty")));

            //                    if (this.readerWaiters > 0)
            //                    {
            CodeConditionStatement ifReadersWaiting = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readerWaiters"),
                    CodeBinaryOperatorType.GreaterThan,
                    new CodePrimitiveExpression(0)));

            //                        try
            //                        {
            CodeTryCatchFinallyStatement tryPulsingReader = new CodeTryCatchFinallyStatement();

            //                            Monitor.Enter(this.readRoot);
            tryPulsingReader.TryStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Enter",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readRoot")));

            //                            Monitor.PulseAll(this.readRoot);
            tryPulsingReader.TryStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "PulseAll",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readRoot")));

            //                        }
            //                        finally
            //                        {
            //                            Monitor.Exit(this.readRoot);
            //                        }
            tryPulsingReader.FinallyStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Exit",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readRoot")));
            
            //                    }
            ifReadersWaiting.TrueStatements.Add(tryPulsingReader);

            //                    else
            //                    {
            //                        if (this.writerWaiters > 0)
            //                        {
            CodeConditionStatement ifWritersWaiting = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writerWaiters"),
                    CodeBinaryOperatorType.GreaterThan,
                    new CodePrimitiveExpression(0)));

            //                        try
            //                        {
            CodeTryCatchFinallyStatement tryPulsingWriter = new CodeTryCatchFinallyStatement();

            //                            Monitor.Enter(this.writeRoot);
            tryPulsingWriter.TryStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Enter",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writeRoot")));

            //                            Monitor.Pulse(this.writeRoot);
            tryPulsingWriter.TryStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Pulse",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writeRoot")));

            //                        }
            //                        finally
            //                        {
            //                            Monitor.Exit(this.writeRoot);
            tryPulsingWriter.FinallyStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Exit",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writeRoot")));

            //                    }
            ifWritersWaiting.TrueStatements.Add(tryPulsingWriter);

            ifReadersWaiting.FalseStatements.Add(ifWritersWaiting);

            //                }
            trySyncRootLock.TryStatements.Add(ifReadersWaiting);

            //            }
            //            finally
            //            {

            //                Monitor.Exit(this.syncRoot);
            trySyncRootLock.FinallyStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression(typeof(Monitor)),
                    "Exit",
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "syncRoot")));

            //            }
            this.Statements.Add(trySyncRootLock);

            //        }
        }
    }
}