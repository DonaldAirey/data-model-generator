// <copyright file="ForeignKeyConstraintSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Creates foreign key constraint on a table.
    /// </summary>
    public class ForeignKeyConstraintSchema : ConstraintSchema
    {
        /// <summary>
        /// The cascade rule for accepting/rejecting a record.
        /// </summary>
        private CascadeRules acceptRejectRule = CascadeRules.None;

        /// <summary>
        /// The cascade rule for deleting.
        /// </summary>
        private CascadeRules deleteRule = CascadeRules.Cascade;

        /// <summary>
        /// The corresponding columns in the parent table.
        /// </summary>
        private List<ColumnSchema> relatedColumns;

        /// <summary>
        /// The parent table.
        /// </summary>
        private TableSchema relatedTable;

        /// <summary>
        /// The cascade rule for updating.
        /// </summary>
        private CascadeRules updateRule = CascadeRules.Cascade;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyConstraintSchema"/> class.
        /// </summary>
        /// <param name="dataModelSchema">The parent data model schema to which this unique constraint belongs.</param>
        /// <param name="keyrefElement">The description of the unique constraint.</param>
        public ForeignKeyConstraintSchema(DataModelSchema dataModelSchema, XElement keyrefElement)
            : base(dataModelSchema, keyrefElement)
        {
            // This will find the unique constraint in the parent table.
            XAttribute referAttribute = keyrefElement.Attribute(XmlSchema.Refer);
            foreach (TableSchema tableSchema in dataModelSchema.Tables)
            {
                ConstraintSchema constraintSchema = tableSchema.Constraints.FirstOrDefault<ConstraintSchema>(
                    cs => cs.Name == referAttribute.Value);
                if (constraintSchema != null)
                {
                    this.relatedTable = constraintSchema.Table;
                    this.relatedColumns = constraintSchema.Columns;
                }
            }

            // Parse the cascading accept/reject rule out of the specification.
            XAttribute acceptRejectRuleAttribute = keyrefElement.Attribute(XmlSchema.AcceptRejectRule);
            this.acceptRejectRule = acceptRejectRuleAttribute == null ?
                CascadeRules.Cascade : (CascadeRules)Enum.Parse(typeof(CascadeRules), acceptRejectRuleAttribute.Value);

            // Parse the cascading delete rule out of the specification.
            XAttribute deleteRuleAttribute = keyrefElement.Attribute(XmlSchema.DeleteRule);
            this.deleteRule = deleteRuleAttribute == null ?
                CascadeRules.Cascade : (CascadeRules)Enum.Parse(typeof(CascadeRules), deleteRuleAttribute.Value);

            // Parse the cascading update rule out of the specification.
            XAttribute updateRuleAttribute = keyrefElement.Attribute(XmlSchema.UpdateRule);
            this.updateRule = updateRuleAttribute == null ?
                CascadeRules.None : (CascadeRules)Enum.Parse(typeof(CascadeRules), updateRuleAttribute.Value);
        }

        /// <summary>
        /// Gets the parent table of this constraint.
        /// </summary>
        public TableSchema RelatedTable
        {
            get
            {
                return this.relatedTable;
            }
        }

        /// <summary>
        /// Gets the parent columns of this constraint.
        /// </summary>
        public List<ColumnSchema> RelatedColumns
        {
            get
            {
                return this.relatedColumns;
            }
        }

        /// <summary>
        /// Gets the rule that describes what happens to the child record when added to the parent table.
        /// </summary>
        public CascadeRules AcceptRejectRule
        {
            get
            {
                return this.acceptRejectRule;
            }
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is deleted.
        /// </summary>
        public CascadeRules DeleteRule
        {
            get
            {
                return this.deleteRule;
            }
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is updated.
        /// </summary>
        /// <value>The rule that describes what happens to child records when a record is updated.</value>
        public CascadeRules UpdateRule
        {
            get
            {
                return this.updateRule;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether there is a single or multiple paths from the child to the parent table.
        /// </summary>
        /// <value>An indication of whether there is a single or multiple paths from the child to the parent table.</value>
        public bool IsDistinctPathToParent
        {
            get
            {
                // If any of the parent tables are the same then the path is not distinct.
                foreach (ConstraintSchema constraintSchema in this.Table.Constraints)
                {
                    ForeignKeyConstraintSchema foreignKeyConstraintSchema = constraintSchema as ForeignKeyConstraintSchema;
                    if (foreignKeyConstraintSchema != null)
                    {
                        if (foreignKeyConstraintSchema.RelatedTable == this.RelatedTable && foreignKeyConstraintSchema.Name != this.Name)
                        {
                            return false;
                        }
                    }
                }

                // There is only one path from the parent to the child table.
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether there is a single or multiple paths from the parent to the child table.
        /// </summary>
        /// <value>An indication of whether there is a single or multiple paths from the parent to the child table.</value>
        public bool IsDistinctPathToChild
        {
            get
            {
                // If any of the child tables are the same then the path is not distinct.
                foreach (ConstraintSchema constraintSchema in this.RelatedTable.Constraints)
                {
                    ForeignKeyConstraintSchema foreignKeyConstraintSchema = constraintSchema as ForeignKeyConstraintSchema;
                    if (foreignKeyConstraintSchema != null)
                    {
                        if (foreignKeyConstraintSchema.Table == this.Table && foreignKeyConstraintSchema.Name != this.Name)
                        {
                            return false;
                        }
                    }
                }

                // There is only one path from the parent table to the child.
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the foreign key is composed of other simpler foreign keys.
        /// </summary>
        /// <value>An indication of whether the foreign key is composed of other simpler foreign keys.</value>
        public bool IsRedundant
        {
            get
            {
                // This is a list of candidate columns that can be replaced with other columns from other foreign key relations.  Each sibling
                // foreign key relation will be examined to see if it contains the same columns as the ones collected here.  If a redundant column is
                // found, the one belonging to the dependent table will be removed.  A dependent table is one that also depends on one of the parent
                // of another sibling relation.
                List<ColumnSchema> keyColumns = new List<ColumnSchema>();
                foreach (ColumnSchema columnSchema in this.Columns)
                {
                    keyColumns.Add(columnSchema);
                }

                // If all of the child columns can be obtained from other relations, then this foreign key constraint is considered redundant.  All
                // the other sibling foreign key relations will be examined.  When a redundant column is found, the one belonging to a dependent
                // table is removed from the list.
                foreach (ForeignKeyConstraintSchema parentForeignKey in this.RelatedTable.ForeignKeys)
                {
                    foreach (ForeignKeyConstraintSchema childForeignKey in this.Table.ForeignKeys)
                    {
                        if (childForeignKey.Name != this.Name)
                        {
                            if (childForeignKey.RelatedTable.Name == parentForeignKey.RelatedTable.Name)
                            {
                                foreach (ColumnSchema columnSchema in childForeignKey.Columns)
                                {
                                    keyColumns.Remove(columnSchema);
                                }
                            }
                        }
                    }
                }

                // This foreign key is considered redundant when all the elements of the key can be obtained through other independent constraints.
                return keyColumns.Count == 0;
            }
        }
    }
}