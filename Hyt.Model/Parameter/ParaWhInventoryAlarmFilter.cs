using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 保证金订单查询参数
    /// </summary>
    /// <remarks>2016-5-15 杨浩 创建</remarks>
    public class ParaWhInventoryAlarmFilter
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
       /// 仓库系统编号
       /// </summary>
        public int? WarehouseSysNo { get; set; }
       /// <summary>
       /// 商品编码
       /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
              
    }
}
