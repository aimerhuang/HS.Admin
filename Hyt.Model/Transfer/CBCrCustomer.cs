using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 会员信息扩展
    /// </summary>
    /// <remarks>2013-07-11 周唐炬 创建</remarks>
    /// <remarks>2013-07-15 苟治国 修改</remarks>
    [Serializable]
    public class CBCrCustomer : CrCustomer
    {
        /// <summary>
        /// 会员等级名称
        /// </summary>
        /// <remarks>
        /// 2013-07-15 苟治国 创建
        /// </remarks>
        public string LevelName { get; set; }
        /// <summary>
        /// 经销商名称
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public string DealerName { get; set; }
        /// <summary>
        /// 代理商名称
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public string AgentName { get; set; }
        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public int DealerCreatedBy { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的代理商
        /// </summary>
        /// <remarks>
        /// 2016-02-17 王耀发 创建
        /// </remarks>
        public int SelectedAgentSysNo { get; set; }

        public string PName { get; set; }

        public string PNickName { get; set; }

        public string PAccount { get; set; }

        public string PHeadImage { get; set; }

        /// <summary>
        /// 分销等级名
        /// </summary>
        public string SellBusinessGradeName { get; set; }
    }
}
