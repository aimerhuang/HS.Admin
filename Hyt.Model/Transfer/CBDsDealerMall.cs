using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商商城扩展实体
    /// </summary>
    /// <remarks>2013-09-18 郑荣华 创建</remarks>
    public class CBDsDealerMall : DsDealerMall
    {
        /// <summary>
        /// 分销商名称
        /// </summary>
        /// <remarks>2013-09-18 郑荣华 创建</remarks>
        public string DealerName { get; set; }

        /// <summary>
        /// 商城类型名称
        /// </summary>
        /// <remarks>2013-09-18 郑荣华 创建</remarks>
        public string MallName { get; set; }

        /// <summary>
        /// 商城类型代号
        /// </summary>
        /// <remarks>2013-09-18 郑荣华 创建</remarks>
        public string MallCode { get; set; }

        /// <summary>
        /// 分销商用户编号
        /// </summary>
        public int DsUserSysNo { get; set; }

        /// <summary>
        /// 关联APPKey
        /// </summary>
        /// <remarks>2014-07-24 余勇 创建</remarks>
        public string AppName { get; set; }
    }
}
