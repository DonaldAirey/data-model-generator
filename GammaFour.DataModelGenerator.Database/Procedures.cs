// <copyright file="Procedures.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Database
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;

    /// <summary>
    /// Class for generating tables.
    /// </summary>
    internal static class Procedures
    {
        /// <summary>
        /// Generates the DDL for a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        internal static void Generate(StreamWriter streamWriter, TableSchema tableSchema)
        {
            Procedures.CreateCreateProcedure(streamWriter, tableSchema);
            Procedures.CreateDeleteProcedure(streamWriter, tableSchema);
            Procedures.CreateReadProcedure(streamWriter, tableSchema);
            Procedures.CreateUpdateProcedure(streamWriter, tableSchema);
        }

        /// <summary>
        /// Creates a stored procedure to create a record.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        private static void CreateCreateProcedure(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // This list contains all the columns are used to create the record.
            var columnSchemas = (from cs in tableSchema.Columns
                                 where !cs.IsAutoIncrement
                                 select cs).ToList();

            // Generate the parameters for the 'create' procedure.
            string parameters = string.Empty;
            int counter = 0;
            foreach (ColumnSchema columnSchema in columnSchemas)
            {
                parameters += string.Format(
                        CultureInfo.InvariantCulture,
                        "@{0} {1}{2}{3}",
                        columnSchema.CamelCaseName,
                        SchemaUnit.GetSqlDataType(columnSchema),
                        columnSchema.IsNullable ? " NULL" : string.Empty,
                        counter++ < columnSchemas.Count - 1 ? ", " : string.Empty);
            }

            streamWriter.Write(string.Format("CREATE PROCEDURE [dbo].[create{0}] {1}", tableSchema.Name, parameters));

            streamWriter.WriteLine();
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");

            // Generate each of the column descriptions.
            string targetColumns = string.Empty;
            counter = 0;
            foreach (ColumnSchema columnSchema in tableSchema.Columns)
            {
                targetColumns += string.Format(
                        CultureInfo.InvariantCulture,
                        "[{0}]{1}",
                        columnSchema.Name,
                        counter++ < columnSchemas.Count - 1 ? ", " : string.Empty);
            }

            streamWriter.WriteLine(string.Format("    INSERT INTO [dbo].[{0}] ({1})", tableSchema.Name, targetColumns));

            // Generate each of the column descriptions.
            string parameterList = string.Empty;
            counter = 0;
            foreach (ColumnSchema columnSchema in tableSchema.Columns)
            {
                parameterList += string.Format(
                        CultureInfo.InvariantCulture,
                        "@{0}{1}",
                        columnSchema.CamelCaseName,
                        counter++ < columnSchemas.Count - 1 ? ", " : string.Empty);
            }

            streamWriter.WriteLine("    VALUES ({0})", parameterList);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }

        /// <summary>
        /// Creates a stored procedure to delete a record.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        private static void CreateDeleteProcedure(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // This list contains all the columns that can be updated.
            var columnSchemas = (from cs in tableSchema.Columns
                                 where !cs.IsAutoIncrement
                                 select cs).ToList();

            // These are the key elements of the record.
            string parameters = string.Empty;
            int counter = 0;
            foreach (ColumnSchema columnSchema in tableSchema.PrimaryKey.Columns)
            {
                parameters += string.Format(
                    CultureInfo.InvariantCulture,
                    "@key{0} {1}{2}",
                    columnSchema.Name,
                    SchemaUnit.GetSqlDataType(columnSchema),
                    counter++ < tableSchema.PrimaryKey.Columns.Count - 1 ? ", " : string.Empty);
            }

            // Generate the 'delete' procedure.
            streamWriter.WriteLine(string.Format("CREATE PROCEDURE [dbo].[delete{0}] {1}", tableSchema.Name, parameters));
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine(string.Format("    DELETE [dbo].[{0}]", tableSchema.Name));
            string whereClause = string.Empty;
            foreach (ColumnSchema columnSchema in tableSchema.PrimaryKey.Columns)
            {
                whereClause += string.Format(whereClause == string.Empty ? "[{0}] = @key{0}" : " AND [{0}] = @key{0}", columnSchema.Name);
            }

            streamWriter.WriteLine("    WHERE {0}", whereClause);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }

        /// <summary>
        /// Creates a stored procedure to read a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        private static void CreateReadProcedure(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // These are the columns to be returned.
            string columns = string.Empty;
            foreach (ColumnSchema columnSchema in tableSchema.Columns)
            {
                columns += (columns == string.Empty ? string.Empty : ",") + "[" + columnSchema.Name + "]";
            }

            // Generate the 'read' procedure.
            streamWriter.WriteLine("CREATE PROCEDURE [dbo].[read" + tableSchema.Name + "]");
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine("    SELECT " + columns + " FROM [dbo].[" + tableSchema.Name + "]");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }

        /// <summary>
        /// Creates a stored procedure to create a record.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        private static void CreateUpdateProcedure(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // This list contains all the columns that can be updated.
            var columnSchemas = (from cs in tableSchema.Columns
                                 where !cs.IsAutoIncrement
                                 select cs).ToList();

            // Generate a parameter for each of the columns.
            string parameters = string.Empty;
            foreach (ColumnSchema columnSchema in columnSchemas)
            {
                parameters += string.Format(
                    CultureInfo.InvariantCulture,
                    "@{0} {1}{2},",
                    columnSchema.CamelCaseName,
                    SchemaUnit.GetSqlDataType(columnSchema),
                    columnSchema.IsNullable ? " NULL" : string.Empty);
            }

            int counter = 0;
            foreach (ColumnSchema columnSchema in tableSchema.PrimaryKey.Columns)
            {
                parameters += string.Format(
                    CultureInfo.InvariantCulture,
                    "@key{0} {1}{2}",
                    columnSchema.Name,
                    SchemaUnit.GetSqlDataType(columnSchema),
                    counter++ < tableSchema.PrimaryKey.Columns.Count - 1 ? ", " : string.Empty);
            }

            // Generate the 'update' procedure.
            streamWriter.WriteLine(string.Format("CREATE PROCEDURE [dbo].[update{0}] {1}", tableSchema.Name, parameters));
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine(string.Format("    UPDATE [dbo].[{0}]", tableSchema.Name));
            streamWriter.WriteLine("    SET");

            counter = 0;
            foreach (ColumnSchema columnSchema in columnSchemas)
            {
                streamWriter.WriteLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "        [{0}] = @{1}{2}",
                        columnSchema.Name,
                        columnSchema.CamelCaseName,
                        counter++ < columnSchemas.Count - 1 ? "," : string.Empty));
            }

            string whereClause = string.Empty;
            foreach (ColumnSchema columnSchema in tableSchema.PrimaryKey.Columns)
            {
                whereClause += string.Format(whereClause == string.Empty ? "[{0}] = @key{0}" : " AND [{0}] = @key{0}", columnSchema.Name);
            }

            streamWriter.WriteLine("    WHERE {0}", whereClause);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }
    }
}