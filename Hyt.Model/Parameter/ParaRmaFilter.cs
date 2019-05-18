using System;
using System.Collections.Generic;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 退换货查询参数
    /// </summary>
    /// <remarks>2013-07-11 朱家宏 创建</remarks>
    public class ParaRmaFilter
    {
        private int _pageSize;
        private bool _hasWarehouse = true;

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
                if (_pageSize == 0){
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 会员会员号(登录帐号)
        /// </summary>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 退换货开始日期(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 退换货结束日期(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 退换货单据号
        /// </summary>
        public int? RmaId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 销售单来源
        /// </summary>
        public IList<int> OrderSources { get; set; }

        /// <summary>
        /// 退换货状态
        /// </summary>
        public IList<int> RmaStatuses { get; set; }

        /// <summary>
        /// 退换货类型
        /// </summary>
        public int? RmaType { get; set; }

        /// <summary>
        /// 申请单来源:会员(10),客服(20),门店(30)
        /// </summary>
        public IList<int> HandleDepartments { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 是否有入库仓库
        /// </summary>
        public bool HasWarehouse
        {
            get { return _hasWarehouse; }
            set { _hasWarehouse = value; }
        }

        /// <summary>
        /// 申请单来源:会员(10),客服(20),门店(30)
        /// </summary>
        public IList<int> RmaSources { get; set; }

        /// <summary>
        /// 门店退换货单查询（SysNo）
        /// </summary>
        public IList<int> StoreSysNoList { get; set; }
    }
}
