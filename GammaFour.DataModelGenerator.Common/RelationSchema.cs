// <copyright file="RelationSchema.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Creates relationship that connects two tables.
    /// </summary>
    public class RelationSchema : IComparable<RelationSchema>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationSchema"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The parent data model schema.</param>
        /// <param name="keyrefElement">A description of the relation.</param>
        public RelationSchema(DataModelSchema dataModelSchema, XElement keyrefElement)
        {
            // Parse out the name of the constraint.
            this.Name = keyrefElement.Attribute(XmlSchema.Name).Value;
            this.CamelCaseName = CommonConversion.ToCamelCase(keyrefElement.Attribute(XmlSchema.Name).Value);

            // Parse out the parent key.
            XAttribute referAttribute = keyrefElement.Attribute(XmlSchema.Refer);
            string refer = referAttribute.Value;

            // This will search through each of the tables looking for the parent and child components of the relation.
            foreach (TableSchema tableSchema in dataModelSchema.Tables)
            {
                // This is the parent component of the relation.
                ConstraintSchema constraintSchema = tableSchema.Constraints.FirstOrDefault<ConstraintSchema>(cs => cs.Name == refer);
                if (constraintSchema != null)
                {
                    UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
                    this.ParentColumns = uniqueConstraintSchema.Columns;
                    this.ParentKeyConstraint = uniqueConstraintSchema;
                    this.ParentTable = uniqueConstraintSchema.Table;
                }

                // This is the child part of the relation.
                constraintSchema = tableSchema.Constraints.FirstOrDefault<ConstraintSchema>(cs => cs.Name == this.Name);
                if (constraintSchema != null)
                {
                    ForeignKeyConstraintSchema foreignKeyConstraintSchema = constraintSchema as ForeignKeyConstraintSchema;
                    this.ChildColumns = foreignKeyConstraintSchema.Columns;
                    this.ChildKeyConstraint = foreignKeyConstraintSchema;
                    this.ChildTable = foreignKeyConstraintSchema.Table;
                }
            }
        }

        /// <summary>
        /// Gets the camel-case name of the relation.
        /// </summary>
        public string CamelCaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the constraint that binds the parent table to the child table.
        /// </summary>
        public ForeignKeyConstraintSchema ChildKeyConstraint
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent columns of this constraint.
        /// </summary>
        public List<ColumnSchema> ChildColumns
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent table of this constraint.
        /// </summary>
        public TableSchema ChildTable
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the relation.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the UniqueConstraintSchema that guarantees that values in the parent column of a RelationSchema are unique.
        /// </summary>
        public UniqueConstraintSchema ParentKeyConstraint
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent columns of this constraint.
        /// </summary>
        public List<ColumnSchema> ParentColumns
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent table of this constraint.
        /// </summary>
        public TableSchema ParentTable
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether there is a single or multiple paths from the child to the parent table.
        /// </summary>
        /// <value>
        /// An indication of whether there is a single or multiple paths from the child to the parent table.
        /// </value>
        public bool IsDistinctPathToParent
        {
            get
            {
                // If any of the parent tables are the same then the path is not distinct.
                foreach (RelationSchema relationSchema in this.ChildTable.ParentRelations)
                {
                    if (relationSchema.ParentTable == this.ParentTable && relationSchema.Name != this.Name)
                    {
                        return false;
                    }
                }

                // There is only one path from the parent to the child table.
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether there is a single or multiple paths from the parent to the child table.
        /// </summary>
        public bool IsDistinctPathToChild
        {
            get
            {
                // If any of the child tables are the same then the path is not distinct.
                foreach (RelationSchema relationSchema in this.ParentTable.ChildRelations)
                {
                    if (relationSchema.ChildTable == this.ChildTable && relationSchema.Name != this.Name)
                    {
                        return false;
                    }
                }

                // There is only one path from the parent table to the child.
                return true;
            }
        }

        /// <summary>
        /// Gets an identifier that can be used to uniquely reference the parent table.
        /// </summary>
        public string UniqueParentName
        {
            get
            {
                // If the path to the parent is unique, then simply use the parent table's name.  If it not unique, we will decorate the name of the
                // key with the columns that will make it unique.
                string parentIdentifier = default(string);
                if (this.IsDistinctPathToParent)
                {
                    parentIdentifier = this.ParentTable.CamelCaseName + "Key";
                }
                else
                {
                    parentIdentifier = this.ParentTable.CamelCaseName + "By";
                    foreach (ColumnSchema columnSchema in this.ChildColumns)
                    {
                        parentIdentifier += columnSchema.Name;
                    }

                    parentIdentifier += "Key";
                }

                return parentIdentifier;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(RelationSchema other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}