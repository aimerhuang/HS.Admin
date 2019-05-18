using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 数据库升级配置
    /// </summary>
    /// <remarks>2017-1-10 杨浩 创建</remarks>
    public class UpgradeConfig : ConfigBase
    {
        /// <summary>
        /// 版本
        /// </summary>
        public decimal? Version { get; set; }
        /// <summary>
        /// 升级Sql脚本
        /// </summary>
        public string SqlContent { get; set; }
    }
}
