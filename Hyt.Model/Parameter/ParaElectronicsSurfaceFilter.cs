using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 电子面单账号管理筛选
    /// </summary>
    /// <remarks>2015-10-9 王江 创建</remarks>
    public class ParaElectronicsSurfaceFilter
    {
        private int _pageSize;
        private int _currentPage;
        /// <summary>
        /// 账号名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 配送方式编号
        /// </summary>
        public int? DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id
        {
            get
            {
                if (_currentPage < 1)
                {
                    _currentPage = 1;
                }
                return _currentPage;
            }
            set { _currentPage = value; }
        }

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
    }
}
