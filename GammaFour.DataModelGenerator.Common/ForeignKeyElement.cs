// <copyright file="ForeignKeyElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
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
        /// A unique key element.
        /// </summary>
        private UniqueKeyElement uniqueKeyElement;

        /// <summary>
        /// The name of the parent table.
        /// </summary>
        private string uniqueParentName;

        /// <summary>
        /// The unique name of the child table.
        /// </summary>
        private string uniqueChildName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyElement"/> class.
        /// </summary>
        /// <param name="xElement">The description of the unique constraint.</param>
        public ForeignKeyElement(XElement xElement)
            : base(xElement)
        {
            // Initialize the object.
            this.Refer = this.Attribute(XmlSchemaDocument.ReferName).Value;

            // Parse the cascading delete rule out of the specification.
            XAttribute deleteRuleAttribute = xElement.Attribute(XmlSchemaDocument.DeleteRuleName);
            this.DeleteRule = deleteRuleAttribute == null ?
                CascadeRule.Cascade : (CascadeRule)Enum.Parse(typeof(CascadeRule), deleteRuleAttribute.Value);

            // Parse the cascading update rule out of the specification.
            XAttribute updateRuleAttribute = xElement.Attribute(XmlSchemaDocument.UpdateRuleName);
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
                if (this.uniqueKeyElement == null)
                {
                    try
                    {
                        this.uniqueKeyElement = (from uk in this.XmlSchemaDocument.UniqueKeys
                                                 where uk.Name == this.Refer
                                                 select uk).SingleOrDefault();
                        if (this.uniqueKeyElement == default(UniqueKeyElement))
                        {
                            throw new InvalidOperationException($"Foreign key constraint {this.Name} can't find referenced unique key constraint {this.Refer}");
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InvalidOperationException($"Foreign key constraint {this.Name} has multiple unique key constraint named {this.Refer}");
                    }
                }

                return this.uniqueKeyElement;
            }
        }

        /// <summary>
        /// Gets an identifier that can be used to uniquely reference the parent table.
        /// </summary>
        public string UniqueParentName
        {
            get
            {
                if (this.uniqueParentName == null)
                {
                    // Determinesd if there is a distinct path to the parent table.
                    var isDistinctPathToParent = (from fke in this.UniqueKey.Table.ForeignKeys
                                                  where fke.Table == this.Table
                                                  select fke).Count() == 1;

                    // If the path to the parent is unique, then simply use the parent table's name.  If it not unique, we will decorate the name of the
                    // key with the columns that will make it unique.
                    if (isDistinctPathToParent)
                    {
                        this.uniqueParentName = this.UniqueKey.Table.Name;
                    }
                    else
                    {
                        this.uniqueParentName = this.UniqueKey.Table.Name + "By";
                        foreach (ColumnReferenceElement columnReferenceElement in this.Columns)
                        {
                            this.uniqueParentName += columnReferenceElement.Column.Name;
                        }
                    }
                }

                return this.uniqueParentName;
            }
        }

        /// <summary>
        /// Gets an identifier that can be used to uniquely reference the parent table.
        /// </summary>
        public string UniqueChildName
        {
            get
            {
                if (this.uniqueChildName == null)
                {
                    // Determines if there's a single path to the child tables.
                    var isDistinctPathToChild = (from fke in this.UniqueKey.Table.ForeignKeys
                                                 where fke.Table == this.Table
                                                 select fke).Count() == 1;

                    // If the path to the parent is unique, then simply use the parent table's name.  If it not unique, we will decorate the name of the
                    // key with the columns that will make it unique.
                    if (isDistinctPathToChild)
                    {
                        this.uniqueChildName = this.Table.Name.ToPlural();
                    }
                    else
                    {
                        this.uniqueChildName = this.Table.Name.ToPlural() + "By";
                        foreach (ColumnReferenceElement columnReferenceElement in this.Columns)
                        {
                            this.uniqueChildName += columnReferenceElement.Column.Name;
                        }
                    }
                }

                return this.uniqueChildName;
            }
        }

        /// <summary>
        /// Gets the rule that describes what happens to child records when a record is updated.
        /// </summary>
        /// <value>The rule that describes what happens to child records when a record is updated.</value>
        public CascadeRule UpdateRule { get; } = CascadeRule.Cascade;
    }
}