using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 供应链配置
    /// </summary>
    /// <remarks>2016-3-8 刘伟豪 创建</remarks>
    /// <remarks>2016-3-21 刘伟豪 修改 增加各供应链区分</remarks>
    [Serializable]
    public class SupplyConfig : ConfigBase
    {
        public List<SupplyInfo> SupplyList { get; set; }
    }

    /// <summary>
    /// 供应链详情
    /// </summary>
    [Serializable]
    public class SupplyInfo
    {
        /// <summary>
        /// 供应链编码，与枚举类一致：10-客比邻,20-跨境翼,30-前海洋行
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 供应链名称，与枚举类一致，目前有：客比邻，跨境翼，前海洋行
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 网关地址
        /// </summary>
        public string GatewayUrl { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secert { get; set; }
    }
}