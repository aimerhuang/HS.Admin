using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 广告项券筛选字段
    /// </summary>
    /// <remarks>2013-10-11 苟治国 创建</remarks>
    public class ParaFeAdvertItem : BaseEntity
    {
        private DateTime? _endTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                //查询日期上限+1
                return _endTime == null ? (DateTime?)null : _endTime.Value.AddDays(1);
            }
            set { _endTime = value; }
        }
    }
}
