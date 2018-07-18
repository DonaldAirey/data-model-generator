namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System;
	using System.Data;
    using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// Create a representation of the creation of a foreign key constraint.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Dark Bond, Inc.  All Rights Reserved.</copyright>
	public class CodeForeignKeyConstraint : CodeObjectCreateExpression
	{
		/// <summary>
		/// Create a representation of the creation of a foreign key constraint.
		/// </summary>
		/// <param name="relationSchema">The description of the foreign key constraint.</param>
		public CodeForeignKeyConstraint(ForeignKeyConstraintSchema foreignKeyConstraintSchema)
		{
			//			new System.Data.DataColumn[] {this.tableCountry.CountryIdColumn}
			List<CodeExpression> parentFieldList = new List<CodeExpression>();
			foreach (ColumnSchema columnSchema in foreignKeyConstraintSchema.RelatedColumns)
				parentFieldList.Add(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}TableField", CommonConversion.ToCamelCase(foreignKeyConstraintSchema.RelatedTable.Name))),
						String.Format("{0}Column", columnSchema.Name)));

			//			new System.Data.DataColumn[] {this.tableAccountBase.CountryIdColumn}
			List<CodeExpression> childFieldList = new List<CodeExpression>();
			foreach (ColumnSchema columnSchema in foreignKeyConstraintSchema.Columns)
				childFieldList.Add(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}TableField", CommonConversion.ToCamelCase(foreignKeyConstraintSchema.Table.Name))),
						String.Format("{0}Column", columnSchema.Name)));

			//			new System.Data.ForeignKeyConstraint("FK_Country_AccountBase", new System.Data.DataColumn[] {
			//						this.tableCountry.CountryIdColumn}, new System.Data.DataColumn[] {
			//						this.tableAccountBase.CountryIdColumn});
			this.CreateType = new CodeTypeReference(typeof(ForeignKeyConstraint));
			this.Parameters.Add(new CodePrimitiveExpression(foreignKeyConstraintSchema.Name));
			this.Parameters.Add(new CodeArrayCreateExpression(new CodeTypeReference(typeof(DataColumn)), parentFieldList.ToArray()));
			this.Parameters.Add(new CodeArrayCreateExpression(new CodeTypeReference(typeof(DataColumn)), childFieldList.ToArray()));

		}
	}
}
