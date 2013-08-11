using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using JQSQL.Core.Extensions;
using JQSQL.Core.Elements;

namespace JQSQL.Core.Parsers
{
    /// <summary>
    /// This class is used as first entry point to parsing of whole expressions
    /// </summary>
    public class ExpressionParser
    {
        public const char PartSeparator = '.';

        private readonly AttributeFilterParser attrFilterParser;
        private readonly FilterParser filterParser;

        public ExpressionParser()
        {
            attrFilterParser = new AttributeFilterParser();
            filterParser = new FilterParser();
        }

        /// <summary>
        /// Clear given expression from unnecessary characters
        /// </summary>
        /// <param name="expression">Expression to be cleared</param>
        /// <returns>Cleared expression</returns>
        public string ClearExpression(string expression)
        {
            return expression.
                Replace("\n", String.Empty).
                Replace("\t", String.Empty).
                Replace("\r", String.Empty);
        }

        /// <summary>
        /// Parse given expression and return it in JElement format, which has determined properties for each element of expression
        /// </summary>
        /// <param name="expression">Expression to be parsed</param>
        /// <returns>Parsed element metadata</returns>
        public JElement ParseExpression(string expression)
        {
            JElement element = new JElement();

            var parts = expression.Split(PartSeparator);
            ParseElements(null, element, 0, parts);

            return element;
        }

        private void ParseElements(JElement parent, JElement element, int i, string[] parts)
        {
            element.Parent = parent;

            var hasFilter = filterParser.HasFilter(parts[i]);
            if (hasFilter)
            {
                var partItems = filterParser.GetAllParts(parts[i]);
                element.Name = partItems[0];
                for (int pI = 1; pI < partItems.Length; pI++)
                {
                    var indexValue = filterParser.IsIndexFilter(partItems[pI]);
                    if (indexValue != null)
                    {
                        element.HasIndexer = true;
                        element.IndexNo = indexValue.Value;
                    }
                    else
                    {
                        element.AttributeFilter = attrFilterParser.Parse(partItems[pI]);
                    }
                }
            }
            else
            {
                element.Name = parts[i];
            }

            if (i + 1 < parts.Length)
            {
                element.Child = new JElement();
                ParseElements(element, element.Child, i + 1, parts);
            }
        }
    }
}
