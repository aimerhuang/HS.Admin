using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 保证金订单查询参数
    /// </summary>
    /// <remarks>2016-5-15 杨浩 创建</remarks>
    public class ParaDsDealerBailOrderFilter
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
       /// 联系人名称
       /// </summary>
       public string ContactName { get; set; }
       /// <summary>
       /// 联系人方式
       /// </summary>
       public string ContactWay { get; set; }

       /// <summary>
       /// 当前分销商系统编号
       /// </summary>
       /// <remarks>2015-12-16 王耀发 创建</remarks>
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
