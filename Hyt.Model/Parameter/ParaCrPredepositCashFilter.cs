using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商等级筛选字段
    /// </summary>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public struct ParaCrPredepositCashFilter
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
        /// 交易号
        /// </summary>
        public string PdcTradeNo { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string PdcUserName { get; set; }
        /// <summary>
        /// 提现单系统编号（多个逗号分隔）
        /// </summary>
        public string SysNoList { get; set; }
    }
}
