using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;
using Hyt.BLL.Product;
namespace Hyt.BLL.ApiWarehouse.WuZhou
{
    /// <summary>
    /// 五洲仓
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
                return Hyt.Model.CommonEnum.仓库代码.五洲仓;
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
            var fileName = string.Format("五洲仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            List<WuZhouWarehouseModel> listmodel = new List<WuZhouWarehouseModel>();
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                WuZhouWarehouseModel rmodel = new WuZhouWarehouseModel();
                //获取出库单信息
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); 
                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString());
                foreach (var item in itemmodel)
                {
                    rmodel.OrderId = "10000" + model.SysNo;//订单编号
                    rmodel.OrderTime = model.CreatedDate.ToString();//下单时间
                    rmodel.OrderTotal = model.StockOutAmount.ToString();//订单总价
                    
                    rmodel.ProductTotal = model.Receivable.ToString();//商品总价
                    #region 订单信息
                    var soOrder = SoOrderBo.Instance.GetModel(model.OrderSysNO);
                    rmodel.Freight = soOrder.FreightAmount.ToString();//运费
                    #endregion
                    //获取订单收货地址
                    SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
                    rmodel.Consignee = ReceiveAddress.Name;//收货人
                    rmodel.Mobile = ReceiveAddress.MobilePhoneNumber;//联系电话
                    rmodel.IDCardNo = ReceiveAddress.IDCardNo;//身份证号码
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
                        rmodel.AddressProvince = provinceEntity.AreaName;//收货地址省
                        rmodel.AddressCity = cEntity.AreaName;//收货地址市
                        rmodel.AddresseeCounty = aEntity.AreaName;//收货地址区县
                        rmodel.Addressee = ReceiveAddress.StreetAddress;//详细地址
                    }
                    
                    //根据商品编号查询商品详细信息
                    var product = Hyt.BLL.Web.PdProductBo.Instance.GetProductInfo(item.ProductSysNo);
                    //根据商品编号查询价格
                    //var PdPrice =PdPriceBo.Instance.GetModel(product.SysNo);
                    rmodel.ProductName = product.ProductName;//商品名称
                    rmodel.ProductUnitPrice = item.SalesUnitPrice.ToString();//商品单价
                    rmodel.ProductCount = item.Quantity.ToString();//商品数量
                    rmodel.ProductSKU = "";//商品Sku
                    rmodel.IsCustoms = "";//是否完税(是/否)
                    rmodel.SettlementPrice = "";//商品结算价
                    rmodel.Platform = "";//电商平台
                    rmodel.PaymentEnterprise = "";//支付企业
                    rmodel.PaymentNumber = "";//支付单号
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<WuZhouWarehouseModel>(listmodel,
                  new List<string> { "订单号", "下单时间", "订单总价", "商品总价", "运费", "收货人", "联系电话", "身份证号码", "收货地址省", "收货地址市", "收货地址区县", "详细地址", "商品名称", "商品单价", "商品数量", "商品Sku", "是否完税(是/否)", "商品结算价", "电商平台", "支付企业", "支付单号" },
                  fileName);
        }

        #region 五洲仓导出Excel模板实体
        private class WuZhouWarehouseModel
        {

            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderId { get; set; }

            /// <summary>
            /// 下单时间
            /// </summary>
            public string OrderTime { get; set; }

            /// <summary>
            /// 订单总价
            /// </summary>
            public string OrderTotal { get; set; }

            /// <summary>
            /// 商品总价
            /// </summary>
            public string ProductTotal { get; set; }

            /// <summary>
            /// 运费
            /// </summary>
            public string Freight { get; set; }

            /// <summary>
            /// 收货人
            /// </summary>
            public string Consignee { get; set; }


            /// <summary>
            /// 联系电话
            /// </summary>
            public string Mobile { get; set; }

            /// <summary>
            /// 身份证号码
            /// </summary>
            public string IDCardNo { get; set; }

            /// <summary>
            /// 收货地址省
            /// </summary>
            public string AddressProvince { get; set; }

            /// <summary>
            /// 收货地址市
            /// </summary>
            public string AddressCity { get; set; }

            /// <summary>
            /// 收货地址区县
            /// </summary>
            public string AddresseeCounty { get; set; }

            /// <summary>
            /// 详细地址
            /// </summary>
            public string Addressee { get; set; }

            /// <summary>
            /// 商品名称
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 商品单价
            /// </summary>
            public string ProductUnitPrice { get; set; }

            /// <summary>
            /// 商品数量
            /// </summary>
            public string ProductCount { get; set; }

            /// <summary>
            /// 商品Sku
            /// </summary>
            public string ProductSKU { get; set; }


            /// <summary>
            /// 是否完税(是/否)
            /// </summary>
            public string IsCustoms { get; set; }

            /// <summary>
            /// 商品结算价
            /// </summary>
            public string SettlementPrice { get; set; }

            /// <summary>
            /// 电商平台
            /// </summary>
            public string Platform { get; set; }

            /// <summary>
            /// 支付企业
            /// </summary>
            public string PaymentEnterprise { get; set; }

            /// <summary>
            /// 支付单号
            /// </summary>
            public string PaymentNumber { get; set; }
        }
        #endregion
    }
}
