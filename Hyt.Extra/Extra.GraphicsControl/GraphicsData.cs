using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// 图形单实体数据集合
    /// </summary>
    /// <remarks>2015-10-12 杨云奕 添加</remarks>
    public class GraphicsData
    {
        public string Text { get; set; }
        public decimal Data { get; set; }
    }
    /// <summary>
    /// 图像报表实体，行数据集合
    /// </summary>
    /// <remarks>2015-10-12 杨云奕 添加</remarks>
    public class GraphicsRow
    {
        public string RowName { get; set; }
        public List<GraphicsData> graphicsDataList = new List<GraphicsData>();
    }
}
