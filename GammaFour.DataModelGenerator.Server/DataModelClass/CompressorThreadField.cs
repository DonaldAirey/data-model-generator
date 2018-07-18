namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System.CodeDom;
    using System.Threading;

	/// <summary>
	/// Creates a private field that holds the thread that purges the data set of deleted records.
	/// </summary>
	class CompressorThreadField : CodeMemberField
	{
		/// <summary>
		/// Creates a private field that holds the thread that purges the data set of deleted records.
		/// </summary>
		public CompressorThreadField()
		{
			//        // A thread that purges deleted fields when they have become obsolete.
			//        private static System.Threading.Thread garbageCollector;
			this.Attributes = MemberAttributes.Private;
			this.Type = new CodeTypeReference(typeof(Thread));
			this.Name = "compressorThread";

		}
	}
}
