using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 升舱明细导出
    /// </summary>
    /// <remarks>黄伟 2013-12-18 创建</remarks>
    public class CBUpgradeDetailsExport
    {
        public int 订单编号 { get; set; }
        public string 第三方订单号 { get; set; }
        public DateTime 升舱付款时间 { get; set; }
        public DateTime 商城订单时间 { get; set; }
        public string 所属分支机构 { get; set; }
        public string 客户所在城市 { get; set; }
        public string 商城名称 { get; set; }
        public string 升舱来源店面 { get; set; }
        public string 物流类型 { get; set; }
        public string 产品名称 { get; set; }
        public decimal 付款金额 { get; set; }
        public string 发货时间 { get; set; }
        public string 未发货原因 { get; set; }
        public string 备注 { get; set; }
    }
}
