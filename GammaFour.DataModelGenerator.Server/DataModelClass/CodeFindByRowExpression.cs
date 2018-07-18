namespace DarkBond.DataModelGenerator.TenantTargetClass
{
	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates an expression to find a record based on the primary index of a table.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Dark Bond, Inc.  All Rights Reserved.</copyright>
	class CodeFindByRowExpression : CodeCastExpression
	{
		/// <summary>
		/// Creates an expression to find a record based on the primary index of a table.
		/// </summary>
		/// <param name="tableSchema"></param>
		public CodeFindByRowExpression(TableSchema tableSchema, CodeExpression keyExpression, CodeExpression targetDataSet)
		{
			//			((DestinationRow)(DataModel.Destination.Rows.Find(destinationRowByFK_Destination_DestinationOrderKey)));
			this.TargetType = new CodeTypeReference(String.Format("{0}Row", tableSchema.Name));
			this.Expression = new CodeMethodInvokeExpression(
				new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(targetDataSet, String.Format("{0}TableField", tableSchema.CamelCaseName)), "Rows"), "Find", keyExpression);

		}
	}
}
