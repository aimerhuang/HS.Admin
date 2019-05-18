using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 分销商授权信息
    /// </summary>
    /// <remarks>2013-8-27 陶辉 创建</remarks>
    public class UpGradeAuthorization
    {
        /// <summary>
        /// 分销商编号
        /// </summary>
        public int HytDealerSysNo { get; set; }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 店铺账号 
        /// </summary>
        /// <remarks>2013-09-09 黄志勇 添加</remarks>
        public string ShopAccount { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 分销商预存款系统编号 
        /// </summary>
        /// <remarks>2013-09-09 黄志勇 添加</remarks>
        public int PrePaymentSysNo { get; set; }

        /// <summary>
        /// 分销商名称
        /// </summary>
        public string HytDealerName { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal HytLeftAmount { get; set; }

        /// <summary>
        /// 账户冻结金额
        /// </summary>
        public decimal HytFrozenAmount { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 当前授权商城类型，淘宝，京东，亚马逊等
        /// </summary>
        public int MallType { get; set; }

        /// <summary>
        /// 是否使用预存款
        /// </summary>
        public int IsPreDeposit { get; set; }

        /// <summary>
        /// 当前授权授权码
        /// </summary>
        public string MallAuthorizationCode { get; set; }

        /// <summary>
        /// 是否自营
        /// </summary>
        public int IsSelfSupport { get; set; }

        /// <summary>
        /// 分销商用户编号
        /// </summary>
        public int DsUserSysNo { get; set; }

    }
}
