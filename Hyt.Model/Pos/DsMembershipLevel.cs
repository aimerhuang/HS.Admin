using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// 会员卡会员等级
    /// </summary>
    public class DsMembershioLevel
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商等级
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// 会员卡等级名称
        /// </summary>
        public decimal Privilege { get; set; }
        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public int PointToFen { get; set; }
        /// <summary>
        /// 是否上次服务器
        /// </summary>
        public int WebSysNo { get; set; }
    }
}
