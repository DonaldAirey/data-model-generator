// <copyright file="DataHeader.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.ClientModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// A header used to reconcile client data models with the service data model.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification ="Simple data contract doesn't need comparison.")]
    [DataContract]
    public struct DataHeader
    {
        /// <summary>
        /// Gets or sets the list of transactions.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Selected for performance.")]
        [DataMember]
        public List<object[]> Data { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of a volatile data model.
        /// </summary>
        [DataMember]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the requested sequence number.
        /// </summary>
        [DataMember]
        public long Sequence { get; set; }
    }
}