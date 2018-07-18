// <copyright file="UriQueryCollection.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.Navigation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a query in a Uri.
    /// </summary>
    /// <remarks>
    /// This class can be used to parse a query string to access.
    /// </remarks>
    public class UriQueryCollection : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// The query strings parsed out of the URI.
        /// </summary>
        private readonly List<KeyValuePair<string, string>> entries = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UriQueryCollection"/> class.
        /// </summary>
        /// <param name="query">The query string.</param>
        public UriQueryCollection(string query)
        {
            if (query != null)
            {
                int num = query.Length;
                for (int i = ((query.Length > 0) && (query[0] == '?')) ? 1 : 0; i < num; i++)
                {
                    int startIndex = i;
                    int num4 = -1;
                    while (i < num)
                    {
                        char ch = query[i];
                        if (ch == '=')
                        {
                            if (num4 < 0)
                            {
                                num4 = i;
                            }
                        }
                        else if (ch == '&')
                        {
                            break;
                        }

                        i++;
                    }

                    string str = null;
                    string str2 = null;
                    if (num4 >= 0)
                    {
                        str = query.Substring(startIndex, num4 - startIndex);
                        str2 = query.Substring(num4 + 1, (i - num4) - 1);
                    }
                    else
                    {
                        str2 = query.Substring(startIndex, i - startIndex);
                    }

                    this.Add(str != null ? Uri.UnescapeDataString(str) : null, Uri.UnescapeDataString(str2));
                    if ((i == (num - 1)) && (query[i] == '&'))
                    {
                        this.Add(null, string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of entries in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.entries.Count;
            }
        }

        /// <summary>
        /// Gets the <see cref="string"/> with the specified key.
        /// </summary>
        /// <param name="key">The key of the value parsed out of the query string.</param>
        /// <returns>The value for the specified key, or <see langword="null"/> if the query does not contain such a key.</returns>
        public string this[string key]
        {
            get
            {
                foreach (var kvp in this.entries)
                {
                    if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return kvp.Value;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            this.entries.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator of query elements.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.entries.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator of query elements.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance as a query string.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var queryBuilder = new StringBuilder();

            if (this.entries.Count > 0)
            {
                queryBuilder.Append('?');
                var first = true;

                foreach (var kvp in this.entries)
                {
                    if (!first)
                    {
                        queryBuilder.Append('&');
                    }
                    else
                    {
                        first = false;
                    }

                    queryBuilder.Append(Uri.EscapeDataString(kvp.Key));
                    queryBuilder.Append('=');
                    queryBuilder.Append(Uri.EscapeDataString(kvp.Value));
                }
            }

            return queryBuilder.ToString();
        }
    }
}