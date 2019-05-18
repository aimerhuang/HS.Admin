using System;
using System.Collections.Generic;
using Hyt.Model.WorkflowStatus;
namespace Hyt.Model
{
    /// <summary>
    /// 订单池查询实体类
    /// </summary>
    /// <remarks>
    /// 2013/6/14 14:46 余勇 创建
    /// </remarks>
    public partial class CBSyJobPool:SyJobPool 
    {
        #region 自定义字段
       
        /// <summary>
        /// 任务执行人姓名
        /// </summary>
        public string ExecutorSysName { set; get; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { set; get; }

        /// <summary>
        /// 任务执行人手机
        /// </summary>
        public string MobilePhoneNumber { set; get; }
        /// <summary>
        /// 搜索条件选中的代理商
        /// </summary>
        public int AgentSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { set; get; }
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

        /// <summary>
        /// 用户可查看的仓库列表(门店)
        /// </summary>
        public IList<WhWarehouse> Warehouses { get; set; }

        /// <summary>
        /// 是否绑定所有仓库
        /// </summary>
        public bool HasAllWarehouse { get; set; }
    #endregion
    }
}

