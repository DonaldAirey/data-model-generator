namespace DarkBond.DataModelGenerator.TransactionLogItemClass
{
    using System;
    using System.CodeDom;

	/// <summary>
	/// Creates a field that holds the length of the key found in the data.
	/// </summary>
	/// <copyright>Copyright © 2010-2012 - Dark Bond, Inc.  All Rights Reserved.</copyright>
	class KeyLengthField : CodeMemberField
	{
		/// <summary>
		/// Creates a field that holds the length of the key found in the data.
		/// </summary>
		public KeyLengthField()
		{
			//		internal int keyLength;
			this.Attributes = MemberAttributes.Assembly;
			this.Type = new CodeTypeReference(typeof(Int32));
			this.Name = "keyLength";

		}
	}
}
