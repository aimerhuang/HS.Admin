using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 门店查询
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaShopNewCustomerFilter
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
        /// 门店系统编号
        /// </summary>
        public int? ShopNo { get; set; }

        /// <summary>
        /// 有所有门店权限
        /// </summary>
        public bool HasAllShop { get; set; }

        /// <summary>
        /// 当前用户系统编号
        /// </summary>
        public int UserNo { get; set; }
    }
}
