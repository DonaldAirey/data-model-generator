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
        /// <param name="tableElement">The schema description of the table.</param>
        internal static void Generate(StreamWriter streamWriter, TableElement tableElement)
        {
            Procedures.CreateCreateProcedure(streamWriter, tableElement);
            Procedures.CreateDeleteProcedure(streamWriter, tableElement);
            Procedures.CreateReadProcedure(streamWriter, tableElement);
            Procedures.CreateUpdateProcedure(streamWriter, tableElement);
        }

        /// <summary>
        /// Creates a stored procedure to create a record.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableElement">The schema description of the table.</param>
        private static void CreateCreateProcedure(StreamWriter streamWriter, TableElement tableElement)
        {
            // This list contains all the columns are used to create the record.
            var columnElements = (from cs in tableElement.Columns
                                 where !cs.IsAutoIncrement
                                 select cs).ToList();

            // Generate the parameters for the 'create' procedure.
            string parameters = string.Empty;
            int counter = 0;
            foreach (ColumnElement columnElement in columnElements)
            {
                parameters += string.Format(
                        CultureInfo.InvariantCulture,
                        "@{0} {1}{2}{3}",
                        columnElement.Name.ToCamelCase(),
                        SchemaUnit.GetSqlDataType(columnElement),
                        columnElement.IsNullable ? " NULL" : string.Empty,
                        counter++ < columnElements.Count - 1 ? ", " : string.Empty);
            }

            streamWriter.Write(string.Format("CREATE PROCEDURE [dbo].[create{0}] {1}", tableElement.Name, parameters));

            streamWriter.WriteLine();
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");

            // Generate each of the column descriptions.
            string targetColumns = string.Empty;
            counter = 0;
            foreach (ColumnElement columnElement in tableElement.Columns)
            {
                targetColumns += string.Format(
                        CultureInfo.InvariantCulture,
                        "[{0}]{1}",
                        columnElement.Name,
                        counter++ < columnElements.Count - 1 ? ", " : string.Empty);
            }

            streamWriter.WriteLine(string.Format("    INSERT INTO [dbo].[{0}] ({1})", tableElement.Name, targetColumns));

            // Generate each of the column descriptions.
            string parameterList = string.Empty;
            counter = 0;
            foreach (ColumnElement columnElement in tableElement.Columns)
            {
                parameterList += string.Format(
                        CultureInfo.InvariantCulture,
                        "@{0}{1}",
                        columnElement.Name.ToCamelCase(),
                        counter++ < columnElements.Count - 1 ? ", " : string.Empty);
            }

            streamWriter.WriteLine("    VALUES ({0})", parameterList);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }

        /// <summary>
        /// Creates a stored procedure to delete a record.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableElement">The schema description of the table.</param>
        private static void CreateDeleteProcedure(StreamWriter streamWriter, TableElement tableElement)
        {
            // This list contains all the columns that can be updated.
            var columnElements = (from cs in tableElement.Columns
                                 where !cs.IsAutoIncrement
                                 select cs).ToList();

            // These are the key elements of the record.
            string parameters = string.Empty;
            int counter = 0;
            foreach (ColumnReferenceElement columnReferenceElement in tableElement.PrimaryKey.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                parameters += string.Format(
                    CultureInfo.InvariantCulture,
                    "@key{0} {1}{2}",
                    columnElement.Name,
                    SchemaUnit.GetSqlDataType(columnElement),
                    counter++ < tableElement.PrimaryKey.Columns.Count() - 1 ? ", " : string.Empty);
            }

            // Generate the 'delete' procedure.
            streamWriter.WriteLine(string.Format("CREATE PROCEDURE [dbo].[delete{0}] {1}", tableElement.Name, parameters));
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine(string.Format("    DELETE [dbo].[{0}]", tableElement.Name));
            string whereClause = string.Empty;
            foreach (ColumnReferenceElement columnReferenceElement in tableElement.PrimaryKey.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                whereClause += string.Format(whereClause == string.Empty ? "[{0}] = @key{0}" : " AND [{0}] = @key{0}", columnElement.Name);
            }

            streamWriter.WriteLine("    WHERE {0}", whereClause);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }

        /// <summary>
        /// Creates a stored procedure to read a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableElement">The schema description of the table.</param>
        private static void CreateReadProcedure(StreamWriter streamWriter, TableElement tableElement)
        {
            // These are the columns to be returned.
            string columns = string.Empty;
            foreach (ColumnElement columnElement in tableElement.Columns)
            {
                columns += (columns == string.Empty ? string.Empty : ",") + "[" + columnElement.Name + "]";
            }

            // Generate the 'read' procedure.
            streamWriter.WriteLine("CREATE PROCEDURE [dbo].[read" + tableElement.Name + "]");
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine("    SELECT " + columns + " FROM [dbo].[" + tableElement.Name + "]");
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }

        /// <summary>
        /// Creates a stored procedure to create a record.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableElement">The schema description of the table.</param>
        private static void CreateUpdateProcedure(StreamWriter streamWriter, TableElement tableElement)
        {
            // This list contains all the columns that can be updated.
            var columnElements = (from cs in tableElement.Columns
                                 where !cs.IsAutoIncrement
                                 select cs).ToList();

            // Generate a parameter for each of the columns.
            string parameters = string.Empty;
            foreach (ColumnElement columnElement in columnElements)
            {
                parameters += string.Format(
                    CultureInfo.InvariantCulture,
                    "@{0} {1}{2},",
                    columnElement.Name.ToCamelCase(),
                    SchemaUnit.GetSqlDataType(columnElement),
                    columnElement.IsNullable ? " NULL" : string.Empty);
            }

            int counter = 0;
            foreach (ColumnReferenceElement columnReferenceElement in tableElement.PrimaryKey.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                parameters += string.Format(
                    CultureInfo.InvariantCulture,
                    "@key{0} {1}{2}",
                    columnElement.Name,
                    SchemaUnit.GetSqlDataType(columnElement),
                    counter++ < tableElement.PrimaryKey.Columns.Count() - 1 ? ", " : string.Empty);
            }

            // Generate the 'update' procedure.
            streamWriter.WriteLine(string.Format("CREATE PROCEDURE [dbo].[update{0}] {1}", tableElement.Name, parameters));
            streamWriter.WriteLine("AS");
            streamWriter.WriteLine("BEGIN");
            streamWriter.WriteLine(string.Format("    UPDATE [dbo].[{0}]", tableElement.Name));
            streamWriter.WriteLine("    SET");

            counter = 0;
            foreach (ColumnElement columnElement in columnElements)
            {
                streamWriter.WriteLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "        [{0}] = @{1}{2}",
                        columnElement.Name,
                        columnElement.Name.ToCamelCase(),
                        counter++ < columnElements.Count - 1 ? "," : string.Empty));
            }

            string whereClause = string.Empty;
            foreach (ColumnReferenceElement columnReferenceElement in tableElement.PrimaryKey.Columns)
            {
                ColumnElement columnElement = columnReferenceElement.Column;
                whereClause += string.Format(whereClause == string.Empty ? "[{0}] = @key{0}" : " AND [{0}] = @key{0}", columnElement.Name);
            }

            streamWriter.WriteLine("    WHERE {0}", whereClause);
            streamWriter.WriteLine("END");
            streamWriter.WriteLine("GO");
        }
    }
}