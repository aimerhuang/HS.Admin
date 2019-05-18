using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 千机团手机串码查询
    /// </summary>
    /// <remarks>2016/2/18 17:27:02 朱成果 创建</remarks>
    public class ParProductImeiFilter
    {
        private int _pageSize;
        private int _currentPage;

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

        /// <summary>
        /// 是否应用分类所有商品
        /// </summary>
        public int? IsUseCategory { get; set; }
    }
}
