// <copyright file="ColumnElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
// <copyright file="ColumnElement.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
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
        private static Dictionary<string, Func<string, object>> conversionFunctions = new Dictionary<string, Func<string, object>>
        {
            { "System.Boolean", (v) => bool.Parse(v) },
            { "System.DateTime", (v) => DateTime.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Decimal", (v) => decimal.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Double", (v) => double.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Guid", (v) => Guid.Parse(v) },
            { "System.Int32", (v) => int.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Int64", (v) => long.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Single", (v) => float.Parse(v, CultureInfo.InvariantCulture) },
            { "System.Int16", (v) => short.Parse(v, CultureInfo.InvariantCulture) },
            { "System.String", (v) => v },
        };

        /// <summary>
        /// The default, if any, for this column.
        /// </summary>
        private object defaultValue;

        /// <summary>
        /// The maximum length of a variable length data type.
        /// </summary>
        private int maximumLength;

        /// <summary>
        /// Indicates that the column has a default value.
        /// </summary>
        private bool hasDefault;

        /// <summary>
        /// Indicates that the type information has been initialized.
        /// </summary>
        private bool isTypeInfoInitialized = false;

        /// <summary>
        /// The type for this column.
        /// </summary>
        private ColumnType columnType = new ColumnType();

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
            XAttribute isRowVersionAttribute = this.Attribute(XmlSchemaDocument.IsRowVersion);
            this.IsRowVersion = isRowVersionAttribute == null ? false : Convert.ToBoolean(isRowVersionAttribute.Value, CultureInfo.InvariantCulture);

            // This determines if the column allows nulls.
            XAttribute minOccursAttribute = this.Attribute(XmlSchemaDocument.MinOccurs);
            this.columnType.IsNullable = minOccursAttribute == null ? false : Convert.ToInt32(minOccursAttribute.Value, CultureInfo.InvariantCulture) == 0;

            // Determine the IsIdentityColumn property.
            XAttribute autoIncrementAttribute = this.Attribute(XmlSchemaDocument.AutoIncrement);
            this.IsAutoIncrement = autoIncrementAttribute == null ? false : Convert.ToBoolean(autoIncrementAttribute.Value, CultureInfo.InvariantCulture);

            // Determine the AutoIncrementSeed property.
            XAttribute autoIncrementSeedAttribute = this.Attribute(XmlSchemaDocument.AutoIncrementSeed);
            this.AutoIncrementSeed = autoIncrementSeedAttribute == null ? 0 : Convert.ToInt32(autoIncrementSeedAttribute.Value, CultureInfo.InvariantCulture);

            // Determine the AutoIncrementStop property
            XAttribute autoIncrementStepAttribute = this.Attribute(XmlSchemaDocument.AutoIncrementStep);
            this.AutoIncrementStep = autoIncrementStepAttribute == null ? 1 : Convert.ToInt32(autoIncrementStepAttribute.Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a value used to see an auto incrementing column.
        /// </summary>
        public int AutoIncrementSeed { get; private set; }

        /// <summary>
        /// Gets the value used to increment the seed value for an auto-incrementing column.
        /// </summary>
        public int AutoIncrementStep { get; private set; }

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
                return this.Table.Columns.IndexOf(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether the column increments automatically as new rows are created.
        /// </summary>
        public bool IsAutoIncrement { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the column is part of a primary key.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                // This will examine the primary key to see if the column is part of the key.
                return (from ce in this.Table.PrimaryKey.Columns
                        where ce.Column == this
                        select ce).Any();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the column is part of a parent relation.
        /// </summary>
        public bool IsInParentKey
        {
            get
            {
                // Examine each of the parent relations to see if the column is used.
                var list = from fke in this.Table.ParentKeys
                           from ce in fke.Columns
                           where ce.Column == this
                           select ce;

                return (from fke in this.Table.ParentKeys
                        from ce in fke.Columns
                        where ce.Column == this
                        select ce).Any();
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets a indication of whether the column contains the row's version.
        /// </summary>
        public bool IsRowVersion { get; private set; }

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
        /// Compares two <see cref="ColumnElement"/> records.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>-1 if left &lt; right, 0 if left == right, 1 if left &gt; right</returns>
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
            this.maximumLength = int.MaxValue;

            // This section will determine the datatype of the column.  There are three basic flavors.  The first is a simple type which is extracted
            // from the 'type' attribute in the column description element.  The second is through a 'anyType' specification which is an instruction
            // to reference a literal expression for the type.  The third method restricts one of the basic types by setting a maximum length for it.
            XAttribute typeAttribute = this.Attribute(XmlSchemaDocument.Type);
            if (typeAttribute == null)
            {
                // This section will parse a predefined datatype with a restriction.  The 'base' attribute of the 'restriction' element contains the
                // predefined datatype specification.
                XElement simpleElement = this.Element(XmlSchemaDocument.SimpleType);
                XElement restrictionElement = simpleElement.Element(XmlSchemaDocument.Restriction);
                XAttribute baseAttribute = restrictionElement.Attribute(XmlSchemaDocument.Base);

                // Translate the predefined datatype into a CLR type.
                string[] xNameParts = baseAttribute.Value.Split(':');
                XNamespace xNamespace = this.Document.Root.GetNamespaceOfPrefix(xNameParts[0]);
                XName typeXName = XName.Get(xNameParts[1], xNamespace.NamespaceName);
                Type type = XmlSchemaDocument.TypeMap[typeXName];
                this.columnType.FullName = type.IsArray ? type.GetElementType().FullName : type.FullName;
                this.columnType.IsArray = type.IsArray;
                this.columnType.IsPredefined = true;
                this.columnType.IsValueType = type.GetTypeInfo().IsValueType;

                // Extract the maximum length for this column's data.
                XElement maxLengthElement = restrictionElement.Element(XmlSchemaDocument.MaxLength);
                this.maximumLength = Convert.ToInt32(maxLengthElement.Attribute(XmlSchemaDocument.Value).Value, CultureInfo.InvariantCulture);
            }
            else
            {
                // This section will determine the datatype from attributes on the column description element.  The first step is to find out if the
                // instruction 'anyType' is used to specify an explicit datatype or if we can use one of the predefined datatype.
                string[] xNameParts = typeAttribute.Value.Split(':');
                XNamespace xNamespace = this.Document.Root.GetNamespaceOfPrefix(xNameParts[0]);
                XName typeXName = XName.Get(xNameParts[1], xNamespace.NamespaceName);
                XAttribute dataTypeAttribute = this.Attribute(XmlSchemaDocument.DataType);
                if (typeXName == XmlSchemaDocument.AnyType || dataTypeAttribute != null)
                {
                    this.columnType.FullName = dataTypeAttribute.Value;
                    this.columnType.IsArray = false;
                    this.columnType.IsPredefined = false;
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
            XAttribute defaultAttribute = this.Attribute(XmlSchemaDocument.Default);
            this.hasDefault = defaultAttribute != null;
            Func<string, object> defaultFunction = null;
            if (defaultAttribute != null && ColumnElement.conversionFunctions.TryGetValue(this.columnType.FullName, out defaultFunction))
            {
                this.defaultValue = defaultFunction(defaultAttribute.Value);
            }

            // We don't need to run this again.
            this.isTypeInfoInitialized = true;
        }
    }
}