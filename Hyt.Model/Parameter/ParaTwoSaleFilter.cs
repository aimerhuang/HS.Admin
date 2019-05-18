using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 二次销售筛选条件
    /// </summary>
    /// <remarks>2014-9-22 朱成果 创建</remarks>
    public   class ParaTwoSaleFilter
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
        /// 下单时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 筛选仓库编号
        /// </summary>
        public List<int> WarehouseSysNos { get; set; }

        /// <summary>
        /// 选中仓库的编号
        /// </summary>
        public int? SelectWarehouseSysNo { get; set; }

        /// <summary>
        /// 业务员编号
        /// </summary>
        public int? UserID { get; set; }

    }
}
