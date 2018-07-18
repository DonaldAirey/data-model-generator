// <copyright file="ColumnSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>
    /// A description of a column.
    /// </summary>
    public class ColumnSchema : IComparable<ColumnSchema>
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
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="tableSchema">A description of the table.</param>
        /// <param name="columnElement">The XML annotations of the table.</param>
        public ColumnSchema(TableSchema tableSchema, XElement columnElement)
        {
            // Initialize the object
            this.Table = tableSchema;

            // This will extract the name of the column.
            string name = columnElement.Attribute(XmlSchema.Name).Value;
            this.CamelCaseName = CommonConversion.ToCamelCase(name);
            this.Name = name;
            this.MaximumLength = int.MaxValue;

            // This determines if the column allows nulls.
            XAttribute minOccursAttribute = columnElement.Attribute(XmlSchema.MinOccurs);
            int minOccurs = minOccursAttribute == null ? 1 : Convert.ToInt32(minOccursAttribute.Value);
            this.IsNullable = minOccurs == 0;

            // This section will determine the datatype of the column.  There are three basic flavors.  The first is a simple type which is extracted
            // from the 'type' attribute in the column description element.  The second is through a 'anyType' specification which is an instruction
            // to load an explicit type from an assembly.  The third method restricts one of the basic types by setting a maximum length for it.
            XAttribute typeAttribute = columnElement.Attribute(XmlSchema.Type);
            if (typeAttribute == null)
            {
                // This section will parse a predefined datatype with a restriction.  The 'base' attribute of the 'restriction' element contains the
                // predefined datatype specification.
                XElement simpleElement = columnElement.Element(XmlSchema.SimpleType);
                XElement restrictionElement = simpleElement.Element(XmlSchema.Restriction);
                XAttribute baseAttribute = restrictionElement.Attribute(XmlSchema.Base);

                // Translate the predefined datatype into a CLR type.
                string[] xNameParts = baseAttribute.Value.Split(':');
                XNamespace xNamespace = columnElement.Document.Root.GetNamespaceOfPrefix(xNameParts[0]);
                XName typeXName = XName.Get(xNameParts[1], xNamespace.NamespaceName);
                this.Type = XmlSchema.TypeMap[typeXName];

                // Finally, extract the maximum length for this datatype.
                XElement maxLengthElement = restrictionElement.Element(XmlSchema.MaxLength);
                this.MaximumLength = Convert.ToInt32(maxLengthElement.Attribute(XmlSchema.Value).Value);
            }
            else
            {
                // This section will determine the datatype from attributes on the column description element.  The first step is to find out if the
                // instruction 'anyType' is used to specify an explicit datatype or if we can use one of the predefined datatype.
                string[] xNameParts = typeAttribute.Value.Split(':');
                XNamespace xNamespace = columnElement.Document.Root.GetNamespaceOfPrefix(xNameParts[0]);
                XName typeXName = XName.Get(xNameParts[1], xNamespace.NamespaceName);
                XAttribute dataTypeAttribute = columnElement.Attribute(XmlSchema.DataType);
                if (typeXName == XmlSchema.AnyType || dataTypeAttribute != null)
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
                            this.Type = generic.MakeGenericType(type);
                        }
                        else
                        {
                            this.Type = type;
                        }
                    }

                    // A datatype must exist for the parsing to continue.
                    if (this.Type == null)
                    {
                        throw new Exception(string.Format("Unable to load the type {0} from assembly {1}", dataTypeParts[0], assemblyFullName));
                    }
                }
                else
                {
                    // This is the simplest method of specifying a datatype: a direct mapping to a CLR type.
                    this.Type = XmlSchema.TypeMap[typeXName];
                }
            }

            // This evaluates whether the column has a default value and, if it does, converts the text of the default to the native version.
            XAttribute defaultAttribute = columnElement.Attribute(XmlSchema.Default);
            this.HasDefault = defaultAttribute != null;
            Func<string, object> defaultFunction = null;
            if (defaultAttribute != null && ColumnSchema.conversionFunctions.TryGetValue(this.Type, out defaultFunction))
            {
                this.DefaultValue = defaultFunction(defaultAttribute.Value);
            }

            // This indicates whether the type is a value or a pointer.
            this.IsValueType = this.Type.GetTypeInfo().IsValueType;

            // Determine the IsIdentityColumn property.
            XAttribute autoIncrementAttribute = columnElement.Attribute(XmlSchema.AutoIncrement);
            this.IsAutoIncrement = autoIncrementAttribute == null ? false : Convert.ToBoolean(autoIncrementAttribute.Value);

            // Determine the AutoIncrementSeed property.
            XAttribute autoIncrementSeedAttribute = columnElement.Attribute(XmlSchema.AutoIncrementSeed);
            this.AutoIncrementSeed = autoIncrementSeedAttribute == null ? 0 : Convert.ToInt32(autoIncrementSeedAttribute.Value);

            // Determine the AutoIncrementStop property
            XAttribute autoIncrementStepAttribute = columnElement.Attribute(XmlSchema.AutoIncrementStep);
            this.AutoIncrementStep = autoIncrementStepAttribute == null ? 1 : Convert.ToInt32(autoIncrementStepAttribute.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="tableSchema">A description of the parent table.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="type">The column's type.</param>
        /// <param name="isRowVersion">An indication of whether the column is the row version column.</param>
        protected ColumnSchema(TableSchema tableSchema, string name, Type type, bool isRowVersion = false)
        {
            // Initialize the object.
            this.CamelCaseName = CommonConversion.ToCamelCase(name);
            this.IsRowVersion = isRowVersion;
            this.Name = name;
            this.Table = tableSchema;
            this.Type = type;
            this.MaximumLength = int.MaxValue;
        }

        /// <summary>
        /// Gets a value used to see an auto incrementing column.
        /// </summary>
        public int AutoIncrementSeed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value used to increment the seed value for an auto-incrementing column.
        /// </summary>
        public int AutoIncrementStep
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the camel-case name.
        /// </summary>
        public string CamelCaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default value for a column.
        /// </summary>
        public object DefaultValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index of the column.
        /// </summary>
        public int Index
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication whether the column has a default value.
        /// </summary>
        public bool HasDefault
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication of whether the column increments automatically as new rows are created.
        /// </summary>
        public bool IsAutoIncrement
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the column is required to have a value or can be null.
        /// </summary>
        public bool IsNullable
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the column is a value or a pointer.
        /// </summary>
        public bool IsValueType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the column.
        /// </summary>
        public Type Type
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the column is part of a primary key.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                // This will examine the primary key to see if the column is part of the key.
                foreach (ColumnSchema columnSchema in this.Table.PrimaryKey.Columns)
                {
                    if (columnSchema == this)
                    {
                        return true;
                    }
                }

                // The column is not part of the primary key.
                return false;
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
                foreach (RelationSchema relationSchema in this.Table.ParentRelations)
                {
                    foreach (ColumnSchema childColumnSchema in relationSchema.ChildColumns)
                    {
                        if (childColumnSchema == this)
                        {
                            return true;
                        }
                    }
                }

                // The column is not part of a parent key.
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets a indication of whether the column contains the row's version.
        /// </summary>
        public bool IsRowVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maximum length of the data in a column.
        /// </summary>
        public int MaximumLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parent TableSchema of this ColumnSchema.
        /// </summary>
        public TableSchema Table
        {
            get;
            private set;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(ColumnSchema other)
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
    }
}