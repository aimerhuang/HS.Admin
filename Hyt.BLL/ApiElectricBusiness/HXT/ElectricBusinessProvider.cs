using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiElectricBusiness.HXT
{
    public class ElectricBusinessProvider : IElectricBusinessProvider
    {
        /// <summary>
        /// 获取电商编码
        /// </summary>
        public override Model.CommonEnum.电商平台 Code
        {
            get { return Model.CommonEnum.电商平台.广州华迅捷通电子商务有限公司; }
        }
        /// <summary>
        /// 导出excel数据
        /// </summary>
        /// <param name="orderSysNos">订单编号</param>
        public override void OutPutExcelData(List<int> orderSysNos)
        {
            List<SoOrder> orderList = SoOrderBo.Instance.GetAllOrderBySysNos(string.Join(",", orderSysNos.ToArray()));
            List<CBSoOrderItem> items =  SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(string.Join(",", orderSysNos.ToArray()));
            List<CBFnOnlinePayment> fnPayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentList(string.Join(",", orderSysNos.ToArray()));
            List<CBSoReceiveAddress> soOrderAddressList = SoOrderBo.Instance.GetOrderReceiveAddressByList(string.Join(",", orderSysNos.ToArray()));
            List<OutPutOrderExcelData> dataList = new List<OutPutOrderExcelData>();
            
           

            foreach(var mod in orderList)
            {
                List<CBSoOrderItem> tempOrderItems=items.FindAll(p=>p.OrderSysNo==mod.SysNo);
                CBFnOnlinePayment tempPayment = fnPayments.Find(p => p.SourceSysNo == mod.SysNo);
                CBSoReceiveAddress tempAddress = soOrderAddressList.Find(p => p.SysNo == mod.ReceiveAddressSysNo);
                if(tempPayment!=null)
                {
                    CBWhWarehouse warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(mod.DefaultWarehouseSysNo);
                    int indx = 0;
                    foreach (var itemMod in tempOrderItems)
                    {
                        indx++;
                        OutPutOrderExcelData tempMod = new OutPutOrderExcelData()
                       {
                           订单编号 = tempPayment.BusinessOrderSysNo,
                           订单日期 = mod.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                           电商平台代码 = config.CusCode.Trim(),
                           海关电商平台名称 = config.CusName.Trim(),
                           电商企业代码 = config.CusCode.Trim(),
                           海关电商企业名称 = config.CusName.Trim(),
                           海关电商企业备案号 = config.CusCode.Trim(),
                           电商企业智检备案号 = config.ICPCode.Trim(),
                           电商平台企业智检备案号 = config.ICPCode.Trim(),
                           电商平台域名 = "http://www.gaopin999.com/",
                           支付企业代码 = config.PayCode,
                           支付企业名称 = config.PayName,
                           支付交易编号 = tempPayment.VoucherNo,
                           订单商品货款 = mod.ProductAmount.ToString("0.00"),
                           订单运费 = mod.FreightAmount.ToString("0.00"),
                           优惠减免金额 = mod.ProductChangeAmount.ToString("0.00"),
                           订单商品税款 = mod.TaxFee.ToString("0.00"),
                           实际支付金额 = mod.OrderAmount.ToString("0.00"),
                           币制 = "142",
                           订购人注册号 = "",
                           订购人姓名 = tempAddress.Name,
                           订购人证件类型 = "1",
                           订购人证件号码 = tempAddress.IDCardNo,
                           订购人电话 = tempAddress.MobilePhoneNumber,
                           商品批次号 = "",
                           其它费用 = "",
                           备注note = "",
                           物流订单号 = tempPayment.BusinessOrderSysNo,
                           商品序号 = indx.ToString(),
                           商品智检货号 = itemMod.ErpCode,
                           企业商品海关货号 = itemMod.ErpCode,
                           主要商品描述 = "http://www.gaopin999.com/Product/Details/" + itemMod.ProductSysNo,
                           申报数量 = itemMod.Quantity.ToString(),
                           单价 = ((itemMod.SalesAmount + itemMod.ChangeAmount) / itemMod.Quantity).ToString("0.00"),
                           总价 = (itemMod.SalesAmount + itemMod.ChangeAmount).ToString("0.00"),
                           销售网址 = "http://www.gaopin999.com/Product/Details/" + itemMod.ProductSysNo,
                           收货人名称 = tempAddress.Name,
                           收件人证件类型 = "1",
                           收件人证件号 = tempAddress.IDCardNo,
                           收货人电话 = tempAddress.MobilePhoneNumber,
                           收货人地址 = tempAddress.ProvinceName + " " + tempAddress.CityName + "  " + tempAddress.CountryName + "  " + tempAddress.StreetAddress,
                           收货人所在国 = "142",
                           收货人城市 = tempAddress.CityName,
                           收货人省市区代码 = tempAddress.AreaSysNo.ToString(),
                           收货人省市区名称 = tempAddress.ProvinceName + " " + tempAddress.CityName + "  " + tempAddress.CountryName,
                           收货人行政区代码 = tempAddress.AreaSysNo.ToString(),
                           发货人姓名 = warehouse.Contact,
                           发货人电话 = warehouse.Phone,
                           发货人地址 = warehouse.ProvinceName + " " + warehouse.CityName + " " + warehouse.AreaName + " " + warehouse.StreetAddress,
                           发货人城市 = warehouse.CityName,
                           发货人省市区代码 = warehouse.CitySysNo.ToString(),
                           发货人省市区名称 = warehouse.ProvinceName + " " + warehouse.CityName + " " + warehouse.AreaName,
                           发货人所在国 = "142",
                           运单号 = "",
                           提运单号 = "",
                           运费 = mod.FreightAmount.ToString("0.00"),
                           保费 = "0",
                           毛重 = itemMod.GrosWeight.ToString("0.00"),
                           净重 = itemMod.NetWeight.ToString("0.00"),
                           件数 = itemMod.Quantity.ToString(),
                           货物总价 = (itemMod.ChangeAmount + itemMod.SalesAmount).ToString("0.00"),
                           包装种类 = "1",
                           备注 = "",
                           平台来源 = config.CusCode.Trim(),
                           平台编号 = "",
                           平台简称 = "",
                           电商平台下商家编码 = "",
                           商家简称 = "",
                           申报类型 = "1",
                           第一法定数量 = itemMod.GrosWeight.ToString("0.00"),
                           第二法定数量 = itemMod.Quantity.ToString()
                       };
                       dataList.Add(tempMod);
                    }
                   
                }
            }
            var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

           

            //导出Excel，并设置表头列名
            Util.ExcelUtil.ExportSoOrders<OutPutOrderExcelData>(dataList,
                new List<string> { "用户编号","用户名称","报文发送者ID","订单类型","订单编号","订单日期","电商平台代码","海关电商平台名称",
                                   "电商企业代码","海关电商企业名称","海关电商企业备案号","电商企业备案号(智检)","电商平台企业备案号(智检)","电商平台域名",
                                   "支付企业代码（海关备案号）","支付企业名称","支付交易编号","订单商品货款","运费","优惠减免金额","优惠抵扣说明","订单商品税款","实际支付金额","币制","订购人注册号",
                                   "订购人姓名","订购人证件类型","订购人证件号码","订购人电话","商品批次号","其它费用","备注(Notes)","物流订单号","商品序号(Seq)","商品货号（智检）","企业商品货号(海关）",
                                   "主要商品描述","申报数量","单价","总价(Total)","销售网址","收货人名称","收件人证件类型","收件人证件号","收货人电话","收货人地址","收货人所在国","收货人城市",
                                   "收货人省市区代码","收货人省市区名称","收货人行政区代码","发货人姓名","发货人电话","发货人地址","发货人城市","发货人省市区代码","发货人省市区名称","发货人所在国",
                                    "运单号","提运单号","运费","保费","毛重","净重","件数","货物总价","包装种类","备注","平台来源（填写平台备案号）","平台编号","平台简称","电商平台下商家编码","商家简称",
                                    "申报类型","第一法定数量","第二法定数量"
                                 },
                                fileName);
            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                     LogStatus.系统日志目标类型.ExcelExporting, 0, null, "", 1);
        }
    }
    public class OutPutOrderExcelData
    {
        public string 用户编号 { get; set; }
        public string 用户名称 { get; set; }
        public string 报文发送者ID { get; set; }
        public string 订单类型 { get; set; }
        public string 订单编号 { get; set; }
        public string 订单日期 { get; set; }
        public string 电商平台代码 { get; set; }
        public string 海关电商平台名称 { get; set; }
        public string 电商企业代码 { get; set; }
        public string 海关电商企业名称 { get; set; }
        public string 海关电商企业备案号 { get; set; }
        public string 电商企业智检备案号 { get; set; }
        public string 电商平台企业智检备案号 { get; set; }
        public string 电商平台域名 { get; set; }
        public string 支付企业代码 { get; set; }
        public string 支付企业名称 { get; set; }
        public string 支付交易编号 { get; set; }
        public string 订单商品货款 { get; set; }
        public string 订单运费 { get; set; }
        public string 优惠减免金额 { get; set; }
        public string 优惠抵扣说明 { get; set; }
        public string 订单商品税款 { get; set; }
        public string 实际支付金额 { get; set; }
        public string 币制 { get; set; }
        public string 订购人注册号 { get; set; }
        public string 订购人姓名 { get; set; }
        public string 订购人证件类型 { get; set; }
        public string 订购人证件号码 { get; set; }
        public string 订购人电话 { get; set; }
        public string 商品批次号 { get; set; }
        public string 其它费用 { get; set; }
        public string 备注note { get; set; }
        public string 物流订单号 { get; set; }
        public string 商品序号 { get; set; }
        public string 商品智检货号 { get; set; }
        public string 企业商品海关货号 { get; set; }
        public string 主要商品描述 { get; set; }

        public string 申报数量 { get; set; }
        public string 单价 { get; set; }
        public string 总价 { get; set; }
        public string 销售网址 { get; set; }
        public string 收货人名称 { get; set; }
        public string 收件人证件类型 { get; set; }
        public string 收件人证件号 { get; set; }
        public string 收货人电话 { get; set; }
        public string 收货人地址 { get; set; }
        public string 收货人所在国 { get; set; }
        public string 收货人城市 { get; set; }
        public string 收货人省市区代码 { get; set; }
        public string 收货人省市区名称 { get; set; }
        public string 收货人行政区代码 { get; set; }
        public string 发货人姓名 { get; set; }
        public string 发货人电话 { get; set; }
        public string 发货人地址 { get; set; }
        public string 发货人城市 { get; set; }
        public string 发货人省市区代码 { get; set; }
        public string 发货人省市区名称 { get; set; }
        public string 发货人所在国 { get; set; }
        public string 运单号 { get; set; }
        public string 提运单号 { get; set; }
        public string 运费 { get; set; }
        public string 保费 { get; set; }
        public string 毛重 { get; set; }
        public string 净重 { get; set; }
        public string 件数 { get; set; }
        public string 货物总价 { get; set; }
        public string 包装种类 { get; set; }
        public string 备注 { get; set; }
        public string 平台来源 { get; set; }

        public string 平台编号 { get; set; }
        public string 平台简称 { get; set; }
        public string 电商平台下商家编码 { get; set; }
        public string 商家简称 { get; set; }
        public string 申报类型 { get; set; }
        public string 第一法定数量 { get; set; }
        public string 第二法定数量 { get; set; }
    }
}
