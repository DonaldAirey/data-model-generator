// <copyright file="ObservableCollectionExtensions.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents extensions to the ObservableCollection&lt;T&gt; class.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Uses a binary search algorithm to locate a specific element in the ObservableCollection&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <typeparam name="TKey">The type of key on which the search is made.</typeparam>
        /// <param name="list">The observable collection holding the elements to be searched.</param>
        /// <param name="keySelector">A function to select the key from the items in the collection.</param>
        /// <param name="key">The object to locate.</param>
        /// <returns>
        /// The zero-based index of item in the list if found; otherwise, a negative number that is the bitwise complement of the index of the next element that is
        /// larger than item or, if there is no larger element, the bitwise complement of Count.
        /// </returns>
        public static int BinarySearch<T, TKey>(this ObservableCollection<T> list, Func<T, TKey> keySelector, TKey key)
        {
            // Validate the arguments.
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            // This is a standard binary search, ripped from the .NET code by Reflector.  The only real addition is the use of the generic comparer
            // that makes use of the selector and the type of the key.
            int low = 0;
            int high = list.Count - 1;
            while (low <= high)
            {
                int mid = low + ((high - low) >> 1);
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);
                int compare = Comparer<TKey>.Default.Compare(midKey, key);
                if (compare == 0)
                {
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

            // If the item isn't found using a binary search, then the complement of the 'low' variable indicates where in the list the item would go
            // if it were part of the sorted list.
            return ~low;
        }
    }
}
