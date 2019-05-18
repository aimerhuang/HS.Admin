using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{

    /// <summary>
    /// 集装箱实体
    /// </summary>
    /// <remarks>
    ///     
    /// </remarks>
    public class PmContainer
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 集装箱名称
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 集装箱类型
        /// </summary>
        public string CType { get; set; }
        /// <summary>
        /// 箱子容积类型
        /// </summary>
        public string CubeType { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public string CSHeight { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string CSWidth { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public string CSLong { get; set; }
        /// <summary>
        /// 集装箱描述
        /// </summary>
        public string CDis { get; set; }
    }
}
