using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Hyt.Admin.Models;
using Hyt.BLL.Basic;
using Hyt.BLL.Logistics;
using Hyt.BLL.Print;
using Hyt.BLL.Promotion;
using Hyt.BLL.Warehouse;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Extra.Erp.Model;
using Hyt.BLL.ExpressList;
using Hyt.BLL.Base;
using Hyt.Model.ExpressList;
using System.Web.Script.Serialization;
using Extra.Express.Provider;


namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 打印
    /// </summary>
    /// <remarks>2013-08-13 郑荣华 创建</remarks>
    public class PrintController : BaseController
    {
        /// <summary>
        /// 客服电话
        /// </summary>
        const string servicePhone = "4000889898";//客服电话

        #region 入库、配送、结算、借货、配货（拣货，出库）

        /// <summary>
        /// 入库单打印
        /// </summary>
        /// <param name="id">入库单主表系统编号</param>
        /// <returns>入库单打印界面</returns>
        /// <remarks>
        /// 2013-06-24 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1006102)]
        public ActionResult InStock(int? id)
        {
            var model = PrintBo.Instance.GetPrintInstock(id ?? 1);
            if (model != null)
            {
                //model.QuantityCount = model.List.Sum(x => x.RealStockInQuantity);
                model.SourceTypeName = ((WarehouseStatus.入库单据类型)model.SourceType) + "入库";
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印入库单", LogStatus.系统日志目标类型.入库单, model.SysNo,
                                                         CurrentUser.Base.SysNo);
            }

            return View("PickingInStock", model);
        }

        /// <summary>
        /// 入库单批量打印
        /// </summary>
        /// <param name="sysNo">入库单主表系统编号</param>
        /// <returns>入库单批量打印界面</returns>
        /// <remarks>
        /// 2013-07-30 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1006103)]
        public ActionResult InStockBatch(string sysNo)
        {
            var sysnos = Array.ConvertAll(sysNo.Split(','), int.Parse).ToList();
            IList<PrtInstock> models = new List<PrtInstock>();
            foreach (var item in sysnos)
            {
                var model = PrintBo.Instance.GetPrintInstock(item);
                if (model != null)
                {
                    //model.QuantityCount = model.List.Sum(x => x.RealStockInQuantity);
                    model.SourceTypeName = ((WarehouseStatus.入库单据类型)model.SourceType) + "入库";
                    models.Add(model);
                }
            }
            Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "批量打印入库单", LogStatus.系统日志目标类型.入库单, 0,
                                                                    CurrentUser.Base.SysNo);
            return View(models);
        }

        /// <summary>
        /// 配送单打印
        /// </summary>
        /// <param name="id">配送单主表系统编号</param>
        /// <returns>配送单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1006103)]
        public ActionResult Delivery(int? id)
        {
            //兼容火狐浏览器，必须重新刷新前台才能显示
            var isRefresh = Request.Params["isRefresh"];

            //加分销配送单 2013-09-16 郑荣华
            var model = PrintBo.Instance.GetPrintDsDelivery(id ?? 1);
            if (string.IsNullOrEmpty(isRefresh))
            {
                Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印配送单", LogStatus.系统日志目标类型.配送单, id ?? 0,
                                                                  CurrentUser.Base.SysNo);
            }
            return View("DsDelivery", model);

            //var model = PrintBo.Instance.GetPrintDelivery(id ?? 1);
            //if (model != null)
            //{
            //    var receivableCount = model.List.Sum(item => item.Receivable);

            //    ViewBag.ReceivableCount = receivableCount.ToString("C");
            //}
            //return View(model);

        }

        /// <summary>
        /// 结算单打印
        /// </summary>
        /// <param name="id">结算单主表系统编号</param>
        /// <returns>结算单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1003101)]
        public ActionResult Settlement(int? id)
        {
            //兼容火狐浏览器，必须重新刷新前台才能显示
            var isRefresh = Request.Params["isRefresh"];

            var model = PrintBo.Instance.GetPrintSettlement(id ?? 1);

            if (model != null)
            {
                model.ReceivableCount = model.List.Sum(x => x.Receivable);
            }
            if (string.IsNullOrEmpty(isRefresh))
            {
                Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印结算单", LogStatus.系统日志目标类型.结算单, id ?? 0,
                                                                  CurrentUser.Base.SysNo);
            }

            return View(model);
        }

        /// <summary>
        /// 结算单批量打印
        /// </summary>
        /// <param name="sysNo">结算单主表系统编号</param>
        /// <returns>结算单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1003101)]
        public ActionResult SettlementBatch(string sysNo)
        {
            //兼容火狐浏览器，必须重新刷新前台才能显示
            var isRefresh = Request.Params["isRefresh"];

            var sysnos = Array.ConvertAll(sysNo.Split(','), int.Parse).ToList();
            IList<PrtSettlement> models = new List<PrtSettlement>();
            foreach (var item in sysnos)
            {
                var model = PrintBo.Instance.GetPrintSettlement(item);
                if (model != null)
                {
                    model.ReceivableCount = model.List.Sum(x => x.Receivable);
                    models.Add(model);
                }
            }
            if (string.IsNullOrEmpty(isRefresh))
            {
                Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "批量打印结算单", LogStatus.系统日志目标类型.结算单, 0,
                                                                CurrentUser.Base.SysNo);
            }

            return View(models);

        }

        /// <summary>
        /// 借货单打印
        /// </summary>
        /// <param name="id">借货单主表系统编号</param>
        /// <returns>借货单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1004103)]
        public ActionResult Lend(int? id)
        {
            var model = PrintBo.Instance.GetPrintLend(id ?? 1);
            Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印借货单", LogStatus.系统日志目标类型.借货单, id ?? 0,
                                                                CurrentUser.Base.SysNo);
            return View(model);

        }

        /// <summary>
        /// 配货单（出库单、拣货单）打印
        /// </summary>
        /// <param name="id">出库单主表系统编号</param>
        /// <returns>配货单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1003601)]
        public ActionResult Picking(int id)
        {
            //兼容火狐浏览器，必须重新刷新前台才能显示
            var isRefresh = Request.Params["isRefresh"];

            BsArea city;
            BsArea area;
            BsArea prov;

            var order = Hyt.BLL.Order.SoOrderBo.Instance.GetByOutStockSysNo(id);
            var dealerInfo = BLL.Stores.StoresBo.Instance.GetStoreById(order.DealerSysNo);
            var dailiInfo = Hyt.DataAccess.Stores.IStoresDao.Instance.GetDiLiByCreatId(order.DealerSysNo);
            //kis单据号
            ViewBag.KisVoucherNo = BLL.Sys.EasBo.Instance.GetVoucherNo((int)出库状态.出库, order.TransactionSysNo);

            if (dealerInfo != null)
                ViewBag.DealerName = dealerInfo.DealerName;

            if (dailiInfo != null)
                ViewBag.DaiLiName = dailiInfo.UserName;

            WhStockOut master = WhWarehouseBo.Instance.Get(id);
            if (order == null)
            {
                throw new HytException("找不到出库单编号为" + id + "订单");
            }

            //是否更新出库单状态
            bool isUpdate = false;

            if (master.IsPrintedPickupCover == 0 && master.Status == WarehouseStatus.出库单状态.待拣货.GetHashCode())
            {
                isUpdate = true;
                master.IsPrintedPickupCover = 1;
                master.LastUpdateBy = CurrentUser.Base.SysNo;
                master.LastUpdateDate = DateTime.Now;
                master.Status = WarehouseStatus.出库单状态.待打包.GetHashCode();
            }

            if (order.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
            {
                #region 来源淘宝订单 打印

                //分销配货(拣货，出库)单
                var dsModel = PrintBo.Instance.GetPrintDsPicking(id);

                if (dsModel == null)
                {
                    throw new HytException("找不到编号为" + id + "出库单");
                }

                dsModel.QuantityCount = dsModel.List.Sum(item => item.ProductQuantity);
                dsModel.WeightCount = dsModel.List.Sum(item => item.Weight);
                //根据编号获得实际地区全称,可用dsModel.ListDs中地区        
                prov = BasicAreaBo.Instance.GetProvinceEntity(dsModel.AreaSysNo, out city, out area);

                ViewBag.City = city == null ? "找不到对应城市" : city.AreaName;
                ViewBag.Area = area == null ? "找不到对应地区" : area.AreaName;
                ViewBag.Prov = prov == null ? "找不到对应省" : prov.AreaName;
                //关联订单
                dsModel.SoOrder = order;

                if (isUpdate)
                {
                    WhWarehouseBo.Instance.UpdateStockOut(master);
                }
                if (string.IsNullOrEmpty(isRefresh))
                {
                    Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印分销出库单", LogStatus.系统日志目标类型.出库单, id,
                                             CurrentUser.Base.SysNo);
                }


                if (dsModel.IsSelfSupport == (int)DistributionStatus.商城是否自营.是)
                {
                    dsModel.MoneyCount = dsModel.List.Sum(item => item.RealSalesAmount);
                    return View("DsPickingSelfSupport", dsModel);
                }
                else
                {
                    return View("DsPicking2", dsModel);
                }
                #endregion
            }
            else
            {
                #region 商城打印
                //int? b = null;
                //int a = b ?? 0;
                var model = PrintBo.Instance.GetPrintPicking(id);

                if (model == null)
                {
                    throw new HytException("找不到编号为" + id + "出库单");
                }

                model.QuantityCount = model.List.Sum(item => item.ProductQuantity);
                model.MoneyCount = model.List.Sum(item => item.RealSalesAmount);

                //根据编号获得实际地区全称            
                prov = BasicAreaBo.Instance.GetProvinceEntity(model.AreaSysNo, out city, out area);

                ViewBag.City = city == null ? "" : city.AreaName;
                ViewBag.Area = area == null ? "" : area.AreaName;
                ViewBag.Prov = prov == null ? "" : prov.AreaName;


                if (isUpdate)
                {
                    WhWarehouseBo.Instance.UpdateStockOut(master);
                }
                if (string.IsNullOrEmpty(isRefresh))
                {
                    Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印商城出库单", LogStatus.系统日志目标类型.出库单, id,
                                                CurrentUser.Base.SysNo);
                }
                //关联订单
                model.SoOrder = order;

                return View("Picking2", model);

                #endregion
            }
        }
        #endregion
        /// <summary>
        /// 结算单批量打印
        /// </summary>
        /// <param name="sysNo">结算单主表系统编号</param>
        /// <returns>结算单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        public ActionResult PickingBatch(string sysNo)
        {
            ViewBag.sysnos = sysNo;
            return View();
        }

        /// <summary>
        /// 配货单（出库单、拣货单）批量打印子页
        /// </summary>
        /// <param name="id">出库单主表系统编号</param>
        /// <returns>配货单打印界面</returns>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        //public ActionResult PickingBatchSub(int? id)
        //{
        //    var model = PrintBo.Instance.GetPrintPicking(id ?? 1);

        //    if (model != null)
        //    {
        //        var quantityCount = model.List.Sum(item => item.ProductQuantity);
        //        var moneyCount = model.List.Sum(item => item.RealSalesAmount);

        //        ViewBag.quantityCount = quantityCount;
        //        ViewBag.moneyCount = moneyCount.ToString("C");
        //        //根据编号获得实际地区全称            
        //        BsArea city;
        //        BsArea area; //加粗
        //        var prov = BasicAreaBo.Instance.GetProvinceEntity(model.AreaSysNo, out city, out area);

        //        ViewBag.City = city == null ? "" : city.AreaName;
        //        ViewBag.Area = area == null ? "" : area.AreaName;
        //        ViewBag.Prov = prov == null ? "" : prov.AreaName;

        //        if (id != null)
        //        {
        //            var master = WhWarehouseBo.Instance.Get((int)id);
        //            if (master.IsPrintedPackageCover == 0)
        //            {
        //                master.IsPrintedPackageCover = 1;
        //                master.LastUpdateBy = CurrentUser.Base.SysNo;
        //                master.LastUpdateDate = DateTime.Now;
        //                master.Status = Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.待配送.GetHashCode();
        //                WhWarehouseBo.Instance.UpdateStockOut(master);
        //            }
        //        }
        //    }
        //    Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印批量出库单", LogStatus.系统日志目标类型.出库单, id ?? 0,
        //                                        CurrentUser.Base.SysNo);
        //    return View(model);
        //}
        #region 保存快递单


        /// <summary>
        /// 保存快递单
        /// </summary>
        /// <param name="sysno">出库单号</param>
        /// <param name="DsOrderSysNo">订单单号</param>
        /// <returns></returns>
        /// <remarks>2017-11-28 廖移凤</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult AddExpressList(string DsOrderSysNo, int SysNo)
        {
            return Content("0");
            ////添加到数据库
            //#region 没有参数
            //#endregion
            //KdOrderParam pa = new KdOrderParam();
            //pa = BLL.ExpressList.ExpressListBLL.Instance.GetKdOrderParam(SysNo);
            //pa.recMan = BLL.ExpressList.ExpressListBLL.Instance.GetSRecMan(SysNo);
            //pa.sendMan = BLL.ExpressList.ExpressListBLL.Instance.GetRecMan(SysNo);
            //pa.kuaidicom = "zhaijisong";
            //pa.sendMan.mobile = "13265552415";
            //pa.sendMan.printAddr = "邵阳市";
            //pa.payType = "";
            //var result = IKd100ExpressProvider.GetInstance((int)ExpressStatus.快递类型预定义.快递100).OrderTracesSubByJson(pa);
            ////var result = BLL.Express.ElectronicsSurfaceBo.Instance.OrderTracesSubByJson(pa);
            //int b1 = BLL.ExpressList.ExpressListBLL.Instance.AddKdOrderNums(result);
            //ExpressLists el = new ExpressLists()
            //{
            //    ExpressListNo = result.kdOrderNum,
            //    OrderSysNo = DsOrderSysNo,
            //    WhStockInId = SysNo
            //};
            //bool bl = BLL.ExpressList.ExpressListBLL.Instance.AddExpressList(el);
            //if (bl)
            //{
            //    return Content("1");
            //}
            //else
            //{
            //    return Content("0");
            //}
        }
        #endregion

        #region 批量保存快递单
        /// <summary>
        /// 批量保存快递单
        /// </summary>
        /// <param name="id">结算单主表系统编号</param>
        /// <remarks>
        /// 2017-12-1 廖移凤 创建
        /// </remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult BatchKuaiDi(string sysNo)
        {
            ////兼容火狐浏览器，必须重新刷新前台才能显示
            //var isRefresh = Request.Params["isRefresh"];
            //var sysnos = Array.ConvertAll(sysNo.Split(','), int.Parse).ToList();
            //IList<PrtSettlement> models = new List<PrtSettlement>();
            //List<ExpressLists> list = new List<ExpressLists>();
            //foreach (var item in sysnos)
            //{
            //    ViewData[item.ToString()] = Hyt.BLL.ExpressList.ExpressListBLL.Instance.GetKuaiDiNo(item);

            //    if (BLL.ExpressList.ExpressListBLL.Instance.GetExpressList(item))
            //    {
            //    }
            //    else
            //    {
            //        //查询接口参数
            //        KdOrderParam pa = new KdOrderParam();
            //        pa = BLL.ExpressList.ExpressListBLL.Instance.GetKdOrderParam(item);

            //        pa.recMan = BLL.ExpressList.ExpressListBLL.Instance.GetSRecMan(item);

            //        pa.sendMan = BLL.ExpressList.ExpressListBLL.Instance.GetRecMan(item);

            //        pa.cargo = pa.cargo.Length > 5 ? pa.cargo.Substring(0, 5) : pa.cargo;

            //        pa.kuaidicom = "zhaijisong";
            //        pa.sendMan.mobile = "13265552415";
            //        pa.sendMan.printAddr = "深圳市";
            //        pa.recMan.printAddr = "深圳市";

            //        pa.payType = "";
            //        pa.needTemplate = 1;

            //        var result = new Result(); IKd100ExpressProvider.GetInstance((int)ExpressStatus.快递类型预定义.快递100).OrderTracesSubByJson(pa);
            //        //var result = BLL.Express.ElectronicsSurfaceBo.Instance.OrderTracesSubByJson(pa);//对接接口

            //        BLL.ExpressList.ExpressListBLL.Instance.AddKdOrderNums(result);

            //        ExpressLists el = new ExpressLists()
            //       {
            //           ExpressListNo = result.kdOrderNum,
            //           OrderSysNo = pa.orderId.ToString(),
            //           WhStockInId = item
            //       };
            //        BLL.ExpressList.ExpressListBLL.Instance.AddExpressList(el);

            //        list.Add(el);
            //    }
            //}
            //string json = (new JavaScriptSerializer()).Serialize(list);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 第三方快递包裹单打印

        /// <summary>
        /// 第三方快递包裹单打印
        /// </summary>
        /// <param name="id">出库单主表系统编号</param>
        /// <param name="deliveryTypeSysNo">快递方式系统编号</param>
        /// <returns>第三方快递包裹单打印界面</returns>
        /// <remarks>
        /// 2013-07-12 郑荣华 创建//------------------------------------------------------------
        /// </remarks>
        [Privilege(PrivilegeCode.LG1006103, PrivilegeCode.WH1003601)]
        public ActionResult Pack(int? id, int? deliveryTypeSysNo)
        {



            //兼容火狐浏览器，必须重新刷新前台才能显示
            var isRefresh = Request.Params["isRefresh"];



            int outStockSysNo = id ?? 1000;
            var model = PrintBo.Instance.GetPrintPack(outStockSysNo);
            //根据编号获得实际地区全称
            if (model == null)
            {
                throw new HytException("找不到出库单编号为" + id + "订单");
            }



            model.FromCity = model.FromCity == null
                                 ? ""
                                 : BasicAreaBo.Instance.GetAreaFullName(int.Parse(model.FromCity));
            model.ToCity = model.ToCity == null ? "" : BasicAreaBo.Instance.GetAreaFullName(int.Parse(model.ToCity));

            model.FromAddress = "快递员您辛苦了，麻烦及时送达给我们这位尊贵的顾客，谢谢！";
            var order = BLL.Order.SoOrderBo.Instance.GetEntity(model.OrderSysNo);
            if (order != null)
            {
                var dsOrders = BLL.MallSeller.DsOrderBo.Instance.GetEntityByTransactionSysNo(order.TransactionSysNo);
                var mallOrderIds = new List<string>();
                if (dsOrders != null && dsOrders.Any())
                {
                    foreach (var ds in dsOrders)
                    {
                        if (!mallOrderIds.Contains(ds.MallOrderId))
                        {
                            mallOrderIds.Add(ds.MallOrderId);
                        }
                    }

                    //商城订单与升舱订单区分(已无合并升舱) 2014-05-15 朱家宏 修改
                    var dsOrder = dsOrders.First();
                    var dsDealerMall = BLL.MallSeller.DsOrderBo.Instance.GetDsDealerMall(dsOrder.DealerMallSysNo);
                    model.FromName = dsDealerMall.ShopName;
                    model.FromTel = dsDealerMall.ServicePhone;
                }
                else
                {
                    model.FromName = "商城";
                    model.FromTel = servicePhone;
                }
                model.DsOrderSysNo = string.Join("|", mallOrderIds);
            }

            var isUpdate = false;

            var master = WhWarehouseBo.Instance.Get(outStockSysNo);
            if (master.IsPrintedPackageCover == 0 && master.Status == WarehouseStatus.出库单状态.待打包.GetHashCode())
            {
                isUpdate = true;
                master.IsPrintedPackageCover = 1;
                master.LastUpdateBy = CurrentUser.Base.SysNo;
                master.LastUpdateDate = DateTime.Now;
                master.Status = WarehouseStatus.出库单状态.待配送.GetHashCode();
            }

            #region 模板获取

            //配送方式名称
            ViewBag.DeliveryTypeName = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo ?? 4).DeliveryTypeName;

            var modelTemplate = DeliveryPrintTemplateBo.Instance.GetDeliveryPrintTemplateList(deliveryTypeSysNo ?? 4);
            if (modelTemplate != null && modelTemplate.Count > 0)
            {
                ViewBag.Template = modelTemplate[0].Template; //模板代码
            }
            else
            {
                ViewBag.Template = ""; //无模板则调用初始化模板
            }
            //模板列表
            ViewBag.modelTemplate = modelTemplate;

            #endregion


            if (isUpdate)
            {
                WhWarehouseBo.Instance.UpdateStockOut(master);
                Hyt.BLL.Order.SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo,
                                                                    "订单拣货打包完毕，待分配配送",
                                                                    CurrentUser.Base.UserName);




            }
            if (string.IsNullOrEmpty(isRefresh))
            {
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "打印包裹单", LogStatus.系统日志目标类型.出库单, outStockSysNo,
                                        CurrentUser.Base.SysNo);
            }


            return View(model);
        }

        #endregion

        #region 快递打印模板维护 2013-07-22 郑荣华

        /// <summary>
        /// 快递打印模板维护
        /// </summary>
        /// <param name="deliveryTypeSysNo">快递方式编号</param>
        /// <returns>快递打印模板界面</returns>
        /// <remarks>
        /// 2013-07-22 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001901)]
        public ActionResult DeliveryPrintTemplate(int? deliveryTypeSysNo)
        {
            var model = DeliveryPrintTemplateBo.Instance.GetDeliveryPrintTemplateList(deliveryTypeSysNo ?? 1);
            if (model != null && model.Count > 0)
            {
                if (model[0].Template != null)
                {
                    model[0].Template = TemplateReplace(model[0].Template, model[0].Name, false);
                }
                else
                {
                    model[0].Template = "";
                }
            }
            ViewBag.DeliveryTypeName = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo ?? 1).DeliveryTypeName;
            ViewBag.DeliveryTypeSysNo = deliveryTypeSysNo ?? 1;

            return View(model);
        }

        /// <summary>
        /// 获取单个打印模板信息
        /// </summary>
        /// <param name="templateSysNo">模板系统编号</param>
        /// <param name="replace">是否替换成标准字符串，默认true</param>
        /// <returns>单个打印模板信息</returns>
        /// <remarks>
        /// 2013-07-23 郑荣华 创建
        /// 2014-07-23 增加修改打印模板权限 余勇
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001901, PrivilegeCode.LG1001902)]
        public JsonResult GetDeliveryPrintTemplate(int templateSysNo, bool replace = true)
        {
            var model = DeliveryPrintTemplateBo.Instance.GetDeliveryPrintTemplate(templateSysNo);
            if (model != null && replace)
            {
                model.Template = TemplateReplace(model.Template, model.Name, false);

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax新增快递打印模板
        /// </summary>
        /// <param name=""></param>
        /// <returns>
        /// 成功返回true,失败返回false
        /// </returns>
        /// <remarks> 
        /// 2013-07-23 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001901)]
        public JsonResult DeliveryPrintTemplateCreate()
        {
            var model = new LgDeliveryPrintTemplate();
            model.BackgroundImage = Request.Form["BackGroundImage"];
            model.DeliveryTypeSysNo = int.Parse(Request.Form["DeliveryTypeSysNo"]);
            model.Name = Request.Form["templateName"];
            model.Status = int.Parse(Request.Form["status"]);
            model.Template = TemplateReplace(Request.Unvalidated().Form["txtcode"], model.Name);
            //model.Template = TemplateReplace(Server.UrlDecode(Request.Form["txtcode"]), model.Name);

            model.CreatedBy = BaseController.CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = BaseController.CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            return Json(DeliveryPrintTemplateBo.Instance.CreateDeliveryPrintTemplate(model));
        }

        /// <summary>
        /// Ajax修改快递打印模板
        /// </summary>
        /// <param name=""></param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-07-23 郑荣华 创建
        /// </remarks>  
        [Privilege(PrivilegeCode.LG1001901)]
        public ActionResult DeliveryPrintTemplateUpdate()
        {

            var model = new LgDeliveryPrintTemplate();
            model.BackgroundImage = Request.Form["BackGroundImage"];
            model.DeliveryTypeSysNo = int.Parse(Request.Form["DeliveryTypeSysNo"]);
            model.Name = Request.Form["templateName"];
            model.Status = int.Parse(Request.Form["status"]);
            model.Template = TemplateReplace(Request.Unvalidated().Form["txtcode"], model.Name);
            model.SysNo = int.Parse(Request.Form["TemplateSysNo"]);

            model.LastUpdateBy = BaseController.CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            return Json(DeliveryPrintTemplateBo.Instance.UpdateDeliveryPrintTemplate(model));
        }

        /// <summary>
        /// 替换成标准字符串
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="valueToKey">正向替换</param>
        /// <returns>替换后的字符串</returns>
        /// <remarks> 
        /// 2013-07-23 郑荣华 创建//-----------------------------------------------------
        /// </remarks>
        public string TemplateReplace(string code, string name, bool valueToKey = true)
        {
            //var printTime = '@DateTime.Now.ToString("yyyy-MM-dd")';
            //var electronicName = '电子产品';
            var dic = new Dictionary<string, string>();
            dic.Add(name, "初始源码");
            dic.Add("fromName", "\"寄件人姓名\"");
            dic.Add("fromTel", "\"寄件人电话\"");
            dic.Add("fromAddress", "\"寄件人地址\"");
            dic.Add("fromCity", "\"寄件人省市区\"");

            dic.Add("toName", "\"收件人姓名\"");
            dic.Add("toTel", "\"收件人固定电话\"");
            dic.Add("toAddress", "\"收件人地址\"");
            dic.Add("toCity", "\"收件人省市区\"");
            dic.Add("toMobile", "\"收件人手机号码\"");
            dic.Add("stockOutSysNo", "\"出库单系统编号\"");
            dic.Add("printTime", "\"发货时间\"");
            dic.Add("electronicName", "\"电子产品\"");
            dic.Add("orderSysNo", "\"订单编号\"");
            dic.Add("dsOrderSysNo", "\"升舱订单号\"");

            if (valueToKey)
                return dic.Aggregate(code, (current, item) => current.Replace(item.Value, item.Key));
            return dic.Aggregate(code, (current, item) => current.Replace(item.Key, item.Value));
        }

        /// <summary>
        /// 检查模板名称重复,新建时设置成sysNo=0
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>可添加返回true,否则返回false</returns>
        /// <remarks>
        /// 2013-11-30 郑荣华 创建 
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001901)]
        public JsonResult TemplateNameCheck(string templateName, int sysNo = 0)
        {
            var list = DeliveryPrintTemplateBo.Instance.GetDeliveryPrintTemplateList(templateName, sysNo);
            return Json(list.Count <= 0);
        }
        #endregion

        /// <summary>
        /// 门店下单打印小票
        /// </summary>
        /// <param name="whStockOutSysNo">出库单ID</param>
        /// <returns>出库单及明细</returns>
        /// <returns>
        /// 2013-07-09 黄志勇 创建
        /// 2013-09-17 朱家宏 修改:增加分销商订单相关信息
        /// </returns>
        [Privilege(PrivilegeCode.SO1005801)]
        public ActionResult ShopInvoice(int whStockOutSysNo)
        {
            WhStockOut model = null;
            var stockOut = BLL.Warehouse.WhWarehouseBo.Instance.Get(whStockOutSysNo);
            if (stockOut != null)
            {
                //订单
                var order = BLL.Order.SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
                ViewBag.OrderSysNo = order.SysNo;
                //门店
                var warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(order.DefaultWarehouseSysNo);
                ViewBag.WarehouseName = warehouse != null ? warehouse.WarehouseName : "未知";

                //收货地址
                ViewBag.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

                //付款方式
                ViewBag.PayType = BLL.Basic.PaymentTypeBo.Instance.GetEntity(order.PayTypeSysNo);
                //提货时间
                ViewBag.SignTime = stockOut.SignTime.ToString("yyyy-MM-dd HH:mm");
                //商品
                model = WhWarehouseBo.Instance.Get(whStockOutSysNo);
                ViewBag.WhStockOutItem = model;
                ViewBag.Total = model.Items.Sum(i => i.RealSalesAmount);

                //分销商订单
                var dsOrders = BLL.MallSeller.DsOrderBo.Instance.GetEntityByTransactionSysNo(order.TransactionSysNo);
                if (dsOrders.Any())
                {
                    var dsOrder = dsOrders.First();
                    var mall = BLL.MallSeller.DsOrderBo.Instance.GetDsDealerMall(dsOrder.DealerMallSysNo);
                    ViewBag.mallOrderId = dsOrder.MallOrderId;
                    ViewBag.mallShopName = mall.ShopName;
                }
                ViewBag.CouponAmount = order.CouponAmount;//优惠券金额
                var ordercoupon = Hyt.BLL.Order.SoOrderBo.Instance.GetCouponByOrderSysNo(order.SysNo).FirstOrDefault();
                if (ordercoupon != null)
                {
                    var coupon = PromotionBo.Instance.GetCoupon(ordercoupon.CouponCode);
                    if (coupon != null)
                    {
                        ViewBag.CouponDes = coupon.Description;//优惠券内容
                    }
                }
                ViewBag.DiscountAmount = order.ProductDiscountAmount + order.OrderDiscountAmount;//折扣金额
                ViewBag.DeliveryRemarks = order.DeliveryRemarks;//配送备注
                ViewBag.Remarks = order.InternalRemarks;//对内备注
                ViewBag.CoinPay = order.CoinPay;    //会员币支付

            }
            return View("ShopInvoice", model);
        }

        /// <summary>
        /// 获取销售单商品类型
        /// </summary>
        /// <param name="soItemSysNo">订单明细编号</param>
        /// <param name="realSalesAmount">实际销售金额</param>
        /// <returns>类型描述</returns>
        /// <remarks>2013-12-31 朱家宏 创建</remarks>
        public static string GetProductSalesType(int soItemSysNo, decimal realSalesAmount)
        {
            var tag = " ";
            var soItem = BLL.Order.SoOrderBo.Instance.GetOrderItem(soItemSysNo);
            if (soItem != null && soItem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
            {
                if (realSalesAmount > 0)
                {
                    tag = " 【加购】 ";
                }
                else
                {
                    tag = " 【赠品】 ";
                }
            }
            return tag;
        }

        /// <summary>
        /// 打印门店退货单
        /// </summary>
        /// <param name="rmaSysNo">rma编号</param>
        /// <param name="reqSource">来路</param>
        /// <returns>退货单打印视图</returns>
        /// <remarks>2013-07-15 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RC1004801)]
        public ActionResult BrowseReturnedBill(int rmaSysNo, string reqSource)
        {
            var rma = BLL.RMA.RmaBo.Instance.GetRMA(rmaSysNo);

            //总金额
            //实退商品金额总和
            decimal amount = 0;
            if (rma.RMAItems != null)
            {
                amount = rma.RMAItems.Sum(o => o.RefundProductAmount);
                ViewBag.ItemsCount = rma.RMAItems.Count();
            }

            ViewBag.RefundTotal = Util.FormatUtil.FormatCurrency(-amount, 2, "￥");


            //门店（入库仓库）
            var warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(rma.WarehouseSysNo);
            ViewBag.WarehouseName = warehouse != null ? warehouse.WarehouseName : "未知";

            //收货信息
            var soOrder = BLL.Order.SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
            var receiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);
            ViewBag.ReceiveAddress = receiveAddress.StreetAddress;
            ViewBag.ReceiveMobile = receiveAddress.MobilePhoneNumber;
            ViewBag.ReceiveName = receiveAddress.Name;

            //订单来源
            ViewBag.OrderSource = EnumUtil.GetDescription(typeof(OrderStatus.销售单来源), soOrder.OrderSource);
            //支付方式
            ViewBag.PaymentName = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo).PaymentName;

            //退款方式
            ViewBag.RefundType = Util.EnumUtil.GetDescription(typeof(RmaStatus.退换货退款方式), rma.RefundType);

            //分销商订单
            var dsOrders = BLL.MallSeller.DsOrderBo.Instance.GetEntityByTransactionSysNo(soOrder.TransactionSysNo);
            if (dsOrders.Any())
            {
                var dsOrder = dsOrders.First();
                var mall = BLL.MallSeller.DsOrderBo.Instance.GetDsDealerMall(dsOrder.DealerMallSysNo);
                ViewBag.mallOrderId = dsOrder.MallOrderId;
                ViewBag.mallShopName = mall.ShopName;
            }

            //客户姓名
            var custome = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(soOrder.CustomerSysNo);
            if (custome != null)
            {
                ViewBag.CustomerName = custome.Name;
                ViewBag.CustomerMobilePhoneNumber = custome.MobilePhoneNumber;
            }

            var view = "RmaReturned";
            if (reqSource == "shop")
            {
                view = "ShopRmaReturned";
            }

            return View(view, rma);
        }
        #region 批量打印
        /// <summary>
        /// 多页打印演示
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-07-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.LG1006103, PrivilegeCode.WH1003601)]
        public ActionResult MultiplePageDemo()
        {
            return View();
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        /// <param name="ids">出库单编号</param>
        /// <param name="type">类型 1- 打印拣货单 2-打印包裹单</param>
        /// <param name="tsysno">对应的快递模版编号</param>
        /// <param name="deliveryType">快递</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        public ActionResult PrintMultiOutStock(string ids, int type, string deliveryType, int tsysno)
        {
            List<int> lstids = ids.Split(',').Select(m => int.Parse(m)).ToList();
            ViewBag.ids = lstids;
            ViewBag.DeliveryType = deliveryType;//配送方式名称,为空将显示默认的配送方式名称
            if (type == 1)//打印拣货单
            {
                var lst = Hyt.BLL.Print.PrintBo.Instance.GetPrintPickingList(lstids);//非升舱订单
                foreach (var item in lst)
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetByOutStockSysNo(item.SysNo);
                    item.SoOrder = order;
                    //ViewBag.KisVoucherNo = BLL.Sys.EasBo.Instance.GetVoucherNo((int)出库状态.出库, order.TransactionSysNo);
                }
                var DsPickingList = Hyt.BLL.Print.PrintBo.Instance.GetPrintDsPickingList(lstids);//升舱订单相关;
                foreach (var item in DsPickingList)
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetByOutStockSysNo(item.SysNo);
                    item.SoOrder = order;
                }
                ViewBag.DsPickingList = DsPickingList;//升舱订单相关;
                ViewBag.DeliveryType = deliveryType;
                //kis单据号
                ViewBag.DealerName = "";
                ViewBag.DaiLiName = "";
                return View("MultiPicking", lst);
            }
            else if (type == 2)//打印包裹单
            {
                var modelTemplateList = DeliveryPrintTemplateBo.Instance.GetDeliveryPrintTemplateList(tsysno);
                ViewBag.TemplateList = modelTemplateList;
                return View("MultiPack", Hyt.BLL.Print.PrintBo.Instance.GetPrintPackList(lstids));
            }
            return Content("");
        }
        #endregion
    }
}
