// <copyright file="OptimisticConcurrencyFault.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// A fault that occurs when an update is made to an already updated record.
    /// </summary>
    [DataContract]
    public class OptimisticConcurrencyFault
    {
        /// <summary>
        /// The unique key of the record where the fault occurred.
        /// </summary>
        [DataMember]
        private ReadOnlyCollection<object> keyElementsField = new ReadOnlyCollection<object>(new List<object>());

        /// <summary>
        /// The target table where the fault occurred.
        /// </summary>
        [DataMember]
        private string tableNameField;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimisticConcurrencyFault"/> class.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="keyElements">The key elements.</param>
        public OptimisticConcurrencyFault(string tableName, ReadOnlyCollection<object> keyElements)
        {
            // Initialize the object
            this.keyElementsField = new ReadOnlyCollection<object>(keyElements);
            this.tableNameField = tableName;
        }

        /// <summary>
        /// Gets the unique key of the record where the fault occurred.
        /// </summary>
        public ReadOnlyCollection<object> KeyElements
        {
            get
            {
                return this.keyElementsField;
            }
        }

        /// <summary>
        /// Gets the target table where the fault occurred.
        /// </summary>
        public string TableName
        {
            get
            {
                return this.tableNameField;
            }
        }
    }
}