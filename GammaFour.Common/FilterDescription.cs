// <copyright file="FilterDescription.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;

    /// <summary>
    /// Describes a filter.
    /// </summary>
    public class FilterDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDescription"/> class.
        /// </summary>
        public FilterDescription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDescription"/> class.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="groupName">The name of the filter's group.</param>
        /// <param name="isEnabled">An indication of whether the filter is enabled or not.</param>
        public FilterDescription(string name, string groupName, bool isEnabled)
        {
            this.Name = name;
            this.GroupName = groupName;
            this.IsEnabled = isEnabled;
        }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the filter is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }
    }
}