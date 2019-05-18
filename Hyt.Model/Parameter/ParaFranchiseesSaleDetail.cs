using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 加盟商当日达对账报表
    /// </summary>
    /// <remarks>2014-08-21 余勇 创建</remarks>
    public class ParaFranchiseesSaleDetail
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
        /// 报表月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public List<int> WhSelected{ get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
    }
}
