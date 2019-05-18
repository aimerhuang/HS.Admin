using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 商品商检
    /// </summary>
    /// <remarks>2016-03-23 王耀发 创建</remarks>
    public struct ParaIcpGoodsItemFilter
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
        /// 海关类型
        /// </summary>
        public int IcpType { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>名称编号
        /// </summary>
        public string ProductName { get; set; }
    }
}
