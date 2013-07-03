using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQSQL.Core
{
    public class ExpressionParser
    {
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
        /// <returns>JElement</returns>
        public JElement ParseExpression(string expression)
        {
            JElement element = new JElement();

            var parts = expression.Split('.');
            ParseElements(null, element, 0, parts);

            return element;
        }

        private void ParseElements(JElement parent, JElement element, int i, string[] parts)
        {
            element.Parent = parent;
            element.HasIndexer = parts[i].EndsWith("]");
            if (element.HasIndexer)
            {
                element.IndexNo = Convert.ToInt32(parts[i].Substring(parts[i].IndexOf("[") + 1).Replace("]", ""));
                element.Name = parts[i].Replace(String.Format("[{0}]", element.IndexNo), "");
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
