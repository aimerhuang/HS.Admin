using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商EAS关联
    /// </summary>
    /// <remarks>2013-10-10 黄志勇 创建</remarks>
    [Serializable]
    public class CBDsEasAssociation : DsEasAssociation
    {
        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 店铺账号
        /// </summary>
        public string ShopAccount { get; set; }
        /// <summary>
        /// 是否使用旗帜
        /// </summary>
        public bool IsUseFlag { get; set; }
    }

    /// <summary>
    /// 商城订单和商城类型
    /// </summary>
    /// <remarks>2013-11-28 黄志勇 创建</remarks>
    [Serializable]
    public class CBOrderMallType
    {
        /// <summary>
        /// 商城订单事务编号
        /// </summary>
        public string OrderTransactionSysNo { get; set; }
        /// <summary>
        /// 分销商城类型系统编号
        /// </summary>
        public int MallTypeSysNo { get; set; }
    }
}
