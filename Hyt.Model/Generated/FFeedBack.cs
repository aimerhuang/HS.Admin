using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    /// <remarks>
    /// 2016-05-05 周海鹏 创建
    [Serializable]
    public partial class FFeedBack
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 客户系统编号
        /// </summary>
        [Description("客户系统编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        [Description("分销商系统编号")]
        public int DsDealerSysNo { get; set; }
        /// <summary>
        /// 反馈类型1（产品咨询）
        /// </summary>
        [Description("反馈类型1（产品咨询）")]
        public int FeedType1 { get; set; }
        /// <summary>
        /// 反馈类型2（售后问题(订单或物流)）
        /// </summary>
        [Description("反馈类型2（售后问题(订单或物流)）")]
        public int FeedType2 { get; set; }
        /// <summary>
        /// 反馈类型3（意见建议/投诉）
        /// </summary>
        [Description("意见建议/投诉")]
        public int FeedType3 { get; set; }
        /// <summary>
        /// 反馈类型4（其他问题）
        /// </summary>
        [Description("其他问题")]
        public int FeedType4 { get; set; }
        /// <summary>
        /// 反馈内容
        /// </summary>
        [Description("反馈内容")]
        public string FeedRemarks { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string FeedTel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime FeedAddTime { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        [Description("处理时间")]
        public string UpdateTime { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        [Description("处理状态")]
        public int State { get; set; }
        /// <summary>
        /// 是否删除（伪删除）
        /// </summary>
        [Description("是否删除（伪删除）")]
        public int IsDelete { get; set; }
    }
}
