// <copyright file="RecordNotFoundFault.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// A fault that occurs when an update is made to an already updated record.
    /// </summary>
    [DataContract]
    public class RecordNotFoundFault
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundFault"/> class.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="keyElements">The key elements.</param>
        public RecordNotFoundFault(string tableName, object[] keyElements)
        {
            // Initialize the object
            this.KeyElements = new ReadOnlyCollection<object>(keyElements);
            this.TableName = tableName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundFault"/> class.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="keyElements">The key elements.</param>
        public RecordNotFoundFault(string tableName, ReadOnlyCollection<object> keyElements)
        {
            // Initialize the object
            this.KeyElements = keyElements;
            this.TableName = tableName;
        }

        /// <summary>
        /// Gets the unique key of the record where the fault occurred.
        /// </summary>
        [DataMember]
        public ReadOnlyCollection<object> KeyElements { get; private set; }

        /// <summary>
        /// Gets the target table where the fault occurred.
        /// </summary>
        [DataMember]
        public string TableName { get; private set; }
    }
}