using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 系统_金蝶_用户关联表
    /// </summary>
    public class SyKingdeeUser
    {

        /// <summary>
        /// 主建
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 第三方类别
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 系统用户编号
        /// </summary>
        public int SyUserSysNo { get; set; }


        /// <summary>
        /// 金蝶用户代码
        /// </summary>
        public string KingdeeUserCode { get; set; }


        #region 扩展属性

        /// <summary>
        /// 系统用户名
        /// </summary>
        public string SyUserName { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; } 


        #endregion

    }


    public enum 第三方用户类别 { 
        金蝶=10
    }
}
