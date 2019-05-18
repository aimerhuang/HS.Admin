using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 融e购返回header报文
    /// </summary>
    public class RongEgouResultParameters
    {
        /// <summary>
        /// Api接口名称
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// 请求流水号
        /// </summary>
        public string req_sid { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 接入系统标识
        /// </summary>
        public string app_key { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string auth_code { get; set; }

        /// <summary>
        /// 返回代码
        /// </summary>
        public string ret_code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string ret_msg { get; set; }

        /// <summary>
        /// 交易签名 
        /// </summary>
        public string sign { get; set; }
    }
}
