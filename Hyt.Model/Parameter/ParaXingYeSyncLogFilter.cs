using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 兴业嘉同步日志查询参数
    /// </summary>
    /// <remarks>2018-03-22 罗熙 创建</remarks>
    public class ParaXingYeSyncLogFilter
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
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建的开始日期(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 创建的结束时间(止)
        /// </summary>
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// 同步的开始日期(起)
        /// </summary>
        public DateTime? LastsyncBeginTime { get; set; }

        /// <summary>
        /// 同步的结束时间(止)
        /// </summary>
        public DateTime? LastsyncEndDate { get; set; }

        /// <summary>
        /// 流程标识
        /// </summary>
        public string FlowIdentify { get; set; }

        /// <summary>
        /// LiJia状态代码
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 仓库编号(将整型改为字符串便于上传多仓库编号参数，用逗号,分隔 余勇 2014-07-02)
        /// </summary>
        public string WarehouseSysNo { get; set; }

        /// <summary>
        /// 拥有的仓库
        /// </summary>
        public string Warehouses { get; set; }
    }
}
