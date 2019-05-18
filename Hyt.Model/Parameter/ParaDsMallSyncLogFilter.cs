using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商商城同步日志筛选条件
    /// </summary>
    /// <remarks>2017-11-1 杨浩 创建</remarks>
    public class ParaDsMallSyncLogFilter
    {
        private int _pageSize;
        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageIndex { get; set; }

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
        /// 当前页
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 同步开始日期
        /// </summary>
        public DateTime? LastsyncBeginTime { get; set; }
        /// <summary>
        /// 同步结束日期
        /// </summary>
        public DateTime? LastsyncEndDate { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
