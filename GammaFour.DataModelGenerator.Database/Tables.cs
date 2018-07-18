// <copyright file="Tables.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Database
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using GammaFour.DataModelGenerator.Common;

    /// <summary>
    /// Class for generating tables.
    /// </summary>
    internal static class Tables
    {
        /// <summary>
        /// Generates the DDL for a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        internal static void Generate(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // The table is described here.
            streamWriter.WriteLine(string.Format("CREATE TABLE [dbo].[{0}]", tableSchema.Name));
            streamWriter.WriteLine("(");

            // Generate each of the column descriptions.
            foreach (ColumnSchema columnSchema in tableSchema.Columns)
            {
                streamWriter.WriteLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "    [{0}] {1}{2},",
                        columnSchema.Name,
                        SchemaUnit.GetSqlDataType(columnSchema),
                        columnSchema.IsNullable ? " NULL" : " NOT NULL"));
            }

            // Generate the unique keys for this table.
            Tables.CreateKeys(streamWriter, tableSchema);
            streamWriter.WriteLine(")");
            streamWriter.WriteLine("go");

            // Generate the additional indices.
            Tables.CreateIndices(streamWriter, tableSchema);
        }

        /// <summary>
        /// Generate the keys on a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        private static void CreateKeys(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // Put all the non-nullable unique keys into a list.
            foreach (ConstraintSchema constraintSchema in tableSchema.Constraints)
            {
                UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
                if (uniqueConstraintSchema != null)
                {
                    if (uniqueConstraintSchema.IsPrimaryKey)
                    {
                        streamWriter.Write("	CONSTRAINT [{0}] PRIMARY KEY (", uniqueConstraintSchema.Name);
                        List<ColumnSchema> keyFields = uniqueConstraintSchema.Columns;
                        for (int columnIndex = 0; columnIndex < keyFields.Count; columnIndex++)
                        {
                            ColumnSchema columnSchema = keyFields[columnIndex];
                            streamWriter.Write("[{0}]", columnSchema.Name);
                            streamWriter.Write(columnIndex == keyFields.Count - 1 ? string.Empty : ",");
                        }

                        streamWriter.WriteLine(")");
                    }
                }
            }
        }

        /// <summary>
        /// Generate the indices on a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableSchema">The schema description of the table.</param>
        private static void CreateIndices(StreamWriter streamWriter, TableSchema tableSchema)
        {
            // Put all the non-nullable unique keys into a list.
            foreach (ConstraintSchema constraintSchema in tableSchema.Constraints)
            {
                UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
                if (uniqueConstraintSchema != null)
                {
                    if (!uniqueConstraintSchema.IsPrimaryKey)
                    {
                        streamWriter.Write("CREATE INDEX [{0}] ON [{1}] (", uniqueConstraintSchema.Name, tableSchema.Name);
                        List<ColumnSchema> keyFields = uniqueConstraintSchema.Columns;
                        for (int columnIndex = 0; columnIndex < keyFields.Count; columnIndex++)
                        {
                            ColumnSchema columnSchema = keyFields[columnIndex];
                            streamWriter.Write("[{0}]", columnSchema.Name);
                            streamWriter.Write(columnIndex == keyFields.Count - 1 ? string.Empty : ",");
                        }

                        streamWriter.WriteLine(")");
                        streamWriter.WriteLine("go");
                    }
                }
            }
        }
    }
}