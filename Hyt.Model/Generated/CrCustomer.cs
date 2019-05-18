
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-10-30 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class CrCustomer
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int LevelSysNo { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        [Description("用户账号")]
        public string Account { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("用户密码")]
        public string Password { get; set; }
        /// <summary>
        /// 等级积分
        /// </summary>
        [Description("等级积分")]
        public int LevelPoint { get; set; }
        /// <summary>
        /// 经验积分
        /// </summary>
        [Description("经验积分")]
        public int ExperiencePoint { get; set; }
        /// <summary>
        /// 惠源币
        /// </summary>
        [Description("惠源币")]
        public decimal ExperienceCoin { get; set; }
        /// <summary>
        /// 可用积分
        /// </summary>
        [Description("可用积分")]
        public int AvailablePoint { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        public string HeadImage { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Description("昵称")]
        public string NickName { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Description("电子邮箱")]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 邮箱状态：未验证（10）、已验证（20）
        /// </summary>
        [Description("邮箱状态：未验证（10）、已验证（20）")]
        public int EmailStatus { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("手机号码")]
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 手机状态：未验证（10）、已验证（20）
        /// </summary>
        [Description("手机状态：未验证（10）、已验证（20）")]
        public int MobilePhoneStatus { get; set; }
        /// <summary>
        /// 性别：保密（0）、男（1）、女（2）
        /// </summary>
        [Description("性别：保密（0）、男（1）、女（2）")]
        public int Gender { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Description("身份证号码")]
        public string IDCardNo { get; set; }
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
        /// 生日
        /// </summary>
        [Description("生日")]
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 婚姻状况：保密（0）、未婚（1）、已婚（2）
        /// </summary>
        [Description("婚姻状况：保密（0）、未婚（1）、已婚（2）")]
        public int MaritalStatus { get; set; }
        /// <summary>
        /// 月收入
        /// </summary>
        [Description("月收入")]
        public string MonthlyIncome { get; set; }
        /// <summary>
        /// 兴趣爱好
        /// </summary>
        [Description("兴趣爱好")]
        public string Hobbies { get; set; }
        /// <summary>
        /// 注册来源：PC网站（10）、信营全球购B2B2C3G网站（20）
        /// </summary>
        [Description("注册来源：PC网站（10）、信营全球购B2B2C3G网站（20）")]
        public int RegisterSource { get; set; }
        /// <summary>
        /// 注册来源编号
        /// </summary>
        [Description("注册来源编号")]
        public string RegisterSourceSysNo { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Description("注册时间")]
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Description("最后登录IP")]
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Description("最后登录时间")]
        public DateTime LastLoginDate { get; set; }
        /// <summary>
        /// 状态：有效（1）、无效（0）
        /// </summary>
        [Description("状态：有效（1）、无效（0）")]
        public int Status { get; set; }
        /// <summary>
        /// 等级是否固定：固定（1）、不固定/等级升降（0）
        /// </summary>
        [Description("等级是否固定：固定（1）、不固定/等级升降（0）")]
        public int IsLevelFixed { get; set; }
        /// <summary>
        /// 经验积分是否固定：固定（1）、不固定/积分增减（0）
        /// </summary>
        [Description("经验积分是否固定：固定（1）、不固定/积分增减（0）")]
        public int IsExperiencePointFixed { get; set; }
        /// <summary>
        /// 惠源币是否固定：固定（1）、不孤单/惠源币增减（0）
        /// </summary>
        [Description("惠源币是否固定：固定（1）、不孤单/惠源币增减（0）")]
        public int IsExperienceCoinFixed { get; set; }
        /// <summary>
        /// 是否接收邮件:是(1),否(0)
        /// </summary>
        [Description("是否接收邮件:是(1),否(0)")]
        public int IsReceiveEmail { get; set; }
        /// <summary>
        /// 是否接收短信:是(1),否(0)
        /// </summary>
        [Description("是否接收短信:是(1),否(0)")]
        public int IsReceiveShortMessage { get; set; }
        /// <summary>
        /// 是否是公共账户:是(1),否(0)
        /// </summary>
        [Description("是否是公共账户:是(1),否(0)")]
        public int IsPublicAccount { get; set; }
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
        /// 上级
        /// </summary>
        [Description("上级")]
        public int PSysNo { get; set; }
        /// <summary>
        /// 会员推荐关系id目录（数据格式为: ,间N推荐id,...,直接推荐id,被推荐人的Id,）
        /// </summary>		
        [Description("会员推荐关系id目录（数据格式为: ,间N推荐id,...,直接推荐id,被推荐人的Id,）")]
        public string CustomerSysNos { get; set; }

        /// <summary>
        /// Openid
        /// </summary>		
        [Description("Openid")]
        public string Openid { get; set; }
        /// <summary>
        /// 是否关注服务号（0:否 1:是）
        /// </summary>		
        [Description("是否关注服务号（0:否 1:是）")]
        public string Subscribe { get; set; }
        /// <summary>
        /// Privilege
        /// </summary>		
        [Description("Privilege")]
        public string Privilege { get; set; }
        /// <summary>
        /// Unionid
        /// </summary>		
        [Description("Unionid")]
        public string Unionid { get; set; }
        /// <summary>
        /// 可提佣金
        /// </summary>		
        [Description("可提佣金")]
        public decimal Brokerage { get; set; }
        /// <summary>
        /// 累计佣金
        /// </summary>		
        [Description("累计佣金")]
        public decimal BrokerageTotal { get; set; }
        /// <summary>
        /// 冻结佣金
        /// </summary>		
        [Description("冻结佣金")]
        public decimal BrokerageFreeze { get; set; }
        /// <summary>
        /// 直接推荐人数
        /// </summary>		
        [Description("直接推荐人数")]
        public int InviteTotal { get; set; }
        /// <summary>
        /// IndirectTotal
        /// </summary>		
        [Description("IndirectTotal")]
        public int IndirectTotal { get; set; }
        /// <summary>
        /// 分销等级id
        /// </summary>		
        [Description("分销等级id")]
        public int SellBusinessGradeId { get; set; }

        private int isSellBusiness = 0;
        /// <summary>
        /// 是否为分销商
        /// </summary>
        [Description("是否为分销商")]
        public int IsSellBusiness
        {
            get { return isSellBusiness; }
            set { isSellBusiness = value; }
        }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        [Description("分销商系统编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        [Description("支付密码")]
        public string PaymentPassword { get; set; }
    }
}

