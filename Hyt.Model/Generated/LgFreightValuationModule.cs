using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 运费模板 保价金额定义表
    /// </summary>
    /// <remarks>2015-11-27 杨云奕 添加</remarks>
    public class LgFreightValuationModule
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 标价标题
        /// </summary>
        public string lgfvm_title { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public string lgfvm_AreaSysNo { get; set; }
        /// <summary>
        /// 报价范围最小值
        /// </summary>
        public decimal lgfvm_MinValua { get; set; }
        
        /// <summary>
        /// 报价范围最大值
        /// </summary>
        public decimal lgfvm_MaxValua { get; set; }
        /// <summary>
        /// 报价类型
        /// </summary>
        public string lgfvm_ValueType { get; set; }
        /// <summary>
        /// 运费计算值
        /// </summary>
        public decimal lgfvm_FreightValue { get; set; }

        /// <summary>
        /// 报价运费父id
        /// </summary>
        public int lgfvm_pid { get; set; }
    }
}
