using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.BLL.Order;
namespace Hyt.BLL.ApiWarehouse.ZhuoDing
{
    /// <summary>
    /// 卓鼎仓
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
                return Hyt.Model.CommonEnum.仓库代码.卓鼎仓;
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
            var fileName = string.Format("卓鼎仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            List<ZhuoDingWarehouseModel> listmodel = new List<ZhuoDingWarehouseModel>();
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                ZhuoDingWarehouseModel rmodel = new ZhuoDingWarehouseModel();
                //获取出库单信息
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i]));
               #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString());
                foreach (var item in itemmodel)
                {
                    rmodel.OrderId = "10000" + model.SysNo;//订单编号
                    var soOrder = SoOrderBo.Instance.GetModel(model.OrderSysNO);
                    rmodel.Freight = soOrder.FreightAmount.ToString();//运杂费
                    rmodel.NonDeductibleAmount = "";//非现金抵扣金额
                    rmodel.ActualPaymentAmount = soOrder.OrderAmount.ToString();//实际支付金额

                    var Customer = Hyt.BLL.Web.CrCustomerBo.Instance.GetModel(soOrder.OrderCreatorSysNo);
                    rmodel.OrderMakerNumber = Customer.Account;//订购人注册号
                    rmodel.OrderName = Customer.Name;//订购人姓名
                    rmodel.IdentityNumber = Customer.IDCardNo;//订购人证件号码
                    rmodel.PaymentNumber = "";//支付交易编号

                    SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);  //获取订单收货地址
                    rmodel.ConsigneeName = ReceiveAddress.Name;//收货人名称
                    rmodel.ConsigneePhone = ReceiveAddress.PhoneNumber;//收货人电话

                    //显示省市区
                    if (ReceiveAddress != null)
                    {
                        //市
                        Hyt.Model.BsArea cEntity;
                        //地区
                        Hyt.Model.BsArea aEntity;
                        //省
                        Hyt.Model.BsArea provinceEntity;
                        //国家
                        Hyt.Model.BsArea countryEntity;
                        var pEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetCountryEntity(ReceiveAddress.AreaSysNo, out countryEntity, out  provinceEntity, out cEntity, out aEntity);
                        rmodel.Province = provinceEntity.AreaName;//省
                        rmodel.City = cEntity.AreaName;//市
                        rmodel.Area = aEntity.AreaName;//区
                        rmodel.StreetAddress = ReceiveAddress.StreetAddress;//街道地址
                    }
                    rmodel.ProductCode = "";//企业商品货号
                    rmodel.ProductName = item.EasName;//企业商品名称
                    rmodel.TurnoverQuantity = item.Quantity.ToString();//成交数量
                    rmodel.LumpSumPrice = "";//单品总价
                    rmodel.CourierServicesCompany = "";//快递公司
                    rmodel.ExpressNumber = "";//快递号
                    rmodel.DeliveryNumbers = "";//提运单号
                    var product = Hyt.BLL.Web.PdProductBo.Instance.GetProductInfo(item.ProductSysNo); 
                    rmodel.BarCode = product.Barcode;//条形码
                    rmodel.Remarks = "";//备注
                    listmodel.Add(rmodel);
                }
               #endregion

            }
            Util.ExcelUtil.Export<ZhuoDingWarehouseModel>(listmodel,
                  new List<string> { "订单编号", "运杂费", "非现金抵扣金额", "实际支付金额", "订购人注册号", "订购人姓名", "订购人证件号码", "支付交易编号", "收货人名称", "收货人电话", "省", "市", "区", "街道地址", "企业商品货号", "企业商品名称", "成交数量", "单品总价", "快递公司", "快递号", "提运单号", "条形码", "备注"},
                  fileName);
        }

        #region 卓鼎仓导出Excel模板实体
        private class ZhuoDingWarehouseModel
        {
            /// <summary>
            /// 订单编号
            /// </summary>
            public string OrderId { get; set; }

            /// <summary>
            /// 运杂费
            /// </summary>
            public string Freight { get; set; }


            /// <summary>
            /// 非现金抵扣金额
            /// </summary>
            public string NonDeductibleAmount { get; set; }

            /// <summary>
            /// 实际支付金额
            /// </summary>
            public string ActualPaymentAmount { get; set; }

            /// <summary>
            /// 订购人注册号
            /// </summary>
            public string OrderMakerNumber { get; set; }


            /// <summary>
            /// 订购人姓名
            /// </summary>
            public string OrderName { get; set; }

            /// <summary>
            /// 订购人证件号码
            /// </summary>
            public string IdentityNumber { get; set; }


            /// <summary>
            /// 支付交易编号
            /// </summary>
            public string PaymentNumber { get; set; }


            /// <summary>
            /// 收货人名称
            /// </summary>
            public string ConsigneeName { get; set; }

            /// <summary>
            /// 收货人电话
            /// </summary>
            public string ConsigneePhone { get; set; }


            /// <summary>
            /// 省
            /// </summary>
            public string Province { get; set; }

            /// <summary>
            /// 市
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// 区
            /// </summary>
            public string Area { get; set; }

            /// <summary>
            /// 街道地址
            /// </summary>
            public string StreetAddress { get; set; }

            /// <summary>
            /// 企业商品货号
            /// </summary>
            public string ProductCode { get; set; }


            /// <summary>
            /// 企业商品名称
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 成交数量
            /// </summary>
            public string TurnoverQuantity { get; set; }

            /// <summary>
            /// 单品总价
            /// </summary>
            public string LumpSumPrice { get; set; }


            /// <summary>
            /// 快递公司
            /// </summary>
            public string CourierServicesCompany { get; set; }

            /// <summary>
            /// 快递号
            /// </summary>
            public string ExpressNumber { get; set; }

            /// <summary>
            /// 提运单号
            /// </summary>
            public string DeliveryNumbers { get; set; }

            /// <summary>
            /// 条形码
            /// </summary>
            public string BarCode { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remarks { get; set; }

        }
        #endregion
    }
}
