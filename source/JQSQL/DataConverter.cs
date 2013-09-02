using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Microsoft.SqlServer.Server;
//
using JQDotNet.Extensions;

namespace JQSQL
{
    public class Converter
    {
        private const string DefaultTableColumnName = "Results";

        /// <summary>
        /// Transform given json object (ignoring childs) to table
        /// </summary>
        /// <param name="json">Object to be transformed</param>
        /// <returns>List of records</returns>
        public SqlDataRecord[] ConvertToRecord(object json)
        {
            var records = new List<SqlDataRecord>();

            SqlMetaData[] meta = null;
            if (json is IDictionary<string, object>)
            {
                var data = json as IDictionary<string, object>;
                meta = GetMetada(data);
                records.Add(GetDbRecord(meta, data));
            }
            else if (json is SimpleJson.JsonArray)
            {
                var dataCol = json as SimpleJson.JsonArray;
                foreach (var dataItem in dataCol)
                {
                    if (dataItem is IDictionary<string, object>)
                    {
                        var data = dataItem as IDictionary<string, object>;
                        if (meta == null)
                            meta = GetMetada(data);

                        records.Add(GetDbRecord(meta, data));
                    }
                    else
                    {
                        if (meta == null)
                            meta = GetMetada(dataItem);

                        records.Add(GetDbRecord(meta, dataItem));
                    }
                }
            }
            else if (json != null)
            {
                meta = GetMetada(json);
                records.Add(GetDbRecord(meta, json));
            }

            return records.ToArray();
        }

        /// <summary>
        /// Returns metadata for given dictionary object
        /// </summary>
        /// <remarks>
        /// Each key in dictionary object represents a column
        /// </remarks>
        /// <param name="source">Dictionary to be used for metada</param>
        /// <returns>List of metada (columns)</returns>
        private SqlMetaData[] GetMetada(IDictionary<string, object> source)
        {
            SqlMetaData[] meta = new SqlMetaData[source.Keys.Count];
            for (int i = 0; i < source.Keys.Count; i++)
            {
                var keyName = source.Keys.ElementAt(i);
                meta[i] = GetMetada(keyName, source[keyName]);
            }

            return meta;
        }

        /// <summary>
        /// Returns metadata for given value object
        /// </summary>
        /// <remarks>
        /// It is a single column with default column name
        /// </remarks>
        /// <param name="source">Value object to used for metada</param>
        /// <returns>List of metada (columns)</returns>
        private SqlMetaData[] GetMetada(object source)
        {
            return new SqlMetaData[1]
            {
                GetMetada(DefaultTableColumnName, source)
            };
        }

        /// <summary>
        /// Return metada with given column name and determine type by given object value
        /// </summary>
        /// <param name="columnName">Column name of metada</param>
        /// <param name="value">Value to be used to determine the metada type</param>
        /// <returns>Metada</returns>
        private SqlMetaData GetMetada(string columnName, object value)
        {
            var dbType = GetDbType(value);
            if (dbType == SqlDbType.NVarChar)
            {
                return new SqlMetaData(columnName, dbType, 500);
            }
            else
            {
                return new SqlMetaData(columnName, dbType);
            }
        }

        private SqlDataRecord GetDbRecord(SqlMetaData[] metadata, IDictionary<string, object> source)
        {
            SqlDataRecord record = new SqlDataRecord(metadata);
            foreach (var item in source)
            {
                for (int i = 0; i < metadata.Length; i++)
                {
                    if (item.Key == metadata[i].Name)
                    {
                        SetDbRecord(record, item.Value, i);
                        break;
                    }
                }
            }

            return record;
        }

        private SqlDataRecord GetDbRecord(SqlMetaData[] metadata, object value)
        {
            SqlDataRecord record = new SqlDataRecord(metadata);
            SetDbRecord(record, value, 0);
            return record;
        }

        private SqlDataRecord SetDbRecord(SqlDataRecord record, object value, int index)
        {
            if (value.IsPrimitive())
            {
                var cValue = Convert.ChangeType(value, record.GetFieldType(index));
                record.SetValue(index, cValue);
            }
            else
            {
                record.SetValue(index, value.ToString());
            }
            return record;
        }

        private static SqlDbType GetDbType(object value)
        {
            if (value.SafeCast<double>() != null)
            {
                return SqlDbType.Float;
            }
            else if (value.SafeCast<DateTime>() != null)
            {
                return SqlDbType.DateTime;
            }
            else if (value.SafeCast<bool>() != null)
            {
                return SqlDbType.Bit;
            }
            
            return SqlDbType.NVarChar;
        }
    }
}
