using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace JQSQL.Core
{
    public class JSON
    {
        public readonly ExpressionParser Parser;

        public JSON()
        {
            this.Parser = new ExpressionParser();
        }

        /// <summary>
        /// Search given expression in json data and return results
        /// </summary>
        /// <param name="jsonData">JSON data to be searched</param>
        /// <param name="expression">Search expression</param>
        /// <returns>Search results whose types might be different depending on given JSON data structure and expression</returns>
        public object SearchValue(string jsonData, string expression)
        {
            if (String.IsNullOrEmpty(jsonData))
                return null;

            jsonData = Parser.ClearExpression(jsonData);
            object currentData = SimpleJson.SimpleJson.DeserializeObject(jsonData);
            var currentElement = Parser.ParseExpression(expression);

            while (currentElement != null)
            {
                if (currentData is IDictionary<string, object>)
                {
                    var tmpData = currentData as IDictionary<string, object>;
                    if (tmpData.Keys.Contains(currentElement.Name)) // Retrieve selected element
                    {
                        currentData = tmpData[currentElement.Name];
                    }
                    else
                        break;

                    if (currentElement.HasIndexer) // Retrieve n-th element of selected array, Items[0].
                    {
                        if (currentData is SimpleJson.JsonArray)
                        {
                            var tmpDataItems = currentData as SimpleJson.JsonArray;
                            if (tmpDataItems.Count > currentElement.IndexNo.Value)
                            {
                                currentData = tmpDataItems[currentElement.IndexNo.Value];
                            }
                            else
                            {
                                currentData = null;
                                break;
                            }
                        }
                    }
                }
                else if (currentData is SimpleJson.JsonArray)
                {
                    var tmpData = currentData as SimpleJson.JsonArray;
                    if (currentElement.HasIndexer && String.IsNullOrEmpty(currentElement.Name)) // Retrieve n-th element of each array, [0].
                    {
                        if (tmpData.Count > currentElement.IndexNo.Value)
                        {
                            currentData = tmpData[currentElement.IndexNo.Value];
                        }
                        else
                        {
                            currentData = null;
                            break;
                        }
                    }
                    else
                    {
                        SimpleJson.JsonArray foundItems = null;
                        foreach (var tmpDataItem in tmpData)
                        {
                            var tmpDataItems = tmpDataItem as IDictionary<string, object>;
                            if (tmpDataItems.Keys.Contains(currentElement.Name))
                            {
                                if (foundItems == null)
                                {
                                    foundItems = new SimpleJson.JsonArray();
                                }

                                if (tmpDataItems[currentElement.Name] is SimpleJson.JsonArray)
                                {
                                    var tmpDataItemsSource = tmpDataItems[currentElement.Name] as SimpleJson.JsonArray;
                                    if (currentElement.HasIndexer) // Retrieve n-th element of each named array, Items[0]
                                    {
                                        if (tmpDataItemsSource.Count > currentElement.IndexNo.Value)
                                        {
                                            foundItems.Add(tmpDataItemsSource[currentElement.IndexNo.Value]);
                                        }
                                    }
                                    else // Retrieve all elements of each array, Items
                                    {
                                        foundItems.AddRange(tmpDataItemsSource);
                                    }
                                }
                                else
                                {
                                    foundItems.Add(tmpDataItems[currentElement.Name]);
                                }
                            }
                        }

                        currentData = foundItems;
                    }
                }

                currentElement = currentElement.Child;
            }

            return currentData;
        }
    }
}
