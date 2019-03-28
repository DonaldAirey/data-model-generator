// <copyright file="XmlSchemaDocument.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        /// The namespace for the XML Schema elements and attributes.
        /// </summary>
        internal const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// The namespace for the Microsoft Data elements.
        /// </summary>
        internal const string GammaFourDataNamespace = "urn:schemas-gamma-four-com:xml-gfdata";

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSchemaDocument"/> class.
        /// </summary>
        /// <param name="fileContents">The contents of a file that specifies the schema in XML.</param>
        public XmlSchemaDocument(string fileContents)
            : this(XmlReader.Create(new StringReader(fileContents)))
        {
            // The root element of the schema definition.
            XElement rootElement = this.Root.Element(XmlSchemaDocument.Element);

            // Extract the name and the target namespace from the schema.
            this.Name = this.Root.Element(XmlSchemaDocument.Element).Attribute("name").Value;
            this.TargetNamespace = this.Root.Attribute("targetNamespace").Value;

            // This tells us whether to provide an interface to Entity Framework or not.
            XAttribute isVolatileAttribute = this.Root.Element(XmlSchemaDocument.Element).Attribute(XmlSchemaDocument.IsVolatileAttribute);
            this.IsVolatile = isVolatileAttribute == null ? false : Convert.ToBoolean(isVolatileAttribute.Value, CultureInfo.InvariantCulture);

            // The data model description is found on the first element of the first complex type in the module.
            XElement complexTypeElement = rootElement.Element(XmlSchemaDocument.ComplexType);
            XElement choiceElement = complexTypeElement.Element(XmlSchemaDocument.Choice);

            // We're going to remove all the generic XElements from the section of the schema where the tables live and replace them with decorated
            // ones (that is, decorated with links to constraints, foreign keys, etc.)
            List<XElement> tables = choiceElement.Elements(XmlSchemaDocument.Element).ToList();
            foreach (XElement xElement in tables)
            {
                // This create a description of the table.
                choiceElement.Add(new TableElement(xElement));
                xElement.Remove();
            }

            // This will remove all the undecorated unique keys and replace them with decorated ones.
            List<XElement> uniqueElements = rootElement.Elements(XmlSchemaDocument.Unique).ToList();
            foreach (XElement xElement in uniqueElements)
            {
                // This creates the unique constraints.
                rootElement.Add(new UniqueKeyElement(xElement));
                xElement.Remove();
            }

            // From the data model description, create a description of the foreign key constraints and the relation between the tables.
            List<XElement> keyRefElements = rootElement.Elements(XmlSchemaDocument.Keyref).ToList();
            foreach (XElement xElement in keyRefElements)
            {
                // This will create the foreign key constraints.
                rootElement.Add(new ForeignKeyElement(xElement));
                xElement.Remove();
            }

            // Validate the document.
            foreach (TableElement tableElement in this.Tables)
            {
                if (tableElement.PrimaryKey == null)
                {
                    throw new InvalidOperationException($"Table '{tableElement.Name}' must have a primary key.");
                }

                // Make sure the name isn't plural.
                if (tableElement.Name.ToPlural() == tableElement.Name)
                {
                    throw new InvalidOperationException($"Table name '{tableElement.Name}' must not be pluralized.");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSchemaDocument"/> class.
        /// </summary>
        /// <param name="xmlReader">A <see cref="XmlReader"/> that contains the content for the <see cref="XDocument"/>.</param>
        private XmlSchemaDocument(XmlReader xmlReader)
            : base(XDocument.Load(xmlReader))
        {
        }

        /// <summary>
        /// Gets the constraint elements.
        /// </summary>
        public List<ForeignKeyElement> ForeignKeys
        {
            get
            {
                XElement rootElement = this.Root.Element(XmlSchemaDocument.Element);
                return rootElement.Elements(XmlSchemaDocument.Keyref).Cast<ForeignKeyElement>().ToList();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the data model supports a persistent Entity Framework store.
        /// </summary>
        public bool IsVolatile { get; private set; }

        /// <summary>
        /// Gets the name of the data model.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a sorted list of the tables in the data model schema.
        /// </summary>
        public List<TableElement> Tables
        {
            get
            {
                XElement dataModelElement = this.Root.Element(XmlSchemaDocument.Element);
                XElement complexTypeElement = dataModelElement.Element(XmlSchemaDocument.ComplexType);
                XElement choiceElement = complexTypeElement.Element(XmlSchemaDocument.Choice);
                return choiceElement.Elements(XmlSchemaDocument.Element).Cast<TableElement>().ToList();
            }
        }

        /// <summary>
        /// Gets the target namespace.
        /// </summary>
        public string TargetNamespace { get; private set; }

        /// <summary>
        /// Gets the unique key elements.
        /// </summary>
        public List<UniqueKeyElement> UniqueKeys
        {
            get
            {
                XElement rootElement = this.Root.Element(XmlSchemaDocument.Element);
                return rootElement.Elements(XmlSchemaDocument.Unique).Cast<UniqueKeyElement>().ToList();
            }
        }

        /// <summary>
        /// Gets the AcceptRejectRule attribute.
        /// </summary>
        internal static XName AcceptRejectRule { get; } = XName.Get("AcceptRejectRule", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the  indication that a custom type is specified.
        /// </summary>
        internal static XName AnyType { get; } = XName.Get("anyType", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the AutoIncrement attribute.
        /// </summary>
        internal static XName AutoIncrement { get; } = XName.Get("AutoIncrement", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the AutoIncrementSeed attribute.
        /// </summary>
        internal static XName AutoIncrementSeed { get; } = XName.Get("AutoIncrementSeed", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the AutoIncrementStep attribute.
        /// </summary>
        internal static XName AutoIncrementStep { get; } = XName.Get("AutoIncrementStep", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Base attribute.
        /// </summary>
        internal static XName Base { get; } = XName.Get("base", string.Empty);

        /// <summary>
        /// Gets the Choice element.
        /// </summary>
        internal static XName Choice { get; } = XName.Get("choice", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the ComplexType element.
        /// </summary>
        internal static XName ComplexType { get; } = XName.Get("complexType", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the ConstraintOnly attribute.
        /// </summary>
        internal static XName ConstraintOnly { get; } = XName.Get("ConstraintOnly", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the DataType attribute.
        /// </summary>
        internal static XName DataType { get; } = XName.Get("DataType", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Default attribute.
        /// </summary>
        internal static XName Default { get; } = XName.Get("default", string.Empty);

        /// <summary>
        /// Gets the DeleteRule attribute.
        /// </summary>
        internal static XName DeleteRule { get; } = XName.Get("DeleteRule", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Element element.
        /// </summary>
        internal static new XName Element { get; } = XName.Get("element", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Field element.
        /// </summary>
        internal static XName Field { get; } = XName.Get("field", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the IsVolatile attribute.
        /// </summary>
        internal static XName IsVolatileAttribute { get; } = XName.Get("IsVolatile", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the IsPrimaryKey attribute.
        /// </summary>
        internal static XName IsPrimaryKey { get; } = XName.Get("IsPrimaryKey", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the IsRowVersion attribute.
        /// </summary>
        internal static XName IsRowVersion { get; } = XName.Get("IsRowVersion", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Keyref element.
        /// </summary>
        internal static XName Keyref { get; } = XName.Get("keyref", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MaxLength element.
        /// </summary>
        internal static XName MaxLength { get; } = XName.Get("maxLength", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MaxOccurs attribute.
        /// </summary>
        internal static XName MaxOccurs { get; } = XName.Get("maxOccurs", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MinOccurs attribute.
        /// </summary>
        internal static XName MinOccurs { get; } = XName.Get("minOccurs", string.Empty);

        /// <summary>
        /// Gets the Name attribute.
        /// </summary>
        internal static XName ObjectName { get; } = XName.Get("name", string.Empty);

        /// <summary>
        /// Gets the mapping of the XName to the corresponding nullable CLR type.
        /// </summary>
        internal static Dictionary<XName, Type> NullableTypeMap { get; } = new Dictionary<XName, Type>
        {
            { XName.Get("boolean", XmlSchemaNamespace), typeof(bool?) },
            { XName.Get("dateTime", XmlSchemaNamespace), typeof(DateTime?) },
            { XName.Get("decimal", XmlSchemaNamespace), typeof(decimal?) },
            { XName.Get("double", XmlSchemaNamespace), typeof(double?) },
            { XName.Get("float", XmlSchemaNamespace), typeof(float?) },
            { XName.Get("int", XmlSchemaNamespace), typeof(int?) },
            { XName.Get("long", XmlSchemaNamespace), typeof(long?) },
            { XName.Get("short", XmlSchemaNamespace), typeof(short?) },
            { XName.Get("string", XmlSchemaNamespace), typeof(string) },
            { XName.Get("unsignedByte", XmlSchemaNamespace), typeof(byte?) },
            { XName.Get("unsignedInt", XmlSchemaNamespace), typeof(uint?) },
            { XName.Get("unsignedLong", XmlSchemaNamespace), typeof(ulong?) },
            { XName.Get("unsignedShort", XmlSchemaNamespace), typeof(ushort?) }
        };

        /// <summary>
        /// Gets the Refer attribute.
        /// </summary>
        internal static XName Refer { get; } = XName.Get("refer", string.Empty);

        /// <summary>
        /// Gets the Restriction element.
        /// </summary>
        internal static XName Restriction { get; } = XName.Get("restriction", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Selector element.
        /// </summary>
        internal static XName Selector { get; } = XName.Get("selector", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Sequence element.
        /// </summary>
        internal static XName Sequence { get; } = XName.Get("sequence", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the SimpleType element.
        /// </summary>
        internal static XName SimpleType { get; } = XName.Get("simpleType", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Type attribute.
        /// </summary>
        internal static XName Type { get; } = XName.Get("type", string.Empty);

        /// <summary>
        /// Gets the mapping of the XName to the corresponding CLR type.
        /// </summary>
        internal static Dictionary<XName, Type> TypeMap { get; } = new Dictionary<XName, Type>
        {
            { XName.Get("base64Binary", XmlSchemaNamespace), typeof(byte[]) },
            { XName.Get("boolean", XmlSchemaNamespace), typeof(bool) },
            { XName.Get("dateTime", XmlSchemaNamespace), typeof(DateTime) },
            { XName.Get("decimal", XmlSchemaNamespace), typeof(decimal) },
            { XName.Get("double", XmlSchemaNamespace), typeof(double) },
            { XName.Get("float", XmlSchemaNamespace), typeof(float) },
            { XName.Get("int", XmlSchemaNamespace), typeof(int) },
            { XName.Get("long", XmlSchemaNamespace), typeof(long) },
            { XName.Get("short", XmlSchemaNamespace), typeof(short) },
            { XName.Get("string", XmlSchemaNamespace), typeof(string) },
            { XName.Get("unsignedByte", XmlSchemaNamespace), typeof(byte) },
            { XName.Get("unsignedInt", XmlSchemaNamespace), typeof(uint) },
            { XName.Get("unsignedLong", XmlSchemaNamespace), typeof(ulong) },
            { XName.Get("unsignedShort", XmlSchemaNamespace), typeof(ushort) }
        };

        /// <summary>
        /// Gets the system types.
        /// </summary>
        internal static HashSet<string> SystemTypes { get; } = new HashSet<string>
        {
            typeof(byte[]).FullName,
            typeof(bool).FullName,
            typeof(DateTime).FullName,
            typeof(Guid).FullName,
            typeof(decimal).FullName,
            typeof(double).FullName,
            typeof(float).FullName,
            typeof(int).FullName,
            typeof(long).FullName,
            typeof(short).FullName,
            typeof(string).FullName,
            typeof(byte).FullName,
            typeof(uint).FullName,
            typeof(ulong).FullName,
            typeof(ushort).FullName
        };

        /// <summary>
        /// Gets the Unique element.
        /// </summary>
        internal static XName Unique { get; } = XName.Get("unique", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the UpdateRule attribute.
        /// </summary>
        internal static XName UpdateRule { get; } = XName.Get("UpdateRule", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Value attribute.
        /// </summary>
        internal static XName Value { get; } = XName.Get("value", string.Empty);

        /// <summary>
        /// Gets the Verbs attribute.
        /// </summary>
        internal static XName Verbs { get; } = XName.Get("verbs", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the XPath attribute.
        /// </summary>
        internal static XName XPath { get; } = XName.Get("xpath", string.Empty);
    }
}