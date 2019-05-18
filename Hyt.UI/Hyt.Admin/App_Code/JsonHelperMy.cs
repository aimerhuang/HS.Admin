using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Hyt.Admin.App_Code
{

    public static class JsonHelperMy
    {
        #region[GetJSONObject]
        public static DataSet GetJSONObject(string json)
        {
            DataSet dsReturn = new DataSet();
            json = json.Remove(0, 1);
            json = json.Remove(json.Length - 1, 1);
            string[] ss = json.Split(']');
            string key = null;
            string value = null;
            for (int i = 0; i < ss.Length; i++)
            {
                if (ss[i].Length == 0)
                    continue;

                if (ss[i][0] == ',')
                    ss[i] = ss[i].Remove(0, 1);

                if (ss[i].IndexOf(":") != -1)
                {
                    key = ss[i].Substring(0, ss[i].IndexOf(":"));

                    value = ss[i].Remove(0, ss[i].IndexOf(":") + 1) + "]";
                    DataTable dt = JsonToDataTable(value);
                    dt.TableName = key.Replace("\"", "");
                    dsReturn.Tables.Add(dt);

                }
            }
            return dsReturn;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                //str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }
        #endregion

        #region List转换成Json
        /// <summary>
        /// List转换成Json
        /// </summary>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }

        /// <summary>
        /// List转换成Json 
        /// </summary>
        public static string ListToJson<T>(IList<T> list, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName)) jsonName = list[0].GetType().Name;
            Json.Append("{\"" + jsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));

                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region 对象转换为Json
        /// <summary> 
        /// 对象转换为Json 
        /// </summary> 
        /// <param name="jsonObject">对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(object jsonObject)
        {
            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "\"" + objectValue + "\"";
                }
                else if (objectValue is string)
                {
                    value = "\"" + objectValue + "\"";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson(objectValue as IEnumerable);
                }
                else if (objectValue is Enum)
                {
                    value = "\"" + objectValue + "\"";
                }
                else
                {
                    value = "\"" + objectValue + "\"";
                }
                jsonString += "\"" + propertyInfo[i].Name + "\":" + value + ",";
            }
            jsonString = jsonString.Remove(jsonString.Length - 1, 1);
            //jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "}";
        }
        #endregion

        #region 对象集合转换Json
        /// <summary> 
        /// 对象集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(IEnumerable array)
        {
            if (array == null) return "[]";
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            if (jsonString.Length > 1)
                jsonString = jsonString.Remove(jsonString.Length - 1, 1);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary> 
        /// 普通集合转换Json 
        /// </summary> 
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary> 
        /// DataSet转换为Json 
        /// </summary> 
        /// <param name="dataSet">DataSet对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json
        /// <summary> 
        /// Datatable转换为Json 
        /// </summary> 
        /// <param name="table">Datatable对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            if (dt != null)
            {
                DataRowCollection drc = dt.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string strKey = dt.Columns[j].ColumnName;
                        string strValue = drc[i][j].ToString();
                        Type type = dt.Columns[j].DataType;
                        jsonString.Append("\"" + strKey + "\":");
                        strValue = StringFormat(strValue, type);
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append(strValue + ",");
                        }
                        else
                        {
                            jsonString.Append(strValue);
                        }
                    }
                    jsonString.Append("},");
                }
                //zhanghao 2012 01 14 原因：会将匹配的 ‘[’ 删除
                if (drc.Count > 0)
                {
                    jsonString.Remove(jsonString.Length - 1, 1);
                }
            }
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion

        #region DataRow转换为Json

        /// <summary> 
        /// Datatable转换为Json 
        /// </summary> 
        /// <param name="table">DataTable对象</param>
        /// <param name="rowIndex">行号</param>
        /// <returns>Json字符串</returns> 
        public static string ToJson(DataTable table, int rowIndex)
        {
            var jsonString = new StringBuilder();
            jsonString.Append("{");
            if (table != null && table.Rows.Count > rowIndex)
            {
                var row = table.Rows[rowIndex];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string strKey = table.Columns[j].ColumnName;
                    string strValue = row[j].ToString();
                    Type type = table.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < table.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
            }
            jsonString.Append("}");
            return jsonString.ToString();
        }

        /// <summary>
        /// DataTable转换为Json 
        /// </summary>
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName)) jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region DataReader转换为Json
        /// <summary> 
        /// DataReader转换为Json 
        /// </summary> 
        /// <param name="dataReader">DataReader对象</param> 
        /// <returns>Json字符串</returns> 
        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion

        #region Json转换为DataTable

        /// <summary>
        /// Json转换为DataTable
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns>DataTable集合</returns>
        public static DataTable JsonToDataTable(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb = new DataTable();

            if (dic.Count > 0)
            {
                foreach (Dictionary<string, object> drow in dic)
                {
                    if (dtb.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                        {
                            if (drow[key] == null)//2013.07.27 yqf
                            {
                                dtb.Columns.Add(key, typeof(string));
                            }
                            else
                            {
                                dtb.Columns.Add(key, drow[key].GetType());
                            }
                        }
                    }

                    DataRow row = dtb.NewRow();
                    foreach (string key in drow.Keys)
                    {

                        row[key] = drow[key];
                    }
                    dtb.Rows.Add(row);
                }
            }
            return dtb;
        }
        #endregion

        #region Json转换为DataSet

        /// <summary>
        /// Json转换为DataSet
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>DataSet集合</returns>
        public static DataSet JsonToDataSet(string json)
        {
            DataSet ds = new DataSet();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb;

            if (dic.Count > 0)
            {
                foreach (Dictionary<string, object> drow in dic)
                {
                    dtb = new DataTable();
                    if (dtb.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                        {
                            dtb.Columns.Add(key, drow[key].GetType());
                        }
                    }

                    DataRow row = dtb.NewRow();
                    foreach (string key in drow.Keys)
                    {

                        row[key] = drow[key];
                    }
                    dtb.Rows.Add(row);
                    ds.Tables.Add(dtb);
                }

            }
            return ds;
        }
        #region 【曹芳添加：图片上传使用】
        /// <summary>
        /// Json转换为DataSet
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <param name="image">要输出的字段名</param>
        /// <param name="imageStr">具体值</param>
        /// <returns>DataSet集合</returns>
        public static DataSet JsonToDataSetT2(string json, string image, out string imageStr)
        {
            DataSet ds = new DataSet();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb;
            imageStr = string.Empty;
            if (dic.Count > 0)
            {
                foreach (Dictionary<string, object> drow in dic)
                {
                    dtb = new DataTable();
                    if (dtb.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                        {
                            if (key != image)
                            {
                                dtb.Columns.Add(key, drow[key].GetType());
                            }
                        }
                    }

                    DataRow row = dtb.NewRow();
                    foreach (string key in drow.Keys)
                    {
                        if (key == image)
                        {
                            imageStr = drow[key].ToString();
                        }
                        else
                        {
                            row[key] = drow[key];
                        }
                    }
                    dtb.Rows.Add(row);
                    ds.Tables.Add(dtb);
                }

            }
            return ds;
        }
        #endregion
        #endregion

        #region[JsonToDataSetL]
        /// <summary>
        /// Json转换为DataSet(两张表的)
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>DataSet集合</returns>
        public static DataSet JsonToDataSetL(string json)
        {
            DataSet ds = new DataSet();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb = new DataTable();
            DataTable tableInfo = new DataTable(); ;

            if (dic.Count > 0)
            {

                foreach (Dictionary<string, object> drow in dic)
                {
                    if (dic.IndexOf(drow) == 0)
                    {
                        if (dtb.Columns.Count == 0)
                        {
                            foreach (string key in drow.Keys)
                            {
                                dtb.Columns.Add(key, drow[key].GetType());
                            }
                        }
                        DataRow row = dtb.NewRow();
                        foreach (string key in drow.Keys)
                        {

                            row[key] = drow[key];
                        }
                        dtb.Rows.Add(row);
                        ds.Tables.Add(dtb);
                    }
                    else
                    {

                        if (tableInfo.Columns.Count == 0)
                        {
                            foreach (string key in drow.Keys)
                            {
                                tableInfo.Columns.Add(key, drow[key].GetType());
                            }
                        }

                        DataRow row = tableInfo.NewRow();
                        foreach (string key in drow.Keys)
                        {

                            row[key] = drow[key];
                        }
                        tableInfo.Rows.Add(row);
                    }
                }
            }
            ds.Tables.Add(tableInfo);
            return ds;
        }
        #endregion

        #region 实体对象转换为DataTable
        /// <summary>
        /// 实体对象转换为DataTable
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>DataTable</returns>
        public static DataTable ObjToDataTable(object obj)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] pi = obj.GetType().GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                dt.Columns.Add(new DataColumn(pi[i].Name, pi[i].PropertyType));
            }
            dt.Rows.Add(dt.NewRow());
            for (int i = 0; i < pi.Length; i++)
            {
                dt.Rows[0][i] = obj.GetType().InvokeMember(dt.Columns[i].ColumnName, BindingFlags.GetProperty, null, obj, new object[] { });
            }
            return dt;
        }
        #endregion

        #region DataTable转换为实体对象
        /// <summary>
        /// DataTable转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dt">要转换的DataTable</param>
        /// <returns>转换后的实体对象</returns>
        public static T ConvertToEntity<T>(DataTable dt) where T : new()
        {
            System.Data.DataColumnCollection columns = dt.Columns;
            DataRow row = dt.Rows[0];
            int iColumnCount = columns.Count;
            int i;
            int j;
            T t = new T();
            Type elementType;
            elementType = t.GetType();
            System.Reflection.PropertyInfo[] publicProperties = elementType.GetProperties();
            for (i = 0; i < iColumnCount; i++)
            {
                for (j = 0; j < publicProperties.Length; j++)
                {
                    if (columns[i].ColumnName.ToLower() == publicProperties[j].Name.ToLower())
                    {
                        if (publicProperties[j].PropertyType == typeof(int))
                        {
                            int num = 0;
                            try
                            {
                                num = Convert.ToInt32(row[i]);
                            }
                            catch
                            {
                            }
                            publicProperties[j].SetValue(t, num, null);
                        }
                        else
                        {
                            if (publicProperties[j].PropertyType == typeof(string) && row[i] == System.DBNull.Value)
                            {
                                publicProperties[j].SetValue(t, "", null);
                            }
                            else
                            {
                                object value = row[i] == System.DBNull.Value ? null : row[i];
                                publicProperties[j].SetValue(t, value, null);
                            }
                        }
                    }
                }
            }
            return t;
        }

        #endregion

        #region 自定义

        /// <summary>
        /// 获取键值对Json
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetKeyValueJson(string key, object value)
        {
            var tuple = new Dictionary<string, object> { { key, value } };
            string re = JsonConvert.SerializeObject(tuple);
            if (!string.IsNullOrEmpty(re)) return "(" + re + ")";
            return "";
        }

        #endregion
    }
}