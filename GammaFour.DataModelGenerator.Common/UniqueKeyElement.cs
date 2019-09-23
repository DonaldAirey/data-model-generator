// <copyright file="UniqueKeyElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2019 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Xml.Linq;

    /// <summary>
    /// Describes a set of columns that must be unique in a table.
    /// </summary>
    public class UniqueKeyElement : ConstraintElement
    {
        /// <summary>
        /// A value indicating whether gets an indication of whether the constraint is the primary key on a table.
        /// </summary>
        private bool isPrimaryKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueKeyElement"/> class.
        /// </summary>
        /// <param name="uniqueElement">The description of the unique constraint.</param>
        public UniqueKeyElement(XElement uniqueElement)
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