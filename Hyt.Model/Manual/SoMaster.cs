using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 销售单实体
    /// </summary>
    /// <remarks> 2013-06-14 朱家宏 创建</remarks>
    public partial class SoMaster
    {
        /// <summary>
        /// 关联会员
        /// </summary>
        public CrCustomer Customer { get; set; } 

        /// <summary>
        /// 关联收货地址
        /// </summary>
        public SoReceiveAddress ReceiveAddress { get; set; } 
    }
}
