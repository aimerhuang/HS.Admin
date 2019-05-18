using Hyt.BLL.InventorySheet;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.InventorySheet;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    public class InventorySheetController : BaseController
    {

        #region 盘点作业
        #region 盘点单
        /// <summary>
        /// 盘点单
        /// </summary>
        /// <returns></returns>
        /// 2017-08-07 吴琨

        [Privilege(PrivilegeCode.None)]
        public ActionResult InventoryList(int? PageStatus = 1)
        {
            ViewBag.Status = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryStatus>(null, null).ToString());
            ViewBag.PageStatus = PageStatus;
            return View();
        }
        #endregion

        #region 盘点单列表分页
        /// <summary>
        /// 盘点单列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>2017-08-07   吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult InventoryListPage(int id, int? status, string Keyword, int? PageStatus = 1)
        {
            var filter = new Pager<Model.InventorySheet.WhInventory>();
            if (PageStatus == 2) //查询数据录入信息
            {
                filter.PageFilter.Status = (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryStatus.数据录入;
            }
            else if (PageStatus == 3)
            {
                filter.PageFilter.Status = (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryStatus.编制报告;
            }
            else
            {
                filter.PageFilter.Status = status;
            }
            filter.CurrentPage = id;
            filter.PageFilter.Code = Keyword;
            var pager = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetSoOrders(filter);
            var list = new PagedList<Hyt.Model.InventorySheet.WhInventory>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            ViewBag.PageStatus = PageStatus;
            return PartialView("_InventoryList", list);
        }
        #endregion

        #region 编辑品牌 吴琨 2017-7-31

        #region 编辑品牌页面
        /// <summary>
        /// 编辑品牌页面 
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditInventory(string id)
        {
            Hyt.Model.InventorySheet.WhInventory model = new Hyt.Model.InventorySheet.WhInventory();
            ViewBag.Name = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryName>(null, null).ToString());
            ViewBag.Where = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryWhere>(null, null).ToString());
            //ViewBag.Type = GetSyUserList((int)Hyt.Model.WorkflowStatus.SystemStatus.用户组状态.启用);
            //if (!string.IsNullOrEmpty(id))
            //{
            //    model = WhInventoryBo.Instance.GetModelId(Guid.Parse(id));
            //}
            return View(model);
        }
        #endregion

        #region 新建盘点作业
        /// <summary>
        /// 新建盘点作业
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Privilege(PrivilegeCode.None)]
        public ActionResult AddWhInventory(string WhInventoryStr, int Type, string whereStr, string TypeIdStr, string brandIdStr)
        {
            if (string.IsNullOrEmpty(WhInventoryStr))
            {
                return Json(new Result { Status = false, Message = "请选择需要盘点的仓库!" }, JsonRequestBehavior.AllowGet);
            }
            List<CBPdProductStockList> pager = null;
            #region 获取库存商品
            if (Type == (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryType.仓位商品)
            {
                if (!string.IsNullOrEmpty(whereStr))
                {
                    whereStr = whereStr.Replace("物料代码", "p.ErpCode");
                    whereStr = whereStr.Replace("物料名称", "p.EasName");
                    whereStr = whereStr.Replace("仓位代码", "w.erpCode");
                    whereStr = whereStr.Replace("仓位名称", "w.WarehouseName");
                    whereStr = whereStr.Replace("条形码", "p.Barcode");
                }
                pager = PdProductStockBo.Instance.GetPdProductStockListData(WhInventoryStr.Trim(','), whereStr, "", "");
            }
            if (Type == (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryType.品牌)
            {
                if (string.IsNullOrEmpty(brandIdStr))
                {
                    return Json(new Result { Status = false, Message = "请选择需要盘点的商品品牌!" }, JsonRequestBehavior.AllowGet);
                }
                pager = PdProductStockBo.Instance.GetPdProductStockListData(WhInventoryStr.Trim(','), "", brandIdStr.Trim(','), "");
            }
            if (Type == (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryType.商品类别)
            {
                if (string.IsNullOrEmpty(TypeIdStr))
                {
                    return Json(new Result { Status = false, Message = "请选择需要盘点的商品类别!" }, JsonRequestBehavior.AllowGet);
                }
                pager = PdProductStockBo.Instance.GetPdProductStockListData(WhInventoryStr.Trim(','), "", "", TypeIdStr.Trim(','));
            }
            if (pager == null || pager.Count == 0)
            {
                return Json(new Result { Status = false, Message = "没有可用的数据,不能备份!" }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region 获取盘点代码
            var sl = (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryCount() + 1);
            var Code = "";
            if (sl.ToString().Length == 1)
            {
                Code = DateTime.Now.ToString("yyyyMMdd") + "000" + sl;
            }
            if (sl.ToString().Length == 2)
            {
                Code = DateTime.Now.ToString("yyyyMMdd") + "00" + sl;
            }
            if (sl.ToString().Length == 3)
            {
                Code = DateTime.Now.ToString("yyyyMMdd") + "0" + sl;
            }
            if (sl.ToString().Length == 4)
            {
                Code = DateTime.Now.ToString("yyyyMMdd") + sl;
            }
            #endregion

            #region 创建盘点作业
            Hyt.Model.InventorySheet.WhInventory model = new Hyt.Model.InventorySheet.WhInventory()
            {
                Code = Code,
                CreatedBy = CurrentUser.Base.SysNo,
                CreatedName = CurrentUser.Base.UserName,
                Status = (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryStatus.未处理,
                InventoryWhere = whereStr,
                WhInventorySysNo = WhInventoryStr.Trim(','),
                ProductTypeSysNo = TypeIdStr.Trim(','),
                ProductBrandSysNo = brandIdStr.Trim(','),
                AddTime = DateTime.Now
            };
            List<WhInventoryProduct> produceModelList = new List<WhInventoryProduct>();
            foreach (var item in pager)
            {
                var produceModel = new WhInventoryProduct()
                {
                    WarehouseSysNo = item.WarehouseSysNo,
                    ProductSysNo = item.ProductSysNo,
                    ZhangCunQuantity = Convert.ToDecimal(item.StockQuantity),
                    InventoryQuantity = Convert.ToDecimal(item.StockQuantity),
                    adjustmenQuantity = 0,
                    Quantity = 0,
                    Status = -1,
                    Remarks = ""
                };
                produceModelList.Add(produceModel);
            }
            int sysno = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.AddWhInventory(model, produceModelList);
            if (sysno > 0)
            {
                return Json(new Result { Status = true, Message = "成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new Result { Status = true, Message = "失败" }, JsonRequestBehavior.AllowGet);
            }
            #endregion
        }
        #endregion

        #endregion

        #region 品牌管理界面
        /// <summary>
        /// 品牌管理界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2017-08-8 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult BrandPage(int? id, int? status, string name)
        {
            int pageIndex = id ?? 1;
            if (status == null)
            {
                status = -1;
                // status = (int)ProductStatus.品牌状态.启用;
            }
            var pager = new Pager<PdBrand>
            {
                CurrentPage = pageIndex,
                PageFilter = new PdBrand { Status = (int)status, Name = string.IsNullOrEmpty(name) ? name : name.Trim() },
                PageSize = 100
            };
            if (status != null && !string.IsNullOrEmpty(name))
            {
                pager = BLL.Product.PdBrandBo.Instance.GetPdBrandList(pager);
            }

            return PartialView("_EditInventory", pager.Map());
        }
        #endregion

        #region 盘点单详情

        [Privilege(PrivilegeCode.None)]
        public ActionResult InventoryDetail(int? id, int? PageStatus = 1, int? PageIndex = 1)
        {
            int pageSize = 30;
            var model = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryDetail((int)id);
            ViewBag.SysNo = id;
            ViewBag.PageStatus = PageStatus;
            model.dataList = model.dataList.Take(pageSize * (int)PageIndex).Skip(pageSize * ((int)PageIndex - 1)).ToList();
            if (Request.IsAjaxRequest())
            {
                ResultList<WhInventoryProductDetail> result = new ResultList<WhInventoryProductDetail>();
                result.Data = model.dataList;
                if (model.dataList.Count > 0) result.StatusCode = 1; else result.StatusCode = 0;
                return Json(result);
            }
            return View(model);
        }
        #endregion

        #endregion

        #region 盘点数据录入
        [Privilege(PrivilegeCode.None)]
        public ActionResult UploadPDQuantity(int? id, decimal? Quantity, decimal? ZhangCunQuantity)
        {
            Result<object> result = new Result<object>();
            if (id == null || Quantity == null || ZhangCunQuantity == null)
            {
                result.Status = false;
                result.Message = "参数错误,修改失败!";
                return Json(result);
            }

            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UploadPDQuantity((int)id, Convert.ToDecimal(Quantity), (decimal)ZhangCunQuantity))
            {
                result.Status = true;
                result.Message = "修改成功!";
                return Json(result);
            }
            else
            {
                result.Status = false;
                result.Message = "错误,修改失败!";
                return Json(result);
            }
        }
        #endregion

        #region 更新调整数量/实际库存
        [Privilege(PrivilegeCode.None)]
        public ActionResult UploadSJQuantity(int? id, decimal? Quantity)
        {
            Result<object> result = new Result<object>();
            if (id == null || Quantity == null)
            {
                result.Status = false;
                result.Message = "参数错误,修改失败!";
                return Json(result);
            }
            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UploadSJQuantity((int)id, (decimal)Quantity))
            {
                result.Status = true;
                result.Message = "修改成功!";
                return Json(result);
            }
            else
            {
                result.Status = false;
                result.Message = "错误,修改失败!";
                return Json(result);
            }
        }
        #endregion

        #region 更新盘点单状态
        [Privilege(PrivilegeCode.None)]
        public ActionResult UploadStatus(int? id, int? status)
        {
            Result<object> result = new Result<object>();
            if (id == null || status == null)
            {
                result.Status = false;
                result.Message = "参数错误,修改失败!";
                return Json(result);
            }
            #region 更新盘点单状态
            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UploadStatus((int)id, (int)status))
            {
                result.Status = true;
                result.Message = "已通过!";
                return Json(result);
            }
            else
            {
                result.Status = false;
                result.Message = "错误,修改失败!";
                return Json(result);
            }
            #endregion
        }
        #endregion

        #region 盘点报告单

        #region 生成盘点报告单
        /// <summary>
        /// 生成盘点报告单
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public ActionResult AddWhInventoryRepor(int id, int pdStatus)
        {
            Result<object> result = new Result<object>();
            #region 生成盘点报告单
            //获取盘点单信息
            var whlist = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryDetail((int)id);
            if (whlist == null || whlist.dataList == null || whlist.dataList.Count == 0)
            {
                result.Status = false;
                result.Message = "没有适合盘点的商品信息,报告单生成失败!";
                return Json(result);
            }
            if (pdStatus == 1)
            { //生成盘盈
                if (!Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetIsWhInventoryRepor(whlist.Code, pdStatus))
                {
                    whlist.dataList = whlist.dataList.Where(p => p.InventoryQuantity > p.ZhangCunQuantity).ToList();
                }
                else
                {
                    result.Status = false;
                    result.Message = "已生成过盘盈报告单,生成失败!";
                    return Json(result);
                }

            }
            else
            { //生成盘亏
                if (!Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetIsWhInventoryRepor(whlist.Code, pdStatus))
                {
                    whlist.dataList = whlist.dataList.Where(p => p.InventoryQuantity < p.ZhangCunQuantity).ToList();
                }
                else
                {
                    result.Status = false;
                    result.Message = "已生成过盘亏报告单,生成失败!";
                    return Json(result);
                }

            }
            if (whlist.dataList == null || whlist.dataList.Count == 0)
            {
                result.Status = false;
                result.Message = "没有适合盘点的商品信息,报告单生成失败!";
                return Json(result);
            }

            #region 盘点报告单信息
            WhInventoryRepor model = new WhInventoryRepor()
            {
                WhInventoryCode = whlist.Code,
                Remarks = "",
                ReportType = 0,
                PrintCount = 0,
                WarehouseCode = whlist.WhInventorySysNo,
                WarehouseName = "",
                Time = DateTime.Now,
                Make = CurrentUser.Base.UserName,
                Audit = "",
                Tally = "",
                CustodySysNo = "",
                HeadSysNo = "",
                AgentSysNo = "",
                AddUser = CurrentUser.Base.SysNo,
                AuditTime = null,
                YingKuiStatus = pdStatus,
                Status = pdStatus == 1 ? (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus.盘盈入库 : (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus.盘亏入库,
                AddTime = DateTime.Now
            };
            #endregion

            #region 盘点报告单商品信息
            List<WhIReporPrDetails> WhIReporPrDetailsList = new List<WhIReporPrDetails>();
            foreach (var item in whlist.dataList)
            {
                WhIReporPrDetails productList = new WhIReporPrDetails()
                {
                    WarehouseSysNo = item.WarehouseSysNo,
                    ProduceCode = item.ErpCode,
                    ProductName = item.ProductName,
                    Specification = "",
                    AuxiliaryAttribute = "",
                    BatchNumber = "",
                    Unit = "",
                    ADQuantity = item.ZhangCunQuantity,
                    RealityQuantity = item.InventoryQuantity,
                    PlanPrice = item.CostPrice,
                    //UnitPrice = 0,
                    ProfitAndLoss = item.adjustmenQuantity,
                    ProfitAndLossPrice = (item.CostPrice * item.adjustmenQuantity) - (item.CostPrice * item.ZhangCunQuantity),
                    Remark = "",
                    ProcurementTime = null,
                    //ShelfLife=
                    ValidityTime = null,
                    WarehouseName = item.WarehouseNameDate
                };
                WhIReporPrDetailsList.Add(productList);
            }
            #endregion
            if (!Hyt.BLL.InventorySheet.WhInventoryBo.Instance.AddWhInventoryRepor(model, WhIReporPrDetailsList))
            {
                result.Status = false;
                result.Message = "错误,生成失败!";
                return Json(result);
            }
            else
            {
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
            #endregion
        }
        #endregion

        #region 盘点报告单页面
        /// <summary>
        /// 盘点报告单页面 
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult WhInventoryRepor()
        {
            ViewBag.Status = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus>(null, null).ToString());
            return View();
        }
        #endregion

        #region 盘点报告单列表分页
        /// <summary>
        /// 盘点报告单列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>用户列表</returns>
        /// <remarks>2017-08-11   吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult WhInventoryReporPage(int id, int? status)
        {

            var filter = new Pager<WhInventoryRepor>();
            filter.CurrentPage = id;
            filter.PageFilter.Status = status;
            var pager = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryReporPage(filter);
            foreach (var item in pager.Rows)
            {
                item.PanYingStatus = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryReporModel(item.SysNo, 1).DataList.Count > 0;
                item.PanKuiStatus = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryReporModel(item.SysNo, 2).DataList.Count > 0;
            }
            var list = new PagedList<WhInventoryRepor>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_WhInventoryRepor", list);
        }
        #endregion

        #region 盘点报告单维护
        /// <summary>
        /// 盘点报告单维护
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>用户列表</returns>
        /// <remarks>2017-08-11   吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditWhInventoryRepor(int? id, int? PageType = 1)
        {
            WhInventoryRepor model = new WhInventoryRepor();
            if (id != null)
            {
                model = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryReporModel((int)id, (int)PageType);
            }
            ViewBag.PageType = PageType;
            ViewBag.AdminName = CurrentUser.Base.UserName;
            return View(model);
        }
        #endregion

        #endregion

        #region 修改盘点报告单状态
        /// <summary>
        /// 修改盘点报告单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public ActionResult UpdateWhInventoryReporStatus(WhInventoryRepor model, int? id, int? status)
        {
            Result<object> result = new Result<object>();
            if (id == null || status == null)
            {
                result.Status = false;
                result.Message = "参数错误,修改失败!";
                return Json(result);
            }
            #region  修改盘点报告单
            var getModel = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryRepor((int)id);
            getModel.Status = (int)status;
            getModel.Tally = model.Tally;
            getModel.CustodySysNo = model.CustodySysNo;
            getModel.HeadSysNo = model.HeadSysNo;
            getModel.AgentSysNo = model.AgentSysNo;
            getModel.AuditTime = model.AuditTime;
            getModel.Remarks = model.Remarks;

            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UploadWhInventoryRepor(getModel))
            {
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
            else
            {
                result.Status = false;
                result.Message = "错误,提交失败!";
                return Json(result);
            }
            #endregion
        }
        #endregion

        #region 盘点报告审核通过修改库存
        /// <summary>
        /// 盘点报告审核通过修改库存
        /// </summary>
        /// <param name="sysNo">盘点报告单id</param>
        /// <param name="status">盈亏状态</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public ActionResult CompleteWhIReporPrDetails(int? sysNo, int? status)
        {
            Result<object> result = new Result<object>();
            ///获取盘点报告商品信息
            List<WhIReporPrDetails> model = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhIReporPrDetailsPid((int)sysNo, (int)status);

            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UpdatePdProductStock(model))
            {
                ///更新状态
                Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UploadWhInventoryReporStatus((int)sysNo, (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus.完成);
                //UploadStatus  (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryStatus.数据录入
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
            else
            {
                result.Status = false;
                result.Message = "错误,修改失败!";
                return Json(result);
            }

        }
        #endregion

        #region 盘点报告审核通过修改库存
        /// <summary>
        /// 盘点报告审核通过修改库存
        /// </summary>
        /// <param name="sysNo">盘点报告单id</param>
        /// <param name="status">盈亏状态</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public ActionResult GetWhWarehouse(int sysNo, int? whSysId)
        {
            ResultList<uditPdProductStock> result = new ResultList<uditPdProductStock>();
            result.Status = true;
            result.Message = "成功!";
            result.Data = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetuditPdProductStock(sysNo, whSysId);
            return Json(result);
        }
        #endregion

        #region 新增盘点报告单
        /// <summary>
        /// 新增盘点报告单
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public ActionResult AddWhInventoryReporList(WhInventoryRepor model, string productCode, string ProductName, string CSysId, string CSysName, string CErpCode, string CZCount, string ShiSum, string DanJia, string PanYingCount, string PanYingJinE, string BeiZhu,int pdStatus)
        {
            Result<object> result = new Result<object>();
            model.Status = pdStatus == 5 ? (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus.盘盈入库 : (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus.盘亏入库;
            model.WarehouseCode = CSysId.Trim(',');
            model.AddUser = CurrentUser.Base.SysNo;
            model.YingKuiStatus = pdStatus == 5 ? 1 : 2;
            model.AddTime = DateTime.Now;

            List<WhIReporPrDetails> productModelList = new List<WhIReporPrDetails>();
            for (int i = 0; i < productCode.Trim(',').Split(',').Length; i++)
            {
                var productModel = new WhIReporPrDetails()
                {
                    ProduceCode = productCode.Trim(',').Split(',')[i],
                    ProductName = ProductName.Trim(',').Split(',')[i],
                    Specification = "",
                    AuxiliaryAttribute = "",
                    BatchNumber = "",
                    Unit = "",
                    ADQuantity = Convert.ToDecimal(CZCount.Trim(',').Split(',')[i]),
                    RealityQuantity = Convert.ToDecimal(ShiSum.Trim(',').Split(',')[i]),
                    PlanPrice = Convert.ToDecimal(DanJia.Trim(',').Split(',')[i]),
                    UnitPrice = Convert.ToDecimal(DanJia.Trim(',').Split(',')[i]),
                    ProfitAndLoss = Convert.ToDecimal(PanYingCount.Trim(',').Split(',')[i]),
                    ProfitAndLossPrice = Convert.ToDecimal(PanYingJinE.Trim(',').Split(',')[i]),
                    Remark = BeiZhu.Trim('^').Split('^')[i],
                    WarehouseName = CSysName.Split(',')[i],
                    WarehouseSysNo = Convert.ToInt32(CSysId.Trim(',').Split(',')[i])
                };
                productModelList.Add(productModel);
            }
            if (!Hyt.BLL.InventorySheet.WhInventoryBo.Instance.AddWhInventoryRepor(model, productModelList))
            {
                result.Status = false;
                result.Message = "错误,生成失败!";
                return Json(result);
            }
            else
            {
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
        }
        #endregion

        #region 导入盈亏入库商品Excel
        public static bool _starting;
        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-08-15 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult ImportExcel()
        {
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Resuldt<uditPdProductStock>();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {
                        result = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.ImportExcel(httpPostedFileBase.InputStream,
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
        /// 导出盈亏入库模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2017-08-15 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public void ExportTemplate()
        {
            ExportExcel(@"\Templates\Excel\WhIReporPrDetails.xls", "盈亏入库导入模板");
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

        #region 盘点商品数据录入导出Excel
        [Privilege(PrivilegeCode.None)]
        public void ExportPurchaseData(int? id)
        {
            Hyt.Model.InventorySheet.WhInventoryDetail model = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetWhInventoryDetail((int)id);
            BLL.InventorySheet.WhInventoryBo.Instance.ExportPurchaseData(model, "", 0);
        }
        #endregion

        #region 导入盘点商品录入Excel
        public static bool _startingTo;
            /// <summary>
        /// 导入Excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-08-15 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult ImportWhInventoryExcel()
        {
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Resuldt<WhInventoryProductDetailOutput>();
                if (!_startingTo)
                {
                    _startingTo = true;
                    try
                    {
                        result = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.ImportExcelTo(httpPostedFileBase.InputStream,
                            CurrentUser.Base.SysNo);
                    }
                    catch (Exception e)
                    {
                        result.Message = string.Format(e.Message);
                        result.Status = false;
                    }
                    finally
                    {
                        _startingTo = false;
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
        #endregion

        #region 公用模块
        #region 选择用户
        [Privilege(PrivilegeCode.None)]
        public ActionResult GetUserPageIndex(int? id, int? status, int? levelSysNo, int? emailStatus, int? mobilePhoneStatus, int? isReceiveEmail, int? isReceiveShortMessage, int? isPublicAccount, int? isLevelFixed, int? isExperiencePointFixed, int? isExperienceCoinFixed, string account = null, int agentSysNo = 0, int dealerSysNo = 0)
        {
            ParaSyUserFilter filter = new ParaSyUserFilter();
            filter.Id = id==null?1:(int)id;
            filter.Status = status;
            filter.Account = account;
            IDictionary<int, string> dictStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.会员状态));
            ViewBag.dictStatus = dictStatus;
            var pager = SyUserBo.Instance.GetSyUser(filter);
            var list = new PagedList<CBSyUser>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            }; 
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrCustomer", list);
            }
            return View(list);
        }
        #endregion
        #endregion

        #region 其他出入库
        [Privilege(PrivilegeCode.None)]
        public ActionResult OtherOutOfStorage(int? id, string keyWord,int? status, int? PageType = (int)Hyt.Model.InventorySheet.OtherOutOfStorageTypeEnum.其他入库)
        {
            var list = new PagedList<OtherOutOfStorage>();
            if (Request.IsAjaxRequest())
            {
                var filter = new Pager<OtherOutOfStorage>();
                filter.CurrentPage = (int)id;
                filter.PageFilter.Code = keyWord;
                filter.PageFilter.Type = (int)PageType;
                filter.PageFilter.Status = status!=null?(int)status:-1;
                var pager = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetOtherOutOfStoragePage(filter, PageType);
                list = new PagedList<OtherOutOfStorage>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
                return PartialView("_OtherOutOfStorage", list);
            }
            ViewBag.Status = MvcHtmlString.Create(MvcCreateHtml.EnumToString<OtherOutOfStorageStatusEnum>(null, null).ToString());
            ViewBag.PageType = PageType;
            return View(list);
        }
        #endregion

        #region  编辑其他出入库
        /// <summary>
        /// 编辑其他出入库
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>2017-08-16   吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult EditOtherOutOfStorage(int? id,int? PageType = (int)Hyt.Model.InventorySheet.OtherOutOfStorageTypeEnum.其他入库)
        {
            OtherOutOfStorage model = new OtherOutOfStorage();
            if (id != null)
            {
                model = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetOtherOutOfStorageModel((int)id);
            }
            ViewBag.PageType = PageType;
            ViewBag.AdminName = CurrentUser.Base.UserName;
            ViewBag.AdminSysNo = CurrentUser.Base.SysNo;
            return View(model);
        }
        #endregion

        #region 导出其他出入库模板
        /// <summary>
        /// 导出其他出入库模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2017-08-17 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public void ExportTemplateTo()
        {
            ExportExcel(@"\Templates\Excel\OtherOutOfStorage.xls", "其他出入库模板");
        }
        #endregion

        #region 导入其他出入库Excel
        /// <summary>
        /// 导入其他出入库Excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-08-17 吴琨 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult OtherImportExcel()
        {
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Resuldt<OtherOutOfStorageDetailed>();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {
                        result = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.OtherImportExcel(httpPostedFileBase.InputStream,
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
        #endregion

        #region 维护其他入库
        /// <summary>
        /// 维护其他入库
       /// </summary>
       /// <param name="model">其他入库实体</param>
       /// <param name="ProductSysNo">商品系统编号</param>
       /// <param name="ProductCode">商品代码</param>
       /// <param name="BarCode">商品二维码</param>
       /// <param name="ProductName">商品名称</param>
       /// <param name="Count">实收数量</param>
       /// <param name="UnitPrice">单价</param>
       /// <param name="Price">价格</param>
       /// <param name="Remarks">备注</param>
       /// <param name="CollectWarehouseSysNo">仓库系统编号</param>
       /// <param name="CollectWarehouseName">仓库名称</param>
       /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult AddOtherOutOfStorage(OtherOutOfStorage model, string ProductSysNo, string ProductCode, string BarCode, string ProductName, string Count, string UnitPrice, string Price, string Remarks, string CollectWarehouseSysNo, string CollectWarehouseName,int? PageType)
        {
            Result<object> result = new Result<object>();

            #region 其他出入库信息
            model.Type = PageType == (int)OtherOutOfStorageTypeEnum.其他入库 ? (int)OtherOutOfStorageTypeEnum.其他入库 : (int)OtherOutOfStorageTypeEnum.其他出库;
            model.AddTime = DateTime.Now;
            model.Status = (int)OtherOutOfStorageStatusEnum.审核中;
            #endregion

            #region 其他出入库商品明细
            model.ListData = new List<OtherOutOfStorageDetailed>();
            for (int i = 0; i < ProductSysNo.Trim(',').Split(',').Length; i++)
            {
               OtherOutOfStorageDetailed product = new OtherOutOfStorageDetailed();
               product.ProductSysNo = Convert.ToInt32(ProductSysNo.Trim(',').Split(',')[i]);
               product.ProductCode = ProductCode.Trim(',').Split(',')[i];
               product.BarCode = BarCode.Trim(',').Split(',')[i];
               product.ProductName = ProductName.Trim(',').Split(',')[i];
               product.Count = Convert.ToDecimal(Count.Trim(',').Split(',')[i]);
               product.UnitPrice = Convert.ToDecimal(UnitPrice.Trim(',').Split(',')[i]);
               product.Price = Convert.ToDecimal(Price.Trim(',').Split(',')[i]);
               product.Remarks = Remarks.Trim('^').Split('^')[i];
               product.CollectWarehouseSysNo = Convert.ToInt32(CollectWarehouseSysNo.Trim(',').Split(',')[i]);
               product.CollectWarehouseName = CollectWarehouseName.Trim(',').Split(',')[i];
               model.ListData.Add(product);
            }
            #endregion
            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.AddOtherOutOfStorage(model) > 0)
            {
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
            else {
                result.Status = false;
                result.Message = "错误,提交失败!";
                return Json(result);
            }
        }
        #endregion

        #region 其他出入库审核

        /// <summary>
        /// 盘点报告审核通过修改库存
        /// </summary>
        /// <param name="sysNo">盘点报告单id</param>
        /// <param name="status">盈亏状态</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public ActionResult CompleteOtherOutOfStorage(int? sysNo)
        {
            Result<object> result = new Result<object>();
            ///获取盘点报告商品信息
            var  model = Hyt.BLL.InventorySheet.WhInventoryBo.Instance.GetOtherOutOfStorageModel((int)sysNo); ;
            model.Status = (int)OtherOutOfStorageStatusEnum.完成;
            model.ToexamineSysNo = CurrentUser.Base.SysNo;
            model.ToexamineName = CurrentUser.Base.UserName;
            model.ToexamineTime = DateTime.Now;
            if (Hyt.BLL.InventorySheet.WhInventoryBo.Instance.UpdateOtherOutPdProductStock(model))
            {
                result.Status = true;
                result.Message = "成功!";
                return Json(result);
            }
            else
            {
                result.Status = false;
                result.Message = "错误,修改失败!";
                return Json(result);
            }
        }
        #endregion

        #region  查询公用模块
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        /// 2017-8-24 吴琨
        [Privilege(PrivilegeCode.None)]
        public ActionResult GetUtilLike(int type,string keyWord)
        {
            if (type == 1)
            {//查询会员 
               var list = SyUserBo.Instance.GetUtilLike(keyWord.Trim());
               return Json(list);
            }
            else
            {//查询商品
              var list = PdProductBo.Instance.GetUtilLikePdProduct(keyWord.Trim());
              return Json(list);
            }
        }



        /// <summary>
        /// 失去焦点后检查
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        /// 2017-8-24 吴琨
        [Privilege(PrivilegeCode.None)]
        public ActionResult GetUtilInspect(int type, string keyWord)
        {
            if (type == 1)
            {//查询会员 
                var list = SyUserBo.Instance.GetSyUsersByName(keyWord.Trim());
                return Json(list);
            }
            else
            {//查询商品
                var list = PdProductBo.Instance.GetUtilLikePdProductCode(keyWord.Trim());
                return Json(list);
            }
        }


        #endregion




    }




}
