using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 微信自定义菜单
    /// </summary>
    /// <remarks>2016-01-11 王耀发 创建</remarks>
    public class CBMkCustomizeMenu : MkCustomizeMenu
    {

        /// <summary>
        /// 菜单单名称
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }


    }
}
