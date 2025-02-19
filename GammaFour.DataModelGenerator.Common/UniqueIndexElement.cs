// <copyright file="UniqueIndexElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Xml.Linq;

    /// <summary>
    /// Describes a set of columns that must be unique in a table.
    /// </summary>
    public class UniqueIndexElement : ConstraintElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueIndexElement"/> class.
        /// </summary>
        /// <param name="xElement">The description of the unique constraint.</param>
        public UniqueIndexElement(XElement xElement)
            : base(xElement)
        {
            // Parse out the primary key attribute.
            XAttribute isPrimaryIndexAttribute = this.Attribute(XmlSchemaDocument.IsPrimaryKeyName);
            this.IsPrimaryIndex = isPrimaryIndexAttribute == null ? false : bool.Parse(isPrimaryIndexAttribute.Value);
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether the constraint is the primary key on a table.
        /// </summary>
        public bool IsPrimaryIndex { get; }
    }
}