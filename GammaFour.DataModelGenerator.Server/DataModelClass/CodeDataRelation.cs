namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System;
	using System.Data;
    using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// Create a representation of the creation of a data relation.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Dark Bond, Inc.  All Rights Reserved.</copyright>
	public class CodeDataRelation : CodeObjectCreateExpression
	{
		/// <summary>
		/// Create a representation of the creation of a data relation.
		/// </summary>
		/// <param name="relationSchema">The description of a relationship between two tables.</param>
		public CodeDataRelation(RelationSchema relationSchema)
		{
			// Collect the key fields in the parent table.
			List<CodeExpression> parentFieldList = new List<CodeExpression>();
			foreach (ColumnSchema columnSchema in relationSchema.ParentColumns)
				parentFieldList.Add(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}TableField", CommonConversion.ToCamelCase(relationSchema.ParentTable.Name))),
						String.Format("{0}Column", columnSchema.Name)));

			// Collect the referenced fields in the child table.
			List<CodeExpression> childFieldList = new List<CodeExpression>();
			foreach (ColumnSchema columnSchema in relationSchema.ChildColumns)
				childFieldList.Add(
					new CodePropertyReferenceExpression(
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), String.Format("{0}TableField", CommonConversion.ToCamelCase(relationSchema.ChildTable.Name))),
						String.Format("{0}Column", columnSchema.Name)));

			//            new System.Data.ForeignKeyConstraint("FK_Object_Department", new DarkBond.DataModelGenerator.Column[] {
			//                        DarkBond.UnitTest.Server.DataModel.tableObject.ObjectIdColumn}, new DarkBond.DataModelGenerator.Column[] {
			//                        DarkBond.UnitTest.Server.DataModel.tableDepartment.DepartmentIdColumn});
			this.CreateType = new CodeTypeReference(typeof(DataRelation));
			this.Parameters.Add(new CodePrimitiveExpression(relationSchema.Name));
			this.Parameters.Add(new CodeArrayCreateExpression(new CodeTypeReference(typeof(DataColumn)), parentFieldList.ToArray()));
			this.Parameters.Add(new CodeArrayCreateExpression(new CodeTypeReference(typeof(DataColumn)), childFieldList.ToArray()));
			this.Parameters.Add(new CodePrimitiveExpression(false));

		}
	}
}
