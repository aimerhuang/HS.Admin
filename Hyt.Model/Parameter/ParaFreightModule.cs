using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 运费模板筛选字段
    /// </summary>
    /// <remarks>2015-08-06 王耀发 创建</remarks>
    public struct ParaFreightModule
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
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 模板代码
        /// </summary>
        public string ModuleCode { get; set; }
    }
}
