// <copyright file="CompressorLogMethod.cs" company="Dark Bond, Inc.">
//     Copyright © 2014 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Threading;

	/// <summary>
	/// Creates a method that purges the transaction log of obsolete records.
	/// </summary>
	class CompressLogMethod : CodeMemberMethod
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompressLogMethod"/> class.
		/// </summary>
		/// <param name="schema">The data model schema.</param>
		public CompressLogMethod(DataModelSchema dataModelSchema)
		{
			//        /// <summary>
            //        /// Purges the transaction log of obsolete rows.
            //		  /// </summary>
			//		  private void CompressLog()
			//		  {
			this.Comments.Add(new CodeCommentStatement("<summary>", true));
            this.Comments.Add(new CodeCommentStatement("Purges the transaction log of obsolete rows.", true));
			this.Comments.Add(new CodeCommentStatement("</summary>", true));
			this.Attributes = MemberAttributes.Private | MemberAttributes.Final;
			this.Name = "CompressLog";

            //            System.Collections.Generic.LinkedListNode<TransactionLogItem> currentLink = this.transactionLog.Last;
            CodeVariableReferenceExpression currentLink = new CodeVariableReferenceExpression("currentLink");
            this.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference("System.Collections.Generic.LinkedListNode<TransactionLogItem>"),
                    currentLink.VariableName,
					new CodePropertyReferenceExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"),
                        "Last")));

			//			for (; true; )
			//			{
			CodeIterationStatement forever = new CodeIterationStatement(
                new CodeExpressionStatement(new CodeSnippetExpression()),
                new CodePrimitiveExpression(true),
                new CodeExpressionStatement(new CodeSnippetExpression()));

			//				System.DateTime currentTime = System.DateTime.Now;
			forever.Statements.Add(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(DateTime)),
					"currentTime",
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(DateTime)), "Now")));

			//				try
			//				{
			CodeTryCatchFinallyStatement tryReadLock = new CodeTryCatchFinallyStatement();

			//					this.transactionLogLock.EnterReadLock();
			tryReadLock.TryStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					"EnterReadLock"));

			//					for (int count = 0; (count < this.transactionLogBatchSize); count = (count + 1))
			//					{
			CodeIterationStatement forCount = new CodeIterationStatement(
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(typeof(Int32)),
					"count",
					new CodePrimitiveExpression(0)),
					new CodeBinaryOperatorExpression(
						new CodeVariableReferenceExpression("count"),
						CodeBinaryOperatorType.LessThan,
						new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogBatchSize")),
					new CodeAssignStatement(
						new CodeVariableReferenceExpression("count"),
						new CodeBinaryOperatorExpression(
							new CodeVariableReferenceExpression("count"),
							CodeBinaryOperatorType.Add,
							new CodePrimitiveExpression(1))));

			//						if ((currentLink != null))
			//						{
			CodeConditionStatement ifLink = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("currentLink"),
					CodeBinaryOperatorType.IdentityInequality,
					new CodePrimitiveExpression(null)));

            //                            System.Collections.Generic.LinkedListNode<TransactionLogItem> previousLink = currentLink.Previous;
            CodeVariableReferenceExpression previousLink = new CodeVariableReferenceExpression("previousLink");
            ifLink.TrueStatements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference("System.Collections.Generic.LinkedListNode<TransactionLogItem>"),
                    previousLink.VariableName,
                    new CodePropertyReferenceExpression(currentLink, "Previous")));
            
            //							if ((currentTime.Subtract(currentLink.Value.timeStamp) > this.transactionLogItemAge))
			//							{
			CodeConditionStatement ifRecordOld = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeMethodInvokeExpression(
						new CodeVariableReferenceExpression("currentTime"),
						"Subtract",
						new CodeFieldReferenceExpression(
							new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("currentLink"), "Value"),
							"timeStamp")),
					CodeBinaryOperatorType.GreaterThan,
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogItemAge")));

            //                                this.transactionLog.Remove(currentLink);
            ifRecordOld.TrueStatements.Add(
                new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"),
                    "Remove", 
                    currentLink));
            
			//							}
			ifLink.TrueStatements.Add(ifRecordOld);

            //                            currentLink = previousLink;
            ifLink.TrueStatements.Add(new CodeAssignStatement(currentLink, previousLink));

			//						}
			forCount.Statements.Add(ifLink);

			//						if ((currentLink == null))
			//						{
			CodeConditionStatement ifStartOfList = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(
					new CodeVariableReferenceExpression("currentLink"),
					CodeBinaryOperatorType.IdentityEquality,
					new CodePrimitiveExpression(null)));

			//							count = this.transactionLogBatchSize;
			ifStartOfList.TrueStatements.Add(
				new CodeAssignStatement(
					new CodeVariableReferenceExpression("count"),
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogBatchSize")));

            //                            currentLink = this.transactionLog.Last;
            ifStartOfList.TrueStatements.Add(
                new CodeAssignStatement(
                    currentLink,
					new CodePropertyReferenceExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLog"),
                        "Last")));

			//							this.transactionLogLock.ExitReadLock();
			ifStartOfList.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					"ExitReadLock"));

			//							System.Threading.Thread.Sleep(this.logCompressionInterval);
			ifStartOfList.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeTypeReferenceExpression(typeof(Thread)),
					"Sleep",
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "logCompressionInterval")));

			//							this.transactionLogLock.EnterReadLock();
			ifStartOfList.TrueStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					"EnterReadLock"));

			//						}
			forCount.Statements.Add(ifStartOfList);

			//					}
			tryReadLock.TryStatements.Add(forCount);

			//				}
			//				finally
			//				{
			//					this.transactionLogLock.ExitReadLock();
			tryReadLock.FinallyStatements.Add(
				new CodeMethodInvokeExpression(
					new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "transactionLogLock"),
					"ExitReadLock"));

			//				}
			forever.Statements.Add(tryReadLock);

			//				System.Threading.Thread.Sleep(0);
			forever.Statements.Add(
				new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Thread)), "Sleep", new CodePrimitiveExpression(0)));

			//			}
			this.Statements.Add(forever);

			//		}
		}

	}
}
