using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 系统日志筛选字段
    /// </summary>
    /// <remarks>2013-08-14 朱家宏 创建</remarks>
    public struct ParaSystemLogFilter
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
        /// 日志级别
        /// </summary>
        public IList<int> LogLevels { get; set; }

        /// <summary>
        /// 日志来源
        /// </summary>
        public IList<int> Sources { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int? Operator { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string LogIp { get; set; }

        /// <summary>
        /// 操作时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 操作时间(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 操作目标类型
        /// </summary>
        public int? TargetType { get; set; }

        /// <summary>
        /// 操作对象
        /// </summary>
        public int? TargetSysNo { get; set; }

    }
}
