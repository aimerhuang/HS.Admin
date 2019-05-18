using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉采购入库同步模型
    /// </summary>
    /// <remarks>2017-5-18 罗勤尧 创建</remarks>
   public class LiJiaPurchaseInfo
    {
        /// <summary>
        /// 第三方入库单号
        /// </summary>
       public int SourceNo { get; set; }
        /// <summary>
       ///ERP仓库编码(通过仓库接口获取)
        /// </summary>
       public string StoreCode { get; set; }
        /// <summary>
       ///仓库名称
        /// </summary>
       public string StoreName { get; set; }
        /// <summary>
       /// 供应商Id(通过供应商接口获取)
        /// </summary>
       public int SupplierId { get; set; }
        /// <summary>
       /// 供应商名称
        /// </summary>
       public string SupplierName { get; set; }
       /// <summary>
       /// 外币币种CNY=人民币,USD=美元,EUR=欧元,GBP=英镑,HKD=港元,JPY=日元,NZD=纽币,AUD=澳币填写币种英文标识,如:USD
       /// </summary>
       public string ForeignCurency { get; set; }
       /// <summary>
       /// 汇率(外币对人民币汇率)
       /// </summary>
       public float ForeignRate { get; set; }
       /// <summary>
       /// 人民币总金额
       /// </summary>
       public decimal TotalAmount { get; set; }
       /// <summary>
       /// 总数量
       /// </summary>
       public decimal TotalQty { get; set; }
       /// <summary>
       /// 入库时间(格式 2017-05-16 10:55:11)
       /// </summary>
       public string CrTime { get; set; }

       /// <summary>
       /// 入库明细数据集
       /// </summary>
       public List<LiJiaPurchaseItem> data { get; set; }
    }
}
