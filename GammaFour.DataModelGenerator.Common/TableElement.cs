// <copyright file="TableElement.cs" company="Gamma Four, Inc.">
//     Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// A description of a table.
    /// </summary>
    public class TableElement : XElement, IComparable<TableElement>
    {
        /// <summary>
        /// The columns.
        /// </summary>
        private List<ColumnElement> columnElements;

        /// <summary>
        /// The foreign key elements.
        /// </summary>
        private List<ForeignIndexElement> foreignKeyElements;

        /// <summary>
        /// The index of the table.
        /// </summary>
        private int? index;

        /// <summary>
        /// The child foreign key elements.
        /// </summary>
        private List<ForeignIndexElement> childKeyElements;

        /// <summary>
        /// The parent key elements.
        /// </summary>
        private List<ForeignIndexElement> parentKeyElements;

        /// <summary>
        /// The primary key element.
        /// </summary>
        private UniqueIndexElement primaryKeyElement;

        /// <summary>
        /// The unique key elements.
        /// </summary>
        private List<UniqueIndexElement> uniqueKeyElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElement"/> class.
        /// </summary>
        /// <param name="xElement">The element that describes the table.</param>
        public TableElement(XElement xElement)
            : base(xElement)
        {
            // Extract the name from the schema.
            this.Name = this.Attribute(XmlSchemaDocument.ObjectName).Value;

            // This tells us whether the table is persisted in a database or not.
            XAttribute isVolatileAttribute = this.Attribute(XmlSchemaDocument.IsVolatileName);
            this.IsVolatile = isVolatileAttribute == null ? false : Convert.ToBoolean(isVolatileAttribute.Value, CultureInfo.InvariantCulture);

            // This will navigate to the sequence of columns.
            XElement complexType = this.Element(XmlSchemaDocument.ComplexTypeName);
            XElement sequence = complexType.Element(XmlSchemaDocument.SequenceName);

            // Every table has an implicit row version column to track changes.
            sequence.Add(
                new XElement(
                    XmlSchemaDocument.ElementName,
                    new XAttribute("name", "RowVersion"),
                    new XAttribute(XmlSchemaDocument.IsRowVersionName, "true"),
                    new XAttribute("type", "xs:long")));

            // This will replace each of the undecorated elements with decorated ones.
            List<XElement> columnElements = sequence.Elements(XmlSchemaDocument.ElementName).ToList();
            foreach (XElement columnElement in columnElements)
            {
                sequence.Add(new ColumnElement(columnElement));
                columnElement.Remove();
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public List<ColumnElement> Columns
        {
            get
            {
                if (this.columnElements == null)
                {
                    XElement complexType = this.Element(XmlSchemaDocument.ComplexTypeName);
                    XElement sequence = complexType.Element(XmlSchemaDocument.SequenceName);
                    this.columnElements = sequence.Elements(XmlSchemaDocument.ElementName).Cast<ColumnElement>().ToList();
                }

                return this.columnElements;
            }
        }

        /// <summary>
        /// Gets the ForeignKey constraints.
        /// </summary>
        public List<ForeignIndexElement> ForeignKeys
        {
            get
            {
                if (this.foreignKeyElements == null)
                {
                    this.foreignKeyElements = (from fke in this.XmlSchemaDocument.ForeignKeys
                                               where fke.UniqueIndex.Table == this
                                               select fke).ToList();
                }

                return this.foreignKeyElements;
            }
        }

        /// <summary>
        /// Gets the index of this table in the list of tables.
        /// </summary>
        public int Index
        {
            get
            {
                if (!this.index.HasValue)
                {
                    this.index = this.XmlSchemaDocument.Tables.IndexOf(this);
                }

                return this.index.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the table supports write-through operations to a persistent store.
        /// </summary>
        public bool IsVolatile { get; private set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// Gets the foreign keys which are children of this table.
        /// </summary>
        public List<ForeignIndexElement> ChildKeys
        {
            get
            {
                if (this.childKeyElements == null)
                {
                    this.childKeyElements = (from fke in this.XmlSchemaDocument.ForeignKeys
                                             where fke.UniqueIndex.Table == this
                                             orderby fke.Name
                                             select fke).ToList();
                }

                return this.childKeyElements;
            }
        }

        /// <summary>
        /// Gets the foreign keys which are the parents of this table.
        /// </summary>
        public List<ForeignIndexElement> ParentKeys
        {
            get
            {
                if (this.parentKeyElements == null)
                {
                    this.parentKeyElements = (from fke in this.XmlSchemaDocument.ForeignKeys
                                              where fke.Table == this
                                              orderby fke.Name
                                              select fke).ToList();
                }

                return this.parentKeyElements;
            }
        }

        /// <summary>
        /// Gets the primary key on this table.
        /// </summary>
        public UniqueIndexElement PrimaryIndex
        {
            get
            {
                if (this.primaryKeyElement == null)
                {
                    this.primaryKeyElement = (from uk in this.UniqueIndexes
                                              where uk.IsPrimaryIndex
                                              select uk).FirstOrDefault();
                }

                return this.primaryKeyElement;
            }
        }

        /// <summary>
        /// Gets the unique constraints.
        /// </summary>
        public List<UniqueIndexElement> UniqueIndexes
        {
            get
            {
                if (this.uniqueKeyElements == null)
                {
                    this.uniqueKeyElements = (from uke in this.XmlSchemaDocument.UniqueKeys
                                              where uke.Table == this
                                              orderby uke.Name
                                              select uke).ToList();
                }

                return this.uniqueKeyElements;
            }
        }

        /// <summary>
        /// Gets the owner document.
        /// </summary>
        public XmlSchemaDocument XmlSchemaDocument
        {
            get
            {
                return this.Document as XmlSchemaDocument;
            }
        }

        /// <summary>
        /// Equals operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the two operands are equal, false otherwise.</returns>
        public static bool operator ==(TableElement left, TableElement right)
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
        public static bool operator !=(TableElement left, TableElement right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Less Than operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is less than the right operand, false otherwise.</returns>
        public static bool operator <(TableElement left, TableElement right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Less Than or Equal To operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is less than the right operand, false otherwise.</returns>
        public static bool operator <=(TableElement left, TableElement right)
        {
            return Compare(left, right) <= 0;
        }

        /// <summary>
        /// Greater Than operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is greater than the right operand, false otherwise.</returns>
        public static bool operator >(TableElement left, TableElement right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Greater Than or Equal To operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is greater than the right operand, false otherwise.</returns>
        public static bool operator >=(TableElement left, TableElement right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Compares two <see cref="TableElement"/> records.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>-1 if left &lt; right, 0 if left == right, 1 if left &gt; right.</returns>
        public static int Compare(TableElement left, TableElement right)
        {
            // Don't use operators or you'll recurse.  If the left and right objects are the same object, then they're equal.
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }

            // The left operand can never be equal to null.
            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }

            // Reference checking done.  This will compare the names to see if they're the same.
            return left.CompareTo(right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            // Comparing against null will always be false.
            TableElement other = obj as TableElement;
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
        public int CompareTo(TableElement other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}