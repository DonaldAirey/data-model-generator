namespace DarkBond.DataModelGenerator.TenantTargetClass
{
    using System;
    using System.CodeDom;
	using System.Data;
	using System.Data.SqlClient;

    /// <summary>
	/// Converts CLR types to SQL types.
	/// </summary>
	public class TypeConverter
	{
		/// <summary>
		/// Creates an expression of a CLR type as an SQL type.
		/// </summary>
		/// <param name="typeFullName">The native CLR data type to be converted.</param>
		/// <returns>A CodeDOM expression of that data type as an SQL type.</returns>
		public static CodeExpression Convert(String typeFullName)
		{
			string sqlDataType = string.Empty;

			// This will convert the native CLR type to an SQL data type.
			switch (typeFullName)
			{
				case "System.Boolean": sqlDataType = "Bit"; break;
				case "System.Byte": sqlDataType = "TinyInt"; break;
				case "System.Byte[]": sqlDataType = "Image"; break;
				case "System.DateTime": sqlDataType = "DateTime"; break;
				case "System.Decimal": sqlDataType = "Decimal"; break;
				case "System.Double": sqlDataType = "Float"; break;
				case "System.Float": sqlDataType = "Real"; break;
				case "System.Guid": sqlDataType = "UniqueIdentifier"; break;
				case "System.Int16": sqlDataType = "SmallInt"; break;
				case "System.Int32": sqlDataType = "Int"; break;
				case "System.Int64": sqlDataType = "BigInt"; break;
				case "System.Object": sqlDataType = "Variant"; break;
				case "System.String": sqlDataType = "NVarChar"; break;

			default:

				// Enums are converted into a generic data type on the SQL Server.
				sqlDataType = "Int";
				break;

			}

			// This expression can be used when constructing parameters for an SQL query.
			return new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(SqlDbType)), sqlDataType);

		}
	}
}
