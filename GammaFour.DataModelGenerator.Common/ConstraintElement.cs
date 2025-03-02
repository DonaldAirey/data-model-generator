// <copyright file="ConstraintElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
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
        private static readonly Regex XPath = new Regex(@"(\.//)?(\w+:)?(\w+)");

        /// <summary>
        /// The columns in this constraint.
        /// </summary>
        private List<ColumnReferenceElement> columns;

        /// <summary>
        /// The table element.
        /// </summary>
        private TableElement tableElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintElement"/> class.
        /// </summary>
        /// <param name="xElement">The description of the constraint.</param>
        public ConstraintElement(XElement xElement)
            : base(xElement)
        {
            // Parse out the name of the constraint.
            this.Name = this.Attribute(XmlSchemaDocument.ObjectName).Value;

            // Replace the undecorated columns with decorated ones.
            List<XElement> columns = this.Elements(XmlSchemaDocument.FieldName).ToList();
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
                if (this.columns == null)
                {
                    this.columns = this.Elements(XmlSchemaDocument.FieldName).Cast<ColumnReferenceElement>().ToList();
                }

                return this.columns;
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
        /// Gets the name of the index.
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// Gets the table for which this key provides a constraint.
        /// </summary>
        public TableElement Table
        {
            get
            {
                if (this.tableElement == null)
                {
                    // The location of the table is kept in an XPath specification which addresses the target document.  Since we're not actually parsing
                    // a document with this schema, then the interpetation gets a little fuzzy.  We can't actually scan the source document with this
                    // specification, but we can pull it apart to get the table name for which this constraint is intended.
                    XElement selectorElement = this.Element(XmlSchemaDocument.SelectorName);
                    XAttribute xPathAttribute = selectorElement.Attribute(XmlSchemaDocument.XPathName);
                    Match match = ConstraintElement.XPath.Match(xPathAttribute.Value);
                    if (!match.Success)
                    {
                        throw new InvalidOperationException($"Unique Constraint {this.Name} can't parse expression '{xPathAttribute.Value}'");
                    }

                    // Select the table from the name in the XPath specification.
                    this.tableElement = (from te in this.XmlSchemaDocument.Tables
                                         where te.Name == match.Groups[match.Groups.Count - 1].Value
                                         select te).FirstOrDefault();
                    if (this.tableElement == null)
                    {
                        throw new InvalidOperationException($"Constraint {this.Name} can't find referenced table {match.Groups[match.Groups.Count - 1]}");
                    }
                }

                return this.tableElement;
            }
        }

        /// <summary>
        /// Equals operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the two operands are equal, false otherwise.</returns>
        public static bool operator ==(ConstraintElement left, ConstraintElement right)
        {
            // Compare the left to the right.  Don't use operators or you'll recurse.
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Not Equals operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the two operands are not equal, false otherwise.</returns>
        public static bool operator !=(ConstraintElement left, ConstraintElement right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            // Comparing against null will always be false.
            ConstraintElement other = obj as ConstraintElement;
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Call the common method to compare the names.
            return this.CompareTo(other) == 0;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // The name is used to compare these objects.
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(ConstraintElement other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);
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