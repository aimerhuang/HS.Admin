using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 升舱分销商日志
    /// </summary>
    /// <remarks>2014-03-31 朱家宏 创建</remarks>
    public struct ParaDsDealerLogFilter
    {
        private int _pageSize;
        private DateTime? _endDate;
        private string _mallOrderId;

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
        /// 商城订单编号
        /// </summary>
        public string MallOrderId
        {
            get
            {
                return string.IsNullOrEmpty(_mallOrderId)
                           ? string.Empty
                           : _mallOrderId.Trim();
            }
            set { _mallOrderId = value; }
        }


        /// <summary>
        /// 升舱时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 升舱时间(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?)null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int? DealerMallSysNo { get; set; }
        /// <summary>
        /// 商城类型系统编号
        /// </summary>

        public int MallTypeSysNo { get; set; }

        /// <summary>
        ///商城订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 状态:待解决(10),已解决(20)
        /// </summary>
        public int Status { get; set; }

    }
}
