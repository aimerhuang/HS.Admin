using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 联盟广告
    /// </summary>
    /// <remarks>2013-10-18 周唐炬 创建</remarks>
    public partial class UnAdvertisement
    {
        /// <summary>
        /// 商品编号列表
        /// </summary>
        public List<int> Products { get; set; }
        /// <summary>
        /// CPS商品列表
        /// </summary>
        public List<UnCpsProduct> ItemList { get; set; }
    }
}
