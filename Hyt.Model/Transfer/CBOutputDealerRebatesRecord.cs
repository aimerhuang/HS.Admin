using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBOutputDealerRebatesRecord
    {
        public string 所属分销商 { get; set; }
        public string 推荐人账号 { get; set; }
        public string 推荐人名称 { get; set; }
        public string 消费者账号 { get; set; }
        public string 消费者名称 { get; set; }
        public string 动作类型 { get; set; }
        public string 返利类型 { get; set; }
        public string 返利金额 { get; set; }
        public string 来源编号 { get; set; }
        public string 状态 { get; set; }
        public string 返利时间 { get; set; }
    }
}
