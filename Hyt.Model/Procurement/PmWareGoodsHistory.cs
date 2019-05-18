using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    
    /// <summary>
    /// 库存采购操作记录实体
    /// </summary>
    /// <remarks>
    /// 2016-03-05 杨云奕 添加
    /// </remarks>
    public class PmWareGoodsHistory
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 采购库父编号
        /// </summary>
        public int PSysNo { get; set; }
        /// <summary>
        /// 采购流水内容描述
        /// </summary>
        public string TextInfo { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public int GHValue { get; set; }
        /// <summary>
        ///  添加流水记录创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
