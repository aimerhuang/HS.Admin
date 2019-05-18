using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 企业用户筛选字段
    /// </summary>
    /// <remarks>2014-10-15 谭显锋 创建</remarks>
    public struct ParaEnterpriseUserFilter
    {
        private int _pageSize;
        private int _pageIndex;
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

        public int PageIndex
        {
            get
            {
                if (_pageIndex == 0)
                {
                    _pageIndex = 1;
                }
                return _pageIndex;
            }
            set { _pageIndex = value; }

        }
        /// <summary>
        /// 模糊查询key 用户姓名、账号、手机号
        /// </summary>
        public string Key { get; set; }
    }
}
