using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 用于补单显示添加的商品
    /// </summary>
    /// <remarks>2013-07-11 沈强 创建</remarks>
    public class CBPdProductJson
    {
        /// <summary>
        /// 借货单明细系统编号
        /// </summary>
        //public int SysNo { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 借货单中的商品存货数量
        /// </summary>
        public int ProductNum { get; set; }

        /// <summary>
        /// 商品订购数量（由页面控件设置）
        /// </summary>
        public int ProductOrderNum { get; set; }

        /// <summary>
        /// 会员等级价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
