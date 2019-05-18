using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.BLL.CRM;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;
namespace Hyt.BLL.ApiWarehouse.ShaTian
{
    /// <summary>
    /// 沙田仓
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
                return Hyt.Model.CommonEnum.仓库代码.沙田仓;
            }
        }
        /// <summary>
        /// 导出订单
        /// </summary>
        /// <param name="data">查询参数</param>
        /// <returns></returns>
        /// <remarks>2017-07-07 杨浩 创建</remarks>
        public override void ExportOrder(object data)
        {
            List<ShaTianWarehouseModel> listmodel = new List<ShaTianWarehouseModel>();
            var fileName = string.Format("沙田仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                ShaTianWarehouseModel rmodel = new ShaTianWarehouseModel();
                
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); //获取出库单信息
                SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);  //获取订单收货地址

                rmodel.AddresseeName = ReceiveAddress.Name;  //收件人姓名
                rmodel.AddresseeId = ReceiveAddress.IDCardNo;////收件人身份证
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
                    var pEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetCountryEntity(ReceiveAddress.AreaSysNo,out countryEntity, out  provinceEntity, out cEntity, out aEntity);
                    rmodel.AddresseeCountry = countryEntity.AreaName;  //收件人国家
                    rmodel.AddresseeProvince = provinceEntity.AreaName;  //收件人省份
                    rmodel.AddresseeCity = cEntity.AreaName; //收件人城市
                    rmodel.AddresseeStreet = aEntity.AreaName ;  //收件人区/县
                    rmodel.AddresseeAddress1 =ReceiveAddress.StreetAddress; //收件人地址行1
                    rmodel.AddresseeAddress2 = ""; //收件人地址行2
                }
                rmodel.AddresseeZipCode = ReceiveAddress.ZipCode; //收件人邮编
                rmodel.AddresseePhone = ReceiveAddress.MobilePhoneNumber; //收件人手机
                rmodel.AddresseeNumber = ReceiveAddress.PhoneNumber;  //收件人电话
                rmodel.AddresseeEmail =ReceiveAddress.EmailAddress;  //收件人邮箱

                #region 未知来源数据
                rmodel.ChannelId = ""; //渠道编码
                rmodel.RFNumber =OrderNoPrefix+model.SysNo.ToString(); //参考号
                rmodel.AddresseeCompanyName = ""; //收件人公司名称
                rmodel.PlatformStore = ""; //平台店铺
                rmodel.ShopName = ""; //店铺名称
                rmodel.Remarks = model.Remarks; //备注
                rmodel.ParcelWeight = ""; //包裹重量
                rmodel.DeclareValue = ""; //面单申报价值
                rmodel.Currency = ""; //币制
                rmodel.OddNumbers = ""; //报关单号
                rmodel.SmallTicket = ""; //小票号或交易号
                rmodel.SmallTicketJPG = ""; //内件小票jpg图片附件
                rmodel.PalletNumber = ""; //托盘号(卡板号)
                #endregion

                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString()); 
                foreach (var item in itemmodel)
                {
                    rmodel.Number = item.Quantity.ToString(); //数量
                    rmodel.UnitPrice = item.SalesUnitPrice.ToString(); //单价
                    var product = Hyt.BLL.Web.PdProductBo.Instance.GetProductInfo(item.ProductSysNo);
                    rmodel.ProductType = product == null ? "" : product.ProductType.ToString(); //商品类型
                    rmodel.InternalName = product.ProductName; //内件名称
                    rmodel.SKU = product.Barcode; //条形码
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<ShaTianWarehouseModel>(listmodel,
                  new List<string> { "收件人名称", "收件人身份证号码", "收件人国家", "收件人省份", "收件人城市", "收件人街道", "收件人地址行1", "收件人地址行2", "收件人邮编", "收件人手机", "收件人电话", "收件人邮箱", "渠道编码", "参考号", "收件人公司名称", "平台店铺", "店铺名称", "备注", "包裹重量", "面单申报价值", "币制", "报关单号", "小票号或交易号", "内件小票jpg图片附件", "托盘号(卡板号)", "内件名称", "SKU", "数量", "单价", "商品类型" },
                  fileName);
        }

        #region 沙田仓导出Excel模板实体
        private class ShaTianWarehouseModel
        {

            /// <summary>
            /// 收件人姓名
            /// </summary>
            public string AddresseeName { get; set; }

            /// <summary>
            /// 收件人身份证
            /// </summary>
            public string AddresseeId { get; set; }

            /// <summary>
            /// 收件人国家
            /// </summary>
            public string AddresseeCountry { get; set; }

            /// <summary>
            /// 收件人省份
            /// </summary>
            public string AddresseeProvince { get; set; }

            /// <summary>
            /// 收件人城市
            /// </summary>
            public string AddresseeCity { get; set; }

            /// <summary>
            /// 收件人街道
            /// </summary>
            public string AddresseeStreet { get; set; }


            /// <summary>
            /// 收件人地址行1
            /// </summary>
            public string AddresseeAddress1 { get; set; }

            /// <summary>
            /// 收件人地址行2
            /// </summary>
            public string AddresseeAddress2 { get; set; }

            /// <summary>
            /// 收件人邮编
            /// </summary>
            public string AddresseeZipCode { get; set; }

            /// <summary>
            /// 收件人手机
            /// </summary>
            public string AddresseePhone { get; set; }

            /// <summary>
            /// 收件人电话
            /// </summary>
            public string AddresseeNumber { get; set; }

            /// <summary>
            /// 收件人邮箱
            /// </summary>
            public string AddresseeEmail { get; set; }

            /// <summary>
            /// 渠道编码
            /// </summary>
            public string ChannelId { get; set; }

            /// <summary>
            /// 参考号
            /// </summary>
            public string RFNumber { get; set; }

            /// <summary>
            /// 收件人公司名称
            /// </summary>
            public string AddresseeCompanyName { get; set; }

            /// <summary>
            /// 平台店铺
            /// </summary>
            public string PlatformStore { get; set; }


            /// <summary>
            /// 店铺名称
            /// </summary>
            public string ShopName { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remarks { get; set; }

            /// <summary>
            /// 包裹重量
            /// </summary>
            public string ParcelWeight { get; set; }

            /// <summary>
            /// 面单申报价值
            /// </summary>
            public string DeclareValue { get; set; }

            /// <summary>
            /// 币制
            /// </summary>
            public string Currency { get; set; }

            /// <summary>
            /// 报关单号
            /// </summary>
            public string OddNumbers { get; set; }

            /// <summary>
            /// 小票号或交易号
            /// </summary>
            public string SmallTicket { get; set; }

            /// <summary>
            /// 内件小票jpg图片附件
            /// </summary>
            public string SmallTicketJPG { get; set; }

            /// <summary>
            /// 托盘号(卡板号)
            /// </summary>
            public string PalletNumber { get; set; }

            /// <summary>
            /// 内件名称
            /// </summary>
            public string InternalName { get; set; }

            /// <summary>
            /// SKU
            /// </summary>
            public string SKU { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public string Number { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            public string UnitPrice { get; set; }

            /// <summary>
            /// 商品类型
            /// </summary>
            public string ProductType { get; set; }

        }
        #endregion
    }
}
