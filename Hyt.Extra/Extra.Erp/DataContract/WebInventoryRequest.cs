using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.DataContract
{

    /// <summary>
    /// 库存查询参数
    /// </summary>
    /// <remarks>2016-11-23 杨浩 创建</remarks>
    public class WebInventoryRequest:BaseRequest
    {
        /// <summary>
        /// 商城数据库名称
        /// </summary>
        public string FAcctDB { get; set; }
        /// <summary>
        /// 商品开始编码(不可为空。多个商品编码用”,”号分开)
        /// </summary>
        public string FNumberS { get; set; }
        /// <summary>
        /// 商品结束编码
        /// </summary>
        public string FNumberE { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string FStockID { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        public string FKFDate { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string FBatchNo { get; set; }
    }
}
