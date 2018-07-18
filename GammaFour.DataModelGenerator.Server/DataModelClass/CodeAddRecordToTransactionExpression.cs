namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System.CodeDom;

	/// <summary>
	/// Creates a statement method invocation that adds a record to an ADO transaction.
	/// </summary>
	class CodeAddRecordToTransactionExpression : CodeMethodInvokeExpression
	{
		/// <summary>
		/// Creates a statement method invocation that adds a record to an ADO transaction.
		/// </summary>
		/// <param name="transactionExpression">The MiddleTierContext used for the transaction.</param>
		/// <param name="columnSchema">The record that is held for the duration of the transaction.</param>
		public CodeAddRecordToTransactionExpression(CodeVariableReferenceExpression transactionExpression, CodeExpression rowExpression)
		{
			//            middleTierTransaction.AdoResourceManager.AddRecord(departmentRow);
			this.Method = new CodeMethodReferenceExpression(transactionExpression, "AddRecord");
			this.Parameters.Add(rowExpression);

		}
	}
}
