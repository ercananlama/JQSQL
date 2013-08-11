using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using JQSQL.Core.Elements;
using JQSQL.Core.Extensions;

namespace JQSQL.Core.Parsers
{
    /// <summary>
    /// This class is used to access parsing features for attribute filters
    /// </summary>
    public class AttributeFilterParser
    {
        public const string FilterTypeGreaterThanToken = ">";
        public const string FilterTypeGreaterThanOrEqualToken = ">=";
        public const string FilterTypeEqualToken = "=";
        public const string FilterTypeLessThanOrEqualToken = "<=";
        public const string FilterTypeLessThanToken = "<";
        public const string QuoteSymbol = "\"";

        /// <summary>
        /// Parse expression given in attribute filter format
        /// </summary>
        /// <param name="expression">Attribute filter expression</param>
        /// <returns>Parsed attribute filter metada</returns>
        public JAttributeFilter Parse(string expression)
        {
            var attrFilter = new JAttributeFilter();
            attrFilter.Attribute = new JAttribute();
            attrFilter.Filter = new JFilter();

            var measureToken = String.Empty;
            if (expression.Contains(FilterTypeGreaterThanOrEqualToken))
            {
                attrFilter.Filter.FilterType = JFilter.FilterTypes.GreaterThanOrEqual;
                measureToken = FilterTypeGreaterThanOrEqualToken;
            }
            else if (expression.Contains(FilterTypeLessThanOrEqualToken))
            {
                attrFilter.Filter.FilterType = JFilter.FilterTypes.LessThanOrEqual;
                measureToken = FilterTypeLessThanOrEqualToken;
            }
            else if (expression.Contains(FilterTypeEqualToken))
            {
                attrFilter.Filter.FilterType = JFilter.FilterTypes.Equal;
                measureToken = FilterTypeEqualToken;
            }
            else if (expression.Contains(FilterTypeGreaterThanToken))
            {
                attrFilter.Filter.FilterType = JFilter.FilterTypes.GreaterThan;
                measureToken = FilterTypeGreaterThanToken;
            }
            else if (expression.Contains(FilterTypeLessThanToken))
            {
                attrFilter.Filter.FilterType = JFilter.FilterTypes.LessThan;
                measureToken = FilterTypeLessThanToken;
            }

            attrFilter.Attribute.Name = expression.Substring(0, expression.IndexOf(measureToken)).Trim();

            var filterValueContent = expression.Substring(expression.IndexOf(measureToken) + measureToken.Length).Trim();
            if (filterValueContent.StartsWith(QuoteSymbol))
            {
                filterValueContent = filterValueContent.Replace(QuoteSymbol, String.Empty);
                
                var filterDateValue = filterValueContent.SafeCast<DateTime>();
                if (filterDateValue != null)
                {
                    attrFilter.Filter.Value = filterDateValue.Value;
                }
                else
                {
                    attrFilter.Filter.Value = filterValueContent;
                }
            }
            else
            {
                var filterLongValue = filterValueContent.SafeCast<long>();
                if (filterLongValue != null)
                {
                    attrFilter.Filter.Value = filterLongValue.Value;
                    return attrFilter;
                }

                var filterIntValue = filterValueContent.SafeCast<int>();
                if (filterIntValue != null)
                {
                    attrFilter.Filter.Value = filterIntValue.Value;
                    return attrFilter;
                }   
            }
            
            return attrFilter;
        }
    }
}
