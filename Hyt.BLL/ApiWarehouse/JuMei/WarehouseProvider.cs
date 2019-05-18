using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.BLL.CRM;
using Hyt.BLL.Order;
using Hyt.Model.Transfer;
namespace Hyt.BLL.ApiWarehouse.JuMei
{
    /// <summary>
    /// 聚美仓
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
                return Hyt.Model.CommonEnum.仓库代码.聚美;
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
            List<JuMeiWarehouseModel> listmodel = new List<JuMeiWarehouseModel>();
            var fileName = string.Format("聚美仓出库单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var sysnoStr = data.ToString().Trim(','); //获取出库单id字符串
            for (int i = 0; i < sysnoStr.Split(',').Length; i++)
            {
                JuMeiWarehouseModel rmodel = new JuMeiWarehouseModel();
                
                var model = SoOrderBo.Instance.GetEntityTo(Convert.ToInt32(sysnoStr.Split(',')[i])); //获取出库单信息
                SoReceiveAddress ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);  //获取订单收货地址

                rmodel.订单号 = "10000" + model.SysNo;//订单编号
                rmodel.到付金额 = "";
                rmodel.收件人姓名 = ReceiveAddress.Name;  //收件人姓名
                rmodel.收件人电话 = ReceiveAddress.MobilePhoneNumber;//收件人电话
                rmodel.收件人单位 = "";
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
                    rmodel.收件人省 = provinceEntity.AreaName;  //收件人省份
                    rmodel.收件人市 = cEntity.AreaName; //收件人城市
                    rmodel.收件人区 = aEntity.AreaName;  //收件人区/县
                    rmodel.收件人地址 = ReceiveAddress.StreetAddress; //收件人地址行1
                 
                }
                rmodel.备注 = "";
                #region 订单商品明细
                List<CBSoOrderItem> itemmodel = SoOrderItemBo.Instance.GetCBOrderItemListBySysNos(model.OrderSysNO.ToString()); 
                foreach (var item in itemmodel)
                {
                    rmodel.品名 =item.EasName; 
                    rmodel.数量=item.Quantity.ToString();
                    listmodel.Add(rmodel);
                }
                #endregion
            }
            Util.ExcelUtil.Export<JuMeiWarehouseModel>(listmodel,
                  new List<string> { "订单号", "到付金额", "收件人姓名", "收件人电话", "收件人省", "收件人市",
                                    "收件人区", "收件人地址", "收件人单位", "品名", "数量", "备注"},
                  fileName);
        }
        #region 聚美仓导出Excel模板实体
        private class JuMeiWarehouseModel
        {
            public string 订单号 { get; set; }

            public string 到付金额 { get; set; }

            public string 收件人姓名 { get; set; }

            public string 收件人电话 { get; set; }

            public string 收件人省 { get; set; }

            public string 收件人市 { get; set; }

            public string 收件人区 { get; set; }

            public string 收件人地址 { get; set; }

            public string 收件人单位 { get; set; }

            public string 品名 { get; set; }

            public string 数量 { get; set; }

            public string 备注 { get; set; }
        }
        #endregion
    }
}
