// <copyright file="CodeAddRecordToDeleteTransactionExpression.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System.CodeDom;

	/// <summary>
	/// Creates a statement method invocation that adds a record to an ADO transaction.
	/// </summary>
	class CodeAddRecordToDeleteTransactionExpression : CodeMethodInvokeExpression
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeAddRecordToDeleteTransactionExpression"/> class.
		/// </summary>
		/// <param name="transactionExpression">The MiddleTierContext used for the transaction.</param>
		/// <param name="columnSchema">The record that is held for the duration of the transaction.</param>
		public CodeAddRecordToDeleteTransactionExpression(CodeVariableReferenceExpression transactionExpression, CodeExpression rowExpression)
		{
            //			q67.AddRecord(u68.CommitAddRow, u68.RollbackAddRow);
            this.Method = new CodeMethodReferenceExpression(transactionExpression, "AddRecord");
			this.Parameters.Add(new CodeMethodReferenceExpression(rowExpression, "CommitDeleteRow"));
            this.Parameters.Add(new CodeMethodReferenceExpression(rowExpression, "RollbackDeleteRow"));
        }
	}
}
