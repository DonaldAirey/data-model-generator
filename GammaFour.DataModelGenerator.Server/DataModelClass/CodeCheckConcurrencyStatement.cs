namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System.CodeDom;

	/// <summary>
	/// Creates a statement for optimistic concurrency checking.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Dark Bond, Inc.  All Rights Reserved.</copyright>
	class CodeCheckConcurrencyStatement : CodeConditionStatement
	{
		/// <summary>
		/// Creates a statement for optimistic concurrency checking.
		/// </summary>
		/// <param name="tableSchema"></param>
		public CodeCheckConcurrencyStatement(TableSchema tableSchema, CodeVariableReferenceExpression rowExpression, CodeExpression keyExpression)
		{
			//            // The Optimistic Concurrency check allows only one client to update a record at a time.
			//            if ((employeeRow.RowVersion != rowVersion)) {
			//                throw new System.ServiceModel.FaultException<OptimisticConcurrencyFault>(new DarkBond.OptimisticConcurrencyFault("The Employee record ({0}) is busy.  Please try again later.", employeeId));
			//            }
			this.Condition = new CodeBinaryOperatorExpression(
                new CodePropertyReferenceExpression(rowExpression, "RowVersion"),
                CodeBinaryOperatorType.IdentityInequality,
                new CodeArgumentReferenceExpression("rowVersion"));
			this.TrueStatements.Add(new CodeThrowConcurrencyExceptionStatement(tableSchema, keyExpression));

		}
	}
}
