using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商商城查询实体
    /// </summary>
    /// <remarks>
    /// 2013-09-18 郑荣华 创建
    /// </remarks>
    public class ParaDsDealerMallFilter
    {
        /// <summary>
        /// 分销商系统编号
        /// </summary>     
        public int? DealerSysNo { get; set; }

        /// <summary>
        /// 分销商城类型系统编号
        /// </summary>   
        public int? MallTypeSysNo { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 是否自营 
        /// </summary>
        public int? IsSelfSupport { get; set; }
    }
}
