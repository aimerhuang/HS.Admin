using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2016-04-16 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class DsDealerApply
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
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string ContactName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [Description("联系方式")]
        public string ContactWay { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        [Description("公司名称")]
        public string CompanyName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Description("身份证号码")]
        public string IDCard { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        [Description("提交时间")]
        public DateTime CommitDate { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [Description("扩展字段")]
        public string Extend { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        [Description("处理人")]
        public int HandlerSysNo { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        [Description("处理时间")]
        public DateTime HandleDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
    }
}
