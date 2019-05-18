using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 产品价格传送类
    /// </summary>
    /// <remarks>2013-06-27 邵斌 创建</remarks>
    [Serializable]
    public class CBPdPrice : PdPrice
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName{get;set;}
        
        /// <summary>
        /// 会员等级价格
        /// </summary>
        public string PriceName { get; set; }
    }
}
