using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP库存
    /// </summary>
    /// <remarks>2014-03-05 周唐炬 创建</remarks>
    public class AppInventory : BaseEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 借货单系统编号
        /// </summary>
        public int ProductLendSysNo { get; set; }
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 借货数量
        /// </summary>
        public int LendQuantity { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int SaleQuantity { get; set; }
        /// <summary>
        /// 还货数量
        /// </summary>
        public int ReturnQuantity { get; set; }
        /// <summary>
        /// 强制完结数量
        /// </summary>
        public int ForceCompleteQuantity { get; set; }
    }
}
