// <copyright file="ItemInt32Property.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.TableClass
{
    using System;
    using System.CodeDom;

	/// <summary>
	/// Represents a declaration of a property that gets the parent row.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Dark Bond, Inc.  All Rights Reserved.</copyright>
	public class ItemInt32Property : CodeMemberProperty
	{
		/// <summary>
		/// Generates a property to get a parent row.
		/// </summary>
		/// <param name="foreignKeyConstraintSchema">The foreign key that references the parent table.</param>
		public ItemInt32Property(TableSchema tableSchema)
		{
			// Construct the type names for the table and rows within the table.
			string rowTypeName = String.Format("{0}Row", tableSchema.Name);

			//		/// <summary>
			//		/// Indexer to a row in the AccountBase table.
			//		/// </summary>
			//		/// <param name="index">The integer index of the row.</param>
			//		/// <returns>The AccountBase row found at the given index.</returns>
			//		public AccountBaseRow this[int index]
			//		{
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("Indexer to a row in the {0} table.", tableSchema.Name), true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Comments.Add(new CodeCommentStatement("<param name=\"index\">The integer index of the row.</param>", true));
			this.Comments.Add(new CodeCommentStatement(String.Format("<returns>The {0} row found at the given index.</returns>", tableSchema.Name), true));
			this.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			this.Type = new CodeTypeReference(rowTypeName);
			this.Name = "Item";
			this.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Int32)), "index"));

			//			get
			//			{
			//				try
			//				{
			//					((TenantTarget)this.DataSet).dataLock.EnterReadLock();
			//					return ((AccountBaseRow)(this.Rows[index]));
			//				}
			CodeTryCatchFinallyStatement tryCatchFinallyStatement = new CodeTryCatchFinallyStatement();
			tryCatchFinallyStatement.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(new CodeTypeReference(String.Format("Tenant{0}", tableSchema.DataModel.Name)), new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataSet")),
						"dataLock"),
					"EnterReadLock"));
			tryCatchFinallyStatement.TryStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(new CodeTypeReference(String.Format("{0}Row", tableSchema.Name)), new CodeIndexerExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Rows"), new CodeArgumentReferenceExpression("index")))));

			//				finally
			//				{
			//					((TenantTarget)this.DataSet).dataLock.ExitReadLock();
			//				}
			tryCatchFinallyStatement.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(
						new CodeCastExpression(new CodeTypeReference(String.Format("Tenant{0}", tableSchema.DataModel.Name)), new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "DataSet")),
						"dataLock"),
					"ExitReadLock"));
			this.GetStatements.Add(tryCatchFinallyStatement);

			//			}
			//		}
		}

	}
}
