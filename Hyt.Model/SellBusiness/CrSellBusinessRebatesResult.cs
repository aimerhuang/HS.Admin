using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SellBusiness
{
    /// <summary>
    /// 分销商返利清单
    /// </summary>
    /// <remarks>2015-09-11 杨浩 创建</remarks>
    public class CrSellBusinessRebatesResult
    {
        /// <summary>
        /// 执行状态
        /// </summary>
        public int Result { get;set;}
        /// <summary>
        /// 推荐人系统编号
        /// </summary>
        public int RecommendId { get; set; }
        /// <summary>
        /// 间一推荐人系统编号
        /// </summary>
        public int IndirectId { get; set; }
        /// <summary>
        /// 间二推荐人系统编号
        /// </summary>
        public int Indirect2Id { get; set; }
        /// <summary>
        /// 直接推荐人返利
        /// </summary>
        public decimal RecommendReates { get; set; }
        /// <summary>
        /// 间一推荐人返利
        /// </summary>
        public decimal Indirect1Reates { get; set; }
        /// <summary>
        /// 间二推荐人返利
        /// </summary>
        public decimal Indirect2Reates { get; set; }
        /// <summary>
        /// 直接推荐人手机号码
        /// </summary>
        public string RecommendPhone { get; set; }
        /// <summary>
        /// 间一推荐人手机号码
        /// </summary>
        public string Indirect1Phone { get; set; }
        /// <summary>
        /// 间二推荐人手机号码
        /// </summary>
        public string Indirect2Phone { get; set; }

        /// <summary>
        /// 直接推荐人昵称
        /// </summary>
        public string RecommendNickname { get; set; }
        /// <summary>
        /// 间二推荐人昵称
        /// </summary>
        public string Indirect1Nickname { get; set; }
        /// <summary>
        /// 间二推荐人昵称
        /// </summary>
        public string Indirect2Nickname { get; set; }
    }
}
