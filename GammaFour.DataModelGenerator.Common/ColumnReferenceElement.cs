// <copyright file="ColumnReferenceElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
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
        private static Regex xPath = new Regex(@"(\w+:|@)?(\w+)");

        /// <summary>
        /// The name of the underlying column.
        /// </summary>
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnReferenceElement"/> class.
        /// </summary>
        /// <param name="xElement">The XML annotations of the table.</param>
        public ColumnReferenceElement(XElement xElement)
            : base(xElement)
        {
            // Pull the column name out of the XPath.
            Match match = ColumnReferenceElement.xPath.Match(this.Attribute(XmlSchema.XPath).Value);
            this.name = match.Groups[match.Groups.Count - 1].Value;
        }

        /// <summary>
        /// Gets the column to which this element refers.
        /// </summary>
        public ColumnElement Column
        {
            get
            {
                ConstraintElement parentConstraint = this.Parent as ConstraintElement;
                ColumnElement columnElement = (from ce in parentConstraint.Table.Columns
                                               where ce.Name == this.name
                                               select ce).SingleOrDefault();
                if (columnElement == default(ColumnElement))
                {
                    throw new InvalidOperationException($"XSD Parsing error: Unable to find match the columns in foreign key index {parentConstraint.Name} to {this.name}");
                }

                return columnElement;
            }
        }
    }
}