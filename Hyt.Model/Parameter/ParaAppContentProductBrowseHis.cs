using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 商品历史浏览记录参数
    /// </summary>
    /// <remarks>2013-9-4 黄伟 创建</remarks>
    class ParaAppContentProductBrowseHis:BaseEntity
    {
        /// <summary>
        /// 客户账户
        /// </summary>
        public string CustomerAccount { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
    }
}
