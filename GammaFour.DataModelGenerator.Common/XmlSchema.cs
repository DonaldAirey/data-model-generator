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
        /// Gets the AcceptRejectRule attribute.
        /// </summary>
        internal static XName AcceptRejectRule { get; } = XName.Get("AcceptRejectRule", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the  indication that a custom type is specified.
        /// </summary>
        internal static XName AnyType { get; } = XName.Get("anyType", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the AutoIncrement attribute.
        /// </summary>
        internal static XName AutoIncrement { get; } = XName.Get("AutoIncrement", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the AutoIncrementSeed attribute.
        /// </summary>
        internal static XName AutoIncrementSeed { get; } = XName.Get("AutoIncrementSeed", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the AutoIncrementStep attribute.
        /// </summary>
        internal static XName AutoIncrementStep { get; } = XName.Get("AutoIncrementStep", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Base attribute.
        /// </summary>
        internal static XName Base { get; } = XName.Get("base", string.Empty);

        /// <summary>
        /// Gets the Choice element.
        /// </summary>
        internal static XName Choice { get; } = XName.Get("choice", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the ComplexType element.
        /// </summary>
        internal static XName ComplexType { get; } = XName.Get("complexType", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the ConstraintOnly attribute.
        /// </summary>
        internal static XName ConstraintOnly { get; } = XName.Get("ConstraintOnly", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the DataType attribute.
        /// </summary>
        internal static XName DataType { get; } = XName.Get("DataType", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Default attribute.
        /// </summary>
        internal static XName Default { get; } = XName.Get("default", string.Empty);

        /// <summary>
        /// Gets the DeleteRule attribute.
        /// </summary>
        internal static XName DeleteRule { get; } = XName.Get("DeleteRule", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Element element.
        /// </summary>
        internal static XName Element { get; } = XName.Get("element", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Field element.
        /// </summary>
        internal static XName Field { get; } = XName.Get("field", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the IsPrimaryKey attribute.
        /// </summary>
        internal static XName IsPrimaryKey { get; } = XName.Get("IsPrimaryKey", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the IsRowVersion attribute.
        /// </summary>
        internal static XName IsRowVersion { get; } = XName.Get("IsRowVersion", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Keyref element.
        /// </summary>
        internal static XName Keyref { get; } = XName.Get("keyref", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MaxLength element.
        /// </summary>
        internal static XName MaxLength { get; } = XName.Get("maxLength", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MaxOccurs attribute.
        /// </summary>
        internal static XName MaxOccurs { get; } = XName.Get("maxOccurs", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the MinOccurs attribute.
        /// </summary>
        internal static XName MinOccurs { get; } = XName.Get("minOccurs", string.Empty);

        /// <summary>
        /// Gets the Name attribute.
        /// </summary>
        internal static XName Name { get; } = XName.Get("name", string.Empty);

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
        internal static XName Restriction { get; } = XName.Get("restriction", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Selector element.
        /// </summary>
        internal static XName Selector { get; } = XName.Get("selector", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the Sequence element.
        /// </summary>
        internal static XName Sequence { get; } = XName.Get("sequence", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the SimpleType element.
        /// </summary>
        internal static XName SimpleType { get; } = XName.Get("simpleType", XmlSchema.XmlSchemaNamespace);

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
        /// Gets the Unique element.
        /// </summary>
        internal static XName Unique { get; } = XName.Get("unique", XmlSchema.XmlSchemaNamespace);

        /// <summary>
        /// Gets the UpdateRule attribute.
        /// </summary>
        internal static XName UpdateRule { get; } = XName.Get("UpdateRule", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the Value attribute.
        /// </summary>
        internal static XName Value { get; } = XName.Get("value", string.Empty);

        /// <summary>
        /// Gets the Verbs attribute.
        /// </summary>
        internal static XName Verbs { get; } = XName.Get("verbs", XmlSchema.GammaFourDataNamespace);

        /// <summary>
        /// Gets the XPath attribute.
        /// </summary>
        internal static XName XPath { get; } = XName.Get("xpath", string.Empty);
    }
}