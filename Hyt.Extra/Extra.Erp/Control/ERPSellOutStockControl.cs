using Extra.Erp.Model;
using Extra.Erp.Model.BaseData;
using Hyt.DataAccess.Order;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Control
{
    public class ERPSellOutStockControl
    {
        /// <summary>
        /// 采购方式
        /// </summary>
        public string FCustID = "番来购商城";
        public EasResult<List<EasSearchSellOutStockItem>> ErpSellOutStockList(string FBillNo, DateTime? startTime, DateTime? endTime)
        {
            string actionPath = "/base/StockOutList";
            string postDataPath = Config.root + actionPath;
            //\"APP_Key\":\"" + Config.APP_Key + "\",\"APP_scode\":\"" + Config.APP_scode +"\",
            string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, "{ \"Ftrantype\":\"21\",\"FBillNo\":\"" + FBillNo + "\",\"FStartDate\":\"" + (startTime == null ? "" : startTime.Value.ToString("yyyy-MM-dd")) + "\",\"FEndDate\":\"" + (endTime == null ? "" : endTime.Value.ToString("yyyy-MM-dd")) + "\"}");
            EasResult<List<EasSearchSellOutStockItem>> result = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult<List<EasSearchSellOutStockItem>>>(posData);
            return result;
        }
        public Extra.Erp.Model.Result ErpSellOutStockAdd(int orderSysNo,  int ActionType = 0)
        {
            Extra.Erp.Model.Result result = new Model.Result();
            try
            {
                var stockout = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetModel(orderSysNo);
                WhWarehouse wareHouse = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWarehouse(stockout.WarehouseSysNo);
                SoOrder odrder = Hyt.DataAccess.Order.ISoOrderDao.Instance.GetEntity(stockout.OrderSysNO);
                odrder.ReceiveAddress = ISoReceiveAddressDao.Instance.GetOrderReceiveAddress(odrder.ReceiveAddressSysNo);
                string actionPath = "/base/StockOutAdd";
                //if (ActionType == 1)
                //{
                //    actionPath = "/bill/stockModi";
                //}

                string postDataPath = Config.root + actionPath;
                EasSellOutStock mod = new EasSellOutStock();
                mod.FBillNo = odrder.OrderNo;
                mod.FEntryID = "1";
                mod.Fdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mod.FConsignee = odrder.ReceiveAddress.Name;
                mod.FDeptID ="";
                mod.FEmpID = "";
                mod.FExplanation = "";
                mod.FFetchAdd = odrder.ReceiveAddress.StreetAddress;
                mod.FFManagerID = "";
                mod.FSaleStyle = "网单销售";
                mod.FSManagerID = "";
                mod.FCustID = FCustID;
                mod.FDCStockID = wareHouse.ErpCode;
                mod.FROB = "1";
                List<Hyt.Model.Transfer.CBSoOrderItem> orderItems = Hyt.DataAccess.Order.ISoOrderDao.Instance.GetOrderItemsByOrderId(new int[] { odrder.SysNo });
                int indx = 1;
                foreach (var itemMod in orderItems)
                {
                    WhStockOutItem outStockItem = stockout.Items.First(p => p.ProductSysNo == itemMod.ProductSysNo);
                    if (outStockItem != null)
                    {
                        EasSellOutStockItem item = new EasSellOutStockItem()
                        {
                            FItemID = itemMod.ErpCode,
                            Fauxqty = outStockItem.ProductQuantity.ToString(),
                            FItemName = itemMod.ProductName,
                            FDCStockID = wareHouse.ErpCode,
                            FUnitID = "kg",
                            FConsignPrice = itemMod.SalesUnitPrice.ToString("0.00"),
                            FConsignAmount = itemMod.SalesAmount.ToString("0.00")
                        };
                        mod.item.Add(item);
                    }
                }
                string strProduct = Hyt.Util.Serialization.JsonUtil.ToJson(mod);
                strProduct = strProduct.Replace("\"APP_Key\":\"\",\"APP_scode\":\"\",", "");
                string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, strProduct);
                if(posData.IndexOf("没有对应")!=-1)
                {
                    result.Status = false;
                    result.Message = posData;
                }
                else
                {
                    EasResult easResult = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult>(posData);
                    result.Status = easResult.success;
                    result.Message = easResult.message;
                }
            }
            catch(Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }
    }
}
