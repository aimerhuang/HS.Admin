using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 业务数据配置
    /// </summary>
    [Serializable]
    public class SySmartConfig {
        public int SysNo { get; set; }
        public string Type { get; set; }
        public string TypeIdentity { get; set; }
        public string Timestamp { get; set; }
        public string Config { get; set; }
    }
}
