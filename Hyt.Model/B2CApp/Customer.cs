using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    #region Model

    /// <summary>
    /// 客户
    /// </summary>
    /// <remarks>2013-8-15 杨浩 添加</remarks>
    public class Customer
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 等级系统编号
        /// </summary>
        public int LevelSysNo { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 等级积分
        /// </summary>
        public int LevelPoint { get; set; }

        /// <summary>
        /// 经验积分
        /// </summary>
        public int ExperiencePoint { get; set; }

        /// <summary>
        /// 惠源币
        /// </summary>
        public int ExperienceCoin { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 邮箱状态：未验证（10）、已验证（20）
        /// </summary>
        public int EmailStatus { get; set; }

        /// <summary>
        /// 性别：保密（0）、男（1）、女（2）
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCardNo { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// App令牌安全访问
        /// </summary>
        public string AppToken { get; set; }

        /// <summary>
        /// 等级名
        /// </summary>
        public string LevelName { get; set; }
    }

    #endregion
}
