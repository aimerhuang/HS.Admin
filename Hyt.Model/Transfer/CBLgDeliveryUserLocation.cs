using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配送员位置信息组合实体
    /// </summary>
    /// <remarks>2013-06-25 周唐炬 创建</remarks>
    public class CBLgDeliveryUserLocation:LgDeliveryUserLocation
    {

        /// <summary>
        /// 配送员姓名
        /// </summary>
        /// <remarks> 2013-06-19 郑荣华 创建</remarks>
        public string UserName { get; set; }

    }
}
