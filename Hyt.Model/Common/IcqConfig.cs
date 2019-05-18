using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 商检配置
    /// </summary>
    /// <remarks>2016-3-20 杨浩 创建</remarks>
    public class IcqConfig : ConfigBase
    {
        /// <summary>
        /// 商品备案报文类型
        /// </summary>
        public string GoodsMessageType { get; set; }
        /// <summary>
        /// 订单报文类型
        /// </summary>
        public string OrderMessageType { get; set; }
        /// <summary>
        /// 报文发送者标识
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 报文接收人标识
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// FTP地址
        /// </summary>
        public string FtpUrl { get; set; }

        /// <summary>
        /// FTP地址用户名
        /// </summary>
        public string FtpName { get; set; }

        /// <summary>
        /// FTP密码
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// 业务类型 单向海关申报填CUS、单向国检申报填CIQ、同时发送可填BBC
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string SignerInfo { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
    }
}
