using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 入库明细
    /// </summary>
    /// <remarks>2015-08-06 王耀发 创建</remarks>
    public struct ParaProductStockInDetailFilter
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
        /// 当前仓库
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string StockInNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string EasName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
