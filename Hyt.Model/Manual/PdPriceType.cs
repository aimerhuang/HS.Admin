using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 价格来源类型
    /// </summary>
    /// <remarks>2013-07-17 杨浩 创建</remarks>
    [Serializable]
    public class PdPriceType
    {
        /// <summary>
        /// 价格来源
        /// </summary>
        public int PriceSource { get; set; }

        /// <summary>
        /// 价格来源编号
        /// </summary>
        public int SourceSysNo { get; set; }

        /// <summary>
        /// 价格类型名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
