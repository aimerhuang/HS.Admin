using Extra.UpGrade;
using Hyt.BLL.Procurement;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Procurement;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Util.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    public class ProcurementController : BaseController
    {
        //
        // GET: /Procurement/      
        public ActionResult Index()
        {
            return View();
        }
        #region 厂家采购基本数据定义
        /// <summary>
        /// 基础数据列表定义
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult BaseProcumentPageList()
        {
            ViewBag.LogisticsCompanyList = PmBaseDataBo.Instance.GetPmLogisticsCompanyList();
            ViewBag.ManufacturerList = PmBaseDataBo.Instance.GetPmManufacturert();
            ViewBag.ContainerList = PmBaseDataBo.Instance.GetPmContainerList();
            return View();
        }

        #region 集装箱基础数据
        /// <summary>
        /// 添加或者更新集装箱数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult InsertOrUpdateContainer(int? sysNo)
        {
            ViewBag.Container = new PmContainer();
            if (sysNo != null)
            {
                PmContainer container = PmBaseDataBo.Instance.GetPmContainerBySysNo(sysNo.Value);
                ViewBag.Container = container;
            }

            return View();
        }
        /// <summary>
        /// 保存集装箱基础数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SaveOrUpdateContainer(PmContainer mod)
        {
            try
            {
                if (mod.SysNo == 0)
                {
                    PmBaseDataBo.Instance.InsertPmContainer(mod);
                    return Json(new { Status = true, Msg = "保存成功" });
                }
                else
                {
                    PmBaseDataBo.Instance.UpdatePmContainer(mod);
                    return Json(new { Status = true, Msg = "修改成功" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = true, Msg = e.Message });
            }
        }
        #endregion

        #region 生产厂家数据类型
        /// <summary>
        /// 添加和修改生产厂家的基础数据
        /// </summary>
        /// <param name="sysNo">系统编号，可以为空</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult InnerOrUpdateManufacturer(int? sysNo, string type)
        {
            ViewBag.Manufacturer = new PmManufacturer();
            ViewBag.DiaInfo = "";
            IList<PdCategory> pdCategoryList = Hyt.BLL.Product.PdCategoryBo.Instance.GetAllCategory().Where(p => p.Status == 1 && p.ParentSysNo == 0).ToList();
            ViewBag.PdCategoryList = pdCategoryList;
            if (sysNo != null)
            {
                PmManufacturer manufacturer = PmBaseDataBo.Instance.GetPmManufacturerBySysNo(sysNo.Value);
                ViewBag.Manufacturer = manufacturer;
            }
            if (!string.IsNullOrEmpty(type))
            {
                ViewBag.DiaInfo = "HideReturnButton();";
            }
            return View();
        }
        /// <summary>
        /// 保存生产厂家的基础数据
        /// </summary>
        /// <param name="mod">数据模型</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SaveOrUpdateManufacturer(PmManufacturer mod)
        {
            try
            {
                if (mod.SysNo == 0)
                {
                    PmBaseDataBo.Instance.InsertPmManufacturer(mod);
                    return Json(new { Status = true, Msg = "保存成功" });
                }
                else
                {
                    PmBaseDataBo.Instance.UpdatePmManufacturer(mod);
                    return Json(new { Status = true, Msg = "修改成功" });
                }

            }
            catch (Exception e)
            {
                return Json(new { Status = true, Msg = e.Message });
            }
        }

        [Privilege(PrivilegeCode.None)]
        public JsonResult GetCategoryList(int SysNo)
        {
            IList<PdCategory> pdCategoryList = Hyt.BLL.Product.PdCategoryBo.Instance.GetAllCategory().Where(p => p.Status == 1 && p.ParentSysNo == SysNo).ToList();
            return Json(pdCategoryList);
        }
        #endregion

        #region 物流公司基础数据
        /// <summary>
        /// 添加物流公司信息
        /// </summary>
        /// <param name="sysNo">系统编号，可以为空</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult InnerOrUpdateLogisticsCompany(int? sysNo)
        {
            ViewBag.LogisticsCompany = new PmLogisticsCompany();
            if (sysNo != null)
            {
                PmLogisticsCompany logisticsCompany = PmBaseDataBo.Instance.GetPmLogisticsCompanyBySysNo(sysNo.Value);
                ViewBag.LogisticsCompany = logisticsCompany;
            }

            return View();
        }
        /// <summary>
        /// 保存物流公司信息
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SaveOrUpdateLogisticsCompany(PmLogisticsCompany mod)
        {
            try
            {
                if (mod.SysNo == 0)
                {
                    PmBaseDataBo.Instance.InsertPmLogisticsCompany(mod);
                    return Json(new { Status = true, Msg = "保存成功" });
                }
                else
                {
                    PmBaseDataBo.Instance.UpdatePmLogisticsCompany(mod);
                    return Json(new { Status = true, Msg = "修改成功" });
                }

            }
            catch (Exception e)
            {
                return Json(new { Status = true, Msg = e.Message });
            }
        }
        #endregion
        #endregion

        #region 创建采购单

        /// <summary>
        /// 获取采购申请单列表
        /// </summary>
        /// <param name="id">分页id</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100101)]
        public ActionResult GetProcurementPager(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                Pager<CBPmProcurementOrder> pager = new Pager<CBPmProcurementOrder>();
                pager.CurrentPage = id ?? 1;
                pager.PageSize = 10;
                PmProcurementBo.Instance.GetPmProcurementOrderPager(ref pager);
                var model = new PagedList<CBPmProcurementOrder>();
                model.TotalItemCount = pager.TotalRows;
                model.TData = pager.Rows;
                model.CurrentPageIndex = pager.CurrentPage;
                return PartialView("_AjaxGetProcurementPager", model);
            }
            return View();
        }

        /// <summary>
        /// 创建和修改采购单
        /// </summary>
        /// <param name="SysNo">系统编号 可以为空</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100102)]
        public ActionResult CreateOrUpdateProcuremen(int? SysNo)
        {
            CBPmProcurementOrder order = new CBPmProcurementOrder();
            if (SysNo != null)
            {
                order = PmProcurementBo.Instance.GetCBPmProcurementOrder(SysNo.Value);
            }
            ViewBag.Order = order;
            ViewBag.WabTypeList = PmProcurementBo.Instance.GetProcurementWebTypeList();
            if (order.SysNo == 0)
            {
                order.Po_Number = "OP" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                ViewBag.CurrentName = CurrentUser.Base.UserName;
                ViewBag.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.CurrentName = order.CreateName;
                ViewBag.CurrentTime = order.Po_CreateTime;
            }
            return View();
        }
        /// <summary>
        /// 修改采购单状态
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="Status">状态数据</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100103)]
        public JsonResult UpdataOrderStatus(int SysNo, int Status)
        {
            PmProcurementBo.Instance.ProcurmentOrderStatus(SysNo, Status, CurrentUser.Base.SysNo);
            return Json(new { Status = true, Msg = "更新成功！！" });
        }
        /// <summary>
        /// 保存采购单数据
        /// </summary>
        /// <param name="order">订单数据</param>
        /// <param name="orderItems">订单商品集合</param>
        /// <param name="orderPrices">网路销售价格列表</param>
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100102)]
        public JsonResult SaveProcuremen(CBPmProcurementOrder order, string orderItemList, string webPriceList)
        {
            try
            {
                order.orderItemList = JsonUtil.ToObject<List<CBPmProcurementOrderItem>>(orderItemList);
                order.webPriceList = JsonUtil.ToObject<List<CBPmProcurementWebPrice>>(webPriceList);
                using (var trans = new TransactionScope())
                {
                    order.Po_CreateBy = CurrentUser.Base.SysNo;
                    order.Po_CreateTime = DateTime.Now;
                    order.Po_UpdateBy = 0;
                    order.Po_UpdateTime = DateTime.Now;
                    PmProcurementBo.Instance.CreateOrUpdatePmProcurementOrder(order);
                    trans.Complete();
                    return Json(new { Status = true, Msg = "", SysNo = order.SysNo });
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Msg = e.Message });
            }

        }


        #endregion
        #region 采购分货单


        /// <summary>
        /// 获取采购申请单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100201)]
        public ActionResult GetPointsOrderPager(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                //Pager<CBPmPointsOrder> pager = new Pager<CBPmPointsOrder>();
                //pager.CurrentPage = id ?? 1;
                //pager.PageSize = 10;
                //PmProcurementBo.Instance.GetPmPointsOrderPager(ref pager);
                //var model = new PagedList<CBPmPointsOrder>();
                //model.TotalItemCount = pager.TotalRows;
                //model.TData = pager.Rows;
                //model.CurrentPageIndex = pager.CurrentPage;
                Pager<CBPmProcurementOrder> pager = new Pager<CBPmProcurementOrder>();
                pager.CurrentPage = id ?? 1;
                pager.PageSize = 3;
                PmProcurementBo.Instance.GetPmProcurementOrderPager(ref pager);
                var model = new PagedList<CBPmProcurementOrder>();
                model.TotalItemCount = pager.TotalRows;
                model.TData = pager.Rows;
                model.CurrentPageIndex = pager.CurrentPage;
                string pSysNoList = "";
                foreach (var item in pager.Rows)
                {
                    if (!string.IsNullOrEmpty(pSysNoList))
                    {
                        pSysNoList += ",";
                    }
                    pSysNoList += item.SysNo;
                }

                ViewBag.OrderList = PmProcurementBo.Instance.GetPointsOrderListByPSysNo(pSysNoList);
                pSysNoList = "";
                foreach (var mod in ViewBag.OrderList as List<CBPmPointsOrder>)
                {
                    if (!string.IsNullOrEmpty(pSysNoList))
                    {
                        pSysNoList += ",";
                    }
                    pSysNoList += mod.SysNo;
                }
                ViewBag.DeliveryOrderList = PmProcurementBo.Instance.GetDeliveryListByPSysNo(pSysNoList);

                return PartialView("_AjaxGetPmPointsOrderPager", model);
            }
            return View();
        }
        /// <summary>
        /// 修改分货单状态信息
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <param name="Status">状态编号</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.CG100203)]
        public JsonResult UpdataPointsOrderStatus(int SysNo, int Status)
        {
            PmProcurementBo.Instance.UpdatePointOrderStatus(SysNo, Status);
            return Json(new { Status = true, Msg = "" });
        }

        /// <summary>
        /// 创建分货单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100202)]
        public ActionResult CreateUpdatePointsOrder(int? SysNo, int? PSysNo)
        {
            CBPmPointsOrder order = new CBPmPointsOrder(); ;
            if (SysNo != null)
            {

                order = PmProcurementBo.Instance.GetCBPmPointsOrder(SysNo.Value);
                if (order != null)
                {
                    order.listItems = PmProcurementBo.Instance.GetCBPmPointsOrderItems(SysNo.Value);
                    if (order.listItems == null)
                    {
                        order.listItems = new List<CBPmPointsOrderItem>();
                    }

                }
                else
                {
                    order = new CBPmPointsOrder();
                }

                ViewBag.CurrentName = order.CreateName;
                ViewBag.CurrentTime = order.Po_CreateTime.ToString("yyyy-MM-dd");
            }
            else if (PSysNo != null)
            {
                order = new CBPmPointsOrder();
                order.Po_Number = "CG" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                order.Po_ProcurementSysNo = PSysNo.Value;
                order.listItems = new List<CBPmPointsOrderItem>();
                ViewBag.CurrentName = CurrentUser.Base.UserName;
                ViewBag.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                return View("ErrorPrivilegeWithMessage", "采购单编号有误请核实数据。");
            }
            ViewBag.Order = order;

            return View();
        }

        /// <summary>
        /// 获取生产厂家的数据
        /// </summary>
        /// <returns></returns>
        ///  <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100202)]
        public JsonResult GetPmManufacturertList()
        {
            List<PmManufacturer> list = PmBaseDataBo.Instance.GetPmManufacturert();
            foreach (PmManufacturer mod in list)
            {
                mod.FName = mod.FName.Replace("\"", "").Replace("\'", "");
            }
            return Json(list);
        }
        /// <summary>
        /// 获取国际物流公司的数据
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.CG100202)]
        public JsonResult GetLogisticsCompanyList()
        {
            List<PmLogisticsCompany> list = PmBaseDataBo.Instance.GetPmLogisticsCompanyList();
            foreach (PmLogisticsCompany mod in list)
            {
                mod.LCName = mod.LCName.Replace("\"", "").Replace("\'", "");
            }
            return Json(list);
        }
        /// <summary>
        /// 获取集装箱类型的数据
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.CG100202)]
        public JsonResult GetContainerList()
        {
            List<PmContainer> list = PmBaseDataBo.Instance.GetPmContainerList();
            foreach (PmContainer mod in list)
            {
                mod.CName = mod.CName.Replace("\"", "").Replace("\'", "");
            }
            return Json(list);
        }
        /// <summary>
        /// 保存分货单数据
        /// </summary>
        /// <param name="SysNo">编号</param>
        /// <param name="Po_ProcurementSysNo">采购但编号</param>
        /// <param name="Po_Number">采购单号</param>
        /// <param name="Po_FactoryName">生产厂家</param>
        /// <param name="strProList">采购商品数据</param>
        /// <returns></returns>
        ///  <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100202)]
        public JsonResult SavePointsOrderData(
            int SysNo,
            int Po_ProcurementSysNo,
            string Po_Number,
            string Po_FactoryName,
            string strProList)
        {
            CBPmPointsOrder order = new CBPmPointsOrder();
            order.SysNo = SysNo;
            order.Po_ProcurementSysNo = Po_ProcurementSysNo;
            order.Po_Number = Po_Number;
            order.Po_FactoryName = Po_FactoryName;
            order.Po_CreateSysNo = CurrentUser.Base.SysNo;
            order.Po_CreateTime = DateTime.Now;
            order.Po_UpdateSysNo = 0;
            order.Po_UpdateTime = DateTime.Now;

            List<CBPmPointsOrderItem> items = PmProcurementBo.Instance.GetCBPmPointsOrderItems(SysNo);

            string[] proList = strProList.Split(',');
            for (int i = 0; i < proList.Length; i++)
            {
                CBPmPointsOrderItem item = new CBPmPointsOrderItem();
                item.Poi_ProcurementItemSysNo = Convert.ToInt32(proList[i]);
                CBPmPointsOrderItem haveItem = items.Find(p => p.Poi_ProcurementItemSysNo == item.Poi_ProcurementItemSysNo);
                if (haveItem != null)
                {
                    item.SysNo = haveItem.SysNo;
                    items.Remove(haveItem);
                }
                order.listItems.Add(item);
            }
            PmProcurementBo.Instance.CreateOrUpdatePmPointsOrder(order);
            if (items.Count > 0)
            {
                ///删除数据
                string delSysNos = "";
                foreach (var item in items)
                {
                    if (!string.IsNullOrEmpty(delSysNos))
                    {
                        delSysNos += ",";
                    }
                    delSysNos += item.Poi_ProcurementItemSysNo;
                }
                PmProcurementBo.Instance.DeletePointsOrderData(delSysNos);
            }

            var cbOrder = PmProcurementBo.Instance.GetCBPmProcurementOrder(order.Po_ProcurementSysNo);
            bool b = true;
            foreach (var item in cbOrder.orderItemList)
            {
                if (item.Poi_Status == 0)
                {
                    b = false;
                    break;
                }
            }
            if (b)
            {
                PmProcurementBo.Instance.ProcurmentOrderStatus(cbOrder.SysNo, 2, CurrentUser.Base.SysNo);
            }
            else
            {
                PmProcurementBo.Instance.ProcurmentOrderStatus(cbOrder.SysNo, 1, CurrentUser.Base.SysNo);
            }
            return Json(new { Status = true, Msg = "" });
        }

        #endregion
        #region 采购分货单配送

        /// <summary>
        /// 创建产品配送分货但
        /// </summary>
        /// <param name="SysNo">系统编号，可以为空</param>
        /// <param name="pSysNo">厂家商品生产但系统编号</param>
        /// <returns></returns>
        ///  <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100302)]
        public ActionResult CreateUpdateProcurementPackOrder(int? SysNo, int? pSysNo)
        {
            CBPmGoodsDelivery mod = new CBPmGoodsDelivery();
            ViewBag.Order = mod;

            if (SysNo != null)
            {
                mod = PmProcurementBo.Instance.GetCBPmGoodsDeliveryBySysNo(SysNo.Value);
                mod.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
                mod.CurrentName = CurrentUser.Base.UserName;

                ViewBag.Order = mod;
            }
            else if (pSysNo != null)
            {
                mod = PmProcurementBo.Instance.GetCBPmGoodsDeliveryByPSysNo(pSysNo.Value);
                mod.gd_PaketNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
                mod.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
                mod.gd_PSysNo = pSysNo.Value;
                mod.CurrentName = CurrentUser.Base.UserName;
                ViewBag.Order = mod;
            }
            else if (SysNo == null && pSysNo == null)
            {
                mod.ListItems = new List<CBPmGoodsDeliveryItem>();
                mod.gd_PaketNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
                mod.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
                mod.gd_PSysNo = 0;
                mod.CurrentName = CurrentUser.Base.UserName;
                ViewBag.Order = mod;
            }
            return View();
        }

        /// <summary>
        /// 保存商品配送分货单数据
        /// </summary>
        /// <param name="pSysNo">父编号SysNo</param>
        /// <param name="companyName">物流公司名称</param>
        /// <param name="containerName">集装箱类型名称</param>
        /// <param name="deliveryNumber">物流编号</param>
        /// <param name="packNumber">包裹编号</param>
        /// <param name="proList">包裹商品名称</param>
        /// <param name="gdType">配送单类型</param> 
        /// <returns></returns>
        /// <remarks>2016-2-15 杨云奕 添加</remarks>
        [Privilege(PrivilegeCode.CG100302, PrivilegeCode.CG100402)]
        public JsonResult SaveDeliveryPackOrderData(int pSysNo, string companyName, string containerName, string deliveryNumber, string packNumber, string proList, int gdType = 1)
        {
            using (var trans = new TransactionScope())
            {
                ///产品配送单
                CBPmGoodsDelivery deliveryMod = new CBPmGoodsDelivery();
                deliveryMod.gd_DeliveryCompanyName = companyName;
                deliveryMod.gd_DeliveryContainer = containerName;
                deliveryMod.gd_DeliveryNumber = deliveryNumber;
                deliveryMod.gd_PSysNo = pSysNo;
                deliveryMod.gd_DeliveryTime = DateTime.Now;
                deliveryMod.gd_DeliveryUserSys = CurrentUser.Base.SysNo;
                deliveryMod.gd_PaketNumber = packNumber;
                deliveryMod.gd_Type = gdType;
                string[] PList = proList.Split(',');
                deliveryMod.gd_GetDeliveryTime = DateTime.Now;
                ///产品配送单商品明细
                deliveryMod.ListItems = new List<CBPmGoodsDeliveryItem>();
                foreach (string mod in PList)
                {
                    string[] dataList = mod.Split('_');
                    CBPmGoodsDeliveryItem item = new CBPmGoodsDeliveryItem();
                    item.gdi_PointItemSysNo = Convert.ToInt32(dataList[0]);
                    item.gdi_GoodSysNo = Convert.ToInt32(dataList[1]);
                    item.gdi_SendQuity = Convert.ToInt32(dataList[2]);
                    if (item.gdi_SendQuity > 0)
                    {
                        deliveryMod.ListItems.Add(item);
                    }
                }
                PmProcurementBo.Instance.CreateOrUpdateDeliveryOrder(deliveryMod);
                ///厂家生产配送回库
                if (gdType == 1)
                {
                    ///保存采购仓商品控制
                    SavePmWareGoodsList(pSysNo, Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态.产品配送);

                    CBPmGoodsDelivery tempMod = PmProcurementBo.Instance.GetCBPmGoodsDeliveryByPSysNo(deliveryMod.gd_PSysNo);
                    bool b = true;
                    foreach (var dataMod in tempMod.ListItems)
                    {
                        if (dataMod.gdi_SendQuity < dataMod.Poi_ProQuity)
                        {
                            b = false;
                            break;
                        }
                    }

                    if (b)
                    {
                        PmProcurementBo.Instance.UpdatePointOrderStatus(pSysNo, (int)Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态.配送完成在途运输);
                    }
                    else
                    {
                        PmProcurementBo.Instance.UpdatePointOrderStatus(pSysNo, (int)Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态.产品配送);
                    }
                }
                ///库存配送到国内
                else if (gdType == 2)
                {
                    SavePmWareGoodsListToSend(deliveryMod.SysNo, Hyt.Model.WorkflowStatus.ProcurementStatus.商品配送状态.配送中);
                }

                trans.Complete();
            }
            return Json(new { Status = true, Msg = "" });

        }

        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.CG100304, PrivilegeCode.CG100402)]
        public JsonResult GetDeliveryInfoData(int SysNo)
        {
            PmGoodsDelivery mod = PmProcurementBo.Instance.GetDeliveryBySysNo(SysNo);
            return Json(mod);
        }
        [Privilege(PrivilegeCode.CG100304, PrivilegeCode.CG100402)]
        public JsonResult SaveDeliveryInfoData(int SysNo, string txt)
        {
            PmGoodsDelivery mod = PmProcurementBo.Instance.GetDeliveryBySysNo(SysNo);
            mod.gd_Status = (int)Hyt.Model.WorkflowStatus.ProcurementStatus.商品配送状态.配送中;
            if (mod.gd_DeliveryInfo != "")
            {
                mod.gd_DeliveryInfo += "<br/>";
            }
            mod.gd_DeliveryInfo += txt;
            PmProcurementBo.Instance.UpdateDeliveryOrder(mod);
            return Json(new { Status = true });
        }

        /// <summary>
        /// 确认收货权限
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.CG100305, PrivilegeCode.CG100402)]
        public JsonResult SetDeliveryCompleteStatus(int SysNo)
        {
            PmGoodsDelivery mod = PmProcurementBo.Instance.GetDeliveryBySysNo(SysNo);
            mod.gd_Status = (int)Hyt.Model.WorkflowStatus.ProcurementStatus.商品配送状态.配送完成;
            mod.gd_DeliveryInfo += "<br/>" + DateTime.Now.ToString("yyyy-MM-dd") + " 配送完成";
            PmProcurementBo.Instance.UpdateDeliveryOrder(mod);

            List<PmGoodsDelivery> deliveryList = PmProcurementBo.Instance.GetDeliveryListByPSysNo(mod.gd_PSysNo.ToString());
            bool b = false;
            foreach (PmGoodsDelivery dlmod in deliveryList)
            {
                if (dlmod.gd_Status != (int)Hyt.Model.WorkflowStatus.ProcurementStatus.商品配送状态.配送完成)
                {
                    b = true;
                    break;
                }
            }
            if (!b)
            {
                if (mod.gd_Type == 1)
                {
                    CBPmPointsOrder pointOrder = PmProcurementBo.Instance.GetCBPmPointsOrder(mod.gd_PSysNo);
                    if (pointOrder.Po_Status == (int)Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态.配送完成在途运输)
                    {
                        PmProcurementBo.Instance.UpdatePointOrderStatus(mod.gd_PSysNo, (int)Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态.完成);
                        ///保存采购仓商品控制
                        SavePmWareGoodsList(mod.gd_PSysNo, Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态.完成);
                    }
                }
                else if (mod.gd_Type == 2)
                {
                    SavePmWareGoodsListToSend(mod.SysNo, Hyt.Model.WorkflowStatus.ProcurementStatus.商品配送状态.配送完成);
                }
            }

            return Json(new { Status = true });
        }

        /// <summary>
        /// 保存采购仓商品数量
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="proSattus"></param>
        public void SavePmWareGoodsList(int SysNo, Hyt.Model.WorkflowStatus.ProcurementStatus.采购分货单状态 proSattus)
        {
            ///CBPmGoodsDelivery cbDelivery = PmProcurementBo.Instance.GetCBPmGoodsDeliveryByPSysNo(SysNo);
            ///更新库存信息
            List<CBPmGoodsDeliveryItem> items = PmProcurementBo.Instance.GetPmGoodsDeliveryItemByOrderPSysNo(SysNo);
            if (proSattus == Model.WorkflowStatus.ProcurementStatus.采购分货单状态.完成)
            {
                foreach (PmGoodsDeliveryItem item in items)
                {
                    PmWareGoods tempGoods = new PmWareGoods()
                    {
                        Freeze = 0,
                        WareNum = item.gdi_SendQuity,
                        StayInWare = item.gdi_SendQuity * -1,
                        ProSysNo = item.gdi_GoodSysNo,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    PmWareGoodsBo.Instance.InnerOrUpdatePmWareGoods(tempGoods);
                }
            }
            else
            {
                foreach (PmGoodsDeliveryItem item in items)
                {
                    PmWareGoods tempGoods = new PmWareGoods()
                    {
                        Freeze = 0,
                        WareNum = 0,
                        StayInWare = item.gdi_SendQuity,
                        ProSysNo = item.gdi_GoodSysNo,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    PmWareGoodsBo.Instance.InnerOrUpdatePmWareGoods(tempGoods);
                }
            }
        }

        /// <summary>
        /// 商品配送状况
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="proSattus"></param>
        public void SavePmWareGoodsListToSend(int SysNo, Hyt.Model.WorkflowStatus.ProcurementStatus.商品配送状态 proSattus)
        {
            //CBPmGoodsDelivery cbDelivery = PmProcurementBo.Instance.GetCBPmGoodsDeliveryByPSysNo(SysNo);
            ///更新库存信息
            List<PmGoodsDeliveryItem> items = PmProcurementBo.Instance.GetPmGoodsDeliveryItemByPSysNo(SysNo);
            if (proSattus == Model.WorkflowStatus.ProcurementStatus.商品配送状态.配送完成)
            {
                foreach (PmGoodsDeliveryItem item in items)
                {
                    PmWareGoods tempGoods = new PmWareGoods()
                    {
                        Freeze = item.gdi_SendQuity * -1,
                        WareNum = 0,
                        StayInWare = 0,
                        ProSysNo = item.gdi_GoodSysNo,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    PmWareGoodsBo.Instance.InnerOrUpdatePmWareGoods(tempGoods);
                }
            }
            else
            {
                foreach (PmGoodsDeliveryItem item in items)
                {
                    PmWareGoods tempGoods = new PmWareGoods()
                    {
                        Freeze = item.gdi_SendQuity,
                        WareNum = item.gdi_SendQuity * -1,
                        StayInWare = 0,
                        ProSysNo = item.gdi_GoodSysNo,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    PmWareGoodsBo.Instance.InnerOrUpdatePmWareGoods(tempGoods);
                }
            }
        }

        [Privilege(PrivilegeCode.CG100302, PrivilegeCode.CG100402)]
        public ActionResult ProcurementPmGoodsDeliveryByPager(int? id)
        {
            if (Request.IsAjaxRequest())
            {

                Pager<CBPmGoodsDelivery> pager = new Pager<CBPmGoodsDelivery>();
                pager.CurrentPage = id ?? 1;
                pager.PageSize = 10;
                pager.PageFilter.gd_Type = 2;
                PmProcurementBo.Instance.GetPmGoodsDeliveryPager(ref pager);
                var model = new PagedList<CBPmGoodsDelivery>();
                model.TotalItemCount = pager.TotalRows;
                model.TData = pager.Rows;
                model.CurrentPageIndex = pager.CurrentPage;
                return PartialView("_AjaxGetPmGoodsDeliveryPager", model);
            }
            return View();
        }
        #endregion

        #region 采购库房商品情况
        [Privilege(PrivilegeCode.CG100401)]
        public ActionResult ProcurementWarehouse(int? id, int? type)
        {
            if (Request.IsAjaxRequest())
            {

                Pager<CBPmWareGoods> pager = new Pager<CBPmWareGoods>();
                pager.CurrentPage = id ?? 1;
                pager.PageSize = 10;
                pager.PageFilter.Type = type ?? -1;
                PmWareGoodsBo.Instance.GetPmWareGoodsPager(ref pager);
                var model = new PagedList<CBPmWareGoods>();
                model.TotalItemCount = pager.TotalRows;
                model.TData = pager.Rows;
                model.CurrentPageIndex = pager.CurrentPage;
                return PartialView("_AjaxGetProcurementWarehousePager", model);
            }
            return View();
        }

        [Privilege(PrivilegeCode.CG100402)]
        public ActionResult ProcurementGoodsSelector(int? id)
        {
            if (Request.IsAjaxRequest())
            {

                Pager<CBPmWareGoods> pager = new Pager<CBPmWareGoods>();
                pager.CurrentPage = id ?? 1;
                pager.PageSize = 10;
                pager.PageFilter.Type = 1;
                PmWareGoodsBo.Instance.GetPmWareGoodsPager(ref pager);
                var model = new PagedList<CBPmWareGoods>();
                model.TotalItemCount = pager.TotalRows;
                model.TData = pager.Rows;
                model.CurrentPageIndex = pager.CurrentPage;
                return PartialView("_AjaxGetProcurementGoodsSelectorPager", model);
            }
            return View();
        }

        [Privilege(PrivilegeCode.CG100402)]
        public JsonResult GetProcurementGoodsList(string idList)
        {
            List<CBPmWareGoods> wareJsonList = PmWareGoodsBo.Instance.GetPmWareGoodsListBySysNos(idList);
            return Json(wareJsonList);
        }
        #endregion

        #region 创建采购付款单金额
        [Privilege(PrivilegeCode.FNCG01001)]
        public ActionResult InsertOrUpdateFnPurchasePaymentOrder(int? SysNo)
        {
            CBPurchasePaymentOrder paymentOrder = new CBPurchasePaymentOrder();
            if (SysNo != null)
            {
                paymentOrder = PurchasePaymentOrderBo.Instance.GetEntity(SysNo.Value);
                ViewBag.CurrentName = paymentOrder.CreateName;
                ViewBag.CurrentTime = paymentOrder.CreateTime.ToString("yyyy-MM-dd");
            }
            else
            {
                paymentOrder.OrderNumber = "FNP" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                paymentOrder.PurchaseOrderItems = new List<CBFnPurchasePaymentOrderItem>();
                ViewBag.CurrentName = CurrentUser.Base.UserName;
                ViewBag.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            ViewBag.Mod = paymentOrder;

            string table = "";
            List<PmProcurementOrder> procurementOrderList = PurchasePaymentOrderBo.Instance.ProcurementOrderByNotInPurchase();
            table += "<table style=\\\"width:100%\\\">";
            foreach (PmProcurementOrder order in procurementOrderList)
            {
                table += "<tr>";
                table += "<td style=\\\"height:30px;\\\" onclick=\\\"ClickProcurementNumberData('" +
                    order.SysNo + "','" + order.Po_Number + "')\\\" >" + order.Po_Number + "</td>";
                table += "</tr>";
            }
            table += "</table>";
            ViewBag.ProcurmentTable = table;

            return View();
        }
        /// <summary>
        /// 保存采购付款单数据
        /// </summary>
        /// <param name="order"></param>
        /// <param name="Items"></param>
        /// <returns></returns>
        /// 
        [Privilege(PrivilegeCode.FNCG01001)]
        public JsonResult SaveFnPurchasePaymentOrder(FnPurchasePaymentOrder order, string _Items)
        {
            List<FnPurchasePaymentOrderItem> Items = JsonUtil.ToObject<List<FnPurchasePaymentOrderItem>>(_Items);
            int PSysNo = 0;
            using (var tran = new TransactionScope())
            {
                decimal totalPurValue = 0;
                foreach (FnPurchasePaymentOrderItem item in Items)
                {
                    totalPurValue += item.PaymentAmount;
                }
                order.TotalAmount = totalPurValue;

                order.CreateBy = CurrentUser.Base.SysNo;
                order.CreateName = CurrentUser.Base.UserName;
                order.CreateTime = DateTime.Now;

                order.FinancialTime = DateTime.Now;
                order.AuditorTime = DateTime.Now;

                if (order.SysNo > 0)
                {
                    PurchasePaymentOrderBo.Instance.UpdateEntity(order);
                    PSysNo = order.SysNo;
                }
                else
                {

                    PSysNo = PurchasePaymentOrderBo.Instance.InsertEntity(order);
                }
                decimal totalValue = 0;
                foreach (FnPurchasePaymentOrderItem item in Items)
                {
                    int itemSysNo = 0;
                    item.PSysNo = PSysNo;


                    FnPaymentVoucher paymentVoucherMod = new FnPaymentVoucher();
                    paymentVoucherMod.CreatedBy = CurrentUser.Base.SysNo;
                    paymentVoucherMod.CreatedDate = DateTime.Now;
                    paymentVoucherMod.CustomerSysNo = 0;
                    paymentVoucherMod.LastUpdateBy = CurrentUser.Base.SysNo;
                    paymentVoucherMod.LastUpdateDate = DateTime.Now;
                    paymentVoucherMod.PayableAmount = item.PaymentAmount;
                    paymentVoucherMod.PayDate = new DateTime(1753, 1, 1);
                    paymentVoucherMod.PayerSysNo = 0;
                    paymentVoucherMod.RefundAccount = item.PayBankIDCard;
                    paymentVoucherMod.RefundAccountName = item.CompanyName;
                    paymentVoucherMod.RefundBank = item.PayBankName;
                    paymentVoucherMod.Source = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.采购单;
                    paymentVoucherMod.SourceSysNo = PSysNo;
                    paymentVoucherMod.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单状态.待付款;
                    paymentVoucherMod.Remarks = "付款至 采购生产厂家-" + item.PayBankName;
                    int PaymentSysNo = 0;
                    if (item.PVSysNo > 0)
                    {
                        paymentVoucherMod.SysNo = item.PVSysNo;
                        Hyt.BLL.Finance.FinanceBo.Instance.UpdatePaymentVoucher(paymentVoucherMod);
                        PaymentSysNo = paymentVoucherMod.SysNo;
                    }
                    else
                    {
                        PaymentSysNo = Hyt.BLL.Finance.FinanceBo.Instance.CreatePaymentVoucher(paymentVoucherMod);
                    }

                    if (item.SysNo > 0)
                    {
                        item.PVSysNo = PaymentSysNo;
                        PurchasePaymentOrderBo.Instance.UpdateItem(item);
                        itemSysNo = item.SysNo;
                    }
                    else
                    {
                        item.PVSysNo = PaymentSysNo;
                        itemSysNo = PurchasePaymentOrderBo.Instance.InsertItem(item);

                    }
                }

                tran.Complete();
            }

            return Json(new { Status = true });
        }

        /// <summary>
        /// 获取生产厂家列表
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        /// 
        [Privilege(PrivilegeCode.FNCG01001)]
        ///获取采购分货单的生产厂家列表
        public JsonResult GetPointsOrderList(string Number)
        {
            CBPmProcurementOrder orderMod = PmProcurementBo.Instance.GetCBPmProcurementOrder(Number);
            List<CBPmPointsOrder> pointOrderList = PmProcurementBo.Instance.GetPointsOrderListBySinglePSysNo(orderMod.SysNo);
            foreach (CBPmPointsOrder order in pointOrderList)
            {
                if (string.IsNullOrEmpty(order.BankIDCard))
                {
                    order.BankIDCard = "";
                }
                if (string.IsNullOrEmpty(order.BankName))
                {
                    order.BankName = "";
                }
                if (string.IsNullOrEmpty(order.FContact))
                {
                    order.FContact = "";
                }
                if (string.IsNullOrEmpty(order.Po_FactoryName))
                {
                    order.Po_FactoryName = "";
                }
            }
            return Json(pointOrderList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProcurmentSysNo"></param>
        /// <returns></returns>
        /// 
        [Privilege(PrivilegeCode.FNCG01001)]
        public JsonResult GetProductDetaInfoList(int ProcurmentSysNo)
        {
            CBPmProcurementOrder proOrder =
                PmProcurementBo.Instance.GetCBPmProcurementOrder(ProcurmentSysNo);
            return Json(proOrder.orderItemList);
        }
        /// <summary>
        /// 获采购付款单数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.FNCG01001)]
        public ActionResult GetCBPurchasePaymentOrderPager(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                if (id == null)
                {
                    id = 1;
                }
                var list = new PagedList<CBPurchasePaymentOrder>();
                var pager = new Pager<CBPurchasePaymentOrder> { CurrentPage = id ?? 1, PageSize = 5 };
                PurchasePaymentOrderBo.Instance.GetPmPointsOrderPager(ref pager);
                list = new PagedList<CBPurchasePaymentOrder>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                List<int> pSysNo = new List<int>();
                foreach (CBPurchasePaymentOrder payOrder in pager.Rows)
                {
                    pSysNo.Add(payOrder.ProcurementSysNo);
                }
                if (pSysNo.Count > 0)
                {
                    List<CBPmProcurementOrderItem> proOrderItemList = PmProcurementBo.Instance.GetPmProcurementOrderItem(pSysNo.ToArray());
                    ViewBag.OrderItemList = proOrderItemList;
                }
                else
                {
                    ViewBag.OrderItemList = new List<CBPmProcurementOrderItem>();
                }
                return PartialView("_AjaxCBPurchasePaymentOrderPager", list);
            }

            return View();
        }

        #endregion

        #region 库存查询操作
        [Privilege(PrivilegeCode.WHKC00002)]
        public ActionResult GetMonthWarehouseHistoryDataPager(int? warehouseSysNo, int? id, int? year)
        {
            if (Request.IsAjaxRequest())
            {

                var list = new PagedList<CBWhWarehouseMonthStockHistory>();
                var pager = new Pager<CBWhWarehouseMonthStockHistory>
                {
                    CurrentPage = id ?? 1,
                    PageSize = list.PageSize,
                    PageFilter = new CBWhWarehouseMonthStockHistory()
                    {
                        WarehouseSysNo = warehouseSysNo.Value,
                        WhYear = year ?? DateTime.Now.Year
                    }
                };
                WhWarehouseMonthStockHistoryBo.Instance.GetWhWarehouseMonthStockHistoryPager(ref pager);
                list = new PagedList<CBWhWarehouseMonthStockHistory>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                return PartialView("_AjaxGetMonthWarehouseHistoryDataPager", list);

            }
            else
            {
                //if(warehouseSysNo!=null)
                //{
                //    DateTime tempTime = DateTime.Now.AddMonths(1);
                //    tempTime = new DateTime(tempTime.Year, tempTime.Month, 1);
                //    tempTime = tempTime.AddDays(-1);
                //    WhWarehouseMonthStockHistoryBo.Instance.InsertOrUpdateStockHistory(tempTime.Year, tempTime.Month, tempTime.Day, warehouseSysNo.Value);
                //}

                ViewBag.WarehouseList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetAllWarehouseList()
                    .Where(p => p.WarehouseType == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税
                        || p.WarehouseType == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.直邮).ToList();


                foreach (WhWarehouse mod in ViewBag.WarehouseList as IList<WhWarehouse>)
                {
                    warehouseSysNo = mod.SysNo;
                    if (warehouseSysNo != null)
                    {
                        DateTime tempTime = DateTime.Now.AddMonths(1);
                        tempTime = new DateTime(tempTime.Year, tempTime.Month, 1);
                        tempTime = tempTime.AddDays(-1);
                        WhWarehouseMonthStockHistoryBo.Instance.InsertOrUpdateStockHistory(tempTime.Year, tempTime.Month, tempTime.Day, warehouseSysNo.Value);
                    }

                }

            }
            return View();
        }

        [Privilege(PrivilegeCode.WHKC00001)]
        public ActionResult GetWhWarehousMouthSaleHistoryPager(int? warehouseSysNo, int? id, int? year, int? month)
        {
            if (Request.IsAjaxRequest())
            {

                var list = new PagedList<CBWhWarehouseMonthStockHistory>();
                var pager = new Pager<CBWhWarehouseMonthStockHistory>
                {
                    CurrentPage = id ?? 1,
                    PageSize = list.PageSize,
                    PageFilter = new CBWhWarehouseMonthStockHistory()
                    {
                        WarehouseSysNo = warehouseSysNo.Value,
                        WhYear = year ?? DateTime.Now.Year,
                        WhMonth = month ?? DateTime.Now.Month
                    }
                };
                WhWarehouseMonthStockHistoryBo.Instance.GetWhWarehousMouthSaleHistoryPager(ref pager);
                list = new PagedList<CBWhWarehouseMonthStockHistory>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                return PartialView("_AjaxGetWhWarehousMouthSaleHistoryPager", list);

            }
            else
            {
                ViewBag.WarehouseList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetAllWarehouseList()
                    .Where(p => p.WarehouseType == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税
                        || p.WarehouseType == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.直邮).ToList();
                foreach (WhWarehouse mod in ViewBag.WarehouseList as IList<WhWarehouse>)
                {
                    warehouseSysNo = mod.SysNo;
                    if (warehouseSysNo != null)
                    {
                        DateTime tempTime = DateTime.Now.AddMonths(1);
                        tempTime = new DateTime(tempTime.Year, tempTime.Month, 1);
                        tempTime = tempTime.AddDays(-1);
                        WhWarehouseMonthStockHistoryBo.Instance.InsertOrUpdateStockHistory(tempTime.Year, tempTime.Month, tempTime.Day, warehouseSysNo.Value);
                    }

                }
            }
            return View();
        }

        [Privilege(PrivilegeCode.WHKC00002, PrivilegeCode.WHKC00001)]
        public ActionResult BindMonthDataTimeArea()
        {
            DateTime startTime = new DateTime(2015, 9, 1);
            DateTime endTime = new DateTime(2016, 4, 1);

            WhWarehouseMonthStockHistoryBo.Instance.CheckAndUpdataMonthArea(startTime, endTime, 2);
            return View();
        }
        #endregion

        #region MyRegion
        [Privilege(PrivilegeCode.FNP000001)]
        public ActionResult GetPaymentListDataPager(int? id, int? paymentSysNo, int? warehouseSysNo, DateTime? startTime, DateTime? endTime)
        {
            if (Request.IsAjaxRequest())
            {
                var list = new PagedList<CBFnReceiptVoucher>();
                var pager = new Pager<CBFnReceiptVoucher>
                {
                    CurrentPage = id ?? 1,
                    PageSize = 5,

                };
                WhWarehouseMonthStockHistoryBo.Instance.GetPaymentListDataPager(ref pager, paymentSysNo, warehouseSysNo, startTime, endTime, "");
                list = new PagedList<CBFnReceiptVoucher>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                List<int> orderSysNoList = new List<int>();
                foreach (CBFnReceiptVoucher mod in pager.Rows)
                {
                    orderSysNoList.Add(mod.SysNo);
                }
                if (orderSysNoList.Count > 0)
                {
                    List<CBSoOrderItem> itemList = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNoList.ToArray());
                    ViewBag.itemList = itemList;
                }
                else
                {
                    ViewBag.itemList = new List<SoOrderItem>();
                }
                return PartialView("_AjaxGetPaymentListDataPager", list);
            }
            else
            {
                ViewBag.WarehouseList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetAllWarehouseList()
                    .Where(p => p.Status == 1).ToList();
                ViewBag.PaymentType = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetAll().Where(p => p.Status == 1).ToList();
            }
            return View();
        }

        #region 销售单导出电子表格
        [Privilege(PrivilegeCode.FNP000001)]
        public FileResult ExportSoOrder(int? paymentSysNo, int? warehouseSysNo, DateTime? startTime, DateTime? endTime, string sysNoList)
        {
            Extra.UpGrade.Export exportBll = new Extra.UpGrade.Export();
            Extra.UpGrade.ExportHead head = new Extra.UpGrade.ExportHead();
            head.HeadText = "导出销售单";
            head.ThDataList.Add("SysNo", new ExportHeadMod() { Text = "订单编号", Type = "数值" });
            head.ThDataList.Add("PaymentName", new ExportHeadMod() { Text = "付款方式" });
            head.ThDataList.Add("CreateDate", new ExportHeadMod() { Text = "单据时间" });
            head.ThDataList.Add("CreatorName", new ExportHeadMod() { Text = "制单人" });
            head.ThDataList.Add("CustomerName", new ExportHeadMod() { Text = "付款人" });
            head.ThDataList.Add("CreditCardNumber", new ExportHeadMod() { Text = "开户卡号" });
            head.ThDataList.Add("VoucherNo", new ExportHeadMod() { Text = "交易凭证" });
            head.ThDataList.Add("IncomeAmount", new ExportHeadMod() { Text = "应收金额", Type = "数值" });
            head.ThDataList.Add("ReceivedAmount", new ExportHeadMod() { Text = "已收金额", Type = "数值" });
            head.ThDataList.Add("WarehouseName", new ExportHeadMod() { Text = "仓库/门店" });
            head.ThDataList.Add("ConfirmeName", new ExportHeadMod() { Text = "确认人员" });

            var pager = new Pager<CBFnReceiptVoucher>
            {
                CurrentPage = 1,
                PageSize = 50000,

            };
            WhWarehouseMonthStockHistoryBo.Instance.GetPaymentListDataPager(ref pager, paymentSysNo, warehouseSysNo, startTime, endTime, sysNoList);
            List<object> objList = new List<object>();
            foreach (CBFnReceiptVoucher mod in pager.Rows)
            {
                mod.CreatorName = (string.IsNullOrEmpty(mod.CreatorName) ? mod.CustomerName : mod.CreatorName);
                objList.Add(mod);
            }
            string table = exportBll.InnerExportDataHaveHaed(head, objList, true, "SysNo");

            List<int> itemSysNo = new List<int>();
            foreach (CBFnReceiptVoucher mod in pager.Rows)
            {
                itemSysNo.Add(mod.SysNo);
            }
            List<CBSoOrderItem> itemList = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(itemSysNo.ToArray());
            foreach (int intSysNo in itemSysNo)
            {
                List<CBSoOrderItem> tempItemList = itemList.Where(p => p.OrderSysNo == intSysNo).ToList();
                exportBll = new Extra.UpGrade.Export();
                head = new Extra.UpGrade.ExportHead();
                head.HeadText = "明细";
                head.ThDataList.Add("GroupName", new ExportHeadMod() { Text = "" });
                head.ThDataList.Add("SysNo", new ExportHeadMod() { Text = "商品编号", Type = "数值" });
                head.ThDataList.Add("ProductName", new ExportHeadMod() { Text = "商品名称" });
                head.ThDataList.Add("Spec", new ExportHeadMod() { Text = "规格描述" });
                head.ThDataList.Add("Quantity", new ExportHeadMod() { Text = "销售数量", Type = "数值" });
                head.ThDataList.Add("OriginalPrice", new ExportHeadMod() { Text = "原始单价", Type = "数值" });
                head.ThDataList.Add("SalesAmount", new ExportHeadMod() { Text = "销售金额", Type = "数值" });
                head.ThDataList.Add("ChangeAmount", new ExportHeadMod() { Text = "调价金额", Type = "数值" });
                head.ThDataList.Add("SalesUnitPrice", new ExportHeadMod() { Text = "总金额", Type = "数值" });

                objList = new List<object>();
                foreach (SoOrderItem mod in tempItemList)
                {
                    mod.GroupName = "";
                    mod.SalesUnitPrice = (mod.SalesAmount + mod.ChangeAmount);
                    //mod.CreatorName = (string.IsNullOrEmpty(mod.CreatorName) ? mod.CustomerName : mod.CreatorName);
                    objList.Add(mod);
                }
                string tableDetail = exportBll.InitExportData(head, objList, false, "SysNo");
                table = table.Replace("[=" + intSysNo + "]", tableDetail);
            }


            byte[] fileContents = System.Text.Encoding.UTF8.GetBytes(table);
            return File(fileContents, "application/ms-excel", "销售收款列表.xls");
        }
        [Privilege(PrivilegeCode.FNCG01001)]
        public FileResult ExportPurchaseOrder()
        {
            Extra.UpGrade.Export exportBll = new Extra.UpGrade.Export();
            Extra.UpGrade.ExportHead head = new Extra.UpGrade.ExportHead();
            head.HeadText = "导出销售单";
            head.ThDataList.Add("SysNo", new ExportHeadMod() { Text = "编号", Type = "数值" });
            head.ThDataList.Add("OrderNumber", new ExportHeadMod() { Text = "采购付款单编号" });
            head.ThDataList.Add("ProcurementNumber", new ExportHeadMod() { Text = "采购单编号" });
            head.ThDataList.Add("CreateName", new ExportHeadMod() { Text = "制单人" });
            head.ThDataList.Add("CreateTime", new ExportHeadMod() { Text = "制单时间" });
            head.ThDataList.Add("BankPaymentInfo", new ExportHeadMod() { Text = "收款信息" });
            head.ThDataList.Add("TotalAmount", new ExportHeadMod() { Text = "付款总金额", Type = "数值" });


            var pager = new Pager<CBPurchasePaymentOrder> { CurrentPage = 1, PageSize = 500000 };
            PurchasePaymentOrderBo.Instance.GetPmPointsOrderPager(ref pager);

            List<object> objList = new List<object>();
            foreach (CBPurchasePaymentOrder mod in pager.Rows)
            {
                mod.BankPaymentInfo = mod.BankPaymentInfo.Replace("|", "<br/>");
                objList.Add(mod);
            }
            string table = exportBll.InnerExportDataHaveHaed(head, objList, true, "ProcurementSysNo");

            List<int> itemSysNo = new List<int>();
            foreach (CBPurchasePaymentOrder mod in pager.Rows)
            {
                itemSysNo.Add(mod.ProcurementSysNo);
            }

            List<CBPmProcurementOrderItem> proOrderItemList = PmProcurementBo.Instance.GetPmProcurementOrderItem(itemSysNo.ToArray());
            foreach (int intSysNo in itemSysNo)
            {
                List<CBPmProcurementOrderItem> tempItemList = proOrderItemList.Where(p => p.Poi_PSysNo == intSysNo).ToList();
                exportBll = new Extra.UpGrade.Export();
                head = new Extra.UpGrade.ExportHead();
                head.HeadText = "明细";
                head.ThDataList.Add("GroupName", new ExportHeadMod() { Text = "" });
                head.ThDataList.Add("Cb_ProName", new ExportHeadMod() { Text = "商品名称" });
                head.ThDataList.Add("Cb_Unit", new ExportHeadMod() { Text = "单位" });
                head.ThDataList.Add("Cb_Spec", new ExportHeadMod() { Text = "规格" });
                head.ThDataList.Add("Poi_ProQuity", new ExportHeadMod() { Text = "采购数量", Type = "数值" });

                objList = new List<object>();
                foreach (CBPmProcurementOrderItem mod in tempItemList)
                {

                    //mod.CreatorName = (string.IsNullOrEmpty(mod.CreatorName) ? mod.CustomerName : mod.CreatorName);
                    objList.Add(mod);
                }
                string tableDetail = exportBll.InitExportData(head, objList, false, "SysNo");
                table = table.Replace("[=" + intSysNo + "]", tableDetail);
            }


            byte[] fileContents = System.Text.Encoding.UTF8.GetBytes(table);
            return File(fileContents, "application/ms-excel", "采购付款单列表.xls");
        }
        #endregion
        #endregion
    }
}
