using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 地区信息查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-08-02 郑荣华 创建
    /// </remarks>
   public class ParaBsAreaFilter
    {
        /// <summary>
        /// 系统编号(查询包括其父级和子级）
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 地区名称和名称拼音
        /// </summary>
        public string AreaNameAndAcronym { get; set; }

        /// <summary>
        /// 地区编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
