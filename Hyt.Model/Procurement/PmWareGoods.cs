using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    /// <summary>
    /// 采购商品库存实体
    /// </summary>
    /// <remarks>
    /// 2016-3-5 杨云奕 添加
    /// </remarks>
    public class CBPmWareGoods : PmWareGoods
    {
        public int Type { get; set; }
        public string ProName { get; set; }
        public string Unit { get; set; }
        public string Spec { get; set; }
    }

    /// <summary>
    /// 库存商品
    /// </summary>
    /// <remarks>
    /// 2016-3-5 杨云奕 添加
    /// </remarks>
    public class PmWareGoods
    {
        public int SysNo { get; set; }
        public int ProSysNo { get; set; }
        public int StayInWare { get; set; }
        public int WareNum { get; set; }
        public int Freeze { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
