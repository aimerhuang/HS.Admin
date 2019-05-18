using System;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 退换货明细-导出excel
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBReportRmaExport
    {
        //public DateTime 下单日期 { get; set; }
        public string 订单号 { get; set; }
        public string 订单来源 { get; set; }
        //public string 下单门店 { get; set; }
        public DateTime 申请日期 { get; set; }
        public DateTime 入库日期 { get; set; }
        public string 会员名 { get; set; }
        public string ERP编码 { get; set; }
        public string 产品名称 { get; set; }
        public int 数量 { get; set; }
        public decimal 单价 { get; set; }
        public decimal 优惠 { get; set; }
        public decimal 退款金额 { get; set; }
        public decimal 实退金额 { get; set; }
        public string 入库仓库 { get; set; }
        public string 收款方式 { get; set; }
        public string 退款方式 { get; set; }
        public string 配送方式 { get; set; }
        public string 售后方式 { get; set; }
        //public string 发票号 { get; set; }
        //public string 收货地址 { get; set; }
        public string 联系电话 { get; set; }
        //public string 送货员 { get; set; }
        //public string 客服 { get; set; }
        public string 结算状态 { get; set; }

        public int 入库单号 { get; set; }

        public string 下单门店 { get; set; }

    }
}