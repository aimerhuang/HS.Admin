using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;
using Hyt.BLL.Product;
using Hyt.BLL.Web;
namespace Hyt.BLL.ApiWarehouse.TongYong
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
                return Hyt.Model.CommonEnum.仓库代码.通用;
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
            var fileName = string.Format("各仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            List<TongYongModel> listmodel = new List<TongYongModel>();
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                TongYongModel rmodel = new TongYongModel();
                //获取出库单信息
                var model = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); 
                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString());
                foreach (var item in itemmodel)
                {
                    rmodel.序号 = item.ErpCode;
                    rmodel.出货仓 = WhWarehouseBo.Instance.GetModel(model.WarehouseSysNo).BackWarehouseName;
                    rmodel.代理商 = "";
                    rmodel.销售部门 = "";
                    rmodel.电商平台名称 = "";
                    rmodel.订单日期 = model.CreatedDate.ToString();
                    rmodel.发货日期 = "";
                    rmodel.商品品名 = item.EasName;
                    rmodel.商品条形码 = item.BarCode;
                    rmodel.商品单位 = "";
                    rmodel.申报数量 = item.Quantity.ToString();
                    rmodel.实发 = item.Quantity.ToString();
                    rmodel.申报单价 = item.SalesUnitPrice.ToString();
                    rmodel.申报总价 = item.SalesAmount.ToString();
                    var soOrder = Hyt.BLL.Order.SoOrderBo.Instance.GetModel(model.OrderSysNO);
                    rmodel.运费 = soOrder.FreightAmount.ToString();//运费
                    rmodel.保价费 = "";
                    rmodel.税款 = "";
                    rmodel.毛重 = "";
                    rmodel.净重 = "";
                    rmodel.收款账号 = "";
                    rmodel.内部订单号 = "10000" + model.SysNo;//订单编号
                    rmodel.销售订单号 = "10000" + model.SysNo;//订单编号
                    rmodel.选用的快递公司 = "";

                    var PeiSong=Hyt.BLL.Logistics.LgDeliveryBo.Instance.GetDeliveryListByOrderSysNo(model.OrderSysNO);
                    rmodel.快递单号 = "";
                    rmodel.问题明细 = "";
                    
                    //获取订单收货地址
                    SoReceiveAddress ReceiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
                    rmodel.电商客户电话 = ReceiveAddress.MobilePhoneNumber;//联系电话
                    rmodel.收件人姓名 = ReceiveAddress.Name;//收货人
                    rmodel.身份证 = ReceiveAddress.IDCardNo;//身份证号码
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
                        rmodel.收件人省份 = provinceEntity.AreaName;//收货地址省
                        rmodel.城市 = cEntity.AreaName;//收货地址市
                        rmodel.区县 = aEntity.AreaName;//收货地址区县
                        rmodel.详细地址 = ReceiveAddress.StreetAddress;//详细地址
                    }
                    rmodel.备注 = "";
                    rmodel.平台编码 = "";
                    rmodel.支付交易号 = sysnoStr.Split(',')[i];
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<TongYongModel>(listmodel,
                  new List<string> { "序号", "出货仓", "代理商", "销售部门", "电商平台名称", "订单日期", 
                      "发货日期", "商品品名", "商品条形码", "商品单位", "申报数量", "实发", "申报单价", "申报总价", 
                      "运费", "保价费", "税款", "毛重（kg）", "净重（kg）", "收款账号", "内部订单号" ,
                      "销售订单号", "选用的快递公司", "快递单号", "问题明细", "电商客户电话", "身份证", "收件人姓名" ,
                      "收件人省份", "城市", "区县", "详细地址", "备注", "平台编码", "支付交易号"
                  },
                  fileName);
        }

        #region 各仓导出Excel模板实体
        private class TongYongModel
        {
            public string 序号 { get; set; }

            public string 出货仓 { get; set; }
            public string 代理商 { get; set; }
            public string 销售部门 { get; set; }
            public string 电商平台名称 { get; set; }
            public string 订单日期 { get; set; }
            public string 发货日期 { get; set; }
            public string 商品品名 { get; set; }
            public string 商品条形码 { get; set; }
            public string 商品单位 { get; set; }
            public string 申报数量 { get; set; }
            public string 实发 { get; set; }
            public string 申报单价 { get; set; }
            public string 申报总价 { get; set; }
            public string 运费 { get; set; }
            public string 保价费 { get; set; }
            public string 税款 { get; set; }
            public string 毛重 { get; set; }
            public string 净重 { get; set; }
            public string 收款账号 { get; set; }

            public string 内部订单号 { get; set; }

            public string 销售订单号 { get; set; }

            public string 选用的快递公司 { get; set; }

            public string 快递单号 { get; set; }

            public string 问题明细 { get; set; }

            public string 电商客户电话 { get; set; }

            public string 身份证 { get; set; }

            public string 收件人姓名 { get; set; }

            public string 收件人省份 { get; set; }

            public string 城市 { get; set; }

            public string 区县 { get; set; }

            public string 详细地址 { get; set; }


            public string 备注 { get; set; }

            public string 平台编码 { get; set; }

            public string 支付交易号 { get; set; }

        }
        #endregion
    }
}
