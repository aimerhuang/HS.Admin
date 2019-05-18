using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.DouShabaoModel
{
    /// <summary>
    /// 豆沙包配置
    /// </summary>
    /// <remarks>2017-7-07 罗熙 创建</remarks>
    public class DouShabaoConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DouShabaoConfig()
        {

        }
        /// <summary>
        /// 豆沙包API调用入口
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// 豆沙包API调用测试入口
        /// </summary>
        public string ApiUrlTest { get; set; }
        /// <summary>
        /// API对接Key值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 商户标识Source
        /// </summary>
        public string Source { get; set; }
    }
}
