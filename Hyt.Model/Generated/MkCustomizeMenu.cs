using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 微信菜单
    /// </summary>
    /// <remarks>2016-1-8 16:23 刘伟豪 创建</remarks>
    [Serializable]
    public partial class MkCustomizeMenu
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 上级菜单编号
        /// </summary>
        [Description("上级菜单编号")]
        public int Pid { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        [Description("菜单类型")]
        public string Type { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Description("菜单名称")]
        public string Name { get; set; }
        /// <summary>
        /// 菜单链接
        /// </summary>
        [Description("菜单链接")]
        public string Url { get; set; }
        /// <summary>
        /// 菜单关键字
        /// </summary>
        [Description("菜单关键字")]
        public string Key { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        [Description("经销商编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int Order { get; set; }
    }
}