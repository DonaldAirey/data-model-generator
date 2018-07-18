// <copyright file="DataModelSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// A description of a data model.
    /// </summary>
    public class DataModelSchema
    {
        /// <summary>
        /// The relations of one table to another.
        /// </summary>
        private List<RelationSchema> relations = new List<RelationSchema>();

        /// <summary>
        /// The tables.
        /// </summary>
        private List<TableSchema> tables = new List<TableSchema>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataModelSchema"/> class.
        /// </summary>
        /// <param name="fileContents">The contents of a file that specifies the schema in XML.</param>
        /// <param name="targetNamespace">The namespace for the generated code.</param>
        public DataModelSchema(string fileContents, string targetNamespace)
        {
            // Initialize the object.
            this.TargetNamespace = targetNamespace;

            // Load the XMLSchema.
            XDocument xDocument = null;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContents)))
            {
                xDocument = XDocument.Load(stream);
            }

            // Parse out the name of the target data model.
            XElement dataModelElement = xDocument.Root.Element(XmlSchema.Element);
            this.Name = dataModelElement.Attribute("name").Value;
            this.CamelCaseName = CommonConversion.ToCamelCase(this.Name);

            // The data model description is found on the first element of the first complex type in the module.
            XElement complexTypeElement = dataModelElement.Element(XmlSchema.ComplexType);
            XElement choiceElement = complexTypeElement.Element(XmlSchema.Choice);

            // From the data model description, create a description of each of the tables.
            foreach (XElement tableElement in choiceElement.Elements(XmlSchema.Element))
            {
                TableSchema tableSchema = new TableSchema(this, tableElement);
                int index = ~this.tables.BinarySearch(tableSchema);
                this.tables.Insert(index, tableSchema);
            }

            // Provide each of the tables with an index.  Note that, because of sorting, this can't be done in the above loop because the order
            // hasn't been established.
            for (int index = 0; index < this.tables.Count; index++)
            {
                this.tables[index].Index = index;
            }

            // From the data model description, create a description of each of the unique constraints (keys).
            foreach (XElement uniqueConstraintElement in dataModelElement.Elements(XmlSchema.Unique))
            {
                UniqueConstraintSchema uniqueConstraintSchema = new UniqueConstraintSchema(this, uniqueConstraintElement);
                uniqueConstraintSchema.Table.Add(uniqueConstraintSchema);
            }

            // From the data model description, create a description of the foreign key constraints and the relation between the tables.
            foreach (XElement keyrefElement in dataModelElement.Elements(XmlSchema.Keyref))
            {
                // This will create the foreign key constraints.
                ForeignKeyConstraintSchema foreignKeyConstraintSchema = new ForeignKeyConstraintSchema(this, keyrefElement);
                foreignKeyConstraintSchema.Table.Add(foreignKeyConstraintSchema);

                // This will create relations between the tables.
                XAttribute isConstraintOnlyAttribute = keyrefElement.Attribute(XmlSchema.ConstraintOnly);
                if (isConstraintOnlyAttribute == null || !Convert.ToBoolean(isConstraintOnlyAttribute.Value))
                {
                    // Add the relation description to the parent and child tables.
                    RelationSchema relationSchema = new RelationSchema(this, keyrefElement);
                    relationSchema.ParentTable.Add(relationSchema);
                    relationSchema.ChildTable.Add(relationSchema);

                    // Add the description of the relation to the data model.
                    int index = this.relations.BinarySearch(relationSchema);
                    this.relations.Insert(~index, relationSchema);
                }
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
        /// Gets the name of the data model.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the relations between parent and child tables in the data model.
        /// </summary>
        public IReadOnlyList<RelationSchema> Relations
        {
            get
            {
                return this.relations;
            }
        }

        /// <summary>
        /// Gets a sorted list of the tables in the data model schema.
        /// </summary>
        public IReadOnlyList<TableSchema> Tables
        {
            get
            {
                return this.tables;
            }
        }

        /// <summary>
        /// Gets the target namespace for the generated data model.
        /// </summary>
        public string TargetNamespace
        {
            get;
            private set;
        }
    }
}