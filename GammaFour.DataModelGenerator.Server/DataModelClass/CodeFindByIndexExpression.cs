// <copyright file="CodeFindByIndexExpression.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.TenantTargetClass
{
	using System;
	using System.CodeDom;
    using System.Collections.Generic;

	/// <summary>
	/// Creates an expression to find a record based on the primary index of a table.
	/// </summary>
	class CodeFindByIndexExpression : CodeMethodInvokeExpression
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeFindByIndexExpression"/> class.
		/// </summary>
		/// <param name="tableSchema">The table schema.</param>
		public CodeFindByIndexExpression(TableSchema tableSchema, CodeExpression keyExpression, CodeExpression targetDataSet)
		{
			//            DarkBond.UnitTest.Server.DataModel.Employee.Find(new EmployeeKey(employeeId));
            foreach (ConstraintSchema constraintSchema in tableSchema.Constraints)
            {
                if (constraintSchema is UniqueConstraintSchema)
                {
                    UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
                    if (uniqueConstraintSchema.IsPrimaryKey)
                    {
                        this.Method = new CodeMethodReferenceExpression(
                                new CodeFieldReferenceExpression(targetDataSet, String.Format("{0}TableField", tableSchema.CamelCaseName)), "Find");
                        this.Parameters.Add(keyExpression);
                    }
                }
            }
		}
	}
}