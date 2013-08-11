using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JQSQL.Core.Extensions;

namespace JQSQL.Core.Utils
{
    public class Comparison
    {
        /// <summary>
        /// Compare given 2 values using correct comparator
        /// </summary>
        /// <param name="value1">First value to compare</param>
        /// <param name="value2">Second value to compare. This value is used to determine which comparator is used</param>
        /// <param name="dateComparator">Compare function for date values</param>
        /// <param name="longComparator">Compare function for long values</param>
        /// <param name="intComparator">Compare function for int values</param>
        /// <returns></returns>
        public static bool CompareValues(
            object value1,
            object value2,
            Func<DateTime, DateTime, bool> dateComparator,
            Func<long, long, bool> longComparator,
            Func<int, int, bool> intComparator)
        {
            if (value2.IsTypeof<long>())
            {
                var cValue1 = value1.SafeCast<long>();
                if (cValue1 != null && longComparator(cValue1.Value, (long)value2))
                {
                    return true;
                }
            }
            else if (value2.IsTypeof<DateTime>())
            {
                var cValue1 = value1.SafeCast<DateTime>();
                if (cValue1 != null && dateComparator(cValue1.Value, (DateTime)value2))
                {
                    return true;
                }
            }
            else if (value2.IsTypeof<int>())
            {
                var cValue1 = value1.SafeCast<int>();
                if (cValue1 != null && intComparator(cValue1.Value, (int)value2))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
