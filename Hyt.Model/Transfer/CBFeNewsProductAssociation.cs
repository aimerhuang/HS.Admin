using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 新闻商品关联表扩展类
    /// </summary>
    /// <remarks>2014-01-15 苟治国 创建</remarks>
    public class CBFeNewsProductAssociation:FeNewsProductAssociation
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 后台显示名称
        /// </summary>
        [Description("后台显示名称")]
        public string EasName { get; set; }
    }
}
