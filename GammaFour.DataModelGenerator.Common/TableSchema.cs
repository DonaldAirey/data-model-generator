// <copyright file="TableSchema.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// A description of a table.
    /// </summary>
    public class TableSchema : IComparable<TableSchema>
    {
        /// <summary>
        /// The columns.
        /// </summary>
        private List<ColumnSchema> columns = new List<ColumnSchema>();

        /// <summary>
        /// The constraints.
        /// </summary>
        private List<ConstraintSchema> constraints = new List<ConstraintSchema>();

        /// <summary>
        /// The child relationships.
        /// </summary>
        private List<RelationSchema> childRelations = new List<RelationSchema>();

        /// <summary>
        /// The foreign keys.
        /// </summary>
        private List<ForeignKeyConstraintSchema> foreignKeys = new List<ForeignKeyConstraintSchema>();

        /// <summary>
        /// The parent relationships.
        /// </summary>
        private List<RelationSchema> parentRelations = new List<RelationSchema>();

        /// <summary>
        /// The unique constraints.
        /// </summary>
        private List<UniqueConstraintSchema> uniqueKeys = new List<UniqueConstraintSchema>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableSchema"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The parent data model schema to which this table belongs.</param>
        /// <param name="tableElement">The element that describes the table.</param>
        public TableSchema(DataModelSchema dataModelSchema, XElement tableElement)
        {
            // Initialize the object.
            string tableName = tableElement.Attribute(XmlSchema.Name).Value;
            this.CamelCaseName = CommonConversion.ToCamelCase(tableName);
            this.DataModel = dataModelSchema;
            this.Name = tableName;

            // If a table begins with the text 'Volatile', then the data will not be saved to the persistent store.
            this.IsPersistent = !this.Name.StartsWith("Volatile");

            // Every table has a row version column which tracks the history of changes to the row.
            this.columns.Add(new RowVersionColumnSchema(this));

            // The description of the columns are found as a sequence of elements that are part of a complex type.
            XElement complexType = tableElement.Element(XmlSchema.ComplexType);
            XElement sequence = complexType.Element(XmlSchema.Sequence);

            // This will parse each of the columns found in the description.
            foreach (XElement columnElement in sequence.Elements(XmlSchema.Element))
            {
                ColumnSchema columnSchema = new ColumnSchema(this, columnElement);
                int index = ~this.columns.BinarySearch(columnSchema);
                this.columns.Insert(index, columnSchema);
            }

            // Provide each of the columns with an index.  Note that, because of sorting, this can't be done in the loop above.
            for (int index = 0; index < this.columns.Count; index++)
            {
                this.columns[index].Index = index;
            }
        }

        /// <summary>
        /// Gets the camel-case name of the table.
        /// </summary>
        public string CamelCaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the child relationships.
        /// </summary>
        public IReadOnlyList<RelationSchema> ChildRelations
        {
            get
            {
                return this.childRelations;
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public IReadOnlyList<ColumnSchema> Columns
        {
            get
            {
                return this.columns;
            }
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        public IReadOnlyList<ConstraintSchema> Constraints
        {
            get
            {
                return this.constraints;
            }
        }

        /// <summary>
        /// Gets the parent DataModelSchema.
        /// </summary>
        public DataModelSchema DataModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index of the table.
        /// </summary>
        public int Index
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the ForeignKey constraints.
        /// </summary>
        public IReadOnlyList<ForeignKeyConstraintSchema> ForeignKeys
        {
            get
            {
                return this.foreignKeys;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication whether the table is written to a persistent store.
        /// </summary>
        public bool IsPersistent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent relations.
        /// </summary>
        public List<RelationSchema> ParentRelations
        {
            get
            {
                return this.parentRelations;
            }
        }

        /// <summary>
        /// Gets a description of the primary key on this table.
        /// </summary>
        public UniqueConstraintSchema PrimaryKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the unique constraints.
        /// </summary>
        public IReadOnlyList<UniqueConstraintSchema> UniqueKeys
        {
            get
            {
                return this.uniqueKeys;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(TableSchema other)
        {
            return this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Adds a constraint to the table.
        /// </summary>
        /// <param name="constraintSchema">The constraint to be added.</param>
        public void Add(ConstraintSchema constraintSchema)
        {
            // Foreign keys are separated into their own list.
            ForeignKeyConstraintSchema foreignKeyConstraintSchema = constraintSchema as ForeignKeyConstraintSchema;
            if (foreignKeyConstraintSchema != null)
            {
                int index = this.foreignKeys.BinarySearch(foreignKeyConstraintSchema);
                this.foreignKeys.Insert(~index, foreignKeyConstraintSchema);
            }

            // Unique keys are separated into their own list.  We also have a special place for the primary key.
            UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
            if (uniqueConstraintSchema != null)
            {
                int index = this.uniqueKeys.BinarySearch(uniqueConstraintSchema);
                this.uniqueKeys.Insert(~index, uniqueConstraintSchema);
                if (uniqueConstraintSchema.IsPrimaryKey)
                {
                    this.PrimaryKey = uniqueConstraintSchema;
                }
            }

            // All constraints on the table are place in this collection.
            this.constraints.Add(constraintSchema);
        }

        /// <summary>
        /// Adds a constraint to the table.
        /// </summary>
        /// <param name="relationSchema">The constraint to be added.</param>
        public void Add(RelationSchema relationSchema)
        {
            // If this table is the parent table, then add the relation to the child relations.
            if (relationSchema.ParentTable == this)
            {
                int index = this.childRelations.BinarySearch(relationSchema);
                this.childRelations.Insert(~index, relationSchema);
            }

            // If this table is the child table, then add this relation to the parent relations.
            if (relationSchema.ChildTable == this)
            {
                int index = this.parentRelations.BinarySearch(relationSchema);
                this.parentRelations.Insert(~index, relationSchema);
            }
        }

        /// <summary>
        /// Gets a unique constraint matching the given column set.
        /// </summary>
        /// <param name="columns">A key described as a set of ColumnSchemas.</param>
        /// <returns>The unique constraint matching the given columns or null if no such constraint exists.</returns>
        public UniqueConstraintSchema GetUniqueConstraint(IList<ColumnSchema> columns)
        {
            // Search for a unique constraint that matches the given column set exactly.
            foreach (ConstraintSchema constraintSchema in this.Constraints)
            {
                UniqueConstraintSchema uniqueConstraintSchema = constraintSchema as UniqueConstraintSchema;
                if (uniqueConstraintSchema != null)
                {
                    // There's no match if the number of columns between the two constraints can't agree.
                    if (uniqueConstraintSchema.Columns.Count != columns.Count)
                    {
                        return null;
                    }

                    // The order of the columns and the columns must match for this test.
                    bool isFound = true;
                    for (int columnIndex = 0; columnIndex < uniqueConstraintSchema.Columns.Count; columnIndex++)
                    {
                        if (uniqueConstraintSchema.Columns[columnIndex] != columns[columnIndex])
                        {
                            isFound = false;
                            break;
                        }
                    }

                    // At this point, a unique constraint matching the given set of columns has been found.
                    if (isFound)
                    {
                        return uniqueConstraintSchema;
                    }
                }
            }

            // At this point, all of the constraints have been examined and none of them contain the given column set.
            return null;
        }
    }
}