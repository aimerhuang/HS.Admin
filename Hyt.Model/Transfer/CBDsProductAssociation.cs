using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品关联关系对应表(扩展)
    /// </summary>
    /// <remarks>2013-09-13 朱成果 创建</remarks>
    public class CBDsProductAssociation : DsProductAssociation
    {
        /// <summary>
        /// 商城产品ERP编号
        /// </summary>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public string HytProductErpCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public string HytProductName { get; set; }

        /// <summary>
        /// 分销商特别价格
        /// </summary>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public decimal SpecialPrice { get; set; }

        /// <summary>
        /// 分销商等级价格
        /// </summary>
        /// <remarks>2013-09-13 朱成果 创建</remarks>
        public decimal PdPrice { get; set; }

    }
}
