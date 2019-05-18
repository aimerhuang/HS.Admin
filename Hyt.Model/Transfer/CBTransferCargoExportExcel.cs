using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用于导出调货单到Excel
    /// </summary>
    public class CBTransferCargoExportExcel
    {
        ///<summary>
        ///编号
        ///</summary>
        public int SysNo { get; set; }

        ///<summary>
        ///订单号
        ///</summary>
        public int OrderSysNo { get; set; }

        ///<summary>
        ///出库单编号
        ///</summary>
        public int StockOutSysNo { get; set; }

        /// <summary>
        /// 订单商品明细
        /// </summary>
        public string OrderProductDtail { get; set; }

        ///<summary>
        ///申请调货仓库
        ///</summary>
        public string ApplyWarehouse { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        public string CreatedName { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        public string CreatedDate { get; set; }

        ///<summary>
        ///配货仓库编号
        ///</summary>
        public string DeliveryWarehouse { get; set; }
        ///<summary>
        ///最后更新人
        ///</summary>
        public string LastName { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        public string LastUpdateDate { get; set; }

        ///<summary>
        ///调货状态
        ///</summary>
        public string Status { get; set; }

        ///<summary>
        ///备注
        ///</summary>
        public string Remarks { get; set; }
    }
}
