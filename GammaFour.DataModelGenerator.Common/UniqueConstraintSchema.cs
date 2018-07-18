// <copyright file="UniqueConstraintSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Xml.Linq;

    /// <summary>
    /// Describes a set of columns that must be unique in a table.
    /// </summary>
    public class UniqueConstraintSchema : ConstraintSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueConstraintSchema"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The parent data model schema to which this unique constraint belongs.</param>
        /// <param name="uniqueElement">The description of the unique constraint.</param>
        public UniqueConstraintSchema(DataModelSchema dataModelSchema, XElement uniqueElement)
            : base(dataModelSchema, uniqueElement)
        {
            // The primary key indicates that this is the key used to physically order the table.
            XAttribute isPrimaryKeyAttribute = uniqueElement.Attribute(XmlSchema.IsPrimaryKey);
            this.IsPrimaryKey = isPrimaryKeyAttribute == null ? false : bool.Parse(isPrimaryKeyAttribute.Value);
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether the constraint is the primary key on a table.
        /// </summary>
        public bool IsPrimaryKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the foreign key that can be used to find a child element using this unique constraint.
        /// </summary>
        public ForeignKeyConstraintSchema ForeignKey
        {
            get
            {
                // Search the foreign constraints to see if any of the column sets exactly match the column set of this constraint.
                foreach (ForeignKeyConstraintSchema foreignKeyConstraintSchema in this.Table.ForeignKeys)
                {
                    if (this.Table.GetUniqueConstraint(foreignKeyConstraintSchema.Columns) == this)
                    {
                        return foreignKeyConstraintSchema;
                    }
                }

                // At this point there are no foreign keys that can use this constraint to find a child element.
                return null;
            }
        }
    }
}