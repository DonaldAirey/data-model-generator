namespace DarkBond.DataModelGenerator.TenantTargetClass
{
	using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a statement method invocation that adds a record to an ADO transaction.
	/// </summary>
	class CodeAcquireRecordWriterLockExpression : CodeMethodInvokeExpression
	{
		/// <summary>
		/// Creates a statement method invocation that adds a record to an ADO transaction.
		/// </summary>
		/// <param name="transactionExpression">The MiddleTierContext used for the transaction.</param>
		/// <param name="columnSchema">The record that is held for the duration of the transaction.</param>
		public CodeAcquireRecordWriterLockExpression(CodeVariableReferenceExpression transactionExpression, CodeVariableReferenceExpression rowExpression)
		{
			//			configurationRow.AcquireWriterLock(middleTierTransaction.AdoResourceManager.Guid, DarkBond.UnitTest.Server.DataModel.lockTimeout);
            this.Method = new CodeMethodReferenceExpression(transactionExpression, "AddLock");
            this.Parameters.Add(new CodeMethodReferenceExpression(rowExpression, "AcquireWriterLock"));
            this.Parameters.Add(new CodeMethodReferenceExpression(rowExpression, "ReleaseLock"));
        }
	}
}
