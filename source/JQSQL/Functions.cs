using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Reflection;
using Microsoft.SqlServer.Server;
using System.Collections;
//
using JQDotNet;
using JQSQL;

public partial class Functions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    [return: SqlFacet(MaxSize = 4000, IsFixedLength = false)]
    public static string Value(string jsonData, string expression)
    {
        var searchResult = JSONQuery.GetValue(jsonData, expression);
        return ToReturnString(searchResult);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static int Count(string jsonData, string expression)
    {
        var count = JSONQuery.GetCount(jsonData, expression);
        return count;
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble Sum(string jsonData, string expression)
    {
        var sum = JSONQuery.GetSum(jsonData, expression);
        if (sum != null)
        {
            return new SqlDouble(sum.Value);
        }
        return new SqlDouble();
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble Average(string jsonData, string expression)
    {
        var average = JSONQuery.GetAvg(jsonData, expression);
        if (average != null)
        {
            return new SqlDouble(average.Value);
        }
        return new SqlDouble();
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static string Max(string jsonData, string expression)
    {
        var max = JSONQuery.GetMax(jsonData, expression);
        return ToReturnString(max);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static string Min(string jsonData, string expression)
    {
        var min = JSONQuery.GetMax(jsonData, expression);
        return ToReturnString(min);
    }

    [Microsoft.SqlServer.Server.SqlProcedure()]
    public static void ToTable(string jsonData, string expression)
    {
        var searchResult = JSONQuery.GetValue(jsonData, expression);

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

    private static string ToReturnString(object value)
    {
        if (value == null)
        {
            return null;
        }

        return value.ToString();
    }
}
