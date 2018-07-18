// <copyright file="AcquireWriterLockITransactionMethod.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.RowClass
{
    using System;
    using System.CodeDom;
    using System.Globalization;
    using DarkBond.ServiceModel;

    /// <summary>
    /// Creates a method that acquires a writer lock.
    /// </summary>
    public class AcquireWriterLockITransactionMethod : CodeMemberMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcquireWriterLockITransactionMethod"/> class.
        /// </summary>
        /// <param name="relationSchema">The table schema.</param>
        public AcquireWriterLockITransactionMethod(TableSchema tableSchema)
        {
            //        /// <summary>
            //        /// Acquires a writer lock for this record and add the row to the given transaction.
            //        /// </summary>
            //        /// <param name="iTransaction">The transaction context for this operation.</param>
            //        public virtual void AcquireWriterLock(ITransaction iTransaction)
            //        {
            this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Acquires a writer lock for this record and adds the row to the given transaction.", true));
            this.Comments.Add(new CodeCommentStatement("</summary>", true));
            this.Comments.Add(new CodeCommentStatement("<param name=\"transaction\">The transaction context for this operation.</param>", true));
            this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            this.Name = "AcquireWriterLock";
            CodeArgumentReferenceExpression transaction = new CodeArgumentReferenceExpression(
                String.Format(CultureInfo.InvariantCulture, "{0}Transaction", CommonConversion.ToCamelCase(tableSchema.DataModel.Name)));
            this.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(String.Format("{0}Transaction", tableSchema.DataModel.Name)),
                    transaction.ParameterName));

            //            if (iTransaction == null)
            //            {
            CodeConditionStatement ifNoTransaction = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    transaction,
                    CodeBinaryOperatorType.IdentityEquality,
                    new CodePrimitiveExpression(null)));

            //                throw new ArgumentNullException("iTransaction");
            ifNoTransaction.TrueStatements.Add(
                new CodeThrowExceptionStatement(
                    new CodeObjectCreateExpression(
                        new CodeTypeReference(typeof(ArgumentNullException)),
                        new CodePrimitiveExpression("iTransaction"))));

            //            }
            this.Statements.Add(ifNoTransaction);

            //            iTransaction.AddLock(this);
            this.Statements.Add(
                new CodeMethodInvokeExpression(
                    transaction,
                    "AddLock",
                    new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "AcquireWriterLock"),
                    new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "ReleaseLock")));

            //            if (this.Table == null)
            //            {
            CodeConditionStatement ifTableDetached = new CodeConditionStatement(
                new CodeBinaryOperatorExpression(
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Table"),
                    CodeBinaryOperatorType.IdentityEquality,
                    new CodePrimitiveExpression(null)));

            //                throw new LockException("Row in table {0} deleted after lock");
            ifTableDetached.TrueStatements.Add(
                new CodeThrowExceptionStatement(
                    new CodeObjectCreateExpression(
                        new CodeTypeReference(typeof(LockException)),
                        new CodePrimitiveExpression(String.Format(CultureInfo.InvariantCulture, "Row in table {0} deleted after lock.", tableSchema.Name)))));

            //            }
            this.Statements.Add(ifTableDetached);

            //        }
        }
    }
}