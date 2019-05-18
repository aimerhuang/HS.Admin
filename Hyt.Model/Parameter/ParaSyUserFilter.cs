using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 用户筛选字段
    /// </summary>
    /// <remarks>2013-08-05 黄志勇 创建</remarks>
    public struct ParaSyUserFilter
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
        /// 用户组编号
        /// </summary>
        public int? GroupSysNo { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreatedBy { get; set; }
    }
}
