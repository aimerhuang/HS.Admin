using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 物流配送信息报表查询实体类
    /// </summary>
    /// <remarks>
    /// 2014-04-10 余勇 创建
    /// </remarks>
    public class CBRptLgExpressInfo
    {
        public string 统计日期 { get; set; }
        public int 成功单量 { get; set; }
        public int 失败单量 { get; set; }
        public int 总单量 { get; set; }
    }
}
