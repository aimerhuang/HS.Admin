using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///仓库物流公司账号关联表
    ///</summary>
    /// <remarks> 2015-10-10 朱成果 生成</remarks>
    public partial class LgStoreLogisticsCompany : BaseEntity
    {

        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///仓库编号
        ///</summary>
        [Description("仓库编号")]
        public int WarehouseSysNo { get; set; }

        ///<summary>
        ///物流账号编号
        ///</summary>
        [Description("物流账号编号")]
        public int AccountSysNo { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        [Description("创建人")]
        public int CreateBy { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        [Description("创建时间")]
        public DateTime CreateDate { get; set; }

    }
}

