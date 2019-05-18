using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;
namespace Hyt.BLL.ApiWarehouse.XingYe
{
    /// <summary>
    /// 兴业仓
    /// </summary>
    /// <remarks>2017-07-07 吴琨 创建</remarks>
    public class WarehouseProvider:IWarehouseProvider
    {
        /// <summary>
        /// 仓库标识
        /// </summary>
        /// <remarks> </remarks>
        public override Hyt.Model.CommonEnum.仓库代码 Code
        {
            get
            {
                return Hyt.Model.CommonEnum.仓库代码.兴业仓;
            }     
        }
        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="data">查询参数</param>
        /// <returns></returns>
        /// <remarks>2017-07-07 吴琨 创建</remarks>
        public override void ExportOrder(object data)
        {
            var fileName = string.Format("兴业仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            List<XingYeWarehouseModel> listmodel = new List<XingYeWarehouseModel>();
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                XingYeWarehouseModel rmodel = new XingYeWarehouseModel();
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); //获取出库单信息
                //获取订单收货地址
                SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString());
                foreach (var item in itemmodel)
                {
                    rmodel.OrderId = "10000" + model.SysNo;//订单编号
                    rmodel.TheRecipient = ReceiveAddress.Name;//收件人
                    rmodel.Mobile = ReceiveAddress.PhoneNumber;//固话
                    rmodel.Phone = ReceiveAddress.MobilePhoneNumber;//手机
                    rmodel.Address = ReceiveAddress.StreetAddress;//地址
                    rmodel.Shipping = item.EasName;//发货信息
                    rmodel.Deliver = model.StockOutDate.ToString();//发货日期
                    rmodel.DeliverPhone = item.Quantity.ToString();//发件人电话
                    rmodel.DeliverAddress =item.BarCode;//发件人地址
                    rmodel.Remarks = model.Remarks;//备注
                    rmodel.UnitPrice = item.SalesUnitPrice.ToString();//单价
                    rmodel.Total = model.Receivable.ToString();//总价
                    rmodel.BusinessType = "";//业务类型
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<XingYeWarehouseModel>(listmodel,
                  new List<string> { "订单编号", "收件人", "固话", "手机", "地址", "发货信息", "发件人", "发件人电话", "发件人地址", "备注", "单价", "总价", "业务类型" },
                  fileName);
        }

        #region 兴业仓导出Excel模板实体
        private class XingYeWarehouseModel
        {
            /// <summary>
            /// 订单编号
            /// </summary>
            public string OrderId { get; set; }

            /// <summary>
            /// 收件人
            /// </summary>
            public string TheRecipient { get; set; }


            /// <summary>
            /// 固话
            /// </summary>
            public string Mobile { get; set; }

            /// <summary>
            /// 手机
            /// </summary>
            public string Phone { get; set; }

            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }


            /// <summary>
            /// 发货信息
            /// </summary>
            public string Shipping { get; set; }

            /// <发件人>
            /// 发货日期
            /// </summary>
            public string Deliver { get; set; }

            /// <summary>
            /// 发件人电话
            /// </summary>
            public string DeliverPhone { get; set; }

            /// <summary>
            /// 发件人地址
            /// </summary>
            public string DeliverAddress { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remarks { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            public string UnitPrice { get; set; }


            /// <summary>
            /// 总价
            /// </summary>
            public string Total { get; set; }

            /// <summary>
            /// 业务类型
            /// </summary>
            public string BusinessType { get; set; }
        }
        #endregion
    }
}
