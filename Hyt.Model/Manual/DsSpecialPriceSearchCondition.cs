using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 仓库信息搜索条件类
    /// </summary>
    /// <remarks>2013-08-07 周瑜 创建</remarks>
    public class DsSpecialPriceSearchCondition
    {
        /// <summary>
        /// 商品编号或分销商编号
        /// </summary>
        public int? SysNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 商品名称或分销商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商编号集合
        /// </summary>
        public List<int> DealerSysNoList { get; set; }
    }
}
