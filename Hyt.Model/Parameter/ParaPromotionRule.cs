using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 促销规则筛选字段
    /// </summary>
    /// <remarks>2013-08-21 黄志勇 创建</remarks>
    public struct ParaPromotionRule
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
        ///规则名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 促销应用类型
        /// </summary>
        public int? PromotionType { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public int? RuleType { get; set; }
        /// <summary>
        /// 规则描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
