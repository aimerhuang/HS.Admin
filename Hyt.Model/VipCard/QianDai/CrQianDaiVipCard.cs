using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.VipCard.QianDai
{
    /// <summary>
    /// 钱袋宝会员卡
    /// </summary>
    /// <remarks>2017-03--31 杨浩 创建</remarks>
    public class CrQianDaiVipCard
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 身份证号,仅支持 18 位格式的身份证号
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 钱袋宝会员卡ID
        /// </summary>
        public int CardId { get; set; }
        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
