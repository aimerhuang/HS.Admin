
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///  2014-06-05 杨浩 T4生成
    /// </remarks>
    public partial class DsUser : BaseEntity
    {
        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///分销商系统编号
        ///</summary>
        [Description("分销商系统编号")]
        public int DealerSysNo { get; set; }

        ///<summary>
        ///分销商账号
        ///</summary>
        [Description("分销商账号")]
        public string Account { get; set; }

        ///<summary>
        ///分销商密码
        ///</summary>
        [Description("分销商密码")]
        public string Password { get; set; }

        ///<summary>
        ///姓名
        ///</summary>
        [Description("姓名")]
        public string Name { get; set; }

        ///<summary>
        ///状态:有效(1);无效(0)
        ///</summary>
        [Description("状态:有效(1);无效(0)")]
        public int Status { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }

        ///<summary>
        ///最后更新人
        ///</summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }


    }
}
