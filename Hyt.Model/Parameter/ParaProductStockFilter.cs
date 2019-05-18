using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 产品库存筛选条件
    /// </summary>
    /// <remarks>2015-08-06 王耀发 创建</remarks>
    public struct ParaProductStockFilter
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
        public string WarehouseSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 商品条形码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string EasName { get; set; }
        /// <summary>
        /// 库存数量大于
        /// </summary>
        public string From_StockQuantity { get; set; }
        /// <summary>
        /// 库存数量小于
        /// </summary>
        public string To_StockQuantity { get; set; }
        
        /// <summary>
        /// 分销商编号
        /// </summary>
        public int DsDealerSysNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 仓库库位系统编号
        /// </summary>
        public int WhPositionSysNo { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public int ProductCategory { get; set; }
        /// <summary>
        /// 产品系统编号列表(多个逗号分隔)
        /// </summary>
        public string ProductSysNos { get; set; }
    }
}
