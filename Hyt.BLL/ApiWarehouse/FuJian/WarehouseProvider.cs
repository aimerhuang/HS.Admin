using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.BLL.CRM;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;
using Hyt.BLL.Web;
namespace Hyt.BLL.ApiWarehouse.FuJian
{
    /// <summary>
    /// 福建仓
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
                return Hyt.Model.CommonEnum.仓库代码.福建;
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
            List<FuJianWarehouseModel> listmodel = new List<FuJianWarehouseModel>();
            var fileName = string.Format("福建仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                FuJianWarehouseModel rmodel = new FuJianWarehouseModel();
                
                var model = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); //获取出库单信息
                SoReceiveAddress ReceiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);  //获取订单收货地址
                rmodel.收货人 = ReceiveAddress.Name;  //收件人姓名
                rmodel.身份证号 = ReceiveAddress.IDCardNo;////收件人身份证
                rmodel.身份证姓名 = ReceiveAddress.Name;  //收件人姓名
                rmodel.联系电话 = ReceiveAddress.MobilePhoneNumber;  //联系电话
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
                    rmodel.省 = provinceEntity.AreaName;  //收件人省份
                    rmodel.市 = cEntity.AreaName; //收件人城市
                    rmodel.区 = aEntity.AreaName;  //收件人区/县
                    rmodel.街道 = ReceiveAddress.StreetAddress; //收件人地址行1
                   
                }
                rmodel.邮编 = ReceiveAddress.ZipCode; //邮编
                rmodel.原始订单号 = "10000" + model.SysNo;//订单编号
                rmodel.仓库代码 = WhWarehouseBo.Instance.GetModel(model.WarehouseSysNo).ErpCode;
                rmodel.配送方式 = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(model.DeliveryTypeSysNo).DeliveryTypeName;
                #region 未知来源数据
                rmodel.销售平台 = "";
                rmodel.进口模式 = "";
                rmodel.价格1 = "";
                rmodel.SKU2 = "";
                rmodel.价格2 = "";
                rmodel.数量2 = "";
                rmodel.SKU3 = "";
                rmodel.价格3 = "";
                rmodel.数量3 = "";
                #endregion

                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString()); 
                foreach (var item in itemmodel)
                {
                    rmodel.SKU1 = item.EasName;
                    rmodel.数量1 = item.Quantity.ToString();
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<FuJianWarehouseModel>(listmodel,
                  new List<string> { "销售平台", "进口模式", "原始订单号", "省", "市", "区", "街道", 
                      "邮编", "身份证号", "身份证姓名", "收货人", "联系电话", "仓库代码", "配送方式", "SKU1", "价格1", 
                      "数量1", "SKU2", "价格2", "数量2", "SKU3", "价格3", "数量3"},
                  fileName);
        }

        #region 福建仓导出Excel模板实体
        private class FuJianWarehouseModel
        {

            public string 销售平台 { get; set; }

            public string 进口模式 { get; set; }

            public string 原始订单号 { get; set; }

            public string 省 { get; set; }

            public string 市 { get; set; }

            public string 区 { get; set; }

            public string 街道 { get; set; }

            public string 邮编 { get; set; }

            public string 身份证号 { get; set; }

            public string 身份证姓名 { get; set; }

            public string 收货人 { get; set; }

            public string 联系电话 { get; set; }

            public string 仓库代码 { get; set; }

            public string 配送方式 { get; set; }

            public string SKU1 { get; set; }

            public string 价格1 { get; set; }

            public string 数量1 { get; set; }

            public string SKU2 { get; set; }

            public string 价格2 { get; set; }

            public string 数量2 { get; set; }

            public string SKU3 { get; set; }

            public string 价格3 { get; set; }

            public string 数量3 { get; set; }
        }
        #endregion
    }
}
