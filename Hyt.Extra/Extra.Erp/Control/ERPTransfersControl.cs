
using Extra.Erp.Model;
using Extra.Erp.Model.BaseData;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Control
{
    public class ERPTransfersControl
    {
        /// <summary>
        /// 查询调拨单记录
        /// </summary>
        /// <param name="FBillNo"></param>
        /// <param name="FStartDate"></param>
        /// <param name="FEndDate"></param>
        /// <returns></returns>
        public EasResult<List<EasTransfers>> GetTransList(string FBillNo, DateTime? FStartDate, DateTime? FEndDate)
        {
            string actionPath = "/base/TransList";
            string postDataPath = Config.root + actionPath;
            EasResult<List<EasTransfers>> result = new EasResult<List<EasTransfers>>();
            try
            {
                string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, "{ \"Ftrantype\":\"41\",\"FBillNo\":\"" + FBillNo + "\",\"FStartDate\":\"" + (FStartDate == null ? "" : FStartDate.Value.ToString("yyyy-MM-dd")) + "\",\"FEndDate\":\"" + (FEndDate == null ? "" : FEndDate.Value.ToString("yyyy-MM-dd")) + "\"}");
                result = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult<List<EasTransfers>>>(posData);
                //result.success = true;
            }catch(Exception e)
            {
                result.success = false;
                result.message = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        public Extra.Erp.Model.EasResult GetTransAdd(int SysNo)
        {
            Extra.Erp.Model.EasResult result = new Model.EasResult();
            string actionPath = "/base/TransAdd";
            //if (ActionType == 1)
            //{
            //    actionPath = "/bill/stockModi";
            //}
            string postDataPath = Config.root + actionPath;
            Hyt.Model.DBAtAllocation mod = Hyt.DataAccess.Warehouse.IAtAllocationDao.Instance.GetDBAtAllocationEntity(SysNo);
            List<DBAtAllocationItem> items = Hyt.DataAccess.Warehouse.IAtAllocationDao.Instance.GetByDBAtAllocationItem(mod.OutWarehouseSysNo,SysNo);
            EasTransfers transMod = new EasTransfers();
            transMod.APP_Key = Config.APP_Key;
            transMod.APP_scode = Config.APP_scode;
            transMod.Fdate = mod.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            transMod.FBillNo = mod.AllocationCode;
            transMod.FEntryID = "1";
            int indx = 0;
            foreach(var itemMod in items)
            {
                indx++;
                transMod.item.Add(new EasTransfersItem()
                {
                     FEntryID = indx.ToString(),
                     FAmount = itemMod.CostAmount,
                     FAuxPrice = itemMod.CostPrice,
                     Fauxqty = itemMod.Quantity,
                     FItemID = itemMod.ErpCode,
                     FItemName = itemMod.ProductName,
                     FUnitID = "KG",
                     FDCStockID = mod.EnterWarehousCode,
                     FSCStockID = mod.OutWarehousCode
                });
            }
            string strProduct = Hyt.Util.Serialization.JsonUtil.ToJson(transMod);
            strProduct = strProduct.Replace("\"APP_Key\":\"\",\"APP_scode\":\"\",", "");
            string posData = Hyt.Util.MyHttp.PostJsonData(postDataPath, strProduct);
            result = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult>(posData);
            return result;
        }
    }
}
