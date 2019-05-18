using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 配送方式 预设值
    /// 数据表：LgDeliveryType
    /// </summary>
    /// <remarks>2013-07-06 吴文强 创建</remarks>
    public class DeliveryType
    {
        /// <summary>
        /// 百城当日达 系统编号
        /// </summary>
        public static int 百城当日达 { get { return 1; } }

        /// <summary>
        /// 自提 系统编号
        /// </summary>
        public static int 自提 { get { return 2; } }

        /// <summary>
        /// 第三方快递 系统编号
        /// </summary>
        public static int 第三方快递 { get { return 3; } }

        /// <summary>
        /// 普通百城当日达 系统编号
        /// </summary>
        public static int 普通百城当日达 { get { return 4; } }

        /// <summary>
        /// 加急百城当日达 系统编号
        /// </summary>
        public static int 加急百城当日达 { get { return 5; } }

        /// <summary>
        /// 定时百城当日达 系统编号
        /// </summary>
        public static int 定时百城当日达 { get { return 6; } }

        /// <summary>
        /// 门店自提 系统编号
        /// </summary>
        public static int 门店自提 { get { return 7; } }
        /// <summary>
        /// 小区配送 系统编号
        /// </summary>
        public static int 小区配送 { get { return 14; } }
        /// <summary>
        /// 百世汇通 系统编号
        /// </summary>
        public static int 百世汇通 { get { return 16; } }

        /// <summary>
        /// 百世汇通电子面单
        /// </summary>
        public static int 百世汇通电子面单
        {
            get
            {
                var bestExpressDeliveryTypeNo = ConfigurationManager.AppSettings["BestExpressDeliveryTypeNo"];
                if (!String.IsNullOrEmpty(bestExpressDeliveryTypeNo))
                {
                    return Convert.ToInt32(bestExpressDeliveryTypeNo);
                }
                return 36;
            }
        }
    }
}
