// <copyright file="ConstraintElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    /// <summary>
    /// Describes a constraint on a table in a data model.
    /// </summary>
    public class ConstraintElement : XElement, IComparable<ConstraintElement>
    {
        /// <summary>
        /// Used to parse the XPath specification from constraints.
        /// </summary>
        private static Regex xPath = new Regex(@"(\.//)?(\w+:)?(\w+)");

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintElement"/> class.
        /// </summary>
        /// <param name="xElement">The description of the constraint.</param>
        public ConstraintElement(XElement xElement)
            : base(xElement)
        {
            // Parse out the name of the constraint.
            this.Name = this.Attribute(XmlSchema.Name).Value;

            // Replace the undecorated columns with decorated ones.
            List<XElement> columns = this.Elements(XmlSchema.Field).ToList();
            foreach (XElement column in columns)
            {
                this.Add(new ColumnReferenceElement(column));
                column.Remove();
            }
        }

        /// <summary>
        /// Gets the columns in the constraint.
        /// </summary>
        public List<ColumnReferenceElement> Columns
        {
            get
            {
                return this.Elements(XmlSchema.Field).Cast<ColumnReferenceElement>().ToList();
            }
        }

        /// <summary>
        /// Gets the parent document.
        /// </summary>
        public XmlSchemaDocument XmlSchemaDocument
        {
            get
            {
                return this.Document as XmlSchemaDocument;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the constraint can contain null.
        /// </summary>
        public bool IsNullable
        {
            get
            {
                // If all the columns of a given constraint can contain null, then the constraint is nullable.
                return (from cre in this.Columns
                        where cre.Column.IsNullable
                        select cre).Count() == this.Columns.Count();
            }
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// Gets the table for which this key provides a constraint.
        /// </summary>
        public TableElement Table
        {
            get
            {
                // The location of the table is kept in an XPath specification which addresses the target document.  Since we're not actually parsing
                // a document with this schema, then the interpetation gets a little fuzzy.  We can't actually scan the source document with this
                // specification, but we can pull it apart to get the table name for which this constraint is intended.
                XElement selectorElement = this.Element(XmlSchema.Selector);
                XAttribute xPathAttribute = selectorElement.Attribute(XmlSchema.XPath);
                Match match = ConstraintElement.xPath.Match(xPathAttribute.Value);
                if (!match.Success)
                {
                    throw new InvalidOperationException("Unique Constraint references a non-existing table ${match[0]}");
                }

                // Select the table from the name in the XPath specification.
                return (from te in this.XmlSchemaDocument.Tables
                        where te.Name == match.Groups[match.Groups.Count - 1].Value
                        select te).Single();
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(ConstraintElement other)
        {
            return this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Returns a string that represents the current instance (object) of the <see cref="ConstraintElement"/> class.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}