using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hyt.BLL.ApiSupply.U1City
{
    public class Comm
    {
        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果.如果要转换的字符串是非数字,则返回0.</returns>
        public static int StrToInt(object strValue)
        {
            return StrToInt(strValue, 1000);
        }

        /// <summary>
        /// string型转换为int型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object strValue, int defValue)
        {
            if ((strValue == null) || (strValue.ToString() == string.Empty))
            {
                return defValue;
            }

            string val = strValue.ToString();
            if (val.IndexOf(".") >= 0)
            {
                val = val.Split(new char[] { '.' })[0];
            }
            int intValue = 0;
            if (int.TryParse(val, out intValue))
            {
                return intValue;
            }
            return defValue;
        }

        public static string get_Xml_Nodes(XmlDocument Xml, string xPath)
        {
            XmlNodeList Xml_Nodes = Xml.SelectNodes(xPath);
            try
            {
                return Xml_Nodes[0].InnerText.Replace("'", "`");
            }
            catch
            {
                return "";
            }
        }

        public static string get_JsonValueByName(string sResult, string NodeName)
        {
            sResult = sResult.Replace(",null", ",\"null\"");
            SortedList slt = new SortedList();
            try
            {
                slt = (SortedList)Newtonsoft.Json.JsonConvert.DeserializeObject(sResult, System.Type.GetType("System.Collections.SortedList"));
            }
            catch
            {
                return "";
            }

            return slt[NodeName].ToString();
        }

    }
}
