using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{

    public static class MVCGraphicsControl
    {
        /// <summary>
        /// 图形报表MVC插件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name">插件名称</param>
        /// <param name="graphicsType">图形类型</param>
        /// <param name="title">横向标题</param>
        /// <param name="subTitle">横向子标题</param>
        /// <param name="xAxisText">x信息</param>
        /// <param name="yAxisText">y信息</param>
        /// <param name="tooltip">鼠标移动点显示的内容</param>
        /// <param name="graphicsRowData">数据集合</param>
        /// <param name="width">图形的宽度，如果未空则自动宽</param>
        /// <param name="height">图形的高度，需要设置默认值</param>
        /// <returns>返回图形报表html内容</returns>
        /// <remarks>
        /// 2015-10-12 杨云奕 新增
        /// 需要引入 Highcharts 图形报表js信息
        /// </remarks>
        public static MvcHtmlString MVCGraphicsBySingle(this HtmlHelper helper ,string name,
            EnumGraphicsType graphicsType,
            string title,string subTitle,string xAxisText,string yAxisText,
            string tooltip,
            GraphicsRow graphicsRowData, string width = "",string height="400px")
        {
            List<GraphicsRow> list = new List<GraphicsRow>();
            list.Add(graphicsRowData);
            StringBuilder str = new StringBuilder();
            if (!string.IsNullOrEmpty(width))
            {
                str.AppendLine(" <div id=\"" + name + "\" style=\"width:" + width + ";height:" + height + "\"></div>");
            }
            else
            {
                str.AppendLine(" <div id=\"" + name + "\" style=\"min-width: 310px; height: " + height + "; margin: 0 auto\"></div>");
            }
            str.AppendLine("$(function () {");
            str.AppendLine("    $('#container').highcharts({");
            str.AppendLine("        " + Graphics_Type(graphicsType) + ",");
            if (!string.IsNullOrEmpty(title))
            {
                str.AppendLine("        " + Graphics_Title(title, subTitle) + ",");
            }
            str.AppendLine("        " + Graphics_xAxisInfo(graphicsType, graphicsRowData) + ",");
            if (!string.IsNullOrEmpty(yAxisText))
            {
                str.AppendLine("        " + Graphics_yAxisInfo(graphicsType, yAxisText) + ",");
            }
            if (!string.IsNullOrEmpty(tooltip))
            {
                str.AppendLine("        " + Graphics_toolTip(tooltip) + ",");
            }
            str.AppendLine("        " + Graphics_getSeries(graphicsType,list) + "");
            str.AppendLine("    });");
            str.AppendLine("});");
         
            return new MvcHtmlString(str.ToString());
        }

        public static MvcHtmlString MVCGraphicsByMulit(this HtmlHelper helper, string name,
            EnumGraphicsType graphicsType,
            string title, string subTitle, string xAxisText, string yAxisText,
            string tooltip,
            List<GraphicsRow> graphicsDataLists, string width = "", string height = "400px")
        {
            try
            {
                StringBuilder str = new StringBuilder();
                if (!string.IsNullOrEmpty(width))
                {
                    str.AppendLine(" <div id=\"" + name + "\" style=\"width:" + width + ";height:" + height + "\"></div>");
                }
                else
                {
                    str.AppendLine(" <div id=\"" + name + "\" style=\"min-width: 310px; height: " + height + "; margin: 0 auto\"></div>");
                }
                str.AppendLine("<script type=\"text/javascript\">");
                str.AppendLine("$(function () {");
                str.AppendLine("    $('#" + name + "').highcharts({");
                str.AppendLine("        " + Graphics_Type(graphicsType) + ",");
                if (!string.IsNullOrEmpty(title))
                {
                    str.AppendLine("        " + Graphics_Title(title, subTitle) + ",");
                }
                if (Convert.ToString(graphicsType) != "饼形")
                {
                    str.AppendLine("        " + Graphics_xAxisInfo(graphicsType, graphicsDataLists[0]) + ",");
                    str.AppendLine("        " + Graphics_yAxisInfo(graphicsType, yAxisText) + ",");
                }
                if (!string.IsNullOrEmpty(tooltip))
                {
                    str.AppendLine("        " + Graphics_toolTip(tooltip) + ",");
                }
                str.AppendLine("        " + Graphics_getSeries(graphicsType, graphicsDataLists) + "");
                str.AppendLine("    });");
                str.AppendLine("});");
                str.AppendLine("</script>");

                return new MvcHtmlString(str.ToString());
            }
            catch
            {
                return new MvcHtmlString("");
            }
        }

        static string Graphics_Type(EnumGraphicsType graphicsType)
        {
            string chartData = "";
            switch (Convert.ToString(graphicsType))
            {
                case "线形":
                    chartData = " chart: { type: 'line'}";
                    break;
                case "柱形":
                    chartData = " chart: { type: 'column'}";
                    break;
                case "饼形":
                    chartData = " chart: { type: 'pie'}";
                    break;
            }
            return chartData;
        }

        /// <summary>
        /// 图形标题
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subTitle"></param>
        /// <returns></returns>
        static string Graphics_Title(string title, string subTitle)
        {
            string titleData = "";
            if (!string.IsNullOrEmpty(title))
            {
                titleData += "title: { text: '" + title.Replace("'", "”") + "'} ";
            }
            if (!string.IsNullOrEmpty(subTitle))
            {
                if (!string.IsNullOrEmpty(titleData))
                {
                    titleData += ",";
                }
                titleData += "subtitle: { text: '" + subTitle.Replace("'", "”") + "'} ";
            }
            return titleData;
        }
        /// <summary>
        /// x坐标信息
        /// </summary>
        /// <param name="ds_Header"></param>
        /// <param name="headerDataDic"></param>
        /// <param name="headerTxt"></param>
        /// <param name="expHeaderTxt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static string Graphics_xAxisInfo(EnumGraphicsType graphicsType, GraphicsRow graphicsRowData)
        {
            string xAxisInfo = "";
            if (Convert.ToString(graphicsType) != "饼形")
            {
                string columText = "";
                foreach (GraphicsData data in graphicsRowData.graphicsDataList)
                {
                    if (!string.IsNullOrEmpty(columText))
                    {
                        columText += ",";
                    }
                    columText += "'" + data.Text + "'";
                }
                
                if (!string.IsNullOrEmpty(columText))
                {
                    columText = "categories:[" + columText + "]";
                }
                xAxisInfo += columText;
                
                if (!string.IsNullOrEmpty(xAxisInfo))
                {
                    xAxisInfo = "xAxis: {" + xAxisInfo + "}";
                }
            }
            return xAxisInfo;
        }

        /// <summary>
        /// y坐标信息
        /// </summary>
        /// <param name="headerTxt"></param>
        /// <param name="expHeaderTxt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static string Graphics_yAxisInfo(EnumGraphicsType graphicsType, string headerTxt)
        {
            string yAxisInfo = "";
            if (Convert.ToString(graphicsType) != "饼形")
            {
                if (!string.IsNullOrEmpty(headerTxt))
                {
                    yAxisInfo += "title: {text: '" + headerTxt + "'}";
                }
               
                if (!string.IsNullOrEmpty(yAxisInfo))
                {
                    yAxisInfo = "yAxis: {" + yAxisInfo + "}";
                }
            }

            return yAxisInfo;
        }
        /// <summary>
        /// 鼠标在相关线上是的提示信息
        /// </summary>
        /// <param name="mouseToolTip"></param>
        /// <returns></returns>
        static string Graphics_toolTip(string mouseToolTip)
        {
            string toolTip = "";
            if (!string.IsNullOrEmpty(mouseToolTip))
            {
                toolTip += "tooltip: {pointFormat: '" + mouseToolTip + "'}";
            }
            return toolTip;
        }

        /// <summary>
        /// 数据图形
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ds_Header"></param>
        /// <param name="headerDataDic"></param>
        /// <param name="ds_TableData"></param>
        /// <param name="graphicsDs"></param>
        /// <param name="newTable"></param>
        /// <returns></returns>
        static string Graphics_getSeries(EnumGraphicsType graphicsType, List<GraphicsRow> graphicsDataLists)
        {
            string SeriesData = "";
            foreach (GraphicsRow rowData in graphicsDataLists)
            {
                string strRowData = "";
                if (!string.IsNullOrEmpty(SeriesData))
                {
                    SeriesData += ",";
                }
                strRowData += "{name:\"" + rowData.RowName + "\",";
                strRowData += "data:[";
                if (Convert.ToString(graphicsType) != "饼形")
                {
                    string strData = "";
                    foreach (GraphicsData data in rowData.graphicsDataList)
                    {
                        if (!string.IsNullOrEmpty(strData))
                        {
                            strData += ",";
                        }
                        strData += "" + data.Data + "";
                    }
                    strRowData += strData;
                }
                else
                {
                    string strData = "";
                    foreach (GraphicsData data in rowData.graphicsDataList)
                    {
                        if (!string.IsNullOrEmpty(strData))
                        {
                            strData += ",";
                        }
                        strData += "[\"" + data.Text+ "\"," + data.Data + "]";
                    }
                    strRowData += strData;

                }
                strRowData += "]}";
                SeriesData += strRowData;
            }

            return " series: [" + SeriesData + "]";
        }
    }
}
