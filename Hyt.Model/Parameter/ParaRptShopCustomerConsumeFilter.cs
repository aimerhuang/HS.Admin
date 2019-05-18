using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 门店会员消费统计报表参数
    /// </summary>
    /// <remarks>2014-01-06 余勇 创建</remarks>
    public class ParaRptShopCustomerConsumeFilter
    {
        private int _pageSize;

        /// <summary>
        /// 当前页号
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
        /// 统计日期
        /// </summary>
        public string Reptdt { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string ShopName { get; set; }
   
    }
}
