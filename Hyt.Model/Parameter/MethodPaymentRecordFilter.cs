using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 支付筛选字段
    /// </summary>
    /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
    public class MethodPaymentRecordFilter
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
        /// 开始日期(起)
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束日期(止)
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int OrderSysNo { get; set; }

        public string RebatesType { get; set; }
        public string Condition { get; set; }
    }
}
