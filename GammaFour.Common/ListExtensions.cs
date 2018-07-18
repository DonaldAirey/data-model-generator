// <copyright file="ListExtensions.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for handling lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Perform a binary search of a list.
        /// </summary>
        /// <typeparam name="T">The type in the list.</typeparam>
        /// <typeparam name="TKey">The key element.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="key">The key.</param>
        /// <returns>The index of the found item or the one's complement of the location where the record should go.</returns>
        public static int BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
                where TKey : IComparable<TKey>
        {
            // Validate the list argument
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            // Validate the keySelector argument.
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            // Keep dividing the list in half until a match is found.
            int low = 0;
            int high = list.Count - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) / 2);
                TKey midKey = keySelector(list[mid]);
                int compare = midKey.CompareTo(key);
                if (compare == 0)
                {
                    // This is the index where the record was found.
                    return mid;
                }
                else
                {
                    if (compare < 0)
                    {
                        low = mid + 1;
                    }
                    else
                    {
                        high = mid - 1;
                    }
                }
            }

            // The one's compliment of the index indicates where this record should be to preserve the order.
            return ~low;
        }
    }
}