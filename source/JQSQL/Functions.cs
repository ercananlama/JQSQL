using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Reflection;
using Microsoft.SqlServer.Server;
using System.Collections;
//
using JQSQL.Core;
using JQSQL.Core.Data;
using JQSQL.Core.Extensions;

public partial class Functions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    [return: SqlFacet(MaxSize = 4000, IsFixedLength = false)]
    public static string Value(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);
        if (searchResult == null)
        {
            return null;
        }

        return searchResult.ToString();
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static int Count(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);

        Calculator calculator = new Calculator();
        var count = calculator.GetCount(searchResult);

        return count;
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble Sum(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);

        Calculator calculator = new Calculator();
        var sum = calculator.GetSum(searchResult);

        if (sum != null)
        {
            return new SqlDouble(sum.Value);
        }
        return new SqlDouble();
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble Average(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);

        Calculator calculator = new Calculator();
        var average = calculator.GetAverage(searchResult);

        if (average != null)
        {
            return new SqlDouble(average.Value);
        }
        return new SqlDouble();
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static string Max(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);

        Calculator calculator = new Calculator();
        var max = calculator.GetMax(searchResult);

        return max.ToReturnString();
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static string Min(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);

        Calculator calculator = new Calculator();
        var min = calculator.GetMin(searchResult);

        return min.ToReturnString();
    }

    [Microsoft.SqlServer.Server.SqlProcedure()]
    public static void ToTable(string jsonData, string expression)
    {
        JSON json = new JSON();
        var searchResult = json.SearchValue(jsonData, expression);

        Converter converter = new Converter();
        var data = converter.ConvertToRecord(searchResult);

        if (data.Length > 0)
        {
            SqlContext.Pipe.SendResultsStart(data[0]);
            for (int i = 0; i < data.Length; i++)
            {
                SqlContext.Pipe.SendResultsRow(data[i]);
            }
            SqlContext.Pipe.SendResultsEnd();
        }
    }
}
