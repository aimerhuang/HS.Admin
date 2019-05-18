using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 采购单查询参数
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class ParaPrPurchaseReturnFilter
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
        /// 仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 采购单编号
        /// </summary>
        public string PurchaseCode { get; set; }
        /// <summary>
        /// 采购单状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatedDate { get; set; }

    }
}
