namespace DarkBond.DataModelGenerator.RowClass
{
    using System.CodeDom;

	/// <summary>
	/// Creates a collection of statements to check that a writer lock is held for a given row.
	/// </summary>
	class CodeCheckWriterLockHeldStatements : CodeStatementCollection
	{
		/// <summary>
		/// Creates a collection of statements to check that a writer lock is held for a given row.
		/// </summary>
		/// <param name="rowExpression">A CodeDOM expression representing the row that is to be checked.</param>
		/// <param name="tableSchema">The table for which the parent locks are required.</param>
		public CodeCheckWriterLockHeldStatements(CodeExpression rowExpression, TableSchema tableSchema, CodeExpression transactionExpression)
		{
			//                    DarkBond.MiddleTierContext middleTierTransaction = DarkBond.MiddleTierContext.Current;
			//                    if ((this.IsWriterLockHeld(middleTierTransaction.AdoResourceManager.Guid) == false)) {
			//                        throw new System.ServiceModel.FaultException<SynchronizationLockFault>(new DarkBond.SynchronizationLockFault("Attempt to access a Department record without a lock."));
			//                    }
			CodeConditionStatement ifNotHeld = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeMethodInvokeExpression(rowExpression, "IsWriterLockHeld",
						new CodePropertyReferenceExpression(transactionExpression, "TransactionId")),
						CodeBinaryOperatorType.IdentityEquality,
						new CodePrimitiveExpression(false)));
			ifNotHeld.TrueStatements.Add(new CodeThrowSynchronizationExceptionStatement(tableSchema));
			Add(ifNotHeld);

		}
	}
}
