// <copyright file="XmlSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// Describes the elements and attributes of an schema description.
    /// </summary>
    internal static class XmlSchema
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
        /// The AcceptRejectRule attribute.
        /// </summary>
        private static XName acceptRejectRule = XName.Get("AcceptRejectRule", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Indicates a custom type is specified.
        /// </summary>
        private static XName anyType = XName.Get("anyType", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The AutoIncrement attribute.
        /// </summary>
        private static XName autoIncrement = XName.Get("AutoIncrement", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The AutoIncrementSeed attribute.
        /// </summary>
        private static XName autoIncrementSeed = XName.Get("AutoIncrementSeed", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The AutoIncrementStep attribute.
        /// </summary>
        private static XName autoIncrementStep = XName.Get("AutoIncrementStep", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The Base attribute.
        /// </summary>
        private static XName @base = XName.Get("base", string.Empty);

        /// <summary>
        /// The Choice element.
        /// </summary>
        private static XName choice = XName.Get("choice", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The ComplexType element.
        /// </summary>
        private static XName complexType = XName.Get("complexType", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The ConstraintOnly attribute.
        /// </summary>
        private static XName constraintOnly = XName.Get("ConstraintOnly", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The Datatype attribute.
        /// </summary>
        private static XName dataType = XName.Get("DataType", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The Default attribute.
        /// </summary>
        private static XName @default = XName.Get("default", string.Empty);

        /// <summary>
        /// The DeleteRule attribute.
        /// </summary>
        private static XName deleteRule = XName.Get("DeleteRule", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The Element element.
        /// </summary>
        private static XName element = XName.Get("element", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The Field element.
        /// </summary>
        private static XName field = XName.Get("field", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The IsPrimaryKey attribute.
        /// </summary>
        private static XName isPrimaryKey = XName.Get("isPrimaryKey", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The IsRowVersion attribute.
        /// </summary>
        private static XName isRowVersion = XName.Get("isRowVersion", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The Keyref element.
        /// </summary>
        private static XName keyref = XName.Get("keyref", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The MaxLength element.
        /// </summary>
        private static XName maxLength = XName.Get("maxLength", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The MaxOccurs attribute.
        /// </summary>
        private static XName maxOccurs = XName.Get("maxOccurs", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The MinOccurs attribute.
        /// </summary>
        private static XName minOccurs = XName.Get("minOccurs", string.Empty);

        /// <summary>
        /// The Selector element.
        /// </summary>
        private static XName selector = XName.Get("selector", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The sequence element.
        /// </summary>
        private static XName sequence = XName.Get("sequence", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The Name element.
        /// </summary>
        private static XName name = XName.Get("name", string.Empty);

        /// <summary>
        /// The Refer attribute.
        /// </summary>
        private static XName refer = XName.Get("refer", string.Empty);

        /// <summary>
        /// The Restriction element.
        /// </summary>
        private static XName restriction = XName.Get("restriction", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The SimpleType element.
        /// </summary>
        private static XName simpleType = XName.Get("simpleType", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The Type attribute.
        /// </summary>
        private static XName type = XName.Get("type", string.Empty);

        /// <summary>
        /// The Unique element.
        /// </summary>
        private static XName unique = XName.Get("unique", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// The UpdateRule attribute.
        /// </summary>
        private static XName updateRule = XName.Get("UpdateRule", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// The mapping of XmlSchema type names into CLR types.
        /// </summary>
        private static Dictionary<XName, Type> typeMap = new Dictionary<XName, Type>
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
        /// The Value attribute.
        /// </summary>
        private static XName value = XName.Get("value", string.Empty);

        /// <summary>
        /// The XPath attribute.
        /// </summary>
        private static XName xPath = XName.Get("xpath", string.Empty);

        /// <summary>
        /// Gets the AcceptRejectRule attribute.
        /// </summary>
        internal static XName AcceptRejectRule
        {
            get
            {
                return XmlSchema.acceptRejectRule;
            }
        }

        /// <summary>
        /// Gets the  indication that a custom type is specified.
        /// </summary>
        internal static XName AnyType
        {
            get
            {
                return XmlSchema.anyType;
            }
        }

        /// <summary>
        /// Gets the AutoIncrement attribute.
        /// </summary>
        internal static XName AutoIncrement
        {
            get
            {
                return XmlSchema.autoIncrement;
            }
        }

        /// <summary>
        /// Gets the AutoIncrementSeed attribute.
        /// </summary>
        internal static XName AutoIncrementSeed
        {
            get
            {
                return XmlSchema.autoIncrementSeed;
            }
        }

        /// <summary>
        /// Gets the AutoIncrementStep attribute.
        /// </summary>
        internal static XName AutoIncrementStep
        {
            get
            {
                return XmlSchema.autoIncrementStep;
            }
        }

        /// <summary>
        /// Gets the Base attribute.
        /// </summary>
        internal static XName Base
        {
            get
            {
                return XmlSchema.@base;
            }
        }

        /// <summary>
        /// Gets the Choice element.
        /// </summary>
        internal static XName Choice
        {
            get
            {
                return XmlSchema.choice;
            }
        }

        /// <summary>
        /// Gets the ComplexType element.
        /// </summary>
        internal static XName ComplexType
        {
            get
            {
                return XmlSchema.complexType;
            }
        }

        /// <summary>
        /// Gets the DataType attribute.
        /// </summary>
        internal static XName DataType
        {
            get
            {
                return XmlSchema.dataType;
            }
        }

        /// <summary>
        /// Gets the ConstraintOnly attribute.
        /// </summary>
        internal static XName ConstraintOnly
        {
            get
            {
                return XmlSchema.constraintOnly;
            }
        }

        /// <summary>
        /// Gets the Default attribute.
        /// </summary>
        internal static XName Default
        {
            get
            {
                return XmlSchema.@default;
            }
        }

        /// <summary>
        /// Gets the DeleteRule attribute.
        /// </summary>
        internal static XName DeleteRule
        {
            get
            {
                return XmlSchema.deleteRule;
            }
        }

        /// <summary>
        /// Gets the Element element.
        /// </summary>
        internal static XName Element
        {
            get
            {
                return XmlSchema.element;
            }
        }

        /// <summary>
        /// Gets the Field element.
        /// </summary>
        internal static XName Field
        {
            get
            {
                return XmlSchema.field;
            }
        }

        /// <summary>
        /// Gets the IsPrimaryKey attribute.
        /// </summary>
        internal static XName IsPrimaryKey
        {
            get
            {
                return XmlSchema.isPrimaryKey;
            }
        }

        /// <summary>
        /// Gets the IsRowVersion attribute.
        /// </summary>
        internal static XName IsRowVersion
        {
            get
            {
                return XmlSchema.isRowVersion;
            }
        }

        /// <summary>
        /// Gets the Keyref element.
        /// </summary>
        internal static XName Keyref
        {
            get
            {
                return XmlSchema.keyref;
            }
        }

        /// <summary>
        /// Gets the MaxLength element.
        /// </summary>
        internal static XName MaxLength
        {
            get
            {
                return XmlSchema.maxLength;
            }
        }

        /// <summary>
        /// Gets the MaxOccurs attribute.
        /// </summary>
        internal static XName MaxOccurs
        {
            get
            {
                return XmlSchema.maxOccurs;
            }
        }

        /// <summary>
        /// Gets the MinOccurs attribute.
        /// </summary>
        internal static XName MinOccurs
        {
            get
            {
                return XmlSchema.minOccurs;
            }
        }

        /// <summary>
        /// Gets the Name attribute.
        /// </summary>
        internal static XName Name
        {
            get
            {
                return XmlSchema.name;
            }
        }

        /// <summary>
        /// Gets the Refer attribute.
        /// </summary>
        internal static XName Refer
        {
            get
            {
                return XmlSchema.refer;
            }
        }

        /// <summary>
        /// Gets the Restriction element.
        /// </summary>
        internal static XName Restriction
        {
            get
            {
                return XmlSchema.restriction;
            }
        }

        /// <summary>
        /// Gets the Selector element.
        /// </summary>
        internal static XName Selector
        {
            get
            {
                return XmlSchema.selector;
            }
        }

        /// <summary>
        /// Gets the Sequence element.
        /// </summary>
        internal static XName Sequence
        {
            get
            {
                return XmlSchema.sequence;
            }
        }

        /// <summary>
        /// Gets the SimpleType element.
        /// </summary>
        internal static XName SimpleType
        {
            get
            {
                return XmlSchema.simpleType;
            }
        }

        /// <summary>
        /// Gets the Type attribute.
        /// </summary>
        internal static XName Type
        {
            get
            {
                return XmlSchema.type;
            }
        }

        /// <summary>
        /// Gets the mapping of the XName to the corresponding CLR type.
        /// </summary>
        internal static Dictionary<XName, Type> TypeMap
        {
            get
            {
                return XmlSchema.typeMap;
            }
        }

        /// <summary>
        /// Gets the Value attribute.
        /// </summary>
        internal static XName Value
        {
            get
            {
                return XmlSchema.value;
            }
        }

        /// <summary>
        /// Gets the Unique element.
        /// </summary>
        internal static XName Unique
        {
            get
            {
                return XmlSchema.unique;
            }
        }

        /// <summary>
        /// Gets the UpdateRule attribute.
        /// </summary>
        internal static XName UpdateRule
        {
            get
            {
                return XmlSchema.updateRule;
            }
        }

        /// <summary>
        /// Gets the XPath attribute.
        /// </summary>
        internal static XName XPath
        {
            get
            {
                return XmlSchema.xPath;
            }
        }
    }
}