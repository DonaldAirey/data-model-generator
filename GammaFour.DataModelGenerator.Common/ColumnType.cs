// <copyright file="ColumnType.cs" company="Gamma Four, Inc.">
//    Copyright © 2022 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    /// <summary>
    /// Information about the column's data type.
    /// </summary>
    public class ColumnType
    {
        /// <summary>
        /// Gets or sets the full name of the type, including the namespace.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is an array.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is predefined.
        /// </summary>
        public bool IsPredefined { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is a Value type.
        /// </summary>
        public bool IsValueType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is nullable.
        /// </summary>
        public bool IsNullable { get; set; }
    }
}