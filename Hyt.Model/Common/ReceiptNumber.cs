using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 单据编号
    /// </summary>
    /// <remarks>2016-1-1 杨浩 创建</remarks>
    public class SyReceiptNumber
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 示例
        /// </summary>
        public string Demo { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Leng { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Des { get; set; }
    }
}
