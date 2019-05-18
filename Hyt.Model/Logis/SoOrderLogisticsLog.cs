using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Logis
{
    public class SoOrderLogisticsLog
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 类型编码
        /// </summary>
        public string TypeNo { get; set; }
        /// <summary>
        /// 类型代码
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeMsg { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgInfo { get; set; }

        public string MsgResturnData { get; set; }
    }
}
