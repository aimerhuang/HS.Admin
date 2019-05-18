
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 经销商
    /// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class DsDealer
    {   
        /// <summary>
        /// 系统编号 
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        ///// <summary>
        ///// 上级经销商编号
        ///// </summary>
        //[Description("上级经销商编号")]
        //public int ParentId { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int LevelSysNo { get; set; }
        /// <summary>
        /// 系统用户系统编号
        /// </summary>
        [Description("系统用户系统编号")]
        public int UserSysNo { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        [Description("分销商名称")]
        public string DealerName { get; set; }
        /// <summary>
        /// 地区编号
        /// </summary>
        [Description("地区编号")]
        public int AreaSysNo { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        [Description("街道地址")]
        public string StreetAddress { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Contact { get; set; }
        /// <summary>
        /// 联系人座机号码
        /// </summary>
        [Description("联系人座机号码")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        [Description("联系人手机号码")]
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        [Description("联系人邮箱")]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        [Description("状态:启用(1),禁用(0)")]
        public int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// ERP编号
        /// </summary>
        [Description("ERP编号")]
        public string ErpCode { get; set; }
        /// <summary>
        /// ERP名称
        /// </summary>
        [Description("ERP名称")]
        public string ErpName { get; set; }
        /// <summary>
        /// 分销商图片Url
        /// </summary>
        [Description("分销商图片Url")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// IcoUrl
        /// </summary>
        [Description("IcoUrl")]
        public string IcoUrl { get; set; }
        /// <summary>
        /// AppID
        /// </summary>
        [Description("AppID")]
        public string AppID { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        [Description("AppSecret")]
        public string AppSecret { get; set; }
        /// <summary>
        /// 微信公众账号
        /// </summary>
        [Description("微信公众账号")]
        public string WeiXinNum { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        [Description("域名")]
        public string DomainName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题")]
        public string Title { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        [Description("关键字")]
        public string Keyword { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public string Description { get; set; }
        /// <summary>
        /// 三级分销:启用(1),禁用(0)
        /// </summary>
        [Description("三级分销")]
        public int ThreeLevels { get; set; }
        /// <summary>
        /// 微信令牌
        /// </summary>
        [Description("微信令牌")]
        public string Token { get; set; }
        /// <summary>
        /// PC域名
        /// </summary>
        [Description("PC域名")]
        public string PcHost { get; set; }
        /// <summary>
        /// 微信公众号二维码
        /// </summary>
        [Description("微信公众号二维码")]
        public string WxQrCode { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [Description("扩展字段")]
        public string Extensions { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [Description("利嘉会员系统编号-利嘉返回")]
        public int LiJiaSysNo { get; set; }

        private int isWholeSaler = 0;

        /// <summary>
        /// 是否为批发商（0：否 1：是）
        /// </summary>
        public int IsWholeSaler
        {
            get
            {
                return isWholeSaler;
            }
            set
            {
                isWholeSaler = value;
            }
        }
    }

}