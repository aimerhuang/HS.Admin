using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    public class RongEgouParameters
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RongEgouParameters()
        {

        }
        #region 传入公共参数
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
        /// 交互数据包格式 （目前均为XML格式）
        /// </summary>
        public string format { get; set; }

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
        /// 交易签名 
        /// </summary>
        public string sign { get; set; }

        #endregion
    }
}
