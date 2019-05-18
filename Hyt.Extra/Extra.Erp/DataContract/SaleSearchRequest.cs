using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.DataContract
{
    /// <summary>
    /// 销售单查询参数
    /// </summary>
    /// <remarks>2016-12-12 杨浩 创建</remarks>
    public class SaleSearchRequest:BaseRequest
    {
        /// <summary>
        /// 商城数据库名称
        /// </summary>
        public string FAcctDB { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int Ftrantype { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FBillNo { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string FStartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string FEndDate { get; set; }
        private int pageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex 
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        private int pageSize = 20;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

    }

}
