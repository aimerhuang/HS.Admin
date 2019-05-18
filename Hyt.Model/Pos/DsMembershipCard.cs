using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsMembershipCard:DsMembershipCard
    {
        public string DealerName { get; set; }
        public string LevelName { get; set; }
        public decimal PointToFen { get; set; }
    }
    /// <summary>
    /// 会员卡
    /// </summary>
    /// <remarks>
    /// 2016-02-26 杨云奕 添加
    /// </remarks>
    public class DsMembershipCard
    {
        /// <summary>
        ///  自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        public int DsCustomSysNo { get; set; }
        /// <summary>
        /// 会员名称    
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 会员卡卡号
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string LinkTele { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 会员等级编号
        /// </summary>
        public int UserLevel { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public long PointIntegral { get; set; }
        /// <summary>
        /// 是否同步到服务器
        /// </summary>
        public int OnWebType { get; set; }
    }
}
