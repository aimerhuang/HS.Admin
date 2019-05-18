using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Util;
using Hyt.Model.ExcelTemplate;
using System.Data;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;

namespace Hyt.BLL.ApiWarehouse.ShenZhen
{
    /// <summary>
    /// 深圳仓
    /// </summary>
    /// <remarks>2017-07-07 杨浩 创建</remarks>
    public class WarehouseProvider : IWarehouseProvider
    {
        /// <summary>
        /// 仓库标识
        /// </summary>
        /// <remarks> </remarks>
        public override Hyt.Model.CommonEnum.仓库代码 Code
        {
            get
            {
                return Hyt.Model.CommonEnum.仓库代码.深圳仓;
            }
        }

        #region 导出订单
        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="data">查询参数</param>
        /// <returns></returns>
        /// <remarks>2017-07-07 杨浩 创建</remarks>
        public override void ExportOrder(object data)
        {
            var fileName = string.Format("深圳仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            List<ShenZhenWarehouseModel> listmodel = new List<ShenZhenWarehouseModel>();
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                ShenZhenWarehouseModel rmodel = new ShenZhenWarehouseModel();
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); //获取出库单信息
                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString());
                foreach (var item in itemmodel)
                {
                    //根据商品编号查询商品详细信息
                    var product = Hyt.BLL.Web.PdProductBo.Instance.GetProductInfo(item.ProductSysNo); 
                    rmodel.SysNo = product.ErpCode; //序号
                    var ware = Hyt.BLL.Web.WhWarehouseBo.Instance.GetModel(model.WarehouseSysNo);
                    rmodel.WarehouseSysNo = ware.WarehouseName; //出货仓库
                    rmodel.Agent = "";//代理商
                    rmodel.SaleDepartment = "";//销售部门
                    rmodel.PlatformName = ""; //电商平台名称
                    rmodel.OrderTime = model.CreatedDate; //订单日期
                    rmodel.DeliverTime = model.StockOutDate; //发货日期
                    rmodel.ProductName = product.ProductName; //商品名称
                    rmodel.BarCode = product.Barcode; //商品条形码
                    rmodel.ProductCompany = ""; //商品单位
                    rmodel.DeclareCount = ""; //申报数量
                    rmodel.ActualDeliver = ""; //实发
                    rmodel.DeclarePrice = ""; //申报单价
                    rmodel.DeclarePriceSum = "";//申报总价
                    #region 订单信息
                    var soOrder = SoOrderBo.Instance.GetModel(model.OrderSysNO);
                    rmodel.Freight = soOrder.FreightAmount.ToString();//运费
                    #endregion
                    rmodel.InsuredValue = "";//保价费
                    rmodel.Tax = "";//税款
                    rmodel.GrossWeight = "";//毛重
                    rmodel.NetWeight = "";//净重
                    rmodel.CollectionAccount = "";//收款账号
                    rmodel.InternalOrderNumber = "10000" + model.SysNo;//订单编号
                    rmodel.SalesOrderNo = "10000" + model.SysNo;//订单编号
                    rmodel.Courier = "";//选用的快递公司
                    rmodel.CourierNumber = "";//快递单号
                    rmodel.ProblemBreakdown = "";//问题明细
                    rmodel.RetailersPhone = "";//电商客户电话
                    #region 收货人信息
                    SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);  //获取订单收货地址
                    rmodel.Identity = ReceiveAddress.IDCardNo;//身份证
                    rmodel.RecipientName = ReceiveAddress.Name;//收件人姓名

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
                        rmodel.RecipientProvince = provinceEntity.AreaName;//收件人省份
                        rmodel.City = cEntity.AreaName;//城市
                        rmodel.District = aEntity.AreaName;//区县
                        rmodel.Address = ReceiveAddress.StreetAddress;//详细地址
                    }
                    rmodel.Remarks = "";//备注
                    rmodel.PlatformCoding = "";//平台编码
                    rmodel.TransactioNnumber = "";//支付交易号
                    #endregion
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<ShenZhenWarehouseModel>(listmodel,
                  new List<string> { "序号", "出货仓", "代理商", "销售部门", "电商平台名称", "订单日期", "发货日期", "商品品名", "商品条形码", "商品单位", "申报数量", "实发", "申报单价", "申报总价", "运费", "保价费", "税款", "毛重（kg）", "净重（kg）", "收款账号", "内部订单号", "销售订单号", "选用的快递公司", "快递单号", "问题明细", "电商客户电话", "身份证", "收件人姓名", "收件人省份", "城市", "区县", "详细地址", "备注", "平台编码", "支付交易号" },
                  fileName);
        }
        #endregion

        #region 深圳仓导出Excel模板实体
        private class ShenZhenWarehouseModel
        {
            /// <summary>
            /// 序号(SysNo)
            /// </summary>
            public string SysNo { get; set; }

            /// <summary>
            /// 出货仓库(WarehouseSysNo)
            /// </summary>
            public string WarehouseSysNo { get; set; }


            /// <summary>
            /// 代理商
            /// </summary>
            public string Agent { get; set; }

            /// <summary>
            /// 销售部门
            /// </summary>
            public string SaleDepartment { get; set; }

            /// <summary>
            /// 电商平台名称
            /// </summary>
            public string PlatformName { get; set; }


            /// <summary>
            /// 订单日期
            /// </summary>
            public DateTime OrderTime { get; set; }

            /// <summary>
            /// 发货日期
            /// </summary>
            public DateTime DeliverTime { get; set; }

            /// <summary>
            /// 商品名称
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 商品条形码
            /// </summary>
            public string BarCode { get; set; }

            /// <summary>
            /// 商品单位
            /// </summary>
            public string ProductCompany { get; set; }

            /// <summary>
            /// 申报数量
            /// </summary>
            public string DeclareCount { get; set; }


            /// <summary>
            /// 实发
            /// </summary>
            public string ActualDeliver { get; set; }

            /// <summary>
            /// 申报单价
            /// </summary>
            public string DeclarePrice { get; set; }

            /// <summary>
            /// 申报总价
            /// </summary>
            public string DeclarePriceSum { get; set; }

            /// <summary>
            /// 运费
            /// </summary>
            public string Freight { get; set; }

            /// <summary>
            /// 保价费
            /// </summary>
            public string InsuredValue { get; set; }


            /// <summary>
            /// 税款
            /// </summary>
            public string Tax { get; set; }

            /// <summary>
            /// 毛重（kg）
            /// </summary>
            public string GrossWeight { get; set; }

            /// <summary>
            /// 净重（kg）
            /// </summary>
            public string NetWeight { get; set; }


            /// <summary>
            /// 收款账号
            /// </summary>
            public string CollectionAccount { get; set; }

            /// <summary>
            /// 内部订单号
            /// </summary>
            public string InternalOrderNumber { get; set; }

            /// <summary>
            /// 销售订单号
            /// </summary>
            public string SalesOrderNo { get; set; }

            /// <summary>
            /// 选用的快递公司
            /// </summary>
            public string Courier { get; set; }

            /// <summary>
            /// 快递单号
            /// </summary>
            public string CourierNumber { get; set; }


            /// <summary>
            /// 问题明细
            /// </summary>
            public string ProblemBreakdown { get; set; }

            /// <summary>
            /// 电商客户电话
            /// </summary>
            public string RetailersPhone { get; set; }

            /// <summary>
            /// 身份证
            /// </summary>
            public string Identity { get; set; }


            /// <summary>
            /// 收件人姓名
            /// </summary>
            public string RecipientName { get; set; }


            /// <summary>
            /// 收件人省份
            /// </summary>
            public string RecipientProvince { get; set; }

            /// <summary>
            /// 城市
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// 区县
            /// </summary>
            public string District { get; set; }

            /// <summary>
            /// 详细地址
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remarks { get; set; }

            /// <summary>
            /// 平台编码
            /// </summary>
            public string PlatformCoding { get; set; }

            /// <summary>
            /// 支付交易号
            /// </summary>
            public string TransactioNnumber { get; set; }

        }
        #endregion


    }
}
