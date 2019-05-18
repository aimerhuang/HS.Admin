using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 返利记录筛选字段
    /// </summary>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public struct ParaDealerRebatesRecordFilter
    {
        private int _pageSize;

        /// <summary>
        /// 当前页
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }
        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商编号集合
        /// </summary>
        public List<int> DealerSysNoList { get; set; }
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
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
        public string RebatesType { get; set; }
        public string Condition { get; set; }
    }
}
