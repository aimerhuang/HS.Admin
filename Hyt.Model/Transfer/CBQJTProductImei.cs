using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 千机团串码设置扩展表
    /// </summary>
    /// <remarks>2016/2/18 17:19:16 朱成果 创建</remarks>
    public class CBQJTProductImei : QJTProductImei
    {
        /// <summary>
        /// 商品分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
