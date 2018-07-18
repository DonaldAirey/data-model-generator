// <copyright file="ConstraintSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Describes a constraint on a table in a data model.
    /// </summary>
    public class ConstraintSchema : IComparable<ConstraintSchema>
    {
        /// <summary>
        /// The columns in the constraint.
        /// </summary>
        private List<ColumnSchema> columns = new List<ColumnSchema>();

        /// <summary>
        /// The parent data model schema.
        /// </summary>
        private DataModelSchema dataModelSchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintSchema"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The parent data model schema to which this constraint belongs.</param>
        /// <param name="constraintElement">The description of the constraint.</param>
        public ConstraintSchema(DataModelSchema dataModelSchema, XElement constraintElement)
        {
            // Initialize the object.
            this.dataModelSchema = dataModelSchema;

            // Parse out the name of the constraint.
            this.Name = constraintElement.Attribute(XmlSchema.Name).Value;
            this.CamelCaseName = CommonConversion.ToCamelCase(this.Name);

            // Extract the table name from the 'selector' element.
            XElement selectorElement = constraintElement.Element(XmlSchema.Selector);
            XAttribute xPathAttribute = selectorElement.Attribute(XmlSchema.XPath);
            string tableName = ConstraintSchema.ParseXPath(xPathAttribute.Value);
            this.Table = dataModelSchema.Tables.First<TableSchema>(ts => ts.Name == tableName);

            // This will parse out the columns from one or more 'field' elements.
            foreach (XElement fieldElement in constraintElement.Elements(XmlSchema.Field))
            {
                xPathAttribute = fieldElement.Attribute(XmlSchema.XPath);
                string columnName = ConstraintSchema.ParseXPath(xPathAttribute.Value);
                this.Columns.Add(this.Table.Columns.First<ColumnSchema>(c => c.Name == columnName));
            }

            // If all the columns of a given constraint can contain null, then the constraint is nullable.
            int nullColumnCount = 0;
            foreach (ColumnSchema columnSchema in this.columns)
            {
                if (columnSchema.IsNullable)
                {
                    nullColumnCount++;
                }
            }

            this.IsNullable = this.columns.Count == nullColumnCount;
        }

        /// <summary>
        /// Gets the camel-case name of the constraint.
        /// </summary>
        public string CamelCaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the columns in the constraint.
        /// </summary>
        public List<ColumnSchema> Columns
        {
            get
            {
                return this.columns;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets or sets an indication of whether the constraint can contain a null.
        /// </summary>
        public bool IsNullable
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the constraint.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the table to which the constraint is applied.
        /// </summary>
        public TableSchema Table
        {
            get;
            internal set;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(ConstraintSchema other)
        {
            return this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Returns a string that represents the current instance (object) of the <see cref="ConstraintSchema"/> class.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Parse a value out of an XPath specification.
        /// </summary>
        /// <param name="xPath">The text of an XPath specification.</param>
        /// <returns>The table or column name embedded in the XPath.</returns>
        private static string ParseXPath(string xPath)
        {
            string[] xPathParts = xPath.Split(':');
            return xPathParts[1];
        }
    }
}