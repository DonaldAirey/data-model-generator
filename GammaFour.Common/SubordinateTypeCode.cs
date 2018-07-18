// <copyright file="SubordinateTypeCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The different ways that debt can relate to other debt.
    /// </summary>
    public enum SubordinateTypeCode
    {
        /// <summary>
        /// Equipment Trust Type
        /// </summary>
        EquipmentTrust,

        /// <summary>
        /// Debentures Type
        /// </summary>
        Debentures,

        /// <summary>
        /// First Mortgage Type
        /// </summary>
        FirstMortgage,

        /// <summary>
        /// General Refinance Type
        /// </summary>
        GeneralRefinance,

        /// <summary>
        /// Lower Tier 2 Type
        /// </summary>
        LowerTier2,

        /// <summary>
        /// Not Categorized
        /// </summary>
        None,

        /// <summary>
        /// Notes Type
        /// </summary>
        Notes,

        /// <summary>
        /// Preferred Equipment Trust Type
        /// </summary>
        PreferredEquipmentTrust,

        /// <summary>
        /// Second Mortgage Type
        /// </summary>
        SecondMortgage,

        /// <summary>
        /// Senior Type
        /// </summary>
        Senior,

        /// <summary>
        /// Senior Notes Type
        /// </summary>
        SeniorNotes,

        /// <summary>
        /// Senior Subordinate Debenture Type
        /// </summary>
        SeniorSubordinateDebenture,

        /// <summary>
        /// Subordinate Type
        /// </summary>
        Subordinate,

        /// <summary>
        /// Tier 1 Type
        /// </summary>
        Tier1,

        /// <summary>
        /// Unsecured Credit Type
        /// </summary>
        UnsecuredCredit
    }
}
