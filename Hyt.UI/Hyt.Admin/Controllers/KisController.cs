using Extra.Erp.DataContract;
using Hyt.BLL.Kis;
using Hyt.Model.Kis;
using Hyt.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hyt.Admin.Controllers
{
    public class KisController : Controller
    {
        //
        // GET: /Kis/
        //测试方法
         public ActionResult Test()
        {
             //模拟Json数据
            string Jsonstr = "{\"FAcctDB\":\"\",\"FBillNo\":\"XSCK20170601403116\",\"FConsignee\":\"hytformal\",\"FCustID\":\"01.010\",\"FDCStockID\":\"01.001\",\"FDeptID\":\"\",\"FEmpID\":\"140\",\"FEntryID\":\"0\",\"FExplanation\":\"\",\"FFManagerID\":\"\",\"FFetchAdd\":\"\",\"FROB\":\"1\",\"FSManagerID\":\"\",\"FSaleStyle\":\"线上电商平台\",\"Fdate\":\"2017-06-01\",\"item\":[{\"FBatchNo\":\"\",\"FConsignAmount\":\"444.00\",\"FConsignPrice\":\"111.00\",\"FDCStockID\":\"01.001\",\"FDiscountAmount\":\"0.00\",\"FDiscountRate\":\"\",\"FItemID\":\"02.01.02.63\",\"FItemName\":\"\",\"FKFDate\":null,\"FKFPeriod\":\"\",\"FPeriodDate\":null,\"FUnitID\":\"ping\",\"Fauxqty\":\"30\",\"Fnote\":null}]}";
            //string pstr = System.Web.HttpContext.Current.Request.Form["Param"];
            //var sr = new StreamReader(Request.InputStream);
            //var stream = sr.ReadToEnd();
            JavaScriptSerializer s = new JavaScriptSerializer(); //继承自 System.Web.Script.Serialization;
            JsonModel jr = s.Deserialize<JsonModel>(Jsonstr); //JSON串没问题就可以转
             
            string exception = "";
            string responseStr = Hyt.Util.MyHttp.PostJsonData("http://localhost:42025/Kis/InsertKis", Jsonstr, ref exception);
           //var re= InsertKis(jr);
            return Content("");
        }

         //InsertKis
        public JsonResult InsertKis(JsonModel json)
        {
            string addds = JsonHelper.ToJson(json);
            Hyt.BLL.Log.LocalLogBo.Instance.WriteTo(addds, "Kis");
            if (json==null)
            {
                var sr = new StreamReader(Request.InputStream);
                var stream = sr.ReadToEnd();
                JavaScriptSerializer s = new JavaScriptSerializer(); //继承自 System.Web.Script.Serialization;
                json = s.Deserialize<JsonModel>(stream); //只要你的JSON串没问题就可以转
            }
        
            Hyt.Model.Kis.KisResultT<StockOutAddResponse> res = new KisResultT<StockOutAddResponse>();
            res.success = false;
            res.message = "失败";
            res.error_code = -1;
            res.data = new StockOutAddResponse();
            //res.data.FBillNo = json.FBillNo;
            //res.data.OutFBillNo = json.FBillNo;


            IcStockBillTemBo target = new IcStockBillTemBo(); // TODO: 初始化为适当的值

            ICStockBillEntryTemBo targeta = new ICStockBillEntryTemBo(); // TODO: 初始化为适当的值
            IcStockBillTem soIdList = new IcStockBillTem(); // TODO: 初始化为适当的值
            soIdList.FInBillNo = json.FBillNo;
            soIdList.FDate = json.Fdate;
            soIdList.FTranType = 21;
            soIdList.FBillerID = "16394";
            soIdList.FBillNo = json.FBillNo;
            soIdList.FConsignee = json.FConsignee;
            soIdList.FCurrencyID = "人民币";
            soIdList.FDeliveryPlace = "";
           
            soIdList.FDeptID = json.FDeptID;
            soIdList.FEmpID = json.FEmpID;
            soIdList.FentryCount = 0;
            soIdList.FExplanation = json.FExplanation;
            soIdList.FFetchDate = "";
            soIdList.FFManagerID = "-1";
            soIdList.FInventoryType = "";
            soIdList.FK3BillerID = 0;
            soIdList.FK3BillTypeID = 0;
            soIdList.FK3CurrencyID =1;
            soIdList.FK3DeptID = 0;
            soIdList.FRob = Int32.Parse(json.FROB);

            soIdList.FSupplyID = json.FCustID;
            soIdList.FK3SupplyID = 2201;
            //主表插入数据
           var r= target.InsertEntity(soIdList);

            List<ICStockBillEntryTem> ItemList = new List<ICStockBillEntryTem>();
             // TODO: 初始化为适当的值
            foreach(JsonItem js in json.item)
            {
                ICStockBillEntryTem sost = new ICStockBillEntryTem();
                sost.FInBillNo = json.FBillNo;
                sost.FDCStockID = js.FDCStockID;
                sost.FItemID = js.FItemID;
                sost.FKFDate = js.FKFDate;

                if (string.IsNullOrEmpty(js.FConsignPrice))
                {
                    sost.Fauxprice = 0;
                }
                else { sost.Fauxprice = decimal.Parse(js.FConsignPrice); }

                if (string.IsNullOrEmpty(js.FConsignAmount))
                {
                    sost.Famount = 0;
                }
                else { sost.Famount = decimal.Parse(js.FConsignAmount); }

                if (string.IsNullOrEmpty(js.FKFPeriod))
                {
                    sost.FKFPeriod = 0;
                }
                else { sost.FKFPeriod = Int32.Parse(js.FKFPeriod); }
               
                sost.FPeriodDate = js.FPeriodDate;
                sost.FUnitID = js.FUnitID;

                if (string.IsNullOrEmpty(js.Fauxqty))
                {
                    sost.Fauxqty = 0;
                    sost.FQty = 0;
                }
                else { sost.Fauxqty = decimal.Parse(js.Fauxqty);
                sost.FQty = decimal.Parse(js.Fauxqty);
                }
                sost.Fnote = js.Fnote;
                //明细表插入数据
                //targeta.InsertEntity(sost);
                ItemList.Add(sost);
            }
            
            if (r>0)
            {
               var re= targeta.InsertEntityList(ItemList);
               if (re>0)
               {
                   //执行存储过程
                   target.EXEC();
                   res.success = true;
                   res.message = "成功";
                   res.error_code = 0;
                   res.data = new StockOutAddResponse();
                   res.data.FBillNo = json.FBillNo;
                   res.data.OutFBillNo = "";
               }
             }
          
            //
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
