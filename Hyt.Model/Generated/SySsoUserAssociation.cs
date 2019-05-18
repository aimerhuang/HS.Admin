using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// SSO系统用户关联
    /// </summary>
    /// <remarks>
    /// 2014-10-14 谭显锋 创建
    /// </remarks>
    [Serializable]
    public class SySsoUserAssociation
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// SSO系统编号
        /// </summary>
        [Description("SSO系统编号")]
        public int SsoId { get; set; }
        /// <summary>
        /// 系统用户编号
        /// </summary>
        [Description("系统用户编号")]
        public int UserSysNo { get; set; }
    }
}
