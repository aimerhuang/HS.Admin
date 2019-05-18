using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 申请加盟
    /// </summary>
    /// <remarks>
    /// 2016-03-30 周海鹏 创建
    /// </remarks>
    public partial class DsApplyStore
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 店铺编号
        /// </summary>
        [Description("店铺编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 申请人联系地址
        /// </summary>
        [Description("申请人联系地址")]
        public string ApplicantAddress { get; set; }
        /// <summary>
        /// 申请人真实姓名
        /// </summary>
        [Description("申请人真实姓名")]
        public string ApplicantContacts { get; set; }
        /// <summary>
        /// 申请人手机号码
        /// </summary>
        [Description("申请人手机号码")]
        public string ApplicantMobilephone { get; set; }
        /// <summary>
        /// 申请人性别
        /// </summary>
        [Description("申请人性别")]
        public string ApplicantSex { get; set; }
        /// <summary>
        /// 申请人微信号
        /// </summary>
        [Description("申请人微信号")]
        public string ApplicantWechat { get; set; }
        /// <summary>
        /// 申请人是否有店面
        /// </summary>
        [Description("申请人是否有店面")]
        public string ApplicantIsStore { get; set; }
        /// <summary>
        /// 申请人店面所在位置
        /// </summary>
        [Description("申请人店面所在位置")]
        public string ApplicantStorePostion { get; set; }
        /// <summary>
        /// 申请人店面面积
        /// </summary>
        [Description("申请人店面面积")]
        public string ApplicantStoreAreas { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        [Description("申请时间")]
        public DateTime ApplicantTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 处理状态 0默认未处理
        /// </summary>
        [Description("处理状态 0默认未处理")]
        public int State { get; set; }
        /// <summary>
        /// 移除状态 0 默认
        /// </summary>
        [Description("移除状态 0 默认")]
        public int Isdelete { get; set; }

    }

}
