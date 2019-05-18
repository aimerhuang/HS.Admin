using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 格格家配置
    /// </summary>
    /// <remarks>2017-08-18 黄杰 创建</remarks>
    public class GeGeJiaConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GeGeJiaConfig()
        {

        }

        /// <summary>
        /// 格格家API调用入口
        /// </summary>
        public string ApiUrl { get; set; }


        /// <summary>
        /// 格格家开放平台应用Partner
        /// </summary>
        public string Partner { get; set; }

        /// <summary>
        /// 格格家开放平台应用AppKey
        /// </summary>
        public string AppKey { get; set; }

    }
}
