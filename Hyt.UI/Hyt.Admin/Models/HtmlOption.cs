using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 生成带分组的select
    /// <select name="test">
    ///<optgroup label="==禁选项1=="></optgroup> 
    ///<option value="6993628">选项1</option>
    ///</select>
    /// </summary>
    /// <remarks>2013－08-28 朱成果 创建</remarks>
    public class HtmlOption : System.Web.WebPages.Html.SelectListItem
    {
        /// <summary>
        /// 显示文字
        /// </summary>
        public string label { get; set; }
 
        /// <summary>
        /// 是否生成optgroup
        /// </summary>
        public bool optgroup { get; set; }

        /// <summary>
        /// 是否在百城当日送地图内
        /// </summary>
        public bool IsInMap { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseNo { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
    }
}