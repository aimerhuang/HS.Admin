using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 第三方配送信息报表查询实体类
    /// </summary>
    /// <remarks>
    /// 2014-09-24 余勇 创建
    /// </remarks>
    public class CBRptExpressLgDelivery
    {
        public string 统计日期 { get; set; }
        public string 办事处 { get; set; }
        public string 快递公司 { get; set; }
        public int 总单量 { get; set; }
    }
}
