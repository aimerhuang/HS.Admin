using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 返利记录筛选字段
    /// </summary>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public struct ParaCustomerRebatesRecordFilter
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
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 返利记录系统编号（多个逗号分隔）
        /// </summary>
        public string SysNoList { get; set; }
    }
}
