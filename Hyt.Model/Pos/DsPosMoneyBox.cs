using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsPosMoneyBox : DsPosMoneyBox
    {
        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 收银软件名称
        /// </summary>
        public string PosSYName { get; set; }
    }
    /// <summary>
    /// 收钱箱实体
    /// </summary>
    public class DsPosMoneyBox
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 收银机编号
        /// </summary>
        public int DsPosSysNo { get; set; }
        /// <summary>
        /// 钱箱初始金额
        /// </summary>
        public decimal SaveMoney { get; set; }
        /// <summary>
        /// 设置时间
        /// </summary>
        public DateTime ActionTime { get; set; }
        /// <summary>
        /// 设置人员名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 设置时间
        /// </summary>
        public DateTime SetTime { get; set; }
    }
}
