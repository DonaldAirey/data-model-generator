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
        private static Dictionary<Type, Func<string, object>> conversionFunctions = new Dictionary<Type, Func<string, object>>
        {
            { typeof(bool), (v) => bool.Parse(v) },
            { typeof(DateTime), (v) => DateTime.Parse(v) },
            { typeof(decimal), (v) => decimal.Parse(v) },
            { typeof(double), (v) => double.Parse(v) },
            { typeof(Guid), (v) => Guid.Parse(v) },
            { typeof(int), (v) => int.Parse(v) },
            { typeof(long), (v) => long.Parse(v) },
            { typeof(float), (v) => float.Parse(v) },
            { typeof(short), (v) => short.Parse(v) },
            { typeof(string), (v) => v },
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
        /// Indicates that the column has a value type.
        /// </summary>
        private bool isValueType;

        /// <summary>
        /// The type for this column.
        /// </summary>
        private Type type;

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
            this.IsRowVersion = isRowVersionAttribute == null ? false : Convert.ToBoolean(isRowVersionAttribute.Value);

            // This determines if the column allows nulls.
            XAttribute minOccursAttribute = this.Attribute(XmlSchemaDocument.MinOccurs);
            this.IsNullable = minOccursAttribute == null ? false : Convert.ToInt32(minOccursAttribute.Value) == 0;

            // Determine the IsIdentityColumn property.
            XAttribute autoIncrementAttribute = this.Attribute(XmlSchemaDocument.AutoIncrement);
            this.IsAutoIncrement = autoIncrementAttribute == null ? false : Convert.ToBoolean(autoIncrementAttribute.Value);

            // Determine the AutoIncrementSeed property.
            XAttribute autoIncrementSeedAttribute = this.Attribute(XmlSchemaDocument.AutoIncrementSeed);
            this.AutoIncrementSeed = autoIncrementSeedAttribute == null ? 0 : Convert.ToInt32(autoIncrementSeedAttribute.Value);

            // Determine the AutoIncrementStop property
            XAttribute autoIncrementStepAttribute = this.Attribute(XmlSchemaDocument.AutoIncrementStep);
            this.AutoIncrementStep = autoIncrementStepAttribute == null ? 1 : Convert.ToInt32(autoIncrementStepAttribute.Value);
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
        /// Gets a value indicating whether the column is required to have a value or can be null.
        /// </summary>
        public bool IsNullable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the column is a value or a pointer.
        /// </summary>
        public bool IsValueType
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.isValueType;
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
        /// Gets the <see cref="Type"/> of the column.
        /// </summary>
        public Type Type
        {
            get
            {
                // Make sure the type information has been extracted.
                if (!this.isTypeInfoInitialized)
                {
                    this.InitializeTypeInfo();
                }

                return this.type;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(ColumnElement other)
        {
            return this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Gets a display name for the column.
        /// </summary>
        /// <returns>The name of the column.</returns>
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
                this.type = XmlSchemaDocument.TypeMap[typeXName];

                // Finally, extract the maximum length for this datatype.
                XElement maxLengthElement = restrictionElement.Element(XmlSchemaDocument.MaxLength);
                this.maximumLength = Convert.ToInt32(maxLengthElement.Attribute(XmlSchemaDocument.Value).Value);
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
                    // An 'anyType' datatype is an instruction to load an explicit type from a specification found in the 'DataType' attribute.
                    string dataTypeText = dataTypeAttribute.Value;
                    string[] dataTypeParts = dataTypeText.Split(',');
                    string assemblyFullName = string.Format(
                        CultureInfo.InvariantCulture,
                        "{0},{1},{2},{3}",
                        dataTypeParts[1],
                        dataTypeParts[2],
                        dataTypeParts[3],
                        dataTypeParts[4]);
                    Assembly assembly = Assembly.Load(new AssemblyName(assemblyFullName));
                    if (assembly != null)
                    {
                        Type type = assembly.GetType(dataTypeParts[0]);
                        if (type.GetTypeInfo().IsValueType && this.IsNullable)
                        {
                            Type generic = typeof(Nullable<>);
                            this.type = generic.MakeGenericType(type);
                        }
                        else
                        {
                            this.type = type;
                        }
                    }

                    // A datatype must exist for the parsing to continue.
                    if (this.type == null)
                    {
                        throw new Exception(string.Format("Unable to load the type {0} from assembly {1}", dataTypeParts[0], assemblyFullName));
                    }
                }
                else
                {
                    // This is the simplest method of specifying a datatype: a direct mapping to a CLR type.
                    this.type = this.IsNullable ? XmlSchemaDocument.NullableTypeMap[typeXName] : XmlSchemaDocument.TypeMap[typeXName];
                }
            }

            // This evaluates whether the column has a default value and, if it does, converts the text of the default to the native version.
            XAttribute defaultAttribute = this.Attribute(XmlSchemaDocument.Default);
            this.hasDefault = defaultAttribute != null;
            Func<string, object> defaultFunction = null;
            if (defaultAttribute != null && ColumnElement.conversionFunctions.TryGetValue(this.type, out defaultFunction))
            {
                this.defaultValue = defaultFunction(defaultAttribute.Value);
            }

            // This indicates whether the type is a value or a pointer.
            this.isValueType = this.type.GetTypeInfo().IsValueType;

            // We don't need to run this again.
            this.isTypeInfoInitialized = true;
        }
    }
}