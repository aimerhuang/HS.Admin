using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 菜单扩展
    /// </summary>
    /// 2013-09-26 朱家宏 创建
    public partial class SyMenu
    {
        /// <summary>
        /// 菜单等级
        /// </summary>
        [Description("菜单等级")]
        public int Level { get; set; }
    }
}
