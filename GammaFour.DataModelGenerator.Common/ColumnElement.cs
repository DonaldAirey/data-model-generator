﻿// <copyright file="ColumnElement.cs" company="Gamma Four, Inc.">
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
    using System.Xml.Linq;

    /// <summary>
    /// A description of a column.
    /// </summary>
    public class ColumnElement : XElement, IComparable<ColumnElement>
    {
        /// <summary>
        /// Maps the full name of the type to a function that parses a default value for that type.
        /// </summary>
        private static readonly Dictionary<string, Func<string, object>> ConversionFunctions = new Dictionary<string, Func<string, object>>
        {
            { "System.Boolean", (v) => bool.Parse(v) },
            { "System.DateTime", (v) => DateTime.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Decimal", (v) => decimal.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Double", (v) => double.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Guid", (v) => Guid.Parse(v) },
            { "System.Int16", (v) => short.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Int32", (v) => int.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Int64", (v) => long.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Single", (v) => float.Parse(v, CultureInfo.InvariantCulture) },
            { "System.String", (v) => v },
            { "System.UInt16", (v) => ushort.Parse(v, CultureInfo.InvariantCulture) },
            { "System.UInt32", (v) => uint.Parse(v, CultureInfo.InvariantCulture) },
            { "System.UInt64", (v) => ulong.Parse(v, CultureInfo.InvariantCulture) },
        };

        /// <summary>
        /// The type for this column.
        /// </summary>
        private readonly ColumnType columnType = new ColumnType();

        /// <summary>
        /// The default, if any, for this column.
        /// </summary>
        private object defaultValue;

        /// <summary>
        /// The number of fractional digits in a fix-precision number.
        /// </summary>
        private int fractionDigits = 6;

        /// <summary>
        /// Indicates that the column has a default value.
        /// </summary>
        private bool hasDefault;

        /// <summary>
        /// The index of the column in the table.
        /// </summary>
        private int? index;

        /// <summary>
        /// A value indicating whether ths column is part of the primary key.
        /// </summary>
        private bool? isPrimaryIndex;

        /// <summary>
        /// Indicates that the type information has been initialized.
        /// </summary>
        private bool isTypeInfoInitialized;

        /// <summary>
        /// A value indicating whether the column is part of a parent primary key.
        /// </summary>
        private bool? isInParentKey;

        /// <summary>
        /// Indicates the element has a simple type declaration.
        /// </summary>
        private bool hasSimpleType;

        /// <summary>
        /// The maximum length of a variable length data type.
        /// </summary>
        private int maximumLength;

        /// <summary>
        /// The total number of digits in a fixed-precision datatype.
        /// </summary>
        private int totalDigits = 21;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnElement"/> class.
        /// </summary>
        /// <param name="xElement">The XML annotations of the table.</param>
        public ColumnElement(XElement xElement)
            : base(xElement)
        {
            // Parse the name out of the XML.
            this.Name = this.Attribute(XmlSchemaDocument.ObjectName).Value;

            // This determines if the column allows nulls.
            XAttribute isRowVersionAttribute = this.Attribute(XmlSchemaDocument.IsRowVersionName);
            this.IsRowVersion = isRowVersionAttribute == null ? false : Convert.ToBoolean(isRowVersionAttribute.Value, CultureInfo.InvariantCulture);

            // This determines if the column allows nulls.
            XAttribute minOccursAttribute = this.Attribute(XmlSchemaDocument.MinOccursName);
            this.columnType.IsNullable = minOccursAttribute == null ? false : Convert.ToInt32(minOccursAttribute.Value, CultureInfo.InvariantCulture) == 0;
        }

        /// <summary>
        /// Gets the default value for a column.
        /// </summary>
        public object DefaultValue
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.defaultValue;
            }
        }

        /// <summary>
        /// Gets the number of fractional digits in a fixed-precision number.
        /// </summary>
        public int FractionDigits
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.fractionDigits;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication whether the column has a default value.
        /// </summary>
        public bool HasDefault
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.hasDefault;
            }
        }

        /// <summary>
        /// Gets the index of this item in the list of columns.
        /// </summary>
        public int Index
        {
            get
            {
                if (!this.index.HasValue)
                {
                    this.index = this.Table.Columns.IndexOf(this);
                }

                return this.index.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the column is part of a primary key.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                // This will examine the primary key to see if the column is part of the key.
                if (!this.isPrimaryIndex.HasValue)
                {
                    this.isPrimaryIndex = (from ce in this.Table.PrimaryIndex.Columns
                                         where ce.Column == this
                                         select ce).Any();
                }

                return this.isPrimaryIndex.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the column is part of a parent relation.
        /// </summary>
        public bool IsInParentKey
        {
            get
            {
                if (!this.isInParentKey.HasValue)
                {
                    // Examine each of the parent relations to see if the column is used.
                    var list = from fke in this.Table.ParentIndexes
                               from ce in fke.Columns
                               where ce.Column == this
                               select ce;

                    this.isInParentKey = (from fke in this.Table.ParentIndexes
                                          from ce in fke.Columns
                                          where ce.Column == this
                                          select ce).Any();
                }

                return this.isInParentKey.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets a indication of whether the column contains the row's version.
        /// </summary>
        public bool IsRowVersion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the element has a simple type description.
        /// </summary>
        public bool HasSimpleType
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.hasSimpleType;
            }
        }

        /// <summary>
        /// Gets the maximum length of the data in a column.
        /// </summary>
        public int MaximumLength
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.maximumLength;
            }
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// Gets the parent TableElement of this ColumnElement.
        /// </summary>
        public TableElement Table
        {
            get
            {
                return this.Parent.Parent.Parent as TableElement;
            }
        }

        /// <summary>
        /// Gets the total digits in a fixed-precision number.
        /// </summary>
        public int TotalDigits
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.totalDigits;
            }
        }

        /// <summary>
        /// Gets the <see cref="ColumnType"/> of the column.
        /// </summary>
        public ColumnType ColumnType
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.columnType;
            }
        }

        /// <summary>
        /// Equals operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the two operands are equal, false otherwise.</returns>
        public static bool operator ==(ColumnElement left, ColumnElement right)
        {
            // Compare the left to the right.  Don't use operators or you'll recurse.
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Not Equals operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the two operands are not equal, false otherwise.</returns>
        public static bool operator !=(ColumnElement left, ColumnElement right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Less Than operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is less than the right operand, false otherwise.</returns>
        public static bool operator <(ColumnElement left, ColumnElement right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Less Than or Equal To operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is less than the right operand, false otherwise.</returns>
        public static bool operator <=(ColumnElement left, ColumnElement right)
        {
            return Compare(left, right) <= 0;
        }

        /// <summary>
        /// Greater Than operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is greater than the right operand, false otherwise.</returns>
        public static bool operator >(ColumnElement left, ColumnElement right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Greater Than or Equal To operation.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>true if the left operand is greater than the right operand, false otherwise.</returns>
        public static bool operator >=(ColumnElement left, ColumnElement right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Compares two <see cref="ColumnElement"/> rows.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>-1 if left &lt; right, 0 if left == right, 1 if left &gt; right.</returns>
        public static int Compare(ColumnElement left, ColumnElement right)
        {
            // Don't use operators or you'll recurse.  If the left and right objects are the same object, then they're equal.
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }

            // The left operand can never be equal to null.
            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }

            // Reference checking done.  This will compare the names to see if they're the same.
            return left.CompareTo(right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            // Comparing against null will always be false.
            ColumnElement other = obj as ColumnElement;
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Call the common method to compare the names.
            return this.CompareTo(other) == 0;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // The name is used to compare these objects.
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(ColumnElement other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Initializes all the properties that define the type of this column.
        /// </summary>
        private void InitializeTypeInfo()
        {
            // The code below will work out the maximum length based on either the default values or the explicit values.
            this.hasSimpleType = false;
            this.maximumLength = int.MaxValue;

            // This section will determine the datatype of the column.  There are three basic flavors.  The first is a simple type which is extracted
            // from the 'type' attribute in the column description element.  The second is through a 'anyType' specification which is an instruction
            // to reference a literal expression for the type.  The third method restricts one of the basic types by setting a maximum length for it.
            XAttribute typeAttribute = this.Attribute(XmlSchemaDocument.TypeName);
            if (typeAttribute == null)
            {
                // Indicates that an explcit element describing the type is present.
                this.hasSimpleType = true;

                // This section will parse a predefined datatype with a restriction.  The 'base' attribute of the 'restriction' element contains the
                // predefined datatype specification.
                XElement simpleElement = this.Element(XmlSchemaDocument.SimpleTypeName);
                XElement restrictionElement = simpleElement.Element(XmlSchemaDocument.RestrictionName);
                XAttribute baseAttribute = restrictionElement.Attribute(XmlSchemaDocument.BaseName);

                // Translate the predefined datatype into a CLR type.
                string[] xNameParts = baseAttribute.Value.Split(':');
                XNamespace xNamespace = this.Document.Root.GetNamespaceOfPrefix(xNameParts[0]);
                XName typeXName = XName.Get(xNameParts[1], xNamespace.NamespaceName);
                Type type = XmlSchemaDocument.TypeMap[typeXName];
                this.columnType.FullName = type.IsArray ? type.GetElementType().FullName : type.FullName;
                this.columnType.IsArray = type.IsArray;
                this.columnType.IsPredefined = true;
                this.columnType.IsValueType = type.GetTypeInfo().IsValueType;

                // Extract the number of fractional digits in a fixed precision number.
                XElement fractionDigitsElement = restrictionElement.Element(XmlSchemaDocument.FractionDigitsName);
                if (fractionDigitsElement != null)
                {
                    this.fractionDigits = Convert.ToInt32(fractionDigitsElement.Attribute(XmlSchemaDocument.ValueName).Value, CultureInfo.InvariantCulture);
                }

                // Extract the maximum length for this column's data.
                XElement maxLengthElement = restrictionElement.Element(XmlSchemaDocument.MaxLengthName);
                if (maxLengthElement != null)
                {
                    this.maximumLength = Convert.ToInt32(maxLengthElement.Attribute(XmlSchemaDocument.ValueName).Value, CultureInfo.InvariantCulture);
                }

                // Extract the total number of digits in a fixed precision number.
                XElement totalDigitsElement = restrictionElement.Element(XmlSchemaDocument.TotalDigitsName);
                if (totalDigitsElement != null)
                {
                    this.totalDigits = Convert.ToInt32(totalDigitsElement.Attribute(XmlSchemaDocument.ValueName).Value, CultureInfo.InvariantCulture);
                }
            }
            else
            {
                // This section will determine the datatype from attributes on the column description element.  The first step is to find out if the
                // instruction 'anyType' is used to specify an explicit datatype or if we can use one of the predefined datatype.
                string[] xNameParts = typeAttribute.Value.Split(':');
                XNamespace xNamespace = this.Document.Root.GetNamespaceOfPrefix(xNameParts[0]);
                XName typeXName = XName.Get(xNameParts[1], xNamespace.NamespaceName);
                XAttribute dataTypeAttribute = this.Attribute(XmlSchemaDocument.DataTypeName);
                if (typeXName == XmlSchemaDocument.AnyTypeName || dataTypeAttribute != null)
                {
                    this.columnType.FullName = dataTypeAttribute.Value;
                    this.columnType.IsArray = false;
                    this.columnType.IsPredefined = XmlSchemaDocument.SystemTypes.Contains(dataTypeAttribute.Value);
                    this.columnType.IsValueType = true;
                }
                else
                {
                    // This is the simplest method of specifying a datatype: a direct mapping to a CLR type.
                    Type type = XmlSchemaDocument.TypeMap[typeXName];
                    this.columnType.FullName = type.IsArray ? type.GetElementType().FullName : type.FullName;
                    this.columnType.IsArray = type.IsArray;
                    this.columnType.IsPredefined = true;
                    this.columnType.IsValueType = type.GetTypeInfo().IsValueType;
                }
            }

            // This evaluates whether the column has a default value and, if it does, converts the text of the default to the native version.
            XAttribute defaultAttribute = this.Attribute(XmlSchemaDocument.DefaultName);
            this.hasDefault = defaultAttribute != null;
            Func<string, object> defaultFunction = null;
            if (defaultAttribute != null && ColumnElement.ConversionFunctions.TryGetValue(this.columnType.FullName, out defaultFunction))
            {
                this.defaultValue = defaultFunction(defaultAttribute.Value);
            }

            // We don't need to run this again.
            this.isTypeInfoInitialized = true;
        }
    }
}