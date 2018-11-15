// <copyright file="Tables.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Database
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
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
        /// <param name="tableElement">The schema description of the table.</param>
        internal static void Generate(StreamWriter streamWriter, TableElement tableElement)
        {
            // The table is described here.
            streamWriter.WriteLine(string.Format("CREATE TABLE [dbo].[{0}]", tableElement.Name));
            streamWriter.WriteLine("(");

            // Generate each of the column descriptions.
            foreach (ColumnElement columnElement in tableElement.Columns)
            {
                streamWriter.WriteLine(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "    [{0}] {1}{2},",
                        columnElement.Name,
                        SchemaUnit.GetSqlDataType(columnElement),
                        columnElement.IsNullable ? " NULL" : " NOT NULL"));
            }

            // Generate the unique keys for this table.
            Tables.CreateKeys(streamWriter, tableElement);
            streamWriter.WriteLine(")");
            streamWriter.WriteLine("go");

            // Generate the additional indices.
            Tables.CreateIndices(streamWriter, tableElement);
        }

        /// <summary>
        /// Generate the keys on a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableElement">The schema description of the table.</param>
        private static void CreateKeys(StreamWriter streamWriter, TableElement tableElement)
        {
            // Put all the unique, non-nullable keys into a list.
            foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
            {
                if (uniqueKeyElement.IsPrimaryKey)
                {
                    streamWriter.Write("	CONSTRAINT [{0}] PRIMARY KEY (", uniqueKeyElement.Name);
                    List<ColumnReferenceElement> keyFields = uniqueKeyElement.Columns;
                    for (int columnIndex = 0; columnIndex < keyFields.Count; columnIndex++)
                    {
                        ColumnElement columnElement = keyFields[columnIndex].Column;
                        streamWriter.Write("[{0}]", columnElement.Name);
                        streamWriter.Write(columnIndex == keyFields.Count - 1 ? string.Empty : ",");
                    }

                    streamWriter.WriteLine(")");
                }
            }
        }

        /// <summary>
        /// Generate the indices on a table.
        /// </summary>
        /// <param name="streamWriter">The file to which the DDL is written.</param>
        /// <param name="tableElement">The schema description of the table.</param>
        private static void CreateIndices(StreamWriter streamWriter, TableElement tableElement)
        {
            // Put all the non-nullable unique keys into a list.
            foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
            {
                if (!uniqueKeyElement.IsPrimaryKey)
                {
                    streamWriter.Write("CREATE INDEX [{0}] ON [{1}] (", uniqueKeyElement.Name, tableElement.Name);
                    List<ColumnReferenceElement> keyFields = uniqueKeyElement.Columns;
                    for (int columnIndex = 0; columnIndex < keyFields.Count; columnIndex++)
                    {
                        ColumnElement columnElement = keyFields[columnIndex].Column;
                        streamWriter.Write("[{0}]", columnElement.Name);
                        streamWriter.Write(columnIndex == keyFields.Count - 1 ? string.Empty : ",");
                    }

                    streamWriter.WriteLine(")");
                    streamWriter.WriteLine("go");
                }
            }
        }
    }
}