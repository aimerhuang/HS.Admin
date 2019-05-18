
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 短信咨询
    /// </summary>
    /// <remarks>
    /// 2014-02-25 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class CrSmsQuestion
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>
        [Description("客户编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 咨询电话
        /// </summary>
        [Description("咨询电话")]
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        [Description("咨询内容")]
        public string Question { get; set; }
        /// <summary>
        /// 咨询时间
        /// </summary>
        [Description("咨询时间")]
        public DateTime QuestionDate { get; set; }
        /// <summary>
        /// 回复人
        /// </summary>
        [Description("回复人")]
        public int AnswerSysNo { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        [Description("回复内容")]
        public string Answer { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        [Description("回复时间")]
        public DateTime AnswerDate { get; set; }
        /// <summary>
        /// 状态：待回复（10）、已回复（20）、回复失败（-5）、
        /// </summary>
        [Description("状态：待回复（10）、已回复（20）、回复失败（-5）、")]
        public int Status { get; set; }
    }
}

