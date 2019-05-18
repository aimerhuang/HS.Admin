using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 会员分销商申请拓展表
    /// </summary>
    /// <remarks>2016-04-08 刘伟豪 创建</remarks>
    [Serializable]
    public class CBCrDealerApply : CrDealerApply
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 客户等级编号
        /// </summary>
        public int LevelSysNo { get; set; }

        /// <summary>
        /// 客户等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int? DealerSysNo { get; set; }
        public string AreaName { get; set; }
    }
}