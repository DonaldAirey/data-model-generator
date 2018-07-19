// <copyright file="SchemaUnit.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;

    /// <summary>
    /// This object will load the property table from a formatted file.
    /// </summary>
    public class SchemaUnit
    {
        /// <summary>
        /// Default length for a string.
        /// </summary>
        private const int DefaultStringLength = 256;

        /// <summary>
        /// The data model schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaUnit"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The data model schema.</param>
        public SchemaUnit(DataModelSchema dataModelSchema)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;
        }

        /// <summary>
        /// Generate the code from the inputs.
        /// </summary>
        /// <returns>A buffer containing the generated DDL for the schema.</returns>
        public byte[] Generate()
        {
            // The generated data is written to a memory stream using a writer.
            MemoryStream memoryStream = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            {
                // The tables must be written out to the DDL file so that parent tables are written out before child tables.  As tables are emitted
                // into the DDL file, they are removed from the list.  This continues until the list of tables is empty.
                var orderedTables = from t in this.dataModelSchema.Tables
                                    orderby t.Name
                                    where t.IsPersistent
                                    select t;

                // Write the tables out such that the parent tables are written before the children to prevent forward reference errors when creating
                // the foreign indices.  As tables are emitted to the DDL, they are removed from the list until the list is empty.
                List<TableSchema> tables = orderedTables.ToList<TableSchema>();
                while (tables.Count > 0)
                {
                    foreach (TableSchema tableSchema in tables)
                    {
                        // This will search the tables to see if the current table has any parent dependencies that haven't been written yet.
                        bool isParentDefined = true;
                        foreach (RelationSchema relationSchema in tableSchema.ParentRelations)
                        {
                            if (tables.Contains(relationSchema.ParentTable))
                            {
                                isParentDefined = false;
                                break;
                            }
                        }

                        // If there are parent dependencies that have not yet been generated, then skip this table for now.
                        if (!isParentDefined)
                        {
                            continue;
                        }

                        // The table schema is removed from the list after it is written to the stream.
                        Tables.Generate(streamWriter, tableSchema);
                        tables.Remove(tableSchema);
                        break;
                    }
                }

                // Generate the CRUD procedures (create, delete, update) for each of the tables.
                foreach (TableSchema tableSchema in orderedTables)
                {
                    Procedures.Generate(streamWriter, tableSchema);
                }
            }

            // This buffer contains the complete DDL.
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Converts the system type into an equivalent Sql data type.
        /// </summary>
        /// <param name="columnSchema">The column schema.</param>
        /// <returns>An equivalent SQL datatype.</returns>
        internal static string GetSqlDataType(ColumnSchema columnSchema)
        {
            // This will convert the system datatype into the SQL equivalent.
            switch (columnSchema.Type.FullName)
            {
                case "System.Object":
                case "System.Boolean":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.DateTime":
                case "System.Byte[]":

                    return GetSqlDataType(columnSchema.Type);

                case "System.Guid":

                    return GetSqlDataType(columnSchema.Type);

                case "System.String":
                    return string.Format("NVARCHAR({0})", columnSchema.MaximumLength == int.MaxValue ? "MAX" : Convert.ToString(columnSchema.MaximumLength));

                default:

                    return GetSqlDataType(columnSchema.Type);
            }

            // Failure to convert a data type generates an exception.
            throw new Exception(string.Format("The type {0} can't be converted to an SQL data type", columnSchema.Type.FullName));
        }

        /// <summary>
        /// Converts the system type into an equivalent SQL datatype.
        /// </summary>
        /// <param name="type">Represents a system type declaration.</param>
        /// <returns>An equivalent SQL datatype.</returns>
        internal static string GetSqlDataType(Type type)
        {
            // Null-ables will resolve to the inner type.  The 'IsNullable' flag will instruct the compiler to allow nulls in these column types.
            if (type.FullName.StartsWith("System.Nullable"))
            {
                type = type.GenericTypeArguments[0];
            }

            // This will convert the system datatype into the SQL equivalent.
            switch (type.FullName)
            {
                case "System.Object":
                    return "SQL_VARIANT";
                case "System.Boolean":
                    return "BIT";
                case "System.Int16":
                    return "SMALLINT";
                case "System.Int32":
                    return "INT";
                case "System.Int64":
                    return "BIGINT";
                case "System.Decimal":
                    return "DECIMAL(19,7)";
                case "System.Single":
                    return "REAL";
                case "System.Double":
                    return "FLOAT";
                case "System.DateTime":
                    return "DATETIME";
                case "System.Byte[]":
                    return "VARBINARY(MAX)";
                case "System.Guid":
                    return "UNIQUEIDENTIFIER";
                case "System.String":
                    return "NVARCHAR(MAX)";
            }

            // All other datatypes are assumed to be enumerations which are stored as integers.
            return "int";
        }
    }
}