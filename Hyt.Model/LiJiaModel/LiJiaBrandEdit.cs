using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 品牌修改
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
   public  class LiJiaBrandEdit
    {
        /// <summary>
        /// 品牌id
        /// </summary>
        [Description("品牌id")]
       public int BrandId { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        [Description("品牌名称")]
        public string BrandName { get; set; }
    }
}
