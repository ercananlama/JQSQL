using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
//
using JQSQL.Core.Parsers;
using JQSQL.Core.Extensions;
using JQSQL.Core.Elements;

namespace JQSQL.Core
{
    public class JSON
    {
        private readonly ExpressionParser parser;

        public JSON()
        {
            parser = new ExpressionParser();
        }

        /// <summary>
        /// Search given expression in JSON data and return results
        /// </summary>
        /// <param name="jsonData">JSON data to be searched</param>
        /// <param name="expression">Search expression</param>
        /// <returns>Search results whose types might be different depending on given JSON data structure and expression</returns>
        public object SearchValue(string jsonData, string expression)
        {
            if (String.IsNullOrEmpty(jsonData))
                return null;

            jsonData = parser.ClearExpression(jsonData);
            object currentData = SimpleJson.SimpleJson.DeserializeObject(jsonData);
            var currentElement = parser.ParseExpression(expression);

            while (currentElement != null && currentData != null)
            {
                if (String.IsNullOrEmpty(currentElement.Name))  // [0]. or [0][Attr = "..."].
                {
                    currentData = FilterByAttributes(currentData, currentElement.AttributeFilter);
                    currentData = FilterByIndex(currentData, currentElement);
                }
                else if (currentData is IDictionary<string, object>)
                {
                    currentData = SelectData((IDictionary<string, object>)currentData, currentElement);
                }
                else if (currentData is SimpleJson.JsonArray)
                {
                    var tmpData = currentData as SimpleJson.JsonArray;

                    SimpleJson.JsonArray foundItems = null;
                    foreach (var tmpDataItem in tmpData)
                    {
                        if (tmpDataItem is IDictionary<string, object>)
                        {
                            var results = SelectData((IDictionary<string, object>)tmpDataItem, currentElement);
                            if (results != null)
                            {
                                if (foundItems == null)
                                {
                                    foundItems = new SimpleJson.JsonArray();
                                }

                                if (results is SimpleJson.JsonArray)
                                {
                                    foundItems.AddRange((SimpleJson.JsonArray)results);
                                }
                                else
                                {
                                    foundItems.Add(results);
                                }
                            }
                        }
                    }

                    currentData = foundItems;
                }

                currentElement = currentElement.Child;
            }

            // Return only value when array contains 1 element with primitive type
            if (currentData is SimpleJson.JsonArray)
            {
                var currentDataItems = currentData as SimpleJson.JsonArray;
                if (currentDataItems.Count == 1 && currentDataItems[0].IsPrimitive())
                {
                    currentData = currentDataItems[0];
                }
            }

            return currentData;
        }

        /// <summary>
        /// Extract data from given source data
        /// </summary>
        /// <param name="source">Source data</param>
        /// <param name="element">Element containing the properties of how data is extracted</param>
        /// <returns>Extracted data depending on given source data type and extraction properties</returns>
        public object SelectData(IDictionary<string, object> source, JElement element)
        {
            if (source.Keys.Contains(element.Name))
            {
                var selectedItem = source[element.Name];
                if (selectedItem is SimpleJson.JsonArray)
                {
                    var selectedItems = (SimpleJson.JsonArray)FilterByAttributes(selectedItem, element.AttributeFilter);
                    if (element.HasIndexer)
                    {
                        if (selectedItems.Count > element.IndexNo.Value)
                        {
                            return selectedItems[element.IndexNo.Value];
                        }
                    }
                    else
                    {
                        return selectedItems;
                    }
                }
                else if (selectedItem is IDictionary<string, object>)
                {
                    return FilterByAttributes(selectedItem, element.AttributeFilter);
                }
                else
                {
                    return selectedItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieve item from JSON array at given index 
        /// </summary>
        /// <param name="resultsToFilter">JSON array</param>
        /// <param name="element">Element containing index data</param>
        /// <returns>
        /// If item is found with given index, it is returned. 
        /// If indexing is not needed or applicable, supplied array is returned. 
        /// Otherwise, null is returned.
        /// </returns>
        private object FilterByIndex(object resultsToFilter, JElement element)
        {
            if (resultsToFilter != null && element.HasIndexer && resultsToFilter is SimpleJson.JsonArray)
            {
                var tmpDataItems = resultsToFilter as SimpleJson.JsonArray;
                if (tmpDataItems.Count > element.IndexNo.Value)
                {
                    return tmpDataItems[element.IndexNo.Value];
                }
                else
                {
                    return null;
                }
            }

            return resultsToFilter;
        }

        /// <summary>
        /// Apply given filter on JSON data and return matching results
        /// </summary>
        /// <param name="dataToFilter">JSON data to which given filter is applied</param>
        /// <param name="filter">Filter to apply</param>
        /// <returns>Matching results</returns>
        private object FilterByAttributes(object dataToFilter, Elements.JAttributeFilter filter)
        {
            if (dataToFilter == null || filter == null || !(dataToFilter is SimpleJson.JsonArray || dataToFilter is IDictionary<string, object>))
            {
                return dataToFilter;
            }

            Func<object, object, bool> filterEvaluator = null;

            if (filter.Filter.FilterType == Elements.JFilter.FilterTypes.Equal)
            {
                filterEvaluator = (itemValue, filterValue) =>
                {
                    return Utils.Comparison.CompareValues(itemValue, filterValue,
                        (i, f) =>
                        {
                            return i == f;
                        },
                        (i, f) =>
                        {
                            return i.ToReturnString() == f.ToReturnString();
                        });
                };
            }
            else if (filter.Filter.FilterType == Elements.JFilter.FilterTypes.GreaterThan)
            {
                filterEvaluator = (itemValue, filterValue) =>
                {
                    return Utils.Comparison.CompareValues(itemValue, filterValue,
                        (i, f) =>
                        {
                            return i > f;
                        },
                        (i, f) =>
                        {
                            return i > f;
                        },
                        (i, f) =>
                        {
                            return i > f;
                        });
                };
            }
            else if (filter.Filter.FilterType == Elements.JFilter.FilterTypes.GreaterThanOrEqual)
            {
                filterEvaluator = (itemValue, filterValue) =>
                {
                    return Utils.Comparison.CompareValues(itemValue, filterValue,
                        (i, f) =>
                        {
                            return i >= f;
                        },
                        (i, f) =>
                        {
                            return i >= f;
                        },
                        (i, f) =>
                        {
                            return i >= f;
                        });
                };
            }
            else if (filter.Filter.FilterType == Elements.JFilter.FilterTypes.LessThanOrEqual)
            {
                filterEvaluator = (itemValue, filterValue) =>
                {
                    return Utils.Comparison.CompareValues(itemValue, filterValue,
                        (i, f) =>
                        {
                            return i <= f;
                        },
                        (i, f) =>
                        {
                            return i <= f;
                        },
                        (i, f) =>
                        {
                            return i <= f;
                        });
                };
            }
            else if (filter.Filter.FilterType == Elements.JFilter.FilterTypes.LessThan)
            {
                filterEvaluator = (itemValue, filterValue) =>
                {
                    return Utils.Comparison.CompareValues(itemValue, filterValue,
                        (i, f) =>
                        {
                            return i < f;
                        },
                        (i, f) =>
                        {
                            return i < f;
                        },
                        (i, f) =>
                        {
                            return i < f;
                        });
                };
            }

            if (dataToFilter is SimpleJson.JsonArray)
            {
                return ApplyFilter((SimpleJson.JsonArray)dataToFilter, filter, filterEvaluator);
            }
            else if (dataToFilter is IDictionary<string, object>)
            {
                if (EvaluateFilter((IDictionary<string, object>)dataToFilter, filter, filterEvaluator))
                {
                    return dataToFilter;
                }
            }

            return null;
        }

        /// <summary>
        /// Execute filter on items of given JSON array using evaluator
        /// </summary>
        /// <param name="target">JSON array</param>
        /// <param name="filter">Filter to apply</param>
        /// <param name="filterEvaluator">Evaluation function which contains the logic of how given filter is applied</param>
        /// <returns>Matching results in array format</returns>
        public SimpleJson.JsonArray ApplyFilter(SimpleJson.JsonArray target, Elements.JAttributeFilter filter, Func<object, object, bool> filterEvaluator)
        {
            bool isArrayItemsTypeOfDictionaryObject = false;

            var resultsToReturn = new SimpleJson.JsonArray();
            foreach (var targetItem in target)
            {
                if (targetItem is IDictionary<string, object>)
                {
                    var targetItemSource = targetItem as IDictionary<string, object>;
                    if (EvaluateFilter(targetItemSource, filter, filterEvaluator))
                    {
                        resultsToReturn.Add(targetItemSource);
                    }

                    isArrayItemsTypeOfDictionaryObject = true;
                }
            }

            if (isArrayItemsTypeOfDictionaryObject)
            {
                return resultsToReturn;
            }
            else
            {
                // Right we do not support filtering on array with primitive values, so in that case, return target back
                return target;
            }
        }

        /// <summary>
        /// Execute filter on given JSON object using evaluator
        /// </summary>
        /// <param name="target">JSON object</param>
        /// <param name="filter">Filter to apply</param>
        /// <param name="filterEvaluator">Evaluation function which contains the logic of how given filter is applied</param>
        /// <returns>If given object is matched by filter, returns true. Otherwise, returns false.</returns>
        public bool EvaluateFilter(IDictionary<string, object> target, Elements.JAttributeFilter filter, Func<object, object, bool> filterEvaluator)
        {
            if (target.ContainsKey(filter.Attribute.Name))
            {
                if (filterEvaluator(target[filter.Attribute.Name], filter.Filter.Value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
