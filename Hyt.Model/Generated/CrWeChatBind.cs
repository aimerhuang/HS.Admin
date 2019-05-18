using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    public class BCCrWeChatBind : CrWeChatBind
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string BindWarehousName { get; set; }
        public string OpenId { get; set; }
    }
    public class CrWeChatBind
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 关联的会员账号
        /// </summary>
        public int AccountSysNo { get; set; }
        /// <summary>
        /// 关联时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 备注名称
        /// </summary>
        public string BindName { get; set; }
        /// <summary>
        /// 绑定的仓库编号
        /// </summary>
        public string BindWhSysNos { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int CStatus { get; set; }
    }
}
