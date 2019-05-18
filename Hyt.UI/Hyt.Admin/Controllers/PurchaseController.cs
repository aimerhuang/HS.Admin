using Hyt.BLL.Procurement;
using Hyt.BLL.Promotion;
using Hyt.BLL.Purchase;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 采购单管理
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    /// <summary>
    /// 采购单管理
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class PurchaseController : BaseController
    {
        /// <summary>
        /// 创建采购单
        /// </summary>
        /// <returns></returns>
        /// <param name="id">采购单系统编号</param>
        /// <param name="page">跳转时的当前页码</param>
        /// <remarks>2016-6-15 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult PurchaseCreate(int id = 0, int page = 1)
        {
            var purchase = new PrPurchase();
            ViewBag.WarehouseName = "--请选择--";
            if (id > 0)
            {
                purchase = BLL.Purchase.PrPurchaseBo.Instance.GetPrPurchaseInfo(id);
                ViewBag.WarehouseName = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(purchase.WarehouseSysNo);
                var departInfo = Hyt.BLL.Basic.OrganizationBo.Instance.GetEntity(purchase.DepartmentSysNo);
                if (departInfo != null)
                    ViewBag.DepartName = departInfo.Name;

                var businessInfo = BLL.Sys.SyUserBo.Instance.GetSyUser(purchase.SyUserSysNo);
                if (businessInfo != null)
                    ViewBag.BusinessName = businessInfo.UserName;

                if (purchase.custodian != null)
                {
                    var custodianInfo = BLL.Sys.SyUserBo.Instance.GetSyUser((int)purchase.custodian);
                    ViewBag.CustodianName = custodianInfo.UserName;
                }

            }
            ViewBag.UserId = CurrentUser.Base.SysNo;
            ViewBag.ManufacturerList = PmBaseDataBo.Instance.GetPmManufacturert();
            ViewBag.Warehouse = CurrentUser.Warehouses;
            return View(purchase);
        }

        /// <summary>
        /// 采购单查看
        /// </summary>
        /// <returns></returns>
        /// <param name="id">采购单系统编号</param>
        /// <remarks>2016-6-15 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CG100101)]
        public ActionResult PurchasePreview(int id = 0)
        {
            return PurchaseCreate(id);
        }
        /// <summary>
        /// 创建采购单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public JsonResult PurchaseCreate(PrPurchase model, List<int> delSysNos = null)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0
            };
            try
            {
                if (model.SysNo <= 0)
                {
                    model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.EnterQuantity = 0;
                    model.PaymentStatus = (int)PurchaseStatus.采购单付款状态.未付款;
                    model.PurchaseCode = BLL.Basic.ReceiptNumberBo.Instance.GetPurchaseNo();
                    model.Status = (int)PurchaseStatus.采购单状态.待审核;
                    model.WarehousingStatus = (int)PurchaseStatus.采购单入库状态.未入库;
                }
                BLL.Purchase.PrPurchaseBo.Instance.AddPurchase(model);
                if (delSysNos != null)
                {
                    BLL.Purchase.PrPurchaseDetailsBo.Instance.Delete(string.Join(",", delSysNos));
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 审核采购单
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CG100103)]
        [HttpPost]
        public JsonResult AuditPurchase(int sysNo)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0
            };

            result.Status = PrPurchaseBo.Instance.AuditPurchase(sysNo);
            if (!result.Status)
                result.Message = "审核失败，请重试！";
            else
            {
                var models = PrPurchaseBo.Instance.GetPrPurchaseInfo(sysNo);
                models.Audit = CurrentUser.Base.SysNo;
                models.AuditTime = DateTime.Now;
                PrPurchaseBo.Instance.UpdatePurchase(models);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除采购单
        /// </summary>
        /// <param name="delSysNos">需要删除的采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public JsonResult DeletePurchase(List<int> delSysNos = null)
        {
            var result = new Result
            {
                Status = false
            };

            if (delSysNos != null)
            {
                BLL.Purchase.PrPurchaseBo.Instance.DeletePrPurchase(string.Join(",", delSysNos));
                result.Status = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 采购单列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-15 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult PurchaseList(ParaPrPurchaseFilter para)
        {
            if (Request.IsAjaxRequest())
            {
                para.Id = para.Id > 0 ? para.Id : 1;
                int purchaseSysno = 0;
                int.TryParse(para.PurchaseCode, out purchaseSysno);
                para.SysNo = purchaseSysno;
                var pageList = BLL.Purchase.PrPurchaseBo.Instance.QueryPrPurchase(para);
                return PartialView("_AjaxPagerPurchaseList", pageList);
            }
            else
            {
                //采购单状态下拉绑定数据
                var statustList = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text = @"全部", Value = "0", Selected = true}
                    };
                EnumUtil.ToListItem<PurchaseStatus.采购单状态>(ref statustList);
                ViewData["Status"] = new SelectList(statustList, "Value", "Text");
            }
            return View();
        }
        [Privilege(PrivilegeCode.None)]
        public void ExportPurchaseData(ParaPrPurchaseFilter para)
        {
            List<CBPrPurchaseDetails> TData = BLL.Purchase.PrPurchaseBo.Instance.QueryPrPurchaseByOrderDetail(para);
            Hyt.BLL.Purchase.PrPurchaseBo.Instance.ExportPurchaseData(TData, "", 0);
        }

        /// <summary>
        /// 创建采购退货单
        /// </summary>
        /// <returns></returns>
        /// <param name="id">采购单系统编号</param>
        /// <param name="page">跳转时的当前页码</param>
        /// <remarks>2016-6-22 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.CGR100101)]
        public ActionResult PurchaseReturnCreate(int id = 0, int page = 1)
        {
            var purchase = new PrPurchase();
            if (id > 0)
            {
                purchase = BLL.Purchase.PrPurchaseBo.Instance.GetPrRePurchaseInfo(id);
                var detailList = purchase.PurchaseDetails;
                var indxList = new List<int>();
                foreach (var mod in detailList)
                {
                    indxList.Add(mod.ProductSysNo);
                }
                var stockList = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetAllStockList(purchase.WarehouseSysNo, indxList);
                ViewBag.WarehouseName = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(purchase.WarehouseSysNo);
                ViewBag.StockList = stockList;
            }
            return View(purchase);
        }

        /// <summary>
        /// 创建采购退货单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.CGR100101)]
        [HttpPost]
        public JsonResult PurchaseReturnCreate(PrPurchaseReturn model)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0
            };
            try
            {
                if (model.SysNo <= 0)
                {
                    model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.Status = (int)PurchaseStatus.采购退货单状态.待审核;
                }

                var detailList = model.PurchaseReturnDetails;
                IList<int> indxList = new List<int>();
                foreach (var mod in detailList)
                {
                    indxList.Add(mod.ProductSysNo);
                }

                var stockList = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetAllStockList(model.WarehouseSysNo, indxList);
                foreach (var mod in detailList)
                {
                    PdProductStock stock = stockList.First(p => p.PdProductSysNo == mod.ProductSysNo);
                    if (mod.ReturnQuantity > stock.StockQuantity)
                    {
                        result.StatusCode = -1;
                        result.Message = "采购退货单的退货数量大于退货仓商品库存数，不能进行退货，请核实情况。";
                        return Json(result, JsonRequestBehavior.AllowGet);

                    }
                }

                BLL.Purchase.PrPurchaseReturnBo.Instance.AddPurchaseReturn(model);
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改采购退货单
        /// </summary>
        /// <returns></returns>
        /// <param name="id">采购单系统编号</param>
        /// <param name="page">跳转时的当前页码</param>
        /// <remarks>2016-6-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult PurchaseReturnUpdate(int id = 0, int page = 1)
        {
            var purchaseReturn = new PrPurchaseReturn();
            if (id > 0)
            {
                purchaseReturn = BLL.Purchase.PrPurchaseReturnBo.Instance.GetPrPurchaseReturnInfo(id);
                ViewBag.WarehouseName = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseName(purchaseReturn.WarehouseSysNo);
                ViewBag.PurchaseCode = BLL.Purchase.PrPurchaseBo.Instance.GetPurchaseCode(purchaseReturn.PurchaseSysNo);
            }
            return View(purchaseReturn);
        }
        /// <summary>
        /// 采购单退货查看
        /// </summary>
        /// <returns></returns>
        /// <param name="id">采购单系统编号</param>
        /// <remarks>2016-6-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult PurchaseReturnPreview(int id = 0)
        {
            return PurchaseReturnUpdate(id);
        }
        /// <summary>
        /// 修改采购退货单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public JsonResult PurchaseReturnUpdate(PrPurchaseReturn model, List<int> delSysNos = null)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0
            };
            try
            {
                if (model.SysNo <= 0)
                {
                    model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.Status = (int)PurchaseStatus.采购退货单状态.待审核;
                }
                BLL.Purchase.PrPurchaseReturnBo.Instance.AddPurchaseReturn(model);
                if (delSysNos != null)
                {
                    BLL.Purchase.PrPurchaseReturnDetailsBo.Instance.Delete(string.Join(",", delSysNos));
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 删除采购退货单
        /// </summary>
        /// <param name="delSysNos">需要删除的采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public JsonResult DeletePurchaseReturn(List<int> delSysNos = null)
        {
            var result = new Result
            {
                Status = false
            };

            if (delSysNos != null)
            {
                BLL.Purchase.PrPurchaseReturnBo.Instance.DeletePrPurchaseReturn(string.Join(",", delSysNos));
                result.Status = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 审核采购单
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CGR100103)]
        [HttpPost]
        public JsonResult AuditPurchaseReturn(int sysNo)
        {
            var result = new Result
            {
                Status = false,
                StatusCode = 0
            };

            result.Status = Hyt.BLL.Purchase.PrPurchaseReturnBo.Instance.AuditPurchaseReturn(sysNo);
            if (!result.Status)
                result.Message = "审核失败，请重试！";

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 采购退货单列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-6-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult PurchaseReturnList(ParaPrPurchaseReturnFilter para)
        {
            if (Request.IsAjaxRequest())
            {
                para.Id = para.Id > 0 ? para.Id : 1;
                var pageList = BLL.Purchase.PrPurchaseReturnBo.Instance.QueryPrPurchaseReturn(para);
                return PartialView("_AjaxPagerPurchaseReturnList", pageList);
            }
            else
            {
                //采购单状态下拉绑定数据
                var statustList = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text = @"全部", Value = "0", Selected = true}
                    };
                EnumUtil.ToListItem<PurchaseStatus.采购退货单状态>(ref statustList);
                ViewData["Status"] = new SelectList(statustList, "Value", "Text");
            }
            return View();
        }


        #region 导入采购商品Excel
        public static bool _starting;
        /// <summary>
        /// 导入优惠卡excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-07-03 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult ImportExcel()
        {
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Resuldt();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {
                        result = PrPurchaseBo.Instance.ImportExcel(httpPostedFileBase.InputStream,
                            CurrentUser.Base.SysNo);
                    }
                    catch (Exception e)
                    {
                        result.Message = string.Format(e.Message);
                        result.Status = false;
                    }
                    finally
                    {
                        _starting = false;
                    }
                }
                else
                {
                    result.Message = string.Format("正在导入数据，请稍后再操作");
                    result.Status = false;
                }

                ViewBag.result = HttpUtility.UrlEncode(JsonConvert.SerializeObject(result).ToString());// HttpUtility.UrlEncode();
            }
            return View();
        }


        /// <summary>
        /// 导出采购商品模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2017-07-03 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public void ExportTemplate()
        {
            ExportExcel(@"\Templates\Excel\Product.xls", "采购商品导入模板");
        }

        /// <summary>
        /// 导出excel模板文件
        /// </summary>
        /// <param name="tmpPath">模板路径</param>
        /// <param name="excelFileName">导出文件名</param>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        private void ExportExcel(string tmpPath, string excelFileName)
        {
            ExcelUtil.ExportFromTemplate(new List<object>(), tmpPath, 1, excelFileName, null, false);
        }

        #endregion

    }
}