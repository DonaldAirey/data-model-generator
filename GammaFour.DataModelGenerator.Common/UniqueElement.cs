// <copyright file="UniqueElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Xml.Linq;

    /// <summary>
    /// Describes a set of columns that must be unique in a table.
    /// </summary>
    public class UniqueElement : ConstraintElement
    {
        /// <summary>
        /// A value indicating whether gets an indication of whether the constraint is the primary key on a table.
        /// </summary>
        private readonly bool isPrimaryKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueElement"/> class.
        /// </summary>
        /// <param name="uniqueElement">The description of the unique constraint.</param>
        public UniqueElement(XElement uniqueElement)
            : base(uniqueElement)
        {
            // Parse out the primary key attribute.
            XAttribute isPrimaryKeyAttribute = this.Attribute(XmlSchemaDocument.IsPrimaryKeyName);
            this.isPrimaryKey = isPrimaryKeyAttribute == null ? false : bool.Parse(isPrimaryKeyAttribute.Value);
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether the constraint is the primary key on a table.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return this.isPrimaryKey;
            }
        }
    }
}