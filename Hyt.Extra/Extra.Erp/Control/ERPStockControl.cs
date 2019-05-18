using Extra.Erp.Model;
using Extra.Erp.Model.BaseData;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Control
{
    public class ERPStockControl
    {
        /// <summary>
        /// 仓库-查询
        /// </summary>
        /// <param name="FNumber">仓库编码</param>
        /// <param name="FName">仓库名称</param>
        /// <param name="FParentNumber">上级编码</param>
        /// <returns></returns>
        public  EasResult<EasStock> ErpStockList(string FNumber,string FName,string FParentNumber)
        {
            string actionPath = "/base/stockList";
            string postDataPath = Config.root + actionPath;
            string posData = Hyt.Util.MyHttp.GetResponse(postDataPath, "{APP_Key:\"" + Config.APP_Key + "\",APP_scode:\"" + Config.APP_scode +
                    "\", FNumber:\"" + FName + "\",FNumber:\"" + FNumber + "\",FParentNumber:\"" + FParentNumber + "\"}", "utf-8");
            EasResult<EasStock> result = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult<EasStock>>(posData);
            return result;
        }

        /// <summary>
        /// 添加或修改仓库
        /// </summary>
        /// <param name="StockSysNo">仓库编号</param>
        /// <param name="ActionType">操作类型，0 添加，1 修改</param>
        /// <returns></returns>
        public Extra.Erp.Model.Result ErpStockAdd(int StockSysNo, int ActionType = 0)
        {
            Extra.Erp.Model.Result result = new Model.Result();
            try
            {
                string actionPath = "/base/stockAdd";
                if(ActionType==1)
                {
                    actionPath = "/base/stockModi";
                }
                string postDataPath = Config.root + actionPath;
                CBWhWarehouse warehouse = IWhWarehouseDao.Instance.GetWarehouse(StockSysNo);
                EasStock stock = new EasStock()
                {
                    APP_Key = Config.APP_Key,
                    APP_scode = Config.APP_scode,
                    FAddress = warehouse.ProvinceName + " " + warehouse.CityName + " " + warehouse.AreaName + " " + warehouse.StreetAddress,
                    fdetail = "1",
                    FFullName = "[" + StockSysNo + "]" + "[" + warehouse.ErpRmaCode + "]" + warehouse.BackWarehouseName,
                    FFullNumber = "[" + StockSysNo + "]" + warehouse.ErpRmaCode,

                    FName = warehouse.BackWarehouseName,
                    FNumber = warehouse.ErpRmaCode,
                    FPhone = warehouse.Phone,
                    FUnderStock = "1",
                    intType = ActionType,
                    isParent = 0
                };

            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }
        /// <summary>
        /// 删除仓库
        /// </summary>
        /// <param name="StockSysNo"></param>
        /// <returns></returns>
        public Extra.Erp.Model.Result ErpStockDelete(int StockSysNo)
        {
            Extra.Erp.Model.Result result = new Model.Result();
            string actionPath = "/base/stockDel";
            CBWhWarehouse warehouse = IWhWarehouseDao.Instance.GetWarehouse(StockSysNo);
            string postDataPath = Config.root + actionPath;
            string posData = Hyt.Util.MyHttp.GetResponse(postDataPath, "{APP_Key:\"" + Config.APP_Key + "\",APP_scode:\"" + Config.APP_scode + "\", FNumber:\"" + warehouse.ErpRmaCode + "\"}", "utf-8");
            EasResult easResult = Hyt.Util.Serialization.JsonUtil.ToObject<EasResult>(posData);

            result.Status = easResult.success;
            result.Message = easResult.message;
            return result;
        }
    }
}
