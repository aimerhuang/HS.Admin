using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Stores
{
    /// <summary>
    /// 店铺
    /// </summary>
    ///<remarks>2012-12-14 杨浩 创建</remarks>
    [Serializable]
    public class Store : DsDealer
    {
        /// <summary>
        /// 扩展字段对象
        /// </summary>
        public StoreExtensions ExtensionsObj { get; set; }
    }
}