using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品浏览历史记录
    /// </summary>
    /// <remarks>2013-9-4 黄伟 创建</remarks>
    public class CBCrBrowseHistory : CrBrowseHistory
    {
        /// <summary>
        /// 客户账户
        /// </summary>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }
    }
}
