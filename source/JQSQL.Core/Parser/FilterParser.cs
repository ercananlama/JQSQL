using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JQSQL.Core.Extensions;

namespace JQSQL.Core.Parsers
{
    public class FilterParser
    {
        public const string FilterOpenToken = "[";
        public const string FilterCloseToken = "]";

        /// <summary>
        /// Check given expression part if it contains any kind of filter
        /// </summary>
        /// <param name="expressionPart">
        /// The expression part to be checked for filter.
        /// <remarks>
        /// Expression part is like Message[1][Title = "hi"]. Here [1] and [Title = "hi"] are filters.
        /// </remarks>
        /// </param>
        /// <returns>
        /// Returns true if given expression part contains a filter. Otherwise, returns false.
        /// </returns>
        public bool HasFilter(string expressionPart)
        {
            return expressionPart.EndsWith(FilterCloseToken);
        }

        /// <summary>
        /// Return parts in given expression part
        /// </summary>
        /// <param name="expressionPart">Expression part to be parsed in parts</param>
        /// <returns>Parts</returns>
        public string[] GetAllParts(string expressionPart)
        {
            var parts = expressionPart.Split(new string[] { FilterOpenToken }, StringSplitOptions.None);
            if (parts[0] == String.Empty)
            {
                parts[0] = null;
            }
            for (int i = 1; i < parts.Length; i++)
            {
                parts[i] = parts[i].Replace(FilterCloseToken, String.Empty);
            }
            return parts;
        }

        /// <summary>
        /// Check given filter content if it is an index filter
        /// </summary>
        /// <param name="filterContent">Filter content</param>
        /// <returns>Return index value if it is an index filter. Otherwise, return null</returns>
        public int? IsIndexFilter(string filterContent)
        {
            return filterContent.SafeCast<int>();
        }
    }
}
