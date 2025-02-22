// <copyright file="ColumnReferenceElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    /// <summary>
    /// A description, found in constraints, of a reference to a column in a table.
    /// </summary>
    public class ColumnReferenceElement : XElement
    {
        /// <summary>
        /// Used to parse the XPath specification from constraints.
        /// </summary>
        private static readonly Regex XPath = new Regex(@"(\w+:|@)?(\w+)");

        /// <summary>
        /// The name of the underlying column.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The column element that this class references.
        /// </summary>
        private ColumnElement columnElement;

        /// <summary>
        /// The parent column.
        /// </summary>
        private ColumnElement parentColumn;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnReferenceElement"/> class.
        /// </summary>
        /// <param name="xElement">The XML annotations of the table.</param>
        public ColumnReferenceElement(XElement xElement)
            : base(xElement)
        {
            // Pull the column name out of the XPath.
            Match match = ColumnReferenceElement.XPath.Match(this.Attribute(XmlSchemaDocument.XPathName).Value);
            this.name = match.Groups[match.Groups.Count - 1].Value;
        }

        /// <summary>
        /// Gets the column to which this element refers.
        /// </summary>
        public ColumnElement Column
        {
            get
            {
                if (this.columnElement == null)
                {
                    ConstraintElement parentConstraint = this.Parent as ConstraintElement;
                    this.columnElement = (from ce in parentConstraint.Table.Columns
                                          where ce.Name == this.name
                                          select ce).SingleOrDefault();
                    if (this.columnElement == default(ColumnElement))
                    {
                        throw new InvalidOperationException($"The column {this.name} in constraint {parentConstraint.Name} doesn't exist in table {parentConstraint.Table.Name}.");
                    }
                }

                return this.columnElement;
            }
        }

        /// <summary>
        /// Gets the parent column.
        /// </summary>
        public ColumnElement ParentColumn
        {
            get
            {
                if (this.parentColumn == null)
                {
                    // If this is a forieng index then return the column in the parent table that corresponds to this column reference.
                    ForeignIndexElement foreignIndexElement = this.Parent as ForeignIndexElement;
                    if (foreignIndexElement != null)
                    {
                        this.parentColumn = foreignIndexElement.UniqueIndex.Columns[foreignIndexElement.Columns.IndexOf(this)].Column;
                    }
                }

                return this.parentColumn;
            }
        }
    }
}