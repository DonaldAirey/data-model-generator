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
            this.Refer = this.Attribute(XmlSchema.Refer).Value;

            // Parse the cascading delete rule out of the specification.
            XAttribute deleteRuleAttribute = xElement.Attribute(XmlSchema.DeleteRule);
            this.DeleteRule = deleteRuleAttribute == null ?
                CascadeRules.Cascade : (CascadeRules)Enum.Parse(typeof(CascadeRules), deleteRuleAttribute.Value);

            // Parse the cascading update rule out of the specification.
            XAttribute updateRuleAttribute = xElement.Attribute(XmlSchema.UpdateRule);
            this.UpdateRule = updateRuleAttribute == null ?
                CascadeRules.None : (CascadeRules)Enum.Parse(typeof(CascadeRules), updateRuleAttribute.Value);
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is deleted.
        /// </summary>
        public CascadeRules DeleteRule { get; } = CascadeRules.Cascade;

        /// <summary>
        /// Gets a value indicating whether there is a single or multiple paths from the child to the parent table.
        /// </summary>
        /// <value>An indication of whether there is a single or multiple paths from the child to the parent table.</value>
        public bool IsDistinctPathToParent
        {
            get
            {
                return (from fke in this.Table.ForeignKeys
                        where fke.UniqueKey.Table == this.UniqueKey.Table && fke != this
                        select fke).Count() != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is a single or multiple paths from the parent to the child table.
        /// </summary>
        /// <value>An indication of whether there is a single or multiple paths from the parent to the child table.</value>
        public bool IsDistinctPathToChild
        {
            get
            {
                return (from fke in this.UniqueKey.Table.ForeignKeys
                        where fke.Table == this.Table && fke != this
                        select fke).Count() != 0;
            }
        }

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
                return (from uk in this.XmlSchemaDocument.UniqueKeys
                        where uk.Name == this.Refer
                        select uk).Single();
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
                    parentIdentifier = this.UniqueKey.Table.Name.ToCamelCase() + "Key";
                }
                else
                {
                    parentIdentifier = this.UniqueKey.Table.Name.ToCamelCase() + "By";
                    foreach (ColumnReferenceElement columnReferenceElement in this.ParentColumns)
                    {
                        parentIdentifier += columnReferenceElement.Column.Name;
                    }

                    parentIdentifier += "Key";
                }

                return parentIdentifier;
            }
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is updated.
        /// </summary>
        /// <value>The rule that describes what happens to child records when a record is updated.</value>
        public CascadeRules UpdateRule { get; } = CascadeRules.Cascade;
    }
}