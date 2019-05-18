using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    public class DsDealerApplyWeb
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 分销商编号
        /// </summary>
        [Description("分销商编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型")]
        public int Type { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [Description("联系人姓名")]
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Description("联系电话")]
        public string Mobilephone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Description("电子邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        [Description("联系地址")]
        public string Address { get; set; }
        /// <summary>
        /// 留言备注
        /// </summary>
        [Description("留言备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
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
        /// 更新人
        /// </summary>
        [Description("更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime LastUpdateDate { get; set; }

    }
}
