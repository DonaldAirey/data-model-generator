// <copyright file="ForeignKeyElement.cs" company="Gamma Four, Inc.">
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
    public class ForeignKeyElement : ConstraintElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyElement"/> class.
        /// </summary>
        /// <param name="xElement">The description of the unique constraint.</param>
        public ForeignKeyElement(XElement xElement)
            : base(xElement)
        {
            // Initialize the object.
            this.Refer = this.Attribute(XmlSchemaDocument.Refer).Value;

            // Parse the cascading delete rule out of the specification.
            XAttribute deleteRuleAttribute = xElement.Attribute(XmlSchemaDocument.DeleteRule);
            this.DeleteRule = deleteRuleAttribute == null ?
                CascadeRule.Cascade : (CascadeRule)Enum.Parse(typeof(CascadeRule), deleteRuleAttribute.Value);

            // Parse the cascading update rule out of the specification.
            XAttribute updateRuleAttribute = xElement.Attribute(XmlSchemaDocument.UpdateRule);
            this.UpdateRule = updateRuleAttribute == null ?
                CascadeRule.None : (CascadeRule)Enum.Parse(typeof(CascadeRule), updateRuleAttribute.Value);
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is deleted.
        /// </summary>
        public CascadeRule DeleteRule { get; } = CascadeRule.Cascade;

        /// <summary>
        /// Gets the name of the unique key.
        /// </summary>
        public string Refer { get; private set; }

        /// <summary>
        /// Gets the parent columns.
        /// </summary>
        public List<ColumnReferenceElement> ParentColumns
        {
            get
            {
                return this.UniqueKey.Columns;
            }
        }

        /// <summary>
        /// Gets the unique key that to whic this foreign key refers.
        /// </summary>
        public UniqueKeyElement UniqueKey
        {
            get
            {
                var uniqueKeyElement = (from uk in this.XmlSchemaDocument.UniqueKeys
                                        where uk.Name == this.Refer
                                        select uk).SingleOrDefault();
                if (uniqueKeyElement == default(UniqueKeyElement))
                {
                    throw new InvalidOperationException($"Foreign key constraint {this.Name} can't find referenced unique key constraint {this.Refer}");
                }

                return uniqueKeyElement;
            }
        }

        /// <summary>
        /// Gets an identifier that can be used to uniquely reference the parent table.
        /// </summary>
        public string UniqueParentName
        {
            get
            {
                // Determinesd if there is a distinct path to the parent table.
                var isDistinctPathToParent = (from fke in this.UniqueKey.Table.ForeignKeys
                                              where fke.Table == this.Table
                                              select fke).Count() == 1;

                // If the path to the parent is unique, then simply use the parent table's name.  If it not unique, we will decorate the name of the
                // key with the columns that will make it unique.
                string parentIdentifier = default(string);
                if (isDistinctPathToParent)
                {
                    parentIdentifier = this.UniqueKey.Table.Name;
                }
                else
                {
                    parentIdentifier = this.UniqueKey.Table.Name + "By";
                    foreach (ColumnReferenceElement columnReferenceElement in this.Columns)
                    {
                        parentIdentifier += columnReferenceElement.Column.Name;
                    }
                }

                return parentIdentifier;
            }
        }

        /// <summary>
        /// Gets an identifier that can be used to uniquely reference the parent table.
        /// </summary>
        public string UniqueChildName
        {
            get
            {
                // Determines if there's a single path to the child tables.
                var isDistinctPathToChild = (from fke in this.UniqueKey.Table.ForeignKeys
                                             where fke.Table == this.Table
                                             select fke).Count() == 1;

                // If the path to the parent is unique, then simply use the parent table's name.  If it not unique, we will decorate the name of the
                // key with the columns that will make it unique.
                string childIdentifier = default(string);
                if (isDistinctPathToChild)
                {
                    childIdentifier = this.Table.Name.ToPlural();
                }
                else
                {
                    childIdentifier = this.Table.Name.ToPlural() + "By";
                    foreach (ColumnReferenceElement columnReferenceElement in this.Columns)
                    {
                        childIdentifier += columnReferenceElement.Column.Name;
                    }
                }

                return childIdentifier;
            }
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is updated.
        /// </summary>
        /// <value>The rule that describes what happens to child records when a record is updated.</value>
        public CascadeRule UpdateRule { get; } = CascadeRule.Cascade;
    }
}