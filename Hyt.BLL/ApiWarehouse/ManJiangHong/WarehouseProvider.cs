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

namespace Hyt.BLL.ApiWarehouse.ManJiangHong
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
                return Hyt.Model.CommonEnum.仓库代码.满江红;
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
            var fileName = string.Format("满江红出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            List<ManJiangHongWarehouseModel> listmodel = new List<ManJiangHongWarehouseModel>();
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                ManJiangHongWarehouseModel rmodel = new ManJiangHongWarehouseModel();
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); //获取出库单信息
                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString());
                rmodel.商户订单号 = "10000" + model.SysNo;//订单编号
                rmodel.客户编码 = "";
                rmodel.SKU = "";
                rmodel.批次编号 = "";
                rmodel.过期时间 = "";
                rmodel.DanJia= "";
                rmodel.订单行小计 = "";
                rmodel.运费 = "0";
                rmodel.商品总金额 = "";
                rmodel.订单总金额 = "";
                rmodel.邮政编码 = "";
                rmodel.支付企业编码 = "";
                rmodel.支付流水号 = "";
                rmodel.备注 = "";
                foreach (var item in itemmodel)
                {
                    //根据商品编号查询商品详细信息
                    var product = Hyt.BLL.Web.PdProductBo.Instance.GetProductInfo(item.ProductSysNo);
                    rmodel.产品名称 = item.EasName;
                    rmodel.购买数量 = item.Quantity.ToString();
                    
                    #region 收货人信息
                    SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);  //获取订单收货地址
                    rmodel.收件人身份证号码 = ReceiveAddress.IDCardNo;//身份证
                    rmodel.收件人 = ReceiveAddress.Name;//收件人姓名
                    rmodel.联络电话 = ReceiveAddress.MobilePhoneNumber;
                    rmodel.配送方式 = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(model.DeliveryTypeSysNo).DeliveryTypeName;
                    
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
                        rmodel.省份 = provinceEntity.AreaName;//收件人省份
                        rmodel.城市 = cEntity.AreaName;//城市
                        rmodel.县 = aEntity.AreaName;//区县
                        rmodel.详细地址 = ReceiveAddress.StreetAddress;//详细地址
                    }
                    #endregion
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<ManJiangHongWarehouseModel>(listmodel,
                  new List<string> { "商户订单号", "客户编码", "SKU", "批次编号", "过期时间", "产品名称", "单价(元)", "购买数量", "订单行小计", "收件人", "收件人身份证号码", "省份", "城市", "县(区)", "详细地址", "联络电话", "配送方式", "运费", "商品总金额", "订单总金额", "邮政编码", "支付企业编码", "支付流水号", "备注" },
                  fileName);
        }
        #endregion

        #region 满江红导出Excel模板实体
        private class ManJiangHongWarehouseModel
        {
            public string 商户订单号 { get; set; }
            public string 客户编码 { get; set; }
            public string SKU { get; set; }
            public string 批次编号 { get; set; }
            public string 过期时间 { get; set; }
            public string 产品名称 { get; set; }
            public string DanJia  { get; set; }
            public string 购买数量 { get; set; }
            public string 订单行小计 { get; set; }
            public string 收件人 { get; set; }
            public string 收件人身份证号码 { get; set; }
            public string 省份 { get; set; }
            public string 城市 { get; set; }
            public string 县 { get; set; }
            public string 详细地址 { get; set; }
            public string 联络电话 { get; set; }
            public string 配送方式 { get; set; }
            public string 运费 { get; set; }
            public string 商品总金额 { get; set; }
            public string 订单总金额 { get; set; }
            public string 邮政编码 { get; set; }
            public string 支付企业编码 { get; set; }
            public string 支付流水号 { get; set; }
            public string 备注 { get; set; }

        }
        #endregion


    }
}
