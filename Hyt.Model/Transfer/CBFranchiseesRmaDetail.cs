using System;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 加盟商退换货明细对账报表实体类
    /// </summary>
    /// <remarks>
    /// 2014-08-22 余勇 创建
    /// </remarks>
    public class CBFranchiseesRmaDetail
    {
        public string 订单号 { get; set; }
        public string 订单来源 { get; set; }
        public string 申请日期 { get; set; }
        public string 入库日期 { get; set; }

        public string 商城订单号 { get; set; }

        public string 商城名称 { get; set; }

        public string ERP编码 { get; set; }

        public string 产品名称 { get; set; }

        public int 数量 { get; set; }

        public decimal 单价 { get; set; }

        public decimal 优惠 { get; set; }

        public decimal 退款金额 { get; set; }

        public decimal 实退金额 { get; set; }

        public string 下单门店 { get; set; }

        public string 源单出库仓库 { get; set; }

        public string 入库仓库 { get; set; }

        public string 加盟商ERP编号 { get; set; }

        public string 加盟商ERP名称 { get; set; }

        public string 收款方式 { get; set; }

        public string 退款方式 { get; set; }

        public string 配送方式 { get; set; }

        public string 售后方式 { get; set; }

        public string 结算状态 { get; set; }

        public string 收款单状态 { get; set; }

        public string 收款时间 { get; set; }

    
     
    }

}