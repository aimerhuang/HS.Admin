using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 代理商
    /// </summary>
    /// <remarks>
    /// 2016-4-13 刘伟豪 创建
    /// </remarks>
    [Serializable]
    public partial class DsAgent
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 代理商等级
        /// </summary>
        [Description("代理商等级")]
        public int LevelSysNo { get; set; }

        /// <summary>
        /// 管理员编号
        /// </summary>
        [Description("管理员编号")]
        public int UserSysNo { get; set; }

        /// <summary>
        /// 代理商名称
        /// </summary>
        [Description("代理商名称")]
        public string Name { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        [Description("地区编号")]
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [Description("详细地址")]
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Contact { get; set; }

        /// <summary>
        /// 座机号码
        /// </summary>
        [Description("座机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("手机号码")]
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Description("电子邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 状态：0禁用，1启用
        /// </summary>
        [Description("状态：0禁用，1启用")]
        public int Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Description("创建日期")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        [Description("最后更新日期")]
        public DateTime LastUpdateDate { get; set; }
    }
}