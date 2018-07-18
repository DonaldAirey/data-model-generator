namespace DarkBond.DataModelGenerator.TenantTargetClass
{
	using System;
	using System.CodeDom;

	/// <summary>
	/// Creates a private field that holds the time to wait for a thread to exit before aborting it.
	/// </summary>
	class ThreadWaitTimeField : CodeMemberField
	{
		/// <summary>
		/// Creates a private field that holds the time to wait for a thread to exit before aborting it.
		/// </summary>
		public ThreadWaitTimeField()
		{
			//        // The time to wait for a thread to respond before aborting it.
			//        private const int threadWaitTime = 1000;
			this.Attributes = MemberAttributes.Private | MemberAttributes.Const;
			this.Type = new CodeTypeReference(typeof(Int32));
			this.Name = "threadWaitTime";
			this.InitExpression = new CodePrimitiveExpression(1000);

		}
	}
}
