using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 仓库修改
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
    public class LiJiaStoreEdit
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Description("仓库编码")]
        public string  StoreCode { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Description("仓库名称")]
        public string StoreName { get; set; }
    }
}
