// <copyright file="ForeignIndexElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
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
    public class ForeignIndexElement : ConstraintElement
    {
        /// <summary>
        /// A unique key element.
        /// </summary>
        private UniqueIndexElement uniqueKeyElement;

        /// <summary>
        /// The name of the parent table.
        /// </summary>
        private string uniqueParentName;

        /// <summary>
        /// The unique name of the child table.
        /// </summary>
        private string uniqueChildName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignIndexElement"/> class.
        /// </summary>
        /// <param name="xElement">The description of the unique constraint.</param>
        public ForeignIndexElement(XElement xElement)
            : base(xElement)
        {
            // Initialize the object.
            this.Refer = this.Attribute(XmlSchemaDocument.ReferName).Value;
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
                return this.UniqueIndex.Columns;
            }
        }

        /// <summary>
        /// Gets the unique key that to which this foreign key refers.
        /// </summary>
        public UniqueIndexElement UniqueIndex
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
                        if (this.uniqueKeyElement == default(UniqueIndexElement))
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
                    var isDistinctPathToParent = (from fke in this.UniqueIndex.Table.ForeignIndices
                                                  where fke.Table == this.Table
                                                  select fke).Count() == 1;

                    // If the path to the parent is unique, then simply use the parent table's name.  If it not unique, we will decorate the name of the
                    // key with the columns that will make it unique.
                    if (isDistinctPathToParent)
                    {
                        this.uniqueParentName = this.UniqueIndex.Table.Name;
                    }
                    else
                    {
                        this.uniqueParentName = this.UniqueIndex.Table.Name + "By";
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
                    var isDistinctPathToChild = (from fke in this.UniqueIndex.Table.ForeignIndices
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
    }
}