using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
//
using JQSQL.Core.Extensions;

namespace JQSQL.Core.Data
{
    public class Calculator
    {
        public int GetCount(object data)
        {
            if (data == null)
                return 0;

            var dataItems = data as IList<object>;
            if (dataItems != null)
            {
                return dataItems.Count;
            }
            else
            {
                return 1;
            }
        }

        public double? GetSum(object data)
        {
            int itemCout;
            return GetSum(data, out itemCout);
        }

        public double? GetAverage(object data)
        {
            int itemCout;
            var sum = GetSum(data, out itemCout);

            if (itemCout > 0)
            {
                return sum / Convert.ToDouble(itemCout);
            }

            return sum;
        }

        public object GetMax(object data)
        {
            object max = null;

            Iterate(data, (item) =>
            {
                DateTime? cDateTimeValue;
                double? cDoubleValue;

                if ((cDoubleValue = item.SafeCast<double>()) != null)
                {
                    max = Compare<double>(max, cDoubleValue.Value, (baseVal, val) => baseVal < val);
                }
                else if ((cDateTimeValue = item.SafeCast<DateTime>()) != null)
                {
                    max = Compare<DateTime>(max, cDateTimeValue.Value, (baseVal, val) => baseVal < val);
                }
            });

            return max;
        }

        public object GetMin(object data)
        {
            object min = null;

            Iterate(data, (item) =>
            {
                DateTime? cDateTimeValue;
                double? cDoubleValue;

                if ((cDoubleValue = item.SafeCast<double>()) != null)
                {
                    min = Compare<double>(min, cDoubleValue.Value, (baseVal, val) => baseVal > val);
                }
                else if ((cDateTimeValue = item.SafeCast<DateTime>()) != null)
                {
                    min = Compare<DateTime>(min, cDateTimeValue.Value, (baseVal, val) => baseVal > val);
                }
            });

            return min;
        }

        private double? GetSum(object data, out int itemCount)
        {
            double? sum = null;

            var cItemCount = 0;
            Iterate(data, (item) =>
            {
                var cValue = item.SafeCast<double>();
                if (cValue != null)
                {
                    if (sum == null)
                        sum = 0;

                    sum += cValue;
                    cItemCount++;
                }
            });

            itemCount = cItemCount;

            return sum;
        }

        private void Iterate(object data, Action<object> dataEvaluator)
        {
            var dataItems = data as IList<object>;
            if (dataItems != null)
            {
                foreach (var dataItem in dataItems)
                {
                    dataEvaluator(dataItem);
                }
            }
            else
            {
                dataEvaluator(data);
            }
        }

        private object Compare<T>(object baseCompare, T item, Func<T, T, bool> compare) where T : struct
        {
            if (baseCompare == null)
            {
                baseCompare = item;
            }
            else if (baseCompare.IsTypeof<T>() && compare(baseCompare.SafeCast<T>().Value, item))
            {
                baseCompare = item;
            }

            return baseCompare;
        }
    }
}
