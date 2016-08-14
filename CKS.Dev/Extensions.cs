using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint
{
    /// <summary>
    /// Extensions to the Dictionary object.
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="currentDictionary">The current dictionary.</param>
        /// <param name="itemsToAdd">The items to add.</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> currentDictionary, IEnumerable<KeyValuePair<TKey, TValue>> itemsToAdd)
        {
            foreach (KeyValuePair<TKey, TValue> item in itemsToAdd)
            {
                currentDictionary[item.Key] = item.Value;
            }
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType) {
            return source.IndexOf(value, comparisonType) >= 0;
        }

        #endregion
    }
}
