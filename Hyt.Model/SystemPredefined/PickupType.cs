using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 取件方式 预设值
    /// 数据表：LgPickupType
    /// </summary>
    /// <remarks>2013-07-06 吴文强 创建</remarks>
    public class PickupType
    {
        /// <summary>
        /// 百城当日取件 系统编号
        /// </summary>
        public static int 百城当日取件 { get { return 1; } }

        /// <summary>
        /// 送货至门店 系统编号
        /// </summary>
        public static int 送货至门店 { get { return 2; } }

        /// <summary>
        /// 快递至门店 系统编号
        /// </summary>
        public static int 快递至仓库 { get { return 3; } }

        /// <summary>
        /// 普通取件 系统编号
        /// </summary>
        public static int 普通取件 { get { return 4; } }

        /// <summary>
        /// 加急取件 系统编号
        /// </summary>
        public static int 加急取件 { get { return 5; } }

        /// <summary>
        /// 定时取件 系统编号
        /// </summary>
        public static int 定时取件 { get { return 6; } }

    }
}
