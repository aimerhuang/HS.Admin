using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 微信自动回复信息查询参数实体
    /// </summary>
    /// <remarks>
    /// 2016-04-28 王耀发 创建
    /// </remarks>
    public class ParaMkWeixinConfigFilter
    {
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 微信Token1
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        public int DealerCreatedBy { get; set; }

        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的代理商
        /// </summary>
        public int SelectedAgentSysNo { get; set; }
    }
}
