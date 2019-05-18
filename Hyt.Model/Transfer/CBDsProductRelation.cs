using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBDsProductRelation
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 商城商品编码
        /// </summary>
        public string MallProductId { get; set; }
        /// <summary>
        /// 商城商品属性
        /// </summary>
        public string MallProductAttr { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// Eas名称
        /// </summary>
        public string EasName { get; set; }

    }
}
