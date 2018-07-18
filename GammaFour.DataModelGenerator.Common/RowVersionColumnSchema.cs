// <copyright file="RowVersionColumnSchema.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    /// <summary>
    /// A description of the RowVersion column.
    /// </summary>
    public class RowVersionColumnSchema : ColumnSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RowVersionColumnSchema"/> class.
        /// </summary>
        /// <param name="tableSchema">A description of the parent table.</param>
        public RowVersionColumnSchema(TableSchema tableSchema)
            : base(tableSchema, "RowVersion", typeof(long), true)
        {
        }
    }
}