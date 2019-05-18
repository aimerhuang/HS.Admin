using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 调货配置筛选字段
    /// </summary>
    /// <remarks>2016-04-05 王江 创建</remarks>
    public class ParaTransferCargoConfigFilter
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
        /// 申请调货仓库名称
        /// </summary>
        public string ApplyWarehouseName { get; set; }

        /// <summary>
        /// 配货仓库名称
        /// </summary>
        public string DeliveryWarehouseName { get; set; }

        /// <summary>
        /// 是否根据配货仓库查询[true:配货仓库 false:申请调货仓库]
        /// </summary>
        public bool IsQueryPickingWarehouse { get; set; }
    }
}
