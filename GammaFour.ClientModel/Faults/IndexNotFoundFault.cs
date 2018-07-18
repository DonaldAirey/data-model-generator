// <copyright file="IndexNotFoundFault.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A fault that occurs when a named index doesn't exist on the target table.
    /// </summary>
    [DataContract]
    public class IndexNotFoundFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexNotFoundFault"/> class.
        /// </summary>
        /// <param name="tableName">The name of the table which was the target of the index operation.</param>
        /// <param name="indexName">The name of the index which was the target of the index operation.</param>
        public IndexNotFoundFault(string tableName, string indexName)
        {
            // Initialize the object.
            this.TableName = tableName;
            this.IndexName = indexName;
        }

        /// <summary>
        /// Gets the index of the fault.
        /// </summary>
        [DataMember]
        public string IndexName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the target table name of the fault.
        /// </summary>
        [DataMember]
        public string TableName
        {
            get;
            private set;
        }
    }
}
