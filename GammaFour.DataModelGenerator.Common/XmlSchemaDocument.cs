﻿// <copyright file="XmlSchemaDocument.cs" company="Gamma Four, Inc.">
//    Copyright © 2025 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
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
            : base(XDocument.Parse(fileContents))
        {
            // The root element of the schema definition.
            XElement rootElement = this.Root.Element(XmlSchemaDocument.ElementName);

            // Extract the name and the target namespace from the schema.
            this.Name = this.Root.Element(XmlSchemaDocument.ElementName).Attribute("name").Value;
            this.TargetNamespace = this.Root.Attribute("targetNamespace").Value;

            // This tells us where the data dataModel when compiling the REST API.
            XAttribute usingAttribute = this.Root.Element(XmlSchemaDocument.ElementName).Attribute(XmlSchemaDocument.UsingName);
            if (usingAttribute != null)
            {
                this.UsingNamespaces.AddRange(usingAttribute.Value.Split(','));
            }

            // This tells us whether to provide an interface to Entity Framework or not.
            XAttribute isMasterAttribute = this.Root.Element(XmlSchemaDocument.ElementName).Attribute(XmlSchemaDocument.IsMasterName);
            this.IsMaster = isMasterAttribute == null ? true : Convert.ToBoolean(isMasterAttribute.Value, CultureInfo.InvariantCulture);

            // This tells us whether the generated model should employ relational intergity.
            XAttribute isRelationalAttribute = this.Root.Element(XmlSchemaDocument.ElementName).Attribute(XmlSchemaDocument.IsRelationalName);
            this.IsRelational = isRelationalAttribute == null ? true : Convert.ToBoolean(isRelationalAttribute.Value, CultureInfo.InvariantCulture);

            // This tells the generator what kind of naming convension to use for JSON properties.
            XAttribute jsonNamingPolicyAttribute = this.Root.Element(XmlSchemaDocument.ElementName).Attribute(XmlSchemaDocument.JsonNamingPolicyName);
            if (jsonNamingPolicyAttribute == null)
            {
                // The default naming policy for JSON properties is camel case.
                this.JsonNamingPolicy = JsonNamingPolicy.CamelCase;
            }
            else
            {
                // This will employ the specified naming policity from the System.Text.Json package when mapping JSON property names to POCO
                // properties.
                Type jsonNamingPolicyType = typeof(JsonNamingPolicy);
                PropertyInfo propertyInfo = jsonNamingPolicyType.GetProperty(jsonNamingPolicyAttribute.Value);
                if (propertyInfo != null)
                {
                    this.JsonNamingPolicy = propertyInfo.GetValue(null) as JsonNamingPolicy;
                }
            }

            // The data model description is found on the first element of the first complex type in the module.
            XElement complexTypeElement = rootElement.Element(XmlSchemaDocument.ComplexTypeName);
            XElement choiceElement = complexTypeElement.Element(XmlSchemaDocument.ChoiceName);

            // We're going to remove all the generic XElements from the section of the schema where the tables live and replace them with decorated
            // ones (that is, decorated with links to constraints, foreign keys, etc.)
            List<XElement> tables = choiceElement.Elements(XmlSchemaDocument.ElementName).ToList();
            foreach (XElement xElement in tables)
            {
                // This create a description of the table.
                choiceElement.Add(new TableElement(xElement));
                xElement.Remove();
            }

            // This will remove all the undecorated unique keys and replace them with decorated ones.
            List<XElement> uniqueIndexElements = rootElement.Elements(XmlSchemaDocument.UniqueName).ToList();
            foreach (XElement xElement in uniqueIndexElements)
            {
                // This creates the unique constraints.
                rootElement.Add(new UniqueIndexElement(xElement));
                xElement.Remove();
            }

            // From the data model description, create a description of the foreign key constraints and the relation between the tables.
            List<XElement> keyRefElements = rootElement.Elements(XmlSchemaDocument.KeyrefName).ToList();
            foreach (XElement xElement in keyRefElements)
            {
                // This will create the foreign key constraints if they're requested.
                if (this.IsRelational)
                {
                    rootElement.Add(new ForeignIndexElement(xElement));
                }

                // Make sure we remove this node in any case.
                xElement.Remove();
            }

            // Validate the document.
            foreach (TableElement tableElement in this.Tables)
            {
                if (tableElement.PrimaryIndex == null)
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
        /// Gets the  indication that a custom type is specified.
        /// </summary>
        public static XName AnyTypeName { get; } = XName.Get("anyType", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Base attribute.
        /// </summary>
        public static XName BaseName { get; } = XName.Get("base", string.Empty);

        /// <summary>
        /// Gets the Choice element.
        /// </summary>
        public static XName ChoiceName { get; } = XName.Get("choice", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the ComplexType element.
        /// </summary>
        public static XName ComplexTypeName { get; } = XName.Get("complexType", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the DataType attribute.
        /// </summary>
        public static XName DataTypeName { get; } = XName.Get("dataType", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Default attribute.
        /// </summary>
        public static XName DefaultName { get; } = XName.Get("default", string.Empty);

        /// <summary>
        /// Gets the Element element.
        /// </summary>
        public static XName ElementName { get; } = XName.Get("element", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Field element.
        /// </summary>
        public static XName FieldName { get; } = XName.Get("field", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the FactionDigits element.
        /// </summary>
        public static XName FractionDigitsName { get; } = XName.Get("fractionDigits", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the IsPrimaryKey attribute.
        /// </summary>
        public static XName IsPrimaryKeyName { get; } = XName.Get("isPrimaryIndex", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the IsRelational attribute.
        /// </summary>
        public static XName IsRelationalName { get; } = XName.Get("isRelational", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the IsMaster attribute.
        /// </summary>
        public static XName IsMasterName { get; } = XName.Get("isMaster", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the IsRowVersion attribute.
        /// </summary>
        public static XName IsRowVersionName { get; } = XName.Get("isRowVersion", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the JsonNamingPolicy attribute.
        /// </summary>
        public static XName JsonNamingPolicyName { get; } = XName.Get("jsonNamingPolicy", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Keyref element.
        /// </summary>
        public static XName KeyrefName { get; } = XName.Get("keyref", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MaxLength element.
        /// </summary>
        public static XName MaxLengthName { get; } = XName.Get("maxLength", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MinOccurs attribute.
        /// </summary>
        public static XName MinOccursName { get; } = XName.Get("minOccurs", string.Empty);

        /// <summary>
        /// Gets the Name attribute.
        /// </summary>
        public static XName ObjectName { get; } = XName.Get("name", string.Empty);

        /// <summary>
        /// Gets the Refer attribute.
        /// </summary>
        public static XName ReferName { get; } = XName.Get("refer", string.Empty);

        /// <summary>
        /// Gets the Restriction element.
        /// </summary>
        public static XName RestrictionName { get; } = XName.Get("restriction", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Selector element.
        /// </summary>
        public static XName SelectorName { get; } = XName.Get("selector", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Sequence element.
        /// </summary>
        public static XName SequenceName { get; } = XName.Get("sequence", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the SimpleType element.
        /// </summary>
        public static XName SimpleTypeName { get; } = XName.Get("simpleType", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the TotalDigits element.
        /// </summary>
        public static XName TotalDigitsName { get; } = XName.Get("totalDigits", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Type attribute.
        /// </summary>
        public static XName TypeName { get; } = XName.Get("type", string.Empty);

        /// <summary>
        /// Gets the Unique element.
        /// </summary>
        public static XName UniqueName { get; } = XName.Get("unique", XmlSchemaDocument.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Using attribute.
        /// </summary>
        public static XName UsingName { get; } = XName.Get("using", XmlSchemaDocument.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Value attribute.
        /// </summary>
        public static XName ValueName { get; } = XName.Get("value", string.Empty);

        /// <summary>
        /// Gets the XPath attribute.
        /// </summary>
        public static XName XPathName { get; } = XName.Get("xpath", string.Empty);

        /// <summary>
        /// Gets the name of the data dataModel context.
        /// </summary>
        public string DataModelContext { get; private set; }

        /// <summary>
        /// Gets the namespace of the data dataModel.
        /// </summary>
        public List<string> UsingNamespaces { get; } = new List<string>();

        /// <summary>
        /// Gets the constraint elements.
        /// </summary>
        public List<ForeignIndexElement> ForeignKeys
        {
            get
            {
                XElement rootElement = this.Root.Element(XmlSchemaDocument.ElementName);
                return rootElement.Elements(XmlSchemaDocument.KeyrefName).Cast<ForeignIndexElement>().ToList();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a master data model or a slave.
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enforces referential integrity.
        /// </summary>
        public bool IsRelational { get; set; }

        /// <summary>
        /// Gets a value indicating whether the generated controllers require authentication.
        /// </summary>
        public JsonNamingPolicy JsonNamingPolicy { get; private set; }

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
                XElement dataModelElement = this.Root.Element(XmlSchemaDocument.ElementName);
                XElement complexTypeElement = dataModelElement.Element(XmlSchemaDocument.ComplexTypeName);
                XElement choiceElement = complexTypeElement.Element(XmlSchemaDocument.ChoiceName);
                return choiceElement.Elements(XmlSchemaDocument.ElementName).Cast<TableElement>().ToList();
            }
        }

        /// <summary>
        /// Gets the target namespace.
        /// </summary>
        public string TargetNamespace { get; private set; }

        /// <summary>
        /// Gets the unique key elements.
        /// </summary>
        public List<UniqueIndexElement> UniqueKeys
        {
            get
            {
                XElement rootElement = this.Root.Element(XmlSchemaDocument.ElementName);
                return rootElement.Elements(XmlSchemaDocument.UniqueName).Cast<UniqueIndexElement>().ToList();
            }
        }

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
            { XName.Get("unsignedShort", XmlSchemaNamespace), typeof(ushort?) },
        };

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
            { XName.Get("unsignedShort", XmlSchemaNamespace), typeof(ushort) },
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
            typeof(ushort).FullName,
        };
    }
}