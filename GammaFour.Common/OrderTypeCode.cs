// <copyright file="OrderTypeCode.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Describes the pricing of the order.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed")]
    public enum OrderTypeCode
    {
        /// <summary>
        /// Market
        /// </summary>
        Market,

        /// <summary>
        /// Limit
        /// </summary>
        Limit,

        /// <summary>
        /// Stop
        /// </summary>
        Stop,

        /// <summary>
        /// StopLimit
        /// </summary>
        StopLimit,

        /// <summary>
        /// MarketOnClose
        /// </summary>
        MarketOnClose,

        /// <summary>
        /// WithOrWithout
        /// </summary>
        WithOrWithout,

        /// <summary>
        /// LimitOrBetter
        /// </summary>
        LimitOrBetter,

        /// <summary>
        /// LimitWithOrWithout
        /// </summary>
        LimitWithOrWithout,

        /// <summary>
        /// OnBasis
        /// </summary>
        OnBasis,

        /// <summary>
        /// OnClose
        /// </summary>
        OnClose,

        /// <summary>
        /// LimitOnClose
        /// </summary>
        LimitOnClose,

        /// <summary>
        /// ForexMarket
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex", Justification = "Reviewed")]
        ForexMarket,

        /// <summary>
        /// PreviouslyQuoted
        /// </summary>
        PreviouslyQuoted,

        /// <summary>
        /// PreviouslyIndicated
        /// </summary>
        PreviouslyIndicated,

        /// <summary>
        /// ForexLimit
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex", Justification = "Reviewed")]
        ForexLimit,

        /// <summary>
        /// ForexSwap
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex", Justification = "Reviewed")]
        ForexSwap,

        /// <summary>
        /// ForexPreviouslyQuoted
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Forex", Justification = "Reviewed")]
        ForexPreviouslyQuoted,

        /// <summary>
        /// Funari
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Funari", Justification = "Reviewed")]
        Funari,

        /// <summary>
        /// MarketIfTouched
        /// </summary>
        MarketIfTouched,

        /// <summary>
        /// MarketWithLeftOverAsLimit
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LeftOver", Justification = "Reviewed")]
        MarketWithLeftOverAsLimit,

        /// <summary>
        /// PreviousFundValuationPoint
        /// </summary>
        PreviousFundValuationPoint,

        /// <summary>
        /// NextFundValuationPoint
        /// </summary>
        NextFundValuationPoint,

        /// <summary>
        /// Pegged
        /// </summary>
        Pegged,

        /// <summary>
        /// CounterOrderSelection
        /// </summary>
        CounterOrderSelection
    }
}
