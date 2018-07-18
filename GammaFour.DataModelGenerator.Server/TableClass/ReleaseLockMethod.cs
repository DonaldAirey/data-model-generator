// <copyright file="ReleaseLockMethod.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.TableClass
{
    using System;
    using System.CodeDom;
    using System.Threading;

    /// <summary>
    /// Creates a method that releases all locks.
    /// </summary>
    public class ReleaseLockMethod : CodeMemberMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseLockMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The table schema.</param>
        public ReleaseLockMethod(TableSchema tableSchema)
        {
            //        /// <summary>
            //        /// Releases every lock held by this record.
            //        /// </summary>
            //        /// <param name="transactionId">The unique identifier of the transaction.</param>
            //        public virtual void ReleaseLock(Guid transactionId)
            //        {
            this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Releases every lock held by this record.", true));
            this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.Comments.Add(new CodeCommentStatement("<param name=\"transactionId\">The unique transaction identifier.</param>", true));
            this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            this.Name = "ReleaseLock";
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

            //                if (this.readers.Remove(transactionId) && this.readers.Count == 0)
            //                {
            CodeConditionStatement ifAllReadersReleased = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readers"),
                        "Remove",
                        transactionId),
                        CodeBinaryOperatorType.BooleanAnd,
                        new CodeBinaryOperatorExpression(
                            new CodePropertyReferenceExpression(
                                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "readers"),
                                "Count"),
                            CodeBinaryOperatorType.IdentityEquality,
                            new CodePrimitiveExpression(0))));

            //                    if (this.writerWaiters != 0)
            //                    {
            CodeConditionStatement ifNoWritersWaiting = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writerWaiters"),
                    CodeBinaryOperatorType.IdentityInequality,
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

            //                        }
            ifNoWritersWaiting.TrueStatements.Add(tryPulsingWriter);

            //                    }
            ifAllReadersReleased.TrueStatements.Add(ifNoWritersWaiting);

            //                }
            trySyncRootLock.TryStatements.Add(ifAllReadersReleased);

            //                if (this.writer == transactionId)
            //                {
            CodeConditionStatement ifWriterReleased = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writer"),
                    CodeBinaryOperatorType.IdentityEquality,
                    transactionId));

            //                    this.writer = Guid.Empty;
            ifWriterReleased.TrueStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writer"),
                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(Guid)), "Empty")));

            //                    if (this.readerWaiters > 0)
            //                    {
            CodeConditionStatement ifNoReadersWaiting = new CodeConditionStatement(
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
            ifNoReadersWaiting.TrueStatements.Add(tryPulsingReader);

            //                    else
            //                    {
            //                        if (this.writerWaiters > 0)
            //                        {
            CodeConditionStatement ifWritersWaiting = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "writerWaiters"),
                    CodeBinaryOperatorType.GreaterThan,
                    new CodePrimitiveExpression(0)));

            //                            try
            //                            {
            //                                Monitor.Enter(this.writeRoot);
            //                                Monitor.Pulse(this.writeRoot);
            //                            }
            //                            finally
            //                            {
            //                                Monitor.Exit(this.writeRoot);
            //                            }
            ifWritersWaiting.TrueStatements.Add(tryPulsingWriter);

            //                        }
            ifNoReadersWaiting.FalseStatements.Add(ifWritersWaiting);

            //                    }
            ifWriterReleased.TrueStatements.Add(ifNoReadersWaiting);

            //                }
            trySyncRootLock.TryStatements.Add(ifWriterReleased);

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