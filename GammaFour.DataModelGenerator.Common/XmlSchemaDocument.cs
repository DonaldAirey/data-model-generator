// <copyright file="XmlSchemaDocument.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// A description of a data model.
    /// </summary>
    public class XmlSchemaDocument : XDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSchemaDocument"/> class.
        /// </summary>
        /// <param name="fileContents">The contents of a file that specifies the schema in XML.</param>
        /// <param name="targetNamespace">The namespace for the generated code.</param>
        public XmlSchemaDocument(string fileContents, string targetNamespace)
            : this(XmlReader.Create(new StringReader(fileContents)))
        {
            // Initialize the object.
            this.TargetNamespace = targetNamespace;

            // The root element of the schema definition.
            XElement rootElement = this.Root.Element(XmlSchema.Element);

            // Extract the name from the schema.
            this.Name = this.Root.Element(XmlSchema.Element).Attribute("name").Value;

            // The data model description is found on the first element of the first complex type in the module.
            XElement complexTypeElement = rootElement.Element(XmlSchema.ComplexType);
            XElement choiceElement = complexTypeElement.Element(XmlSchema.Choice);

            // We're going to remove all the generic XElements from the section of the schema where the tables live and replace them with decorated
            // ones (that is, decorated with links to constraints, foreign keys, etc.)
            List<XElement> tables = choiceElement.Elements(XmlSchema.Element).ToList();
            foreach (XElement xElement in tables)
            {
                // This create a description of the table.
                choiceElement.Add(new TableElement(xElement));
                xElement.Remove();
            }

            // This will remove all the undecorated unique keys and replace them with decorated ones.
            List<XElement> uniqueElements = rootElement.Elements(XmlSchema.Unique).ToList();
            foreach (XElement xElement in uniqueElements)
            {
                // This creates the unique constraints.
                rootElement.Add(new UniqueKeyElement(xElement));
                xElement.Remove();
            }

            // From the data model description, create a description of the foreign key constraints and the relation between the tables.
            List<XElement> keyRefElements = rootElement.Elements(XmlSchema.Keyref).ToList();
            foreach (XElement xElement in keyRefElements)
            {
                // This will create the foreign key constraints.
                rootElement.Add(new ForeignKeyElement(xElement));
                xElement.Remove();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSchemaDocument"/> class.
        /// </summary>
        /// <param name="xmlReader">A <see cref="XmlReader"/> that contains the content for the <see cref="XDocument"/>.</param>
        private XmlSchemaDocument(XmlReader xmlReader)
            : base(XDocument.Load(xmlReader))
        {
            // Initialize the object.
            this.NamespaceManager = new XmlNamespaceManager(xmlReader.NameTable);
            this.NamespaceManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
        }

        /// <summary>
        /// Gets the constraint elements.
        /// </summary>
        public List<ForeignKeyElement> ForeignKeys
        {
            get
            {
                XElement rootElement = this.Root.Element(XmlSchema.Element);
                return rootElement.Elements(XmlSchema.Keyref).Cast<ForeignKeyElement>().OrderBy(fke => fke.Name).ToList();
            }
        }

        /// <summary>
        /// Gets the name of the data model.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the XML Namespace manager.
        /// </summary>
        public XmlNamespaceManager NamespaceManager { get; private set; }

        /// <summary>
        /// Gets a sorted list of the tables in the data model schema.
        /// </summary>
        public List<TableElement> Tables
        {
            get
            {
                XElement dataModelElement = this.Root.Element(XmlSchema.Element);
                XElement complexTypeElement = dataModelElement.Element(XmlSchema.ComplexType);
                XElement choiceElement = complexTypeElement.Element(XmlSchema.Choice);
                return choiceElement.Elements(XmlSchema.Element).Cast<TableElement>().OrderBy(te => te.Name).ToList();
            }
        }

        /// <summary>
        /// Gets the target namespace for the generated data model.
        /// </summary>
        public string TargetNamespace { get; private set; }

        /// <summary>
        /// Gets the unique key elements.
        /// </summary>
        public List<UniqueKeyElement> UniqueKeys
        {
            get
            {
                XElement rootElement = this.Root.Element(XmlSchema.Element);
                return rootElement.Elements(XmlSchema.Unique).Cast<UniqueKeyElement>().OrderBy(uke => uke.Name).ToList();
            }
        }
    }
}