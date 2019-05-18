using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 支付方式 系统预定义
    /// 数据表：BsPaymentType
    /// </summary>
    /// <remarks>2013-06-18 杨浩 创建</remarks>
    public class PaymentType
    {
        /// <summary>
        /// 现金到付 系统编号
        /// </summary>
        public static int 现金 { get { return 1; } }

        /// <summary>
        /// 刷卡 系统编号
        /// </summary>
        public static int 刷卡 { get { return 2; } }

        /// <summary>
        /// 支票 系统编号
        /// </summary>
        public static int 支票 { get { return 3; } }

        /// <summary>
        /// 转账 系统编号
        /// </summary>
        public static int 转账 { get { return 4; } }

        /// <summary>
        /// 网银 系统编号
        /// </summary>
        public static int 网银 { get { return 5; } }

        /// <summary>
        /// 支付宝 系统编号
        /// </summary>
        public static int 支付宝 { get { return 6; } }

        /// <summary>
        /// 售后换货 系统编号
        /// </summary>
        public static int 售后换货 { get { return 7; } }

        /// <summary>
        /// 分销商预存 系统编号
        /// </summary>
        public static int 分销商预存 { get { return 8; } }


        /// <summary>
        /// 分销商预付 系统编号
        /// </summary>
        public static int 分销商预付 { get { return 15; } }


        /// <summary>
        /// 现金预付 系统编号
        /// </summary>
        public static int 现金预付 { get { return 9; } }

        /// <summary>
        /// 刷卡预付 系统编号
        /// </summary>
        public static int 刷卡预付 { get { return 10; } }

        /// <summary>
        /// 微信支付  系统编号
        /// </summary>
        public static int 微信支付 { get { return 11; } }

        /// <summary>
        /// 易宝支付  系统编号
        /// </summary>
        public static int 易宝支付 { get { return 12; } }
        /// <summary>
        /// 通联支付
        /// </summary>
        public static int 通联支付 { get { return 13; } }
        /// <summary>
        /// 余额支付
        /// </summary>
        public static int 余额支付 { get { return 14; } }

        /// <summary>
        /// 钱袋宝支付
        /// </summary>
        public static int 钱袋宝 { get { return 16; } }

        /// <summary>
        /// 汇付天下支付
        /// </summary>
        public static int 汇付支付 { get { return 22; } }
    }
}
