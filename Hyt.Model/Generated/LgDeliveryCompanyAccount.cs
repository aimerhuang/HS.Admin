using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///物流公司账号表
    ///</summary>
    /// <remarks> 2015-10-10 朱成果 生成</remarks>
    public partial class LgDeliveryCompanyAccount : BaseEntity
    {

        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///账号名称
        ///</summary>
        [Description("账号名称")]
        public string AccountName { get; set; }

        ///<summary>
        ///配送方式编号
        ///</summary>
        [Description("配送方式编号")]
        public int DeliveryTypeSysNo { get; set; }

        ///<summary>
        ///账号ID
        ///</summary>
        [Description("账号ID")]
        public string AccountId { get; set; }

        ///<summary>
        ///账号密钥
        ///</summary>
        [Description("账号密钥")]
        public string AccountSecretKey { get; set; }

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

        ///<summary>
        ///最后更新人
        ///</summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }

    }
}

