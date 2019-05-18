using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBPrtPack : PrtPack
    {
     
        /// <summary>
        /// 店铺名称，本公司用 商城 
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 分销商商城电话
        /// </summary>
        public string ServicePhone { get; set; }

    }
}
