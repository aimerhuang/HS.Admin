using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉查询参数
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
    public class LiJiaMemberSearch
    {
        /// <summary>
        /// 返回第几面,默认:1
        /// </summary>
        [Description("返回第几面,默认:1")]
        public int page { get; set; }
        /// <summary>
        /// 每页记录数,默认:20
        /// </summary>
        [Description("每页记录数,默认:20")]
        public int rows { get; set; }
        /// <summary>
        /// 查询数据集
        /// </summary>
        [Description("查询数据集")]
        public List<LiJiaSearch> rules { get; set; }
        
    }
}
