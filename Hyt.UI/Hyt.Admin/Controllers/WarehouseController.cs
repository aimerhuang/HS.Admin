using Extra.SMS;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.Config;
using Hyt.BLL.Distribution;
using Hyt.BLL.Finance;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Order;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.Infrastructure.Communication;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.FileProcessor;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.Model.Common;
using Hyt.DataAccess.Warehouse;
using Hyt.Admin.Models;
using System.Data;
using Hyt.Util.Extension;
using Hyt.Model.Generated;
using Hyt.Model.Order;
using Hyt.BLL.InventorySheet;
using System.IO;
using Hyt.Model.ExpressList;
using System.Threading.Tasks;
namespace Hyt.Admin.Controllers
{
    public class WarehouseController : BaseController
    {
        //
        // GET: /Warehouse/

        #region 采购退货出库管理 王耀发 2016-06-24 创建
        [Privilege(PrivilegeCode.None)]
        public ActionResult WhInventoryOutList(int? id, ParaInventoryOutFilter filter)
        {
            var wareHouseList = CurrentUser.Warehouses;
            if (Request.IsAjaxRequest())
            {
                filter.CurrentPage = id ?? 1;
                var pageList = new PagedList<WhInventoryOut>();

                if (filter.SysNo.HasValue) //优先查询条件为出库单编号
                {
                    var model = WhInventoryOutBo.Instance.GetWhInventoryOut(filter.SysNo.Value);
                    if (null != model)
                    {
                        var result = new List<WhInventoryOut> { model };
                        pageList.TData = result;
                        pageList.TotalItemCount = result.Count;
                        pageList.CurrentPageIndex = filter.CurrentPage;
                    }
                }
                else
                {
                    filter.WarehouseSysNoList = new List<int>();
                    if (filter.WarehouseSysNo.HasValue)
                    {
                        filter.WarehouseSysNoList.Add(filter.WarehouseSysNo.Value);
                    }
                    else
                    {
                        if (wareHouseList.Count > 0)
                        {
                            wareHouseList.ForEach(x => filter.WarehouseSysNoList.Add(x.SysNo));
                        }
                    }
                    pageList = WhInventoryOutBo.Instance.GetWhInventoryOutList(filter, pageList.PageSize);
                }
                return PartialView("_AjaxPagerInventoryOutList", pageList);
            }
            else
            {
                ViewBag.Warehouse = wareHouseList;
                InitInventoryOutPageViewData();
            }
            return View();
        }

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        /// <param name=" "></param>
        /// <returns></returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        private void InitInventoryOutPageViewData()
        {
            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };

            //出库单状态下拉绑定数据
            var statustList = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<WarehouseStatus.采购退货出库单状态>(ref statustList);
            ViewData["Status"] = new SelectList(statustList, "Value", "Text");

            //是否打印下拉绑定数据
            //var isPrintList = new List<SelectListItem>()
            //            {
            //                item,
            //                new SelectListItem() {Text = @"未打印", Value = "0"},
            //                new SelectListItem() {Text = @"已打印", Value = "1"},
            //            };
            //ViewData["IsPrinted"] = new SelectList(isPrintList, "Value", "Text");

            //页面来源单据类型绑定数据
            //var selectList = new List<SelectListItem>() { item };

            //EnumUtil.ToListItem<WarehouseStatus.入库单据类型>(ref selectList);
            //ViewData["SourceType"] = new SelectList(selectList, "Value", "Text");
        }

        /// <summary>
        /// 商品出库
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="id">当前页码</param>
        /// <returns>返回出库单</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>  
        [Privilege(PrivilegeCode.None)]
        public ActionResult InventoryOutCreate(int sysNo, int? id)
        {
            if (Request.IsAjaxRequest())
            {
                var model = new PagedList<WhInventoryOutItem> { PageSize = int.MaxValue, CurrentPageIndex = id ?? 1 };
                model = WhInventoryOutBo.Instance.GetInventoryOutItemList(sysNo, model.CurrentPageIndex, model.PageSize);
                return PartialView("_AjaxPagerInventoryOutItem", model);
            }
            else
            {
                var data = GetInventoryOutDetails(sysNo);
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(data.WarehouseSysNo))
                {
                    var message = string.Format("没有权限读取仓库:{0}的信息",
                                                WhWarehouseBo.Instance.GetWarehouseName(data.WarehouseSysNo));
                    return View("ErrorPrivilegeWithMessage", (object)(message));
                }
                return View(data);
            }
        }


        /// <summary>
        /// 商品出库
        /// </summary>
        /// <param name="model">出库单实体</param>
        /// <returns>返回操作状态(Result.StatusCode大于等行0成功,小于0失败)</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult InventoryOutComplete(WhInventoryOut model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    WhInventoryOutBo.Instance.InventoryOutComplete(model.SysNo, model.ItemList, CurrentUser.Base);
                    result.StatusCode = 0;
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "商品出库", LogStatus.系统日志目标类型.出库单, model.SysNo,
                                         CurrentUser.Base.SysNo);
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "商品出库" + ex.Message, LogStatus.系统日志目标类型.出库单, model.SysNo, ex);
            }
            return Json(result);
        }


        /// <summary>
        /// 作废出库单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>返果结果</returns>
        /// <remarks>2016-06-27 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public JsonResult InventoryOutCancel(int sysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    var model = WhInventoryOutBo.Instance.InventoryOutCancel(sysNo, CurrentUser.Base);
                    if (model)
                    {
                        result.StatusCode = 0;
                        result.Status = true;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废出库单", LogStatus.系统日志目标类型.出库单, sysNo,
                                             CurrentUser.Base.SysNo);
                        scope.Complete();
                    }
                    else
                    {
                        result.Message = string.Format("出库单{0}作废失败.", sysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "作废出库单" + ex.Message, LogStatus.系统日志目标类型.出库单, sysNo, ex);
            }

            return Json(result);
        }
        #endregion

        #region 商品入库管理 周唐炬 2013-06-09 创建

        /// <summary>
        /// 入库单列表管理
        /// </summary>
        /// <param name="id">查询参数</param>
        /// <param name="filter">查询参数</param>
        /// <returns>返回入库单列表</returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult InStockManage(int? id, ParaInStockFilter filter)
        {
            var wareHouseList = CurrentUser.Warehouses;
            if (Request.IsAjaxRequest())
            {
                filter.CurrentPage = id ?? 1;
                var pageList = new PagedList<WhStockIn>();

                if (filter.SysNo.HasValue) //优先查询条件为入库单编号
                {
                    var model = InStockBo.Instance.GetStockIn(filter.SysNo.Value);
                    if (null != model)
                    {
                        var result = new List<WhStockIn> { model };
                        pageList.TData = result;
                        pageList.TotalItemCount = result.Count;
                        pageList.CurrentPageIndex = filter.CurrentPage;
                    }
                }
                else
                {
                    filter.WarehouseSysNoList = new List<int>();
                    if (filter.WarehouseSysNo.HasValue)
                    {
                        filter.WarehouseSysNoList.Add(filter.WarehouseSysNo.Value);
                    }
                    else
                    {
                        if (wareHouseList.Count > 0)
                        {
                            wareHouseList.ForEach(x => filter.WarehouseSysNoList.Add(x.SysNo));
                        }
                    }
                    pageList = InStockBo.Instance.GetStockInList(filter, pageList.PageSize);
                }
                return PartialView("_AjaxPagerInStockList", pageList);
            }
            else
            {
                ViewBag.Warehouse = wareHouseList;
                InitInStockPageViewData();
            }
            return View();
        }

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        /// <param name=" "></param>
        /// <returns></returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>
        private void InitInStockPageViewData()
        {
            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };

            //入库单状态下拉绑定数据
            var statustList = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<WarehouseStatus.入库单状态>(ref statustList);
            ViewData["Status"] = new SelectList(statustList, "Value", "Text");

            //是否打印下拉绑定数据
            var isPrintList = new List<SelectListItem>()
                        {
                            item,
                            new SelectListItem() {Text = @"未打印", Value = "0"},
                            new SelectListItem() {Text = @"已打印", Value = "1"},
                        };
            ViewData["IsPrinted"] = new SelectList(isPrintList, "Value", "Text");

            //页面来源单据类型绑定数据
            var selectList = new List<SelectListItem>() { item };

            EnumUtil.ToListItem<WarehouseStatus.入库单据类型>(ref selectList);
            ViewData["SourceType"] = new SelectList(selectList, "Value", "Text");
        }

        /// <summary>
        /// 商品入库
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <param name="id">当前页码</param>
        /// <returns>返回入库单</returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>  
        [Privilege(PrivilegeCode.WH1006301)]
        public ActionResult InStockCreate(int sysNo, int? id)
        {
            if (Request.IsAjaxRequest())
            {
                var model = new PagedList<WhStockInItem> { PageSize = int.MaxValue, CurrentPageIndex = id ?? 1 };
                model = InStockBo.Instance.GetStockInItemList(sysNo, model.CurrentPageIndex, model.PageSize);
                return PartialView("_AjaxPagerInStockItem", model);
            }
            else
            {
                var data = GetInStockDetails(sysNo);
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(data.WarehouseSysNo))
                {
                    var message = string.Format("没有权限读取仓库:{0}的信息",
                                                WhWarehouseBo.Instance.GetWarehouseName(data.WarehouseSysNo));
                    return View("ErrorPrivilegeWithMessage", (object)(message));
                }
                return View(data);
            }
        }

        /// <summary>
        /// 入库单明细
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <param name="id">当前页码</param>
        /// <returns>返回入库单明细</returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult InStockDetails(int sysNo, int? id)
        {
            if (Request.IsAjaxRequest())
            {
                var model = new PagedList<WhStockInItem>() { PageSize = int.MaxValue, CurrentPageIndex = id ?? 1 };
                model = InStockBo.Instance.GetStockInItemList(sysNo, model.CurrentPageIndex, model.PageSize);
                return PartialView("_AjaxPagerInStockItemDetail", model);
            }
            else
            {
                var data = GetInStockDetails(sysNo);
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(data.WarehouseSysNo))
                {
                    var message = string.Format("没有权限读取仓库:{0}的信息",
                                                WhWarehouseBo.Instance.GetWarehouseName(data.WarehouseSysNo));
                    return View("ErrorPrivilegeWithMessage", (object)(message));
                }
                return View(data);
            }
        }

        ///  <summary>
        ///  入库单明细
        ///  </summary>
        ///  <param name="id">入库单系统编号</param>
        /// <returns>返回入库单明细</returns>
        ///  <remarks>2013-06-14 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        private WhStockIn GetInStockDetails(int id)
        {
            var data = InStockBo.Instance.GetStockIn(id);
            if (null != data)
            {
                ViewBag.Status = ((WarehouseStatus.入库单状态)data.Status).ToString();
            }
            return data;
        }

        /// <summary>
        /// 商品入库
        /// </summary>
        /// <param name="model">入库单实体</param>
        /// <returns>返回操作状态(Result.StatusCode大于等行0成功,小于0失败)</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        /// <remarks>2013-07-18 朱成果 修改</remarks>
        [Privilege(PrivilegeCode.WH1006301)]
        public JsonResult InStockComplete(WhStockIn model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var options = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.DefaultTimeout
                    };

                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    InStockBo.Instance.InStockComplete(model.SysNo, model.InvoiceSysNo, model.ItemList, CurrentUser.Base);
                    //result = SavePdProductStockInByReturn(model.WarehouseSysNo, model.ItemList, CurrentUser.Base);
                    result.StatusCode = 0;
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "商品入库", LogStatus.系统日志目标类型.入库单, model.SysNo,
                                         CurrentUser.Base.SysNo);
                    scope.Complete();
                }

                #region 库存入库平台

                var productList = model.ItemList.Select(x => x.ProductSysNo).ToList();
                var products = BLL.Product.PdProductBo.Instance.GetProductListBySysnoList(productList);
                if (products.Count != productList.Count)
                {
                    result.Status = false;
                    result.Message = "产品信息有误！";
                    return Json(result);
                }


                var data = model.ItemList.Select(x => new WhStockInItem()
                {
                    RealStockInQuantity = x.RealStockInQuantity,
                    ProductErpCode = products.Where(y => y.SysNo == x.ProductSysNo).FirstOrDefault().ErpCode,
                }).ToList();


                var reslut = BLL.Warehouse.WhWarehouseBo.Instance.ReduceStock(1, model.WarehouseSysNo, null, data);
                if (!reslut.Status)
                    throw new HytException(reslut.Message);
                #endregion


            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "商品入库" + ex.Message, LogStatus.系统日志目标类型.入库单, model.SysNo, ex);
            }
            return Json(result);
        }
        /// <summary>
        /// 退货商品入库
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="ItemList"></param>
        /// <param name="user"></param>
        /// <returns>2015-09-09 王耀发 添加</returns>
        public Result SavePdProductStockInByReturn(int WarehouseSysNo, List<Model.WhStockInItem> ItemList, SyUser user)
        {
            Result result = new Result();
            PdProductStockIn model = new PdProductStockIn();
            model.StockInNo = DateTime.Now.ToFileTimeUtc().ToString();
            model.StorageTime = DateTime.Now;
            try
            {
                result = PdProductStockInBo.Instance.SavePdProductStockIn(model, CurrentUser.Base);
                if (result.Status)
                {
                    foreach (Model.WhStockInItem Item in ItemList)
                    {
                        PdProductStockInDetail modelInDetail = new PdProductStockInDetail();
                        modelInDetail.ProductStockInSysNo = result.StatusCode;
                        modelInDetail.WarehouseSysNo = WarehouseSysNo;
                        modelInDetail.PdProductSysNo = Item.ProductSysNo;
                        modelInDetail.StorageQuantity = Item.RealStockInQuantity;
                        PdProductStockInDetailBo.Instance.SavePdProductStockInDetail(modelInDetail, CurrentUser.Base);
                        /*保存到库存中*/
                        PdProductStock smodel = new PdProductStock();
                        smodel.WarehouseSysNo = WarehouseSysNo;
                        smodel.PdProductSysNo = Item.ProductSysNo;
                        smodel.StockQuantity = Item.RealStockInQuantity;
                        PdProductStockBo.Instance.SavePdProductStock(smodel, CurrentUser.Base);
                    }
                }
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 作废入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>
        /// <remarks>2013-07-18 朱成果 修改</remarks>
        [Privilege(PrivilegeCode.WH1006501)]
        public JsonResult InStockCancel(int sysNo)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                var options = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.DefaultTimeout
                    };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    var model = InStockBo.Instance.InStockCancel(sysNo, CurrentUser.Base);
                    if (model)
                    {
                        result.StatusCode = 0;
                        result.Status = true;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废入库单", LogStatus.系统日志目标类型.入库单, sysNo,
                                             CurrentUser.Base.SysNo);
                        scope.Complete();
                    }
                    else
                    {
                        result.Message = string.Format("入库单{0}作废失败.", sysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "作废入库单" + ex.Message, LogStatus.系统日志目标类型.入库单, sysNo, ex);
            }

            return Json(result);
        }

        #endregion

        #region 借货单管理 周唐炬 2013-07-09 创建

        /// <summary>
        /// 商品还货
        /// </summary>
        /// <param name=" "></param>
        /// <returns>商品还货视图</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004202)]
        public ActionResult ProductReturn()
        {
            try
            {
                if (null != CurrentUser)
                {
                    ViewBag.Warehouse = AdminAuthenticationBo.Instance.Current.Warehouses;
                }
            }
            catch (Exception ex)
            {
                if (CurrentUser != null)
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, "商品还货" + ex.Message, LogStatus.系统日志目标类型.用户,
                                          CurrentUser.Base.SysNo, ex);
            }
            return View();
        }

        /// <summary>
        /// 商品还货
        /// </summary>
        /// <param name="model">入库单实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1004202)]
        public JsonResult ProductReturn(WhStockIn model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (null != model && null != CurrentUser)
                {
                    //配送员系统编号
                    var deliverymanSysno = model.CreatedBy;
                    var currentUser = CurrentUser.Base;
                    model.CreatedBy = model.LastUpdateBy = currentUser.SysNo;

                    result = ProductLendBo.Instance.ProductReturn(deliverymanSysno, model);
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "商品还货", LogStatus.系统日志目标类型.商品还货, model.SysNo,
                                         CurrentUser.Base.SysNo);

                }
                else
                {
                    result.Message = "商品还货数据有误，请重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "商品还货" + ex.Message, LogStatus.系统日志目标类型.入库单, model.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 获取配送员借货商品列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="products">需要过滤商品列表</param>
        /// <returns>配送员借货商品列表</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004202)]
        public PartialViewResult ProductLendItems(int deliveryUserSysNo, int warehouseSysNo, int[] products)
        {
            try
            {
                var list = WhWarehouseBo.Instance.GetProductLendItmeList(deliveryUserSysNo, warehouseSysNo, WarehouseStatus.借货单状态.已出库,
                                                                         ProductStatus.产品价格来源.配送员进货价);
                List<CBPdProductJson> filterList;
                if (products != null && products.Any())
                {
                    filterList = list.Where(x => !products.Contains(x.ProductSysNo)).ToList();
                }
                else
                {
                    filterList = list.ToList();
                }
                return PartialView("ProductLendItems", filterList);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "获取配送员借货商品列表" + ex.Message, LogStatus.系统日志目标类型.用户,
                                      deliveryUserSysNo, ex);
            }
            return null;
        }

        /// <summary>
        /// 获取指定配送员的借货商品
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNo">商品系统编号数组</param>
        /// <returns>配送员借货商品保含价格</returns>
        /// <remarks>2013-07-19 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004202)]
        public JsonResult GetProductLend(int deliveryUserSysNo, int? warehouseSysNo, List<int> productSysNo)
        {
            var result = new Result<object>() { StatusCode = -1 };
            var list = WhWarehouseBo.Instance.GetProductLendItmeList(deliveryUserSysNo, warehouseSysNo, WarehouseStatus.借货单状态.已出库,
                                                                     ProductStatus.产品价格来源.配送员进货价);
            //提取只包含指定商品系统编号的借货单商品集合
            var tmp = list.Where(c => productSysNo.Contains(c.ProductSysNo)).ToList();
            var data = tmp.Select(c => new
            {
                ProductSysNo = c.ProductSysNo.ToString(),
                ProductName = c.ProductName.ToString(),
                ProductNum = c.ProductNum,
                ProductOrderNum = "1", //商品订购数量，默认为1
                Price = c.Price.ToString("C")
            }).ToList();

            result.Data = data;
            result.Status = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据配送员系统编号获取配送员信用额度信息
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信息</returns>
        /// <remarks>2013-07-19 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004201, PrivilegeCode.WH1004202)]
        public JsonResult GetDeliveryUserInfo(int deliveryUserSysNo, int warehouseSysNo)
        {
            var result = new Result<CBLgDeliveryUserCredit>() { StatusCode = -1 };
            try
            {
                var checkResult = ProductLendBo.Instance.CheckWhProductLendWarehouse(deliveryUserSysNo, warehouseSysNo, ref result.Message);
                if (!checkResult)
                {
                    var model = DeliveryUserCreditBo.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo, warehouseSysNo);
                    if (null != model)
                    {
                        result.Data = model;
                        result.Status = true;
                    }
                    else
                    {
                        result.Message = "配送员还没有添加信用额度信息！";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "根据配送员系统编号获取配送员信用额度信息" + ex.Message, LogStatus.系统日志目标类型.用户,
                                      deliveryUserSysNo, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 创建借货单
        /// </summary>
        /// <param name=" "></param>
        /// <returns>创建借货单视图</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004201)]
        public ActionResult CreateProductLend()
        {
            try
            {
                ViewBag.Warehouse = CurrentUser.Warehouses;
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建借货单" + ex.Message, LogStatus.系统日志目标类型.借货单, 0, ex);
            }
            return View();
        }

        /// <summary>
        /// 创建借货单
        /// </summary>
        /// <param name="model">借货单</param>
        /// <returns>创建借货单视图</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1004201)]
        public JsonResult CreateProductLend(WhProductLend model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (model != null)
                {
                    var checkResult = ProductLendBo.Instance.CheckWhProductLendWarehouse(model.DeliveryUserSysNo, model.WarehouseSysNo, ref result.Message);
                    if (!checkResult)
                    {
                        var deliveryUserCredit =
                            DeliveryUserCreditBo.Instance.GetLgDeliveryUserCredit(model.DeliveryUserSysNo,
                                                                                  model.WarehouseSysNo);
                        if (null != deliveryUserCredit &&
                            deliveryUserCredit.IsAllowBorrow == (int)LogisticsStatus.配送员是否允许借货.是)
                        {
                            var currentUser = CurrentUser.Base;
                            model.CreatedBy = model.LastUpdateBy = model.StockOutBy = currentUser.SysNo;
                            model.CreatedDate = model.LastUpdateDate = model.StockOutDate = DateTime.Now;
                            model.Status = (int)WarehouseStatus.借货单状态.已出库;


                            var id = ProductLendBo.Instance.CreateWhProductLend(model);
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建借货单", LogStatus.系统日志目标类型.借货单, id,
                                                 CurrentUser.Base.SysNo);

                            result.Message = id.ToString(CultureInfo.InvariantCulture);

                            result.StatusCode = 0;
                            result.Status = true;
                        }
                        else
                        {
                            result.Message = "配送员还没有添加信用额度信息或该配送员不能借货！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建借货单" + ex.Message, LogStatus.系统日志目标类型.借货单, 0, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 检查本次借货商品价格与历史借货价格差异
        /// </summary>
        /// <param name="productSysNos">商品系统编号列表</param>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004201)]
        public JsonResult CheckProductPrice(List<int> productSysNos, int deliveryUserSysNo)
        {
            var result = ProductLendBo.Instance.CheckProductPrice(productSysNos, deliveryUserSysNo);
            return Json(result);
        }

        /// <summary>
        /// 借货单列表管理
        /// </summary>
        /// <param name="id">当前页码</param>
        /// <param name="filter">查询参数</param>
        /// <returns>返回借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004101)]
        public ActionResult ProductLend(int? id, ParaProductLendFilter filter)
        {
            var wareHouseList = CurrentUser.Warehouses;
            if (Request.IsAjaxRequest())
            {
                var pageList = new PagedList<WhProductLend>();
                var curPageIndex = id ?? 1;
                filter.CurrentPage = curPageIndex;
                filter.PageSize = pageList.PageSize;

                if (filter.SysNo != null)
                {
                    var sysno = Convert.ToInt32(filter.SysNo);
                    var data = ProductLendBo.Instance.GetWhProductLend(sysno);
                    if (null != data)
                    {
                        var result = new List<WhProductLend> { data };
                        pageList.TData = result;
                        pageList.TotalItemCount = result.Count;
                        pageList.CurrentPageIndex = filter.CurrentPage;
                    }
                }
                else
                {
                    filter.WarehouseSysNoList = new List<int>();
                    if (filter.WarehouseSysNo.HasValue)
                    {
                        filter.WarehouseSysNoList.Add(filter.WarehouseSysNo.Value);
                    }
                    else
                    {
                        if (wareHouseList.Count > 0)
                        {
                            wareHouseList.ForEach(x => filter.WarehouseSysNoList.Add(x.SysNo));
                        }
                    }
                    pageList = ProductLendBo.Instance.GetProductLendList(filter);
                }
                return PartialView("_AjaxPagerProductLendList", pageList);
            }

            //用户仓库
            ViewBag.Warehouse = wareHouseList;

            //入库单状态下拉绑定数据
            var statustList = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text = @"全部", Value = "", Selected = true}
                    };
            EnumUtil.ToListItem<WarehouseStatus.借货单状态>(ref statustList);
            ViewData["Status"] = new SelectList(statustList, "Value", "Text");
            return View();
        }

        /// <summary>
        /// 强制完结借货单
        /// </summary>
        /// <param name="id">借货单系统编号</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks> 
        [Privilege(PrivilegeCode.WH0011)]
        public JsonResult EndProductLend(int id)
        {
            var result = ProductLendBo.Instance.EndProductLend(id, CurrentUser.Base.SysNo);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "强制完结借货单", LogStatus.系统日志目标类型.借货单, id,
                                 CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
            return Json(result);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>Excel文件</returns>
        /// <remarks>2013-07-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004102)]
        public ActionResult ExportExcel(ParaProductLendFilter filter)
        {
            try
            {
                var wareHouseList = CurrentUser.Warehouses;
                filter.CurrentPage = 1;
                filter.PageSize = Int32.MaxValue;
                if (!filter.WarehouseSysNo.HasValue && wareHouseList.Count > 0)
                {
                    filter.WarehouseSysNoList = new List<int>();
                    wareHouseList.ForEach(x => filter.WarehouseSysNoList.Add(x.SysNo));
                }
                var list = ProductLendBo.Instance.WhProductLendExportExcel(filter);
                if (list != null && list.Count > 0)
                {
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "借货单导出Excel", LogStatus.系统日志目标类型.借货单, 0,
                                         CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
                    ExcelUtil.Export<CBWhProductLend>(list);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "借货单导出Excel" + ex.Message, LogStatus.系统日志目标类型.借货单, 0, ex,
                                      CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
            }
            return RedirectToAction("ProductLend");
        }

        /// <summary>
        /// 借货单查看
        /// </summary>
        /// <param name="id">借货单明细系统编号</param>
        /// <returns>明细视图</returns>
        /// <remarks>2013-07-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1004101)]
        public ActionResult ProductLendDetails(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var model = ProductLendBo.Instance.GetWhProductLendItemList(
                    new ParaWhProductLendItemFilter()
                    {
                        ProductLendSysNo = id,
                        PriceSource = (int)ProductStatus.产品价格来源.配送员进货价
                    });
                return PartialView("_AjaxPagerProductLendItemDetail", model);
            }
            else
            {
                var model = ProductLendBo.Instance.GetWhProductLend(id);
                return View(model);
            }
        }

        #endregion

        #region 出库 周瑜 2013-06-15 创建

        /// <summary>
        /// 获取用于简单批量出库的出库单
        /// </summary>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-06-15 周瑜 创建</remarks>
        public ActionResult GetSingleBatchOutStock()
        {
            var data = WhWarehouseBo.Instance.GetSingleBatchDO();
            return View("SingleBatchOutStock", data);
        }

        /// <summary>
        /// 显示出库操作页面， 用于扫描商品，出库等操作
        /// </summary>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-06-15 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1003301)]
        [HttpGet]
        public ActionResult StockOut(int sysno)
        {
            var model = WhWarehouseBo.Instance.GetStockOutInfo(sysno);
            if (model != null)
            {
                CheckIssuedInvoice(model);
                var deliveryList = GetDeliveryListItems(model.WarehouseSysNo);

                ViewData["deliveryList"] = new SelectList(deliveryList, "Value", "Text");
                //如果配送方式为第三方快递, 则在页面中显示下拉框供用户进行选择.
                ViewData["IsThirdpartyExpress"] = model.IsThirdpartyExpress;
            }

            return View(model);
        }

        /// <summary>
        /// 检查出库时是否需要开发票
        /// </summary>
        /// <param name="model">出库实体</param>
        /// <remarks>2013-11-11 周唐炬 创建</remarks>
        private void CheckIssuedInvoice(WhStockOut model)
        {
            var issued = false;//是否需要开发票
            FnInvoice invoice = null;
            if (model.Status != WarehouseStatus.出库单状态.作废.GetHashCode())
            {
                //出库单有发票信息
                if (model.InvoiceSysNo > 0)
                {
                    invoice = InvoiceBo.Instance.GetModel(model.InvoiceSysNo);
                    if (invoice != null && invoice.Status == FinanceStatus.发票状态.待开票.GetHashCode())
                    {
                        issued = true;//需要开票
                    }
                }
                else //出库单没有发票信息
                {
                    //读取订单，检查订单是否有需要发票
                    var order = SoOrderBo.Instance.GetByTransactionSysNo(model.TransactionSysNo);
                    if (order != null && order.InvoiceSysNo > 0)
                    {
                        //读取订单对应发票
                        invoice = InvoiceBo.Instance.GetModel(order.InvoiceSysNo);
                        //订单所有分批出库单
                        var outList = WhWarehouseBo.Instance.GetModelByTransactionSysNo(order.TransactionSysNo);
                        if (outList != null)
                        {
                            //分批出库单中未作废，包含有发票编号的出库单数
                            var filterCount = outList.Count(x => x.Status != WarehouseStatus.出库单状态.作废.GetHashCode() && x.InvoiceSysNo > 0);
                            if (filterCount <= 0)
                            {
                                if (invoice != null && invoice.Status == FinanceStatus.发票状态.待开票.GetHashCode())
                                {
                                    issued = true;//需要开票
                                }
                            }
                        }
                    }
                }
            }
            ViewBag.Invoice = invoice;
            ViewBag.Issued = issued;
        }

        /// <summary>
        /// 根据仓库编号获取配送方式信息
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>配送方式信息列表</returns>
        /// <remarks> 
        /// 2013-06-28 周瑜 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        private List<SelectListItem> GetDeliveryListItems(int warehouseSysNo)
        {
            //页面配送方式下拉绑定数据
            var deliveryList = DeliveryTypeBo.Instance.GetLgDeliveryTypeByWarehouse(warehouseSysNo)
                                             .Where(x => x.IsThirdPartyExpress == 1 && x.ParentSysNo > 0);

            var list = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Text = @"请选择",
                            Value = "",
                            Selected = true
                        }
                };
            list.AddRange(
                deliveryList.Select(
                    x => new SelectListItem { Text = x.DeliveryTypeName.Split('_')[1], Value = Convert.ToString(x.SysNo) }));
            return list;
        }

        /// <summary>
        /// 获取用于单个出库单出库的出库单列表
        /// </summary>
        /// <param name="id">出库单系统编号 OR 页索引</param>
        /// <param name="condition">搜索条件</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-06-17 周瑜 创建</remarks>
        /// <remarks>2013-10-28 沈强 修改</remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        public ActionResult SingleOutStock(int? id, StockOutSearchCondition condition)
        {
            int pageIndex = id ?? 1;
            //pageIndex = condition.CurrentPage == 0 ? pageIndex : condition.CurrentPage;
            const int pageSize = 10;

            if (Request.IsAjaxRequest())
            {
                var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
                var data = WhWarehouseBo.Instance.Search(condition, pageIndex, pageSize, CurrentUser.Base.SysNo,
                                                         hasAllWarehouse);

                return PartialView("_AjaxPagerOutStockList", data);
            }

            //页面高级搜索是否开票下拉绑定数据
            var isInvoiceList = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "", Selected = true},
                    new SelectListItem {Text = @"是", Value = "1"},
                    new SelectListItem {Text = @"否", Value = "0"}
                };
            ViewData["isInvoice"] = isInvoiceList;

            //出库单状态下拉绑定数据
            var statustList = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "", Selected = true}
                };
            EnumUtil.ToListItem<WarehouseStatus.出库单状态>(ref statustList);

            ViewData["status"] = new SelectList(statustList, "Value", "Text", "0");

            var deliveryTypeList = DeliveryTypeBo.Instance.GetDeliveryTypeList();
            ViewBag.DeliveryTypeList = deliveryTypeList;
            var tmpNames = deliveryTypeList.Select(d => d.DeliveryTypeName.Split('_')[0]).ToList();
            var groupNames = new List<string>();
            tmpNames.ForEach(t =>
                {
                    if (!groupNames.Contains(t))
                    {
                        groupNames.Add(t);
                    }
                });
            ViewBag.DeliveryTypeGroupName = groupNames;

            //Status == 0 代表查询所有状态的出库单
            if (condition.Status == 0)
            {
                condition.Status = null;
            }

            IList<WhWarehouse> whWarehouses = CurrentUser.Warehouses;

            ViewBag.WarehouseList = whWarehouses;

            //获取分销商商城列表
            var dsDealerMallList = DsDealerMallBo.Instance.GetList();
            ViewBag.DsDealerMallList = dsDealerMallList;

            var texts = BsCodeBo.Instance.出库单作废原因().Select(s => s.CodeName).ToArray();
            ViewBag.CancelStockOutTexts = string.Join("|", texts);

            return View("SingleOutStock");
        }



        #endregion
        /// <summary>
        /// 根据出库单编号查询出库单信息, 物流信息及发票信息
        /// </summary>
        /// <param name="whStockOutSysNo">出库单系统编号</param>
        /// <param name="showLogistics">true:显示物流信息, false:不显示物流信息</param>
        /// <remarks>2013-06-19 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        public ActionResult StockOutInfo(int whStockOutSysNo, bool showLogistics = true)
        {
            var data = WhWarehouseBo.Instance.GetStockOutInfo(whStockOutSysNo);
            if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(data.WarehouseSysNo))
            {

                var message = string.Format("没有权限读取仓库:{0}的信息",
                                            WhWarehouseBo.Instance.GetWarehouseName(data.WarehouseSysNo));
                return View("ErrorPrivilegeWithMessage", (object)(message));

            }

            //2014-04-11 朱家宏 修改签收图片
            var fileName = data.SignImage;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var saveFolder = string.Format("{0}App\\Logistics\\{1}\\{2}\\{3}", Config.Instance.GetAttachmentConfig().FileServer, fileName.Substring(0, 1), fileName.Substring(1, 2), fileName);
                ViewBag.Pic = saveFolder;
            }

            ViewBag.ShowLogistics = showLogistics;
            ViewBag.uploadPermission =
                AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.WH1003301);
            return PartialView("StockOutInfo", data);
        }

        /// <summary>
        /// 签收图片
        /// </summary>
        /// <returns>view</returns>
        /// <remarks>2014-04-11  朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        public ActionResult PicBrowser(string path)
        {
            ViewBag.path = path;
            return View();
        }

        /// <summary>
        /// 签收图片上传
        /// </summary>
        /// <param name="whStockOutSysNo">出库单号</param>
        /// <returns>view</returns>
        /// <remarks>2014-04-15 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.WH1003301)]
        public ActionResult UploadSignedImg(int whStockOutSysNo)
        {
            if (Request.Files.Count == 0)
            {
                ViewBag.whStockOutSysNo = whStockOutSysNo;
                return View();
            }

            var httpPostedFileBase = Request.Files[0];
            var result = new Result { StatusCode = -1 };

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength == 0)
            {
                result.Message = "图片数据不能为空。";
            }
            else
            {
                try
                {
                    var noteType = LogisticsStatus.配送单据类型.出库单.GetHashCode();
                    var stockOut = WhWarehouseBo.Instance.Get(whStockOutSysNo);

                    if (stockOut == null ||
                        stockOut.Status != Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.已签收.GetHashCode())
                    {
                        result.Message = "单据系统编号有误。";
                    }
                    else
                    {
                        var fileName = string.Format("{0}.jpg",
                            EncryptionUtil.EncryptWithMd5(noteType + "-" + whStockOutSysNo));
                        var inputBytes = ImageUtil.StreamConvertToBytes(httpPostedFileBase.InputStream);
                        var saveFolder = string.Format("App\\Logistics\\{0}\\{1}\\", fileName.Substring(0, 1),
                            fileName.Substring(1, 2));

                        using (var service = new ServiceProxy<IUploadService>())
                        {
                            result.Status = service.Channel.UploadFile(saveFolder, fileName, inputBytes);
                        }

                        if (result.Status)
                        {

                            if (stockOut != null)
                            {
                                stockOut.SignImage = fileName;
                                WhWarehouseBo.Instance.UpdateStockOut(stockOut);
                                result.StatusCode = 0;
                            }
                            result.Message = "上传成功";
                            ViewBag.NewPic = Config.Instance.GetAttachmentConfig().FileServer + saveFolder + fileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "上传图片保存" + ex.Message, ex);
                }
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            return View();
        }

        /// <summary>
        /// 单出库单出库Ajax方法
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="checkedStockItemSysNo">选中的出库单明细系统编号,商品数量字符串</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="isThirdpartyExpress">是否为第三方快递</param>
        /// <param name="stamp">出库时的时间戳</param>
        /// <returns>返回操作状态(Result.StatusCode>=0成功,小于 0失败)</returns>
        /// <remarks>2013-06-19 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1003301)]
        [HttpPost]
        public JsonResult StockOut(int sysNo, string checkedStockItemSysNo, int deliveryTypeSysNo, int isThirdpartyExpress, string stamp)
        {
            var result = new Result { StatusCode = -1 };

            //开始事务
            using (var scope = new TransactionScope())
            {
                if (string.IsNullOrEmpty(checkedStockItemSysNo))
                {
                    result.Message = "没有扫描任何商品， 不能出库。";
                    return Json(result);
                }
                if (checkedStockItemSysNo.LastIndexOf(';', checkedStockItemSysNo.Length - 1) > 0)
                {
                    checkedStockItemSysNo = checkedStockItemSysNo.Remove(checkedStockItemSysNo.Length - 1, 1);
                }
                //出库单明细系统编号与商品数量集合字符串
                var stockItemSysNoAndProductNumList = checkedStockItemSysNo.Split(';');
                if (stockItemSysNoAndProductNumList.Length == 0)
                {
                    result.Message = "没有扫描任何商品， 不能出库。";
                    return Json(result);
                }
                //检查是否所有的商品数量是否都为0或者为空
                var isItemScaned = false;
                foreach (var s in stockItemSysNoAndProductNumList)
                {
                    string productNum = s.Split(',')[1];
                    if (!string.IsNullOrEmpty(productNum) && productNum != "0")
                    {
                        isItemScaned = true;
                        break;
                    }
                }
                if (!isItemScaned)
                {
                    result.Message = "没有扫描任何商品， 不能出库。";
                    return Json(result);
                }

                var master = WhWarehouseBo.Instance.Get(sysNo);

                //检查时间戳是否改变
                if (master.Stamp.ToString() != stamp)
                {
                    result.Message = "此出库单已被其他人修改，请关闭当前窗口后刷新页面！";
                    return Json(result);
                }
                if (master.Status != (int)WarehouseStatus.出库单状态.待出库)
                {
                    result.Message = "此出库单不是待出库状态，不能出库！";
                    return Json(result);
                }

                //第三方快递,订单未收收款,不允许出库
                if (isThirdpartyExpress == 1)
                {
                    var order = SoOrderBo.Instance.GetEntity(master.OrderSysNO);
                    if (order.PayStatus != OrderStatus.销售单支付状态.已支付.GetHashCode())
                    {
                        result.Message = "第三方快递订单未收款,不能出库。";
                        return Json(result);
                    }
                }

                var outData = Hyt.BLL.Warehouse.WhWarehouseBo.GetWhStockOutItemByOut(sysNo);

                foreach (var item in master.Items)
                {
                    foreach (var s in stockItemSysNoAndProductNumList)
                    {
                        if (item.SysNo == Convert.ToInt32(s.Split(',')[0])
                            && !string.IsNullOrEmpty(s.Split(',')[1])
                            && Convert.ToInt32(s.Split(',')[1]) > 0)
                        {
                            item.IsScaned = true;
                            item.ScanedQuantity =
                                Convert.ToInt32(string.IsNullOrEmpty(s.Split(',')[1]) ? "0" : s.Split(',')[1]);
                        }
                    }
                }

                try
                {
                    //减平台仓库库存
                    var _result = WhWarehouseBo.Instance.ReduceStock(-1, master.WarehouseSysNo, outData);
                    if (!_result.Status)
                    {
                        result.Message = _result.Message;
                        result.Status = false;
                        return Json(result);
                    }
                    master.DeliveryTypeSysNo = deliveryTypeSysNo;
                    master.StockOutDate = DateTime.Now;
                    master.StockOutBy = CurrentUser.Base.SysNo;
                    master.LastUpdateBy = CurrentUser.Base.SysNo;
                    master.LastUpdateDate = DateTime.Now;
                    master.Status = (int)WarehouseStatus.出库单状态.待拣货;

                    //WhWarehouseBo.Instance.OutStock(master, CurrentUser.Base.SysNo);
                    WhWarehouseBo.Instance.OutStock(master);



                    var delivery = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo);
                    var deliveryName = (delivery == null)
                        ? "未能找到编号为" + deliveryTypeSysNo + "的配送方式"
                        : delivery.DeliveryTypeName;
                    var logTxt = "订单生成配送方式:<span style=\"color:red\">" + deliveryName + "</span>，待拣货打包";
                    SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo, logTxt,
                        CurrentUser.Base.UserName);
                    result.Status = true;
                    result.Message = master.SysNo + " 出库成功。";
                    result.StatusCode = 0;
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.出库单, master.SysNo, ex,
                        CurrentUser.Base.SysNo);
                }
                scope.Complete();
            }
            return Json(result);
        }

        /// <summary>
        /// 简单批量出库Ajax方法
        /// </summary>
        /// <returns>返回操作状态(Result.StatusCode>=0成功,小于 0失败)</returns>
        /// <remarks>2013-06-27 周瑜 创建</remarks>
        /// <remarks>2014-04-21  没有使用</remarks>
        public JsonResult StockOutBatch()
        {
            var result = new Result { StatusCode = -1 };
            var list = Request.Form.GetValues("checkedStockOutSysNo[]");
            if (list == null)
            {
                result.Message = "没有选中任何出库单，不能出库。";
                return Json(result);
            }
            //出库单系统编号列表
            var sysnoList = list.ToList();
            try
            {
                var masters = sysnoList.Select(sysno => WhWarehouseBo.Instance.Get(Convert.ToInt32(sysno))).ToList();
                //依次出库
                foreach (var master in masters)
                {
                    WhWarehouseBo.Instance.OutStock(master);
                }

                result.Message = "出库成功。";
                result.StatusCode = 0;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 作废出库单
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="text">出库单作废原因</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-01 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1003401)]
        public JsonResult CancelOutStock(int sysNo, string text)
        {
            Result result = null;
            try
            {
                //开始事务
                using (var scope = new TransactionScope())
                {
                    result = WhWarehouseBo.Instance.CancelStockOut(sysNo, CurrentUser.Base.SysNo, text);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result = new Result { Message = ex.Message };
            }

            return Json(result);
        }

     

        #region 发票 周瑜 2013-07-09 创建

        /// <summary>
        /// 获取发票信息列表
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="condition">搜索条件</param>
        /// <returns>发票信息列表</returns>
        /// <remarks>2013-07-09 周瑜 创建</remarks>
        /// <remarks>2013-11-05 周唐炬 重构</remarks>
        [Privilege(PrivilegeCode.FN1001101)]
        public ActionResult GetInvoiceList(int? id, InvoiceSearchCondition condition)
        {
            var wareHouseList = CurrentUser.Warehouses;
            try
            {
                if (Request.IsAjaxRequest())
                {
                    condition.CurrentPage = id ?? 1;
                    condition.WarehouseSysNoList = new List<int>();
                    if (condition.WarehouseSysNo.HasValue)
                    {
                        condition.WarehouseSysNoList.Add(condition.WarehouseSysNo.Value);
                    }
                    else
                    {
                        if (wareHouseList.Count > 0)
                        {
                            wareHouseList.ForEach(x => condition.WarehouseSysNoList.Add(x.SysNo));
                        }
                    }
                    var pageList = InvoiceBo.Instance.Search(condition);
                    return PartialView("_AjaxPagerInvoiceList", pageList);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "获取发票信息列表" + ex.Message, LogStatus.系统日志目标类型.发票, 0, ex);
            }
            //用户仓库
            ViewBag.Warehouse = wareHouseList;
            InvoiceDropList();
            return View("Invoice");
        }

        /// <summary>
        /// 发票维护页下拉框数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-11-05 周唐炬 创建</remarks>
        private void InvoiceDropList()
        {
            //发票状态下拉绑定数据
            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };
            var statustList = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<FinanceStatus.发票状态>(ref statustList);
            ViewBag.Status = new SelectList(statustList, "Value", "Text");

            //发票类型下拉绑定数据
            //出库单状态下拉绑定数据
            var invoiceTypeList = FnInvoiceTypeBo.Instance.GetAll();
            var invoiceTypeListItem = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "", Selected = true}
                };
            invoiceTypeListItem.AddRange(
                invoiceTypeList.Select(x => new SelectListItem { Text = x.Name, Value = Convert.ToString(x.SysNo) }));
            ViewData["invoiceTypeListItem"] = new SelectList(invoiceTypeListItem, "Value", "Text", "");
        }

        /// <summary>
        /// 发票快速搜索
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="condition">条件</param>
        /// <returns>视图页</returns>
        /// <remarks>2014-01-07 周唐炬 添加注释</remarks>
        [Privilege(PrivilegeCode.FN1001101)]
        public ActionResult InvoiceQuickSearch(int? id, InvoiceSearchCondition condition)
        {
            var pageIndex = id ?? 1;
            const int pageSize = 10;
            var wareHouseList = CurrentUser.Warehouses;
            if (Request.IsAjaxRequest())
            {
                condition.CurrentPage = id ?? 1;
                condition.WarehouseSysNoList = new List<int>();
                if (condition.WarehouseSysNo.HasValue)
                {
                    condition.WarehouseSysNoList.Add(condition.WarehouseSysNo.Value);
                }
                else
                {
                    if (wareHouseList.Count > 0)
                    {
                        wareHouseList.ForEach(x => condition.WarehouseSysNoList.Add(x.SysNo));
                    }
                }
                var data = InvoiceBo.Instance.QuickSearch(condition);
                //return PartialView("_AjaxPagerInvoiceList", data);
            }
            InvoiceDropList();
            return View("Invoice");
        }

        #region 暂时不需要发票作废

        ///// <summary>
        ///// 作废发票
        ///// </summary>
        ///// <param name="sysno">发票系统编号</param>
        ///// <returns>返果结果</returns>
        ///// <remarks>2013-07-10 周瑜 创建</remarks>
        //public JsonResult CancelInvoice(int sysno)
        //{
        //    var result = new Result { StatusCode = -1 };
        //    try
        //    {
        //        var data = InvoiceBo.Instance.CancelInvoice(sysno);
        //        if (data)
        //        {
        //            result.StatusCode = 0;
        //            result.Status = true;
        //            result.Message = string.Format("发票{0}作废成功.", sysno);
        //        }
        //        else
        //        {
        //            result.Message = string.Format("发票{0}作废失败.", sysno);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //    }

        //    return Json(result);
        //}

        #endregion

        /// <summary>
        /// 新开发票
        /// </summary>
        /// <returns>开票视图</returns>
        /// <remarks>2013-11-14 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.FN1001301)]
        public ActionResult InvoiceCreate()
        {
            InitInvoicePageViewData(null);
            return View();
        }

        /// <summary>
        /// 新开发票
        /// </summary>
        /// <param name="model">发票实体</param>
        /// <returns>开票是否成功</returns>
        /// <remarks>2013-11-14 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FN1001301)]
        public JsonResult InvoiceCreate(CBFnInvoice model)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    var order = SoOrderBo.Instance.GetEntity(model.OrderSysNo);
                    if (order != null)
                    {
                        //检查订单是否可以新开票
                        if (order.Status == OrderStatus.销售单状态.作废.GetHashCode())
                        {
                            result.Message = "该订单已经作废，不能新开票!";
                        }
                        else
                        {
                            if (order.InvoiceSysNo > 0)
                            {
                                result.Message = "该订单已经存在发票，请尝试搜索订单号开票！";
                            }
                            else
                            {
                                model.Status = FinanceStatus.发票状态.已开票.GetHashCode();
                                model.CreatedBy = model.LastUpdateBy = CurrentUser.Base.SysNo;
                                model.CreatedDate = model.LastUpdateDate = DateTime.Now;

                                var id = SoOrderBo.Instance.InsertOrderInvoice(model);
                                if (id > 0)
                                {
                                    SoOrderBo.Instance.UpdateOrderInvoice(order.SysNo, id);

                                    if (!string.IsNullOrWhiteSpace(model.TransactionSysNo))
                                    {

                                        //更新应收金额大于0 的出库单的发票系统编号
                                        var list =
                                            WhWarehouseBo.Instance.GetModelByTransactionSysNo(model.TransactionSysNo);
                                        var master = list.Where(m => m.Receivable > 0).ToList();
                                        if (master.Any())
                                        {
                                            master.ForEach(x =>
                                                {
                                                    x.InvoiceSysNo = id;
                                                    WhWarehouseBo.Instance.UpdateStockOut(x);
                                                });
                                        }
                                    }
                                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "开票操作", LogStatus.系统日志目标类型.发票, id, CurrentUser.Base.SysNo);
                                }

                                result.StatusCode = 0;
                                result.Status = true;

                            }
                        }
                    }
                    else
                    {
                        result.Message = "该订单不存在，不能开票！";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "开票操作" + ex.Message, LogStatus.系统日志目标类型.发票, 0, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 显示编辑发票信息页面
        /// </summary>
        /// <param name="id">发票系统编号</param>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        /// <returns>编辑发票信息页面</returns>
        [Privilege(PrivilegeCode.FN1001301)]
        public ActionResult InvoiceEdit(int id)
        {
            ViewBag.OrderSysNo = id;
            FnInvoice model = null;
            var order = SoOrderBo.Instance.GetEntity(id);
            if (order != null)
            {
                model = InvoiceBo.Instance.GetInvoiceByTransactionSysNo(order.TransactionSysNo) ??
                        new FnInvoice() { TransactionSysNo = order.TransactionSysNo, InvoiceAmount = order.CashPay };
            }
            InitInvoicePageViewData(model);
            return View(model);
        }

        /// <summary>
        /// 开票操作
        /// </summary>
        /// <param name="model">发票实体</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-01 周瑜 创建</remarks>
        /// <remarks>2013-11-05 周唐炬 重构</remarks>
        [Privilege(PrivilegeCode.FN1001301)]
        public JsonResult InvoiceUpdateEdit(FnInvoice model)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    model.Status = FinanceStatus.发票状态.已开票.GetHashCode();
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;


                    InvoiceBo.Instance.InvoiceTransaction(model, CurrentUser.Base);
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "开票操作", LogStatus.系统日志目标类型.发票, model.SysNo, CurrentUser.Base.SysNo);


                    result.StatusCode = 0;
                    result.Status = true;

                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "开票操作" + ex.Message, LogStatus.系统日志目标类型.发票, model.SysNo, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 出库单- 发票确认
        /// </summary>
        /// <param name="invoiceSysNo">发票系统编号</param>
        /// <param name="invoiceCode">发票代码</param>
        /// <param name="invoiceNo">发票号码</param>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <param name="invoiceRemarks">发票备注</param>
        /// <returns>返果结果</returns>
        /// <remarks>2013-07-01 周瑜 创建</remarks>
        /// <remarks>2013-11-11 周唐炬 重构</remarks>
        [Privilege(PrivilegeCode.WH1003301)]
        public JsonResult InvoiceUpdate(int invoiceSysNo, string invoiceCode, string invoiceNo, int stockOutSysNo, string invoiceRemarks)
        {
            var result = new Result { StatusCode = -1 };

            try
            {
                var invoice = InvoiceBo.Instance.GetModel(invoiceSysNo);
                if (invoice != null)
                {
                    invoice.LastUpdateBy = CurrentUser.Base.SysNo;
                    invoice.LastUpdateDate = DateTime.Now;
                    //将状态置为已开发票
                    invoice.Status = FinanceStatus.发票状态.已开票.GetHashCode();
                    invoice.InvoiceCode = invoiceCode;
                    invoice.InvoiceNo = invoiceNo;
                    invoice.InvoiceRemarks = invoiceRemarks;
                    SoOrderBo.Instance.UpdateOrderInvoice(invoice);
                    //更新出库单发票编号
                    var model = WhWarehouseBo.Instance.GetStockOutInfo(stockOutSysNo);
                    model.InvoiceSysNo = invoice.SysNo;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    WhWarehouseBo.Instance.UpdateStockOut(model);
                }

                result.StatusCode = 0;
                result.Status = true;
                result.Message = "发票信息已经更新.";
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "发票更新操作", LogStatus.系统日志目标类型.发票, invoiceSysNo,
                                     CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "发票更新操作：" + ex.Message, LogStatus.系统日志目标类型.发票, invoiceSysNo,
                                     CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 初始化发票页面状态
        /// </summary>
        /// <param name="model">发票实体</param>
        /// <returns></returns>
        /// <remarks>2013-11-14 周唐炬 创建</remarks>
        private void InitInvoicePageViewData(FnInvoice model)
        {
            //如果发票未创建,则从出库单中查询订单编号及金额
            var invoiceType = FnInvoiceTypeBo.Instance.GetAll();

            var item = new SelectListItem() { Text = @"请选择", Value = "", Selected = true };
            var typeList = new List<SelectListItem>() { item };
            typeList.AddRange(invoiceType.Select(x => new SelectListItem { Text = x.Name, Value = Convert.ToString(x.SysNo) }));
            ViewBag.InvoiceType = model != null ? new SelectList(typeList, "Value", "Text", model.InvoiceTypeSysNo) : new SelectList(typeList, "Value", "Text");

        }
        #endregion

        #region 搜索仓库 何方 2013-06-21 创建

        /// <summary>
        /// 仓库选择组件
        /// </summary>
        /// <param name=""></param>
        /// <returns>仓库选择组件视图</returns>
        /// <remarks>2013-06-24 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.CM1005804, PrivilegeCode.CM1005808)]
        public ActionResult WharehouseSelector()
        {
            return View();
        }

        /// <summary>
        /// 仓库选择组件，弹框模式
        /// </summary>
        /// <param name=""></param>
        /// <returns>仓库选择组件视图</returns>
        /// <remarks>2013-07-06 郑荣华 创建</remarks>
        [Privilege(PrivilegeCode.CM1005804, PrivilegeCode.CM1005808)]
        public ActionResult SelectWharehouse()
        {
            return View("_SelectWareHouse");
        }

        /// <summary>
        /// 搜索仓库 
        /// </summary>
        /// <param name="keyword">关键词.</param>
        /// <param name="areaNoCheck">地区节点是否可被选中</param>
        /// <param name="isRma">是否Rma仓</param>
        /// <returns>ZTree列表</returns>
        /// <remarks>
        /// 2013/6/21 何方 创建
        /// 2013-10-24 郑荣华 添加显示所有仓库 或 只显示有权限仓库功能
        /// 2013-10-24 黄志勇 筛选Rma仓库
        /// </remarks>
        [Privilege(PrivilegeCode.CM1005804, PrivilegeCode.CM1005808)]
        public JsonResult GetWharehouseSelector(string keyword, bool areaNoCheck = true, bool isRma = false)
        {
            var data = WhWarehouseBo.Instance.SearchWharehouseNew(keyword, areaNoCheck, isRma);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 门店查询 黄志勇 2013-6-24 创建

        /// <summary>
        /// 门店创建订单
        /// </summary>
        /// <returns>门店订单页面</returns>
        /// <remarks>2013-6-24 黄志勇 创建</remarks>
        /// <remarks>2013-7-25 朱家宏 修改:权限门店调用方式</remarks>
        [Privilege(PrivilegeCode.SO1004201)]
        public ActionResult ShopOrderCreate()
        {
            //var list = WhWarehouseBo.Instance.GetWhWarehouseListByType((int)WarehouseStatus.仓库类型.门店);

            var list = ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);

            //当前登录人所在门店不为空
            //if ()
            //{
            //    list = list.Where(x => x.SysNo == 1).ToList();
            //}
            ViewBag.Warehouse = list;
            ViewBag.InvoiceTypeList = FnInvoiceBo.Instance.GetFnInvoiceTypeList();
            return View();
        }

        /// <summary>
        /// 门店下单查询
        /// </summary>
        /// <returns>门店下单列表</returns>
        /// <remarks>2013-06-24 余勇 创建</remarks>
        /// <remarks>2013-07-22 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SO1004101)]
        public ActionResult ShopOrder()
        {
            ViewBag.Warehouse = ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
            return View();
        }

        /// <summary>
        /// 门店下单详情
        /// </summary>
        /// <param name="id">订单编号</param>
        /// <returns>门店下单详情页面</returns>
        /// <remarks>2013-07-03 余勇 创建</remarks>
        /// <remarks>2013-12-16 黄志勇 修改（增加升舱信息）</remarks>
        [Privilege(PrivilegeCode.SO1004101)]
        public ActionResult ShopOrderDetail(int id)
        {
            var model = SoOrderBo.Instance.GetEntity(id);
            if (model == null)
            {
                return RedirectToAction("ShopOrder");
            }
            //收货地址
            model.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
            //显示省市区
            if (model.ReceiveAddress != null)
            {
                //城市
                BsArea cEntity;
                //地区
                BsArea aEntity;
                var pEntity = BasicAreaBo.Instance.GetProvinceEntity(model.ReceiveAddress.AreaSysNo,
                                                                                   out cEntity, out aEntity);
                string addressDetatil = string.Empty;
                if (pEntity != null) addressDetatil = pEntity.AreaName;
                if (cEntity != null) addressDetatil = addressDetatil + "  " + cEntity.AreaName;
                if (aEntity != null) addressDetatil = addressDetatil + "  " + aEntity.AreaName;
                ViewBag.AddressDetatil = addressDetatil;
            }
            //会员信息
            model.Customer = SoOrderBo.Instance.SearchCustomer(model.CustomerSysNo);
            //默认仓库名
            if (model.DefaultWarehouseSysNo > 0)
            {
                var defaultWarehouse =
                    WhWarehouseBo.Instance.GetWarehouseEntity(model.DefaultWarehouseSysNo);
                if (defaultWarehouse != null)
                {
                    ViewBag.DefaultWarehouse = defaultWarehouse;
                }
            }

            //发票信息
            model.OrderInvoice = SoOrderBo.Instance.GetFnInvoice(model.InvoiceSysNo);

            //订购商品数量
            model.OrderItemList = SoOrderBo.Instance.GetOrderItemsByOrderId(id);

            //获取出库单
            var outStockList = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(id);
            if (outStockList.Count > 0)
            {
                ViewBag.PickUpDate = outStockList[0].PickUpDate;
                ViewBag.Remarks = outStockList[0].Remarks;
            }

            //升舱信息显示
            if (model.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
            {
                var dsOrderEx = DsOrderBo.Instance.GetDsOrderInfoEx(model.TransactionSysNo);
                if (dsOrderEx.Item1 != null && dsOrderEx.Item2 != null)
                {
                    ViewBag.DsOrder = dsOrderEx.Item1;
                    ViewBag.DealerMall = dsOrderEx.Item2;
                }
            }
            return View(model);
        }

        /// <summary>
        /// 查询门店自提
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2013-07-8 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005101)]
        public ActionResult SearchShopDelivery(ParaOutStockOrderFilter filter)
        {
            Pager<CBOutStockOrder> pager = new Pager<CBOutStockOrder>();
            pager.CurrentPage = filter.Id;
            pager.PageSize = 10;

            //增加当前用户所属门店筛选

            if (CurrentUser.Warehouses.Any())
                filter.Warehouses = CurrentUser.Warehouses;
            else
                filter.Warehouses = new List<WhWarehouse>
                    {
                        new WhWarehouse {WarehouseType = (int) WarehouseStatus.仓库类型.门店}
                    };

            switch (filter.QueryType)
            {
                case 1: //待确认
                    pager = ShopOrderBo.Instance.GetOutStockOrdersToBeConfirmed(filter);
                    break;
                case 2: //待提货
                    pager = ShopOrderBo.Instance.GetOutStockOrdersToBePicked(filter);
                    break;
                case 3: //延时已到期
                    pager = ShopOrderBo.Instance.GetExpiredOutStockOrders(filter);
                    break;
                case 4: //今日已提货
                    pager = ShopOrderBo.Instance.GetPickedOutStockOrdersOfToday(filter);
                    break;
                default: //所有订单
                    pager = ShopOrderBo.Instance.GetOutStockOrders(filter);
                    break;
            }

            PagedList<CBOutStockOrder> list = new PagedList<CBOutStockOrder>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.TotalItemCount = pager.TotalRows;
            list.PageSize = pager.PageSize;
            ViewBag.QueryType = filter.QueryType;
            return PartialView("_ShopDeliveryPager", list);
        }

        /// <summary>
        /// 查询门店订单
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>
        /// 2013-06-24 余勇 创建
        /// 2013-07-05 朱家宏 修改 增加当前用户的门店筛选
        /// </remarks>
        [Privilege(PrivilegeCode.SO1004101)]
        public ActionResult SearchShopOrder(ParaOrderFilter filter)
        {
            Pager<CBSoOrder> pager = new Pager<CBSoOrder>();
            pager.CurrentPage = filter.Id;
            pager.PageSize = 10;

            //增加当前用户所属门店筛选
            if (CurrentUser.Warehouses.Any())
                filter.Warehouses = CurrentUser.Warehouses;
            filter.IsBindAllDealer = true;
            filter.SelectedAgentSysNo = -1;
            switch (filter.Status)
            {
                case 1: //今日订单
                    SoOrderBo.Instance.DoSoOrderQueryFromStoreForToday(ref pager, filter);
                    break;
                case 2: //今日转快递
                    SoOrderBo.Instance.DoSoOrderQueryFromStoreForDeliveryChanged(ref pager, filter);
                    break;
                case 3: //今日已提货
                    SoOrderBo.Instance.DoSoOrderQueryFromStoreForDelivered(ref pager, filter);
                    break;
                default: //所有订单
                    SoOrderBo.Instance.DoSoOrderQueryFromStore(ref pager, filter);
                    break;
            }

            PagedList<CBSoOrder> list = new PagedList<CBSoOrder>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.TotalItemCount = pager.TotalRows;
            list.PageSize = pager.PageSize;
            return PartialView("_ShopOrderPager", list);
        }

        #endregion

        #region 门店下单 余勇 2013-06-19 创建

        /// <summary>
        /// 门店下单支付提货
        /// </summary>
        /// <param name="shopOrderCreateFilter">门店下单支付提货参数实体.</param>
        /// <returns>提交结果json对象</returns>
        /// <remarks>
        /// 2013-06-19 杨浩 修改 增加参数校验及将参数实体化
        /// 2013/10/14 杨浩 增加保存字段“刷卡流水号”
        /// 2013-11-11 杨浩 修改 修改发票信息
        /// </remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1004201)]
        public ActionResult CreateShopOrder(ParaShopOrderCreateFilter shopOrderCreateFilter)
        {
            //获取当前登录用户绑定的分销商
            var ds = 0;
            if (CurrentUser.Dealer != null)
            {
                ds = CurrentUser.Dealer.SysNo;
            }
            Result result = CheckOrder(shopOrderCreateFilter);
            if (result.StatusCode != -1)
            {
                FnInvoice invoice = null;
                if (!string.IsNullOrWhiteSpace(shopOrderCreateFilter.invoiceTitle))
                {
                    invoice = new FnInvoice()
                        {
                            InvoiceTypeSysNo = shopOrderCreateFilter.invoiceType,
                            InvoiceCode = shopOrderCreateFilter.invoiceCode,
                            InvoiceNo = shopOrderCreateFilter.invoiceNo,
                            InvoiceTitle = shopOrderCreateFilter.invoiceTitle,
                            CreatedBy = CurrentUser.Base.SysNo,
                            CreatedDate = DateTime.Now,
                            LastUpdateBy = CurrentUser.Base.SysNo,
                            LastUpdateDate = DateTime.Now,
                            Status = (int)FinanceStatus.发票状态.已开票
                        };
                }
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        var t = SoOrderBo.Instance.CreateShopOrder(shopOrderCreateFilter.shopSysNo,
                                                           shopOrderCreateFilter.customerSysNo,
                                                           shopOrderCreateFilter.receiveName,
                                                           shopOrderCreateFilter.receiveMobilePhoneNumber,
                                                           shopOrderCreateFilter.internalRemarks,
                                                           shopOrderCreateFilter.orderItem, invoice,
                                                           shopOrderCreateFilter.PayType, CurrentUser.Base,
                                                           shopOrderCreateFilter.VoucherNo, shopOrderCreateFilter.EasReceiptCode, shopOrderCreateFilter.CoinPay, shopOrderCreateFilter.CouponCode, GetChangePriceItem(), GetChangePriceItemNew(), shopOrderCreateFilter.Balance, ds);
                        result.StatusCode = t.Item1;

                        //写系统日志 2013/11/15 朱家宏 增加
                        var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店订单-创建销售单",
                                                 LogStatus.系统日志目标类型.订单, t.Item2, null, ip,
                                                 CurrentUser.Base.SysNo);
                        //同步利嘉订单
                        SynchronizeOrder(t.Item2);
                        tran.Complete();
                    }

                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    if (ex.Message.IndexOf("ORA-01438", StringComparison.Ordinal) != -1)
                    {
                        result.Message = "支付金额过大，不允许交易。";
                    }
                    else
                    {
                        result.Message = ex.Message;
                    }

                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店订单-创建销售单",
                                             LogStatus.系统日志目标类型.订单, 0, ex, ip,
                                             CurrentUser.Base.SysNo);
                }
            }
            return Json(result);
        }
        /// <summary>
        /// 门店快速下单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-05-25 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.SO1004201)]
        public ActionResult ShopOrderQuickCreate()
        {
            var list = ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
            ViewBag.Warehouse = list;
            ViewBag.InvoiceTypeList = FnInvoiceBo.Instance.GetFnInvoiceTypeList();

            return View();
        }
        /// <summary>
        /// 门店下单延迟处理
        /// </summary>
        /// <param name="shopOrderDelayFilter">门店下单延迟处理参数实体.</param>
        /// <returns>提交结果json对象</returns>
        /// <remarks>2013-06-19 杨浩 修改 增加参数校验及将参数实体化</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1004201)]
        public ActionResult CreateShopOrderDelay(ParaShopOrderDelayFilter shopOrderDelayFilter)
        {
            //获取当前登录用户绑定的分销商
            var ds = 0;
            if (CurrentUser.Dealer != null)
            {
                ds = CurrentUser.Dealer.SysNo;
            }
            Result result = CheckOrder(shopOrderDelayFilter);

            if (result.StatusCode != -1)
            {
                FnInvoice invoice = null;
                if (!string.IsNullOrWhiteSpace(shopOrderDelayFilter.invoiceTitle))
                {
                    invoice = new FnInvoice()
                    {
                        InvoiceTitle = shopOrderDelayFilter.invoiceTitle,
                        CreatedBy = CurrentUser.Base.SysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = CurrentUser.Base.SysNo,
                        LastUpdateDate = DateTime.Now,
                        Status = (int)FinanceStatus.发票状态.待开票
                    };
                }
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        var t = SoOrderBo.Instance.CreateShopOrderDelayPickUp(shopOrderDelayFilter.shopSysNo,
                                                                      shopOrderDelayFilter.customerSysNo,
                                                                      shopOrderDelayFilter.receiveName,
                                                                      shopOrderDelayFilter.receiveMobilePhoneNumber,
                                                                      shopOrderDelayFilter.internalRemarks,
                                                                      shopOrderDelayFilter.delayReason,
                                                                      shopOrderDelayFilter.pickUpDate,
                                                                      shopOrderDelayFilter.orderItem,
                       invoice, CurrentUser.Base, shopOrderDelayFilter.CoinPay, shopOrderDelayFilter.CouponCode, GetChangePriceItemNew(), GetChangePriceItem(), ds);

                        //写系统日志 2013/11/15 朱家宏 增加 黄志勇修改
                        var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店订单-门店下单延迟处理",
                                                 LogStatus.系统日志目标类型.订单, t.Item1, null, ip,
                                                 CurrentUser.Base.SysNo);
                        result.StatusCode = t.Item1;
                        //同步利嘉订单
                        SynchronizeOrder(t.Item2);
                        tran.Complete();
                    }

                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;

                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店订单-门店下单延迟处理",
                                             LogStatus.系统日志目标类型.订单, 0, ex, ip,
                                             CurrentUser.Base.SysNo);
                }
            }
            return Json(result);
        }


        /// <summary>
        /// 利嘉订单订单同步
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-05-18 罗勤尧 创建</remarks>
        public ActionResult SynchronizeOrder(int orderID)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "利嘉订单同步操作", LogStatus.系统日志目标类型.订单, orderID,
                                    CurrentUser.Base.SysNo);
            Result r = new Result
            {
                Status = false
            };
            //根据订单获取分销商信息
            var dsdealer = BLL.Distribution.DsDealerBo.Instance.GetCBDsDealerByOrderId(orderID);

            //创建新增分销商对象
            DsDealerLiJia dslijia = null;

            if (dsdealer != null)
            {

                dslijia = new DsDealerLiJia();
                dslijia.MemberName = dsdealer.MemberName;
                dslijia.PhoneNumber = dsdealer.PhoneNumber;

                //如果省为空值
                if (string.IsNullOrWhiteSpace(dsdealer.Province))
                {
                    //添加一个默认值
                    dslijia.Province = "广东省";
                }
                else
                {
                    dslijia.Province = dsdealer.Province;
                }
                dslijia.QQ = dsdealer.QQ;
                //如果会员地址为空值
                if (string.IsNullOrWhiteSpace(dsdealer.AddressLine))
                {
                    //添加一个默认值
                    dslijia.AddressLine = "默认地址";
                }
                else
                {
                    dslijia.AddressLine = dsdealer.AddressLine;
                }
                dslijia.BankAccount = dsdealer.BankAccount;
                dslijia.CellPhone = dsdealer.CellPhone;

                //如果市为空值
                if (string.IsNullOrWhiteSpace(dsdealer.City))
                {
                    //添加一个默认值
                    dslijia.City = "深圳市";
                }
                else
                {
                    dslijia.City = dsdealer.City;
                }
                //如果联系人为空值
                if (string.IsNullOrWhiteSpace(dsdealer.Contact))
                {
                    //添加一个默认值
                    dslijia.Contact = dsdealer.MemberName;
                }
                else
                {
                    dslijia.Contact = dsdealer.Contact;
                }

                //如果区为空值
                if (string.IsNullOrWhiteSpace(dsdealer.District))
                {
                    //添加一个默认值
                    dslijia.District = "罗湖区";
                }
                else
                {
                    dslijia.District = dsdealer.District;
                }
                dslijia.Email = dsdealer.Email;
                dslijia.Fax = dsdealer.Fax;
                dslijia.UserName = dsdealer.UserName;

            }
            else
            {
                r.Message = "分销商不存在";
                return Json(r, JsonRequestBehavior.AllowGet);
            }

            try
            {
                //查询本系统是否保存有利嘉分销商会员id    
                var dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                if (dealer.LiJiaSysNo > 0)
                {
                    //获取同步订单数据
                    LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                    lijiamodel.MemberId = dealer.LiJiaSysNo;
                    //上线请注释测试
                    //lijiamodel.Memo = "测试订单";
                    var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                    if (res.Success)
                    {
                        //返回的利嘉erp订单号
                        r.Message = "订单同步成功";
                        r.Status = true;
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        r.Message = "订单同步失败";
                        return Json(r, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    //调接口新增会员
                    var rest = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaMemerber(dslijia);
                    if (rest.Success)
                    {
                        //新增分销商会员成功
                        //查询分销商
                        var dealerinfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                        if (dealerinfo != null)
                        {
                            dealerinfo.LiJiaSysNo = rest.MemberId;
                            //保存对方返回的会员id
                            int i = BLL.Distribution.DsDealerBo.Instance.UpdateLiJiaSysNo(rest.MemberId, dealerinfo.SysNo);
                        }
                        //获取同步订单数据
                        LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                        lijiamodel.MemberId = rest.MemberId;
                        //lijiamodel.MemberId = 348;
                        //上线请注释测试
                        //lijiamodel.Memo = "测试订单";
                        var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                        if (res.Success)
                        {
                            //返回的利嘉erp订单号
                            r.Message = "订单同步成功";
                            r.Status = true;
                            return Json(r, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            r.Message = "订单同步失败";
                            return Json(r, JsonRequestBehavior.AllowGet);
                        }


                    }
                }


                //else
                //{
                //    //查询分销商会员接口
                //    string searchpath = "member.search";
                //    //构建查询参数
                //    //LiJiaMemberSearch lijiasearch = new LiJiaMemberSearch();
                //    //lijiasearch.page = 1;
                //    //lijiasearch.rows = 20;
                //    //lijiasearch.rules = new List<LiJiaSearch>();
                //    //LiJiaSearch search = new LiJiaSearch();
                //    //search.field = "CellPhone";
                //    //search.data = dsdealer.CellPhone;
                //    //lijiasearch.rules.Add(search);
                //    //LiJiaSearch searchT = new LiJiaSearch();
                //    //searchT.field = "UserName";
                //    //searchT.data = dsdealer.UserName;
                //    //lijiasearch.rules.Add(searchT);
                //    //string searchjosn = JsonHelper.ToJson(lijiasearch);
                //    ////访问查询分销商接口
                //    //ResultLiJia searchDsDealer = BLL.Order.LiJiaSoOrderSynchronize.SeachLiJiaMemerber(lijiasearch);
                //    //if (!string.IsNullOrEmpty(searchDsDealer))
                //    //{
                //    //var searchreturn = JsonSerializationHelper.JsonToObject<ResultLiJia>(searchDsDealer);
                //    //if (searchreturn.Success)
                //    //{

                //    //查询本系统保存的利嘉分销商会员id    
                //    var dealerinfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealerByOrderSysNo(orderID);
                //    if (dealerinfo != null)
                //    {
                //        //获取同步订单数据
                //        LiJiaOrderModel lijiamodel = BLL.Order.SoOrderBo.Instance.GetLiJiaOrderByOrderNo(orderID);
                //        lijiamodel.MemberId = dealerinfo.LiJiaSysNo;
                //        //上线请注释测试
                //        //lijiamodel.Memo = "测试订单";
                //        var res = BLL.Order.LiJiaSoOrderSynchronize.AddLiJiaOrder(lijiamodel);
                //        if (res)
                //        {
                //            //返回的利嘉erp订单号
                //            r.Message = "订单同步成功";
                //            r.Status = true;
                //            return Json(r, JsonRequestBehavior.AllowGet);
                //        }
                //        else
                //        {
                //            r.Message = "订单同步失败";
                //            return Json(r, JsonRequestBehavior.AllowGet);
                //        }
                //}

                // }

            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取门店订单调价项(Key= 购物车编号 value=调价值)
        /// </summary>
        /// <remarks>2014-04-18 朱成果 门店订单调价</remarks>
        private Dictionary<int, decimal> GetChangePriceItem()
        {
            if (TempData.ContainsKey("ChangePriceitem") && TempData["ChangePriceitem"] != null)
            {
                var lst = TempData["ChangePriceitem"] as List<SelectListItem>;
                Dictionary<int, decimal> dd = new Dictionary<int, decimal>();
                foreach (var item in lst)
                {
                    dd.Add(int.Parse(item.Text), decimal.Parse(item.Value));
                }
                return dd;
            }
            return null;
        }
        /// <summary>
        /// 获取门店订单调价项(Key= 购物车编号 value=调价值)
        /// </summary>
        /// <remarks>2014-04-18 朱成果 门店订单调价</remarks>
        private List<SelectListItemNew> GetChangePriceItemNew()
        {
            if (TempData.ContainsKey("ChangeDanPriceitem") && TempData["ChangeDanPriceitem"] != null)
            {
                var lst = TempData["ChangeDanPriceitem"] as List<SelectListItemNew>;
                //Dictionary<int, decimal> dd = new Dictionary<int, decimal>();
                //foreach (var item in lst)
                //{
                //    dd.Add(int.Parse(item.Text), decimal.Parse(item.Value));
                //}
                return lst;
            }
            return null;
        }
        /// <summary>
        /// 门店下单提交快递送达
        /// </summary>
        /// <param name="shopOrderShipFilter">门店下单提交快递送达参数实体.</param>
        /// <returns>提交结果json对象</returns>
        /// <remarks>2013-06-19 余勇 修改 增加参数校验及将参数实体化</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1004201)]
        public ActionResult CreateShopOrderShip(ParaShopOrderShipFilter shopOrderShipFilter)
        {
            Result result = CheckOrder(shopOrderShipFilter);
            if (result.StatusCode != -1)
            {
                FnInvoice invoice = null;
                SoReceiveAddress receiveAddress = null;
                CrReceiveAddress crRecieveAddress = new CrReceiveAddress();
                //当选择已有的地址则修改会员地址信息，否则添加
                if (shopOrderShipFilter.ReceiveAddressSysNo > 0)
                {
                    crRecieveAddress =
                        SoOrderBo.Instance.GetCustomerAddressBySysNo(shopOrderShipFilter.ReceiveAddressSysNo);
                    if (crRecieveAddress != null)
                    {
                        crRecieveAddress.Name = shopOrderShipFilter.receiveName;
                        crRecieveAddress.MobilePhoneNumber = shopOrderShipFilter.receiveMobilePhoneNumber;
                        crRecieveAddress.AreaSysNo = shopOrderShipFilter.areaSysNo;
                        crRecieveAddress.StreetAddress = shopOrderShipFilter.streetAddress;
                        receiveAddress = new SoReceiveAddress()
                        {
                            Name = crRecieveAddress.Name,
                            MobilePhoneNumber = crRecieveAddress.MobilePhoneNumber,
                            AreaSysNo = crRecieveAddress.AreaSysNo,
                            StreetAddress = crRecieveAddress.StreetAddress,
                            Gender = crRecieveAddress.Gender,
                            ZipCode = crRecieveAddress.ZipCode,
                            EmailAddress = crRecieveAddress.EmailAddress,
                            FaxNumber = crRecieveAddress.FaxNumber,
                            PhoneNumber = crRecieveAddress.PhoneNumber
                        };
                        SoOrderBo.Instance.UpdateReceiveAddress(crRecieveAddress);
                    }
                }
                else
                {
                    receiveAddress = new SoReceiveAddress()
                    {
                        Name = shopOrderShipFilter.receiveName,
                        MobilePhoneNumber = shopOrderShipFilter.receiveMobilePhoneNumber,
                        AreaSysNo = shopOrderShipFilter.areaSysNo,
                        StreetAddress = shopOrderShipFilter.streetAddress
                    };
                    crRecieveAddress.Name = shopOrderShipFilter.receiveName;
                    crRecieveAddress.MobilePhoneNumber = shopOrderShipFilter.receiveMobilePhoneNumber;
                    crRecieveAddress.AreaSysNo = shopOrderShipFilter.areaSysNo;
                    crRecieveAddress.StreetAddress = shopOrderShipFilter.streetAddress;
                    crRecieveAddress.CustomerSysNo = shopOrderShipFilter.customerSysNo;
                    SoOrderBo.Instance.InsertReceiveAddress(crRecieveAddress);
                }

                if (!string.IsNullOrWhiteSpace(shopOrderShipFilter.invoiceTitle))
                {
                    invoice = new FnInvoice()
                    {
                        InvoiceTitle = shopOrderShipFilter.invoiceTitle
                    };
                }
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        var orderSysNo = SoOrderBo.Instance.CreateShopOrderToCourier(shopOrderShipFilter.shopSysNo,
                                                                    shopOrderShipFilter.customerSysNo, receiveAddress,
                                                                    shopOrderShipFilter.internalRemarks,
                                                                    shopOrderShipFilter.customerMessage,
                                                                    shopOrderShipFilter.orderItem, invoice,
                                                                    shopOrderShipFilter.PayType, CurrentUser.Base,
                                                                    shopOrderShipFilter.VoucherNo, shopOrderShipFilter.EasReceiptCode, shopOrderShipFilter.CoinPay, shopOrderShipFilter.CouponCode, GetChangePriceItem());

                        //写系统日志 2013/11/15 朱家宏 增加 黄志勇修改
                        var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店订单-门店下单提交快递送达",
                                                 LogStatus.系统日志目标类型.订单, orderSysNo, null, ip,
                                                 CurrentUser.Base.SysNo);
                        result.StatusCode = orderSysNo;
                        tran.Complete();
                    }

                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;

                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店订单-门店下单提交快递送达",
                                             LogStatus.系统日志目标类型.订单, 0, ex, ip,
                                             CurrentUser.Base.SysNo);
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 校验门店下单参数
        /// </summary>
        /// <param name="filter">门店下单参数</param>
        /// <returns>结果对象</returns>
        /// <remarks>2013-06-28 余勇 创建</remarks>
        private Result CheckOrder(ParaShopOrderCreateFilter filter)
        {
            Result res = new Result();
            if (filter.shopSysNo == 0)
            {
                res.StatusCode = -1;
                res.Message = "门店编号不能为空";
            }
            else if (filter.customerSysNo == 0)
            {
                res.StatusCode = -1;
                res.Message = "会员编号不能为空";
            }
            else if (filter.orderItem.Count == 0)
            {
                res.StatusCode = -1;
                res.Message = "订购商品不能为空";
            }
            else
            {
                foreach (SoOrderItem item in filter.orderItem)
                {
                    if (item.Quantity == 0)
                    {
                        res.StatusCode = -1;
                        res.Message = "订购商品数量不能为0";
                        break;
                    }
                }
            }
            return res;
        }

        #endregion

        #region 门店提货页面 余勇 2013-07-06 创建

        /// <summary>
        /// 门店提货列表
        /// </summary>
        ///<return>跳转到门店提货列表</return>
        /// <remarks>2013-07-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005101)]
        public ActionResult ShopDeliveryList()
        {
            ViewBag.Shops = ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
            return View();
        }

        /// <summary>
        /// 门店提货
        /// </summary>
        /// <param name="id">出库单编号</param>
        /// <returns>门店提货页面</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult ShopDelivery(int id)
        {
            var outStock = WhWarehouseBo.Instance.Get(id); //出库单实体
            if (outStock == null)
            {
                return RedirectToAction("ShopDeliveryList");
            }
            if (outStock.Status != (int)WarehouseStatus.出库单自提状态.待自提)
            {
                return RedirectToAction("ShopDeliveryList");
            }
            ViewBag.Title = "门店提货-提货";
            ViewBag.ActionType = 1;
            ViewBag.SysNo = id;
            ViewBag.WhStockOut = outStock;
            //门店订单
            var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);
            bool isPayed = model.CashPay == 0 || model.PayStatus == (int)OrderStatus.销售单支付状态.已支付;
            //再次检查订单是否已经付款 2013/10/21 朱成果
            ViewBag.IsPayed = isPayed;
            var invoice = InvoiceBo.Instance.GetModel(model.InvoiceSysNo);

            ViewBag.Invoice = invoice;
            ViewBag.InvoiceTypeList = FnInvoiceBo.Instance.GetFnInvoiceTypeList();

            //2013-11-06 朱家宏 增加开票判断
            ViewBag.DisplayInvoice = !ShopOrderBo.Instance.HasInvoiced(outStock.OrderSysNO);

            return View();
        }

        /// <summary>
        /// 门店提货确认
        /// </summary>
        /// <param name="id">出库单编号</param>
        /// <returns>门店提货页面</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005201)]
        public ActionResult ShopDeliveryConfirm(int id)
        {
            var outstock = WhWarehouseBo.Instance.Get(id); //出库单实体
            if (outstock == null)
            {
                return RedirectToAction("ShopDeliveryList");
            }
            if (outstock.Status != (int)WarehouseStatus.出库单自提状态.待确认)
            {
                return RedirectToAction("ShopDeliveryList");
            }
            ViewBag.WhStockOut = outstock;
            ViewBag.Title = "门店提货-确认";
            ViewBag.ActionType = 2;
            ViewBag.SysNo = id;
            var model = SoOrderBo.Instance.GetEntity(outstock.OrderSysNO);
            bool isPayed = model.CashPay == 0 || model.PayStatus == (int)OrderStatus.销售单支付状态.已支付;
            //再次检查订单是否已经付款 2013/10/21 朱成果
            ViewBag.IsPayed = isPayed;
            return View("ShopDelivery");
        }

        /// <summary>
        /// 门店提货详情
        /// </summary>
        /// <param name="id">出库单编号</param>
        /// <returns>门店提货页面</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005101)]
        public ActionResult ShopDeliveryDetail(int id)
        {
            ViewBag.SysNo = id;
            return View();
        }

        /// <summary>
        /// 提交快速送达信息
        /// </summary>
        /// <param name="id">出库单编号</param>
        /// <returns>快速送达页面</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult ShopDeliveryShip(int id)
        {
            //出库单
            WhStockOut outStock = WhWarehouseBo.Instance.Get(id);
            //门店订单
            var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);
            bool isPayed = model.CashPay == 0 || model.PayStatus == (int)OrderStatus.销售单支付状态.已支付;
            //再次检查订单是否已经付款 2013/10/21 朱成果
            ViewBag.IsPayed = isPayed;
            ViewBag.ReceiveAddressSysNo = outStock.ReceiveAddressSysNo;
            ViewBag.SysNo = id;
            ViewBag.WhStockOut = outStock;
            ViewBag.InvoiceTypeList = FnInvoiceBo.Instance.GetFnInvoiceTypeList();

            var invoice = InvoiceBo.Instance.GetModel(model.InvoiceSysNo);
            ViewBag.Invoice = invoice;
            return View();
        }

        /// <summary>
        /// 门店提货信息
        /// </summary>
        /// <param name="id">出库单编号</param>
        /// <returns>门店提货信息页面</returns>
        /// <remarks>2013-07-6 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005101)]
        public ActionResult ShopDeliveryInfo(int id)
        {
            //出库单
            WhStockOut outStock = WhWarehouseBo.Instance.Get(id);

            if (outStock == null)
            {
                return RedirectToAction("ShopDeliveryList");
            }
            ViewBag.WhStockOut = outStock;
            var Warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(outStock.WarehouseSysNo);
            ViewBag.WarehouseName = Warehouse.WarehouseName;
            //门店订单
            var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);

            //收货地址
            model.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(outStock.ReceiveAddressSysNo);
            //显示省市区
            if (model.ReceiveAddress != null)
            {
                //城市
                BsArea cEntity;
                //地区
                BsArea aEntity;
                var pEntity = BasicAreaBo.Instance.GetProvinceEntity(model.ReceiveAddress.AreaSysNo,
                                                                                   out cEntity, out aEntity);
                string addressDetatil = string.Empty;
                if (pEntity != null) addressDetatil = pEntity.AreaName;
                if (cEntity != null) addressDetatil = addressDetatil + "  " + cEntity.AreaName;
                if (aEntity != null) addressDetatil = addressDetatil + "  " + aEntity.AreaName;
                ViewBag.AddressDetatil = addressDetatil;
            }
            //会员信息
            model.Customer = SoOrderBo.Instance.SearchCustomer(model.CustomerSysNo);

            //发票信息
            model.OrderInvoice = SoOrderBo.Instance.GetFnInvoice(model.InvoiceSysNo);

            //订购商品数量
            ViewBag.OutStockItem = outStock.Items;
            return PartialView("_ShopDeliveryInfo", model);
        }

        #endregion

        #region 门店提货 余勇 2013-07-08 创建

        /// <summary>
        /// 门店自提确认
        /// </summary>
        /// <param name="sysNo">出库单编号</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks> 
        /// <remarks>2013-11-15 黄志勇 修改</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005201)]
        public ActionResult SelfDeliverySure(int sysNo)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店自提确认", LogStatus.系统日志目标类型.出库单, sysNo,
                                    CurrentUser.Base.SysNo);
            Result result = new Result() { StatusCode = 0, Message = "" };
            try
            {
                ShopOrderBo.Instance.SelfDeliverySure(sysNo, CurrentUser.Base);

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-确认",
                                         LogStatus.系统日志目标类型.出库单, sysNo, null, ip,
                                         CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("短信发送失败")) result.StatusCode = -1;
                result.Message = ex.Message;

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-确认",
                                         LogStatus.系统日志目标类型.出库单, sysNo, ex, ip,
                                         CurrentUser.Base.SysNo);
            }
            return Json(result);

        }

        /// <summary>
        /// 延迟自提
        /// </summary>
        /// <param name="sysNO">出库单号</param>
        /// <param name="delayDate">延迟自提日期</param>
        /// <param name="reason">原因</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks> 
        /// <remarks>2013-11-15 黄志勇 修改</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1004201)]
        public ActionResult SelfDeliveryDelay(int sysNO, DateTime delayDate, string reason)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店延迟自提", LogStatus.系统日志目标类型.出库单, sysNO,
                                    CurrentUser.Base.SysNo);
            Result result = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    ShopOrderBo.Instance.SelfDeliveryDelay(sysNO, delayDate, reason);

                    //写系统日志 2013/11/15 朱家宏 增加
                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-延迟自提",
                                             LogStatus.系统日志目标类型.出库单, sysNO, null, ip,
                                             CurrentUser.Base.SysNo);

                    tran.Complete();
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-延迟自提",
                                         LogStatus.系统日志目标类型.出库单, sysNO, ex, ip,
                                         CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 门店自提出库单设为缺货
        /// </summary>
        /// <param name="sysNo">出库单号</param>
        /// <param name="reason">缺货原因</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-06 余勇 创建</remarks> 
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult SetOutOfStock(int sysNo, string reason)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店自提设为缺货", LogStatus.系统日志目标类型.出库单, sysNo,
                                    CurrentUser.Base.SysNo);
            Result result = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    ShopOrderBo.Instance.SetOutOfStock(sysNo, reason, CurrentUser.Base, true);

                    //写系统日志 2013/11/15 朱家宏 增加
                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-缺货",
                                             LogStatus.系统日志目标类型.出库单, sysNo, null, ip,
                                             CurrentUser.Base.SysNo);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-缺货",
                                         LogStatus.系统日志目标类型.出库单, sysNo, ex, ip,
                                         CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 门店提货-未付款
        /// </summary>
        /// <param name="filter">门店提货参数对象</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-30 余勇 创建</remarks> 
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult PickUp(ParaShopDeliveryFilter filter)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店提货-未付款", LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo,
                                   CurrentUser.Base.SysNo);
            Result result = new Result();
            FnInvoice invoice = null;
            //出库单
            WhStockOut outStock = WhWarehouseBo.Instance.Get(filter.stockOutSysNo);
            //门店订单
            var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);
            if (filter.invoiceType > 0)
            {
                invoice = new FnInvoice()
                {
                    InvoiceCode = filter.invoiceCode,
                    InvoiceTitle = filter.invoiceTitle,
                    InvoiceTypeSysNo = filter.invoiceType,
                    InvoiceNo = filter.invoiceNo,
                    InvoiceAmount = model.CashPay,
                    Status = (int)FinanceStatus.发票状态.已开票
                };
            }
            try
            {
                using (var tran = new TransactionScope())
                {
                    ShopOrderBo.Instance.PickUp(filter.stockOutSysNo, filter.payMoney, filter.paymentType,
                        CurrentUser.Base, invoice, filter.VoucherNo, false, (filter.payMoney > 0 ? filter.EasReceiptCode : string.Empty));

                    //写系统日志 2013/11/15 朱家宏 增加
                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-未付款",
                                             LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo, null, ip,
                                             CurrentUser.Base.SysNo);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-未付款",
                                         LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo, ex, ip,
                                         CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 门店提货-已付款
        /// </summary>
        /// <param name="filter">门店提货参数对象</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-07-30 余勇 创建</remarks> 
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult PayedPickUp(ParaShopDeliveryFilter filter)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店提货-已付款", LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo,
                                   CurrentUser.Base.SysNo);
            Result result = new Result();
            FnInvoice invoice = null;
            var WhPickUpCode = ShopOrderBo.Instance.GetEntityByStockOutNo(filter.stockOutSysNo);

            if (WhPickUpCode == null || string.IsNullOrWhiteSpace(WhPickUpCode.Code))
            {
                result.StatusCode = -1;
                result.Message = "该订单无验证码";
            }
            else if (string.IsNullOrWhiteSpace(filter.pickUpCode))
            {
                result.StatusCode = -1;
                result.Message = "验证码不能为空";
            }
            else if (!string.Equals(filter.pickUpCode, WhPickUpCode.Code))
            {
                result.StatusCode = -1;
                result.Message = "验证码不正确";
            }
            //出库单
            WhStockOut outStock = WhWarehouseBo.Instance.Get(filter.stockOutSysNo);

            //门店订单
            var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);

            if (filter.invoiceType > 0)
            {
                invoice = new FnInvoice()
                {
                    InvoiceCode = filter.invoiceCode,
                    InvoiceTitle = filter.invoiceTitle,
                    InvoiceTypeSysNo = filter.invoiceType,
                    InvoiceAmount = model.CashPay,
                    InvoiceNo = filter.invoiceNo,
                    Status = (int)FinanceStatus.发票状态.已开票
                };
            }
            if (result.StatusCode >= 0)
            {
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        ShopOrderBo.Instance.PickUp(filter.stockOutSysNo, null, 0, CurrentUser.Base, invoice, null);

                        //写系统日志 2013/11/15 朱家宏 增加
                        var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-已付款",
                                                 LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo, null, ip,
                                                 CurrentUser.Base.SysNo);

                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    result.StatusCode = -1;
                    result.Message = ex.Message;

                    //写系统日志 2013/11/15 朱家宏 增加
                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-已付款",
                                             LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo, ex, ip,
                                             CurrentUser.Base.SysNo);
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 是否存在未支付出库单，并返回
        /// </summary>
        /// <param name="stockOutNo">待付款的出库单号</param>
        /// <returns>true 未支付 false 已支付</returns>
        /// <remarks>2013-08-08 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult GetUnPaidStockOutNo(int stockOutNo)
        {
            Result<string> result = new Result<string>();
            int outNo = 0;
            //出库单
            WhStockOut outStock = WhWarehouseBo.Instance.Get(stockOutNo);
            if (outStock != null)
            {
                bool r = ShopOrderBo.Instance.GetUnPaidStockOutNo(outStock.OrderSysNO, out outNo);
                if (r && outNo != stockOutNo && outNo > 0)
                {
                    outStock = WhWarehouseBo.Instance.Get(outNo);
                    result.StatusCode = -1;
                    if (outStock.Status == (int)WarehouseStatus.出库单自提状态.待确认)
                    {
                        result.Data = "/Warehouse/ShopDeliveryConfirm/" + outNo;

                    }
                    else if (outStock.Status == (int)WarehouseStatus.出库单自提状态.待自提)
                    {
                        result.Data = "/Warehouse/ShopDelivery/" + outNo;
                    }
                }
            }
            return Json(new { StatusCode = result.StatusCode, Url = result.Data, Id = outNo },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发送门店提货验证码
        /// </summary>
        /// <param name="sysNo">出库单编号</param>
        /// <returns>验证码</returns>
        /// <remarks>2013-07-09 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult SendSelfDeliveryValidation(int sysNo)
        {
            Result result = new Result();
            if (sysNo == 0)
            {
                result.StatusCode = -1;
                result.Message = "出库单编号不能为空";
                return Json(result);
            }
            try
            {
                //出库单
                WhStockOut outStock = WhWarehouseBo.Instance.Get(sysNo);
                //门店订单
                var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);
                //收货地址
                var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
                var Shop = WhWarehouseBo.Instance.GetWarehouseEntity(outStock.WarehouseSysNo); //仓库信息
                if (Shop != null && receiveAddress != null &&
                    !string.IsNullOrWhiteSpace(receiveAddress.MobilePhoneNumber))
                {
                    using (var tran = new TransactionScope())
                    {
                        var res = ShopOrderBo.Instance.SendSelfDeliveryValidation(sysNo, outStock.OrderSysNO,
                                                                                  receiveAddress.MobilePhoneNumber, Shop);
                        if (res.Status == SmsResultStatus.Failue)
                        {
                            result.StatusCode = -1;
                            result.Message = "短信发送失败，请检查当前会员是否支持接收短信";
                        }

                        //写系统日志 2013/11/15 朱家宏 增加
                        var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-发送门店提货验证码",
                                                 LogStatus.系统日志目标类型.出库单, sysNo, null, ip,
                                                 CurrentUser.Base.SysNo);

                        tran.Complete();
                    }
                }
                else
                {
                    result.StatusCode = -1;
                    result.Message = "收货人电话不能为空";
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-发送门店提货验证码",
                                         LogStatus.系统日志目标类型.出库单, sysNo, null, ip,
                                         CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 门店提货转快递
        /// </summary>
        /// <param name="filter">门店提货参数对象</param>
        /// <remarks>
        /// <returns>操作结果</returns>
        /// 2013-07-09 余勇 创建
        /// 2013/10/14 朱家宏 增加 保存字段“刷卡流水号”
        /// </remarks>
        /// <remarks>2013-11-15 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SO1005601)]
        public ActionResult SetOrderToCourier(ParaShopDeliveryFilter filter)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店提货转快递", LogStatus.系统日志目标类型.出库单, filter.stockOutSysNo,
                                  CurrentUser.Base.SysNo);
            Result result = new Result();
            FnInvoice invoice = null;
            SoReceiveAddress receiveAddress = null;
            decimal? receiveMoney = null;
            if (filter.invoiceType > 0)
            {
                invoice = new FnInvoice()
                {
                    InvoiceCode = filter.invoiceCode,
                    InvoiceTitle = filter.invoiceTitle,
                    InvoiceTypeSysNo = filter.invoiceType,
                    InvoiceRemarks = filter.invoiceRemarks
                };
            }
            //出库单
            WhStockOut outStock = WhWarehouseBo.Instance.Get(filter.stockOutSysNo);
            if (outStock.Receivable > 0)
            {
                receiveMoney = outStock.Receivable;
            }
            //门店订单
            var model = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);
            //收货地址
            receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.ReceiveAddressSysNo);
            if (receiveAddress == null)
            {
                receiveAddress = new SoReceiveAddress();
            }
            receiveAddress.AreaSysNo = filter.areaSysNo;
            receiveAddress.StreetAddress = filter.streetAddress;
            try
            {
                using (var tran = new TransactionScope())
                {
                    ShopOrderBo.Instance.SetOrderToCourier(filter.stockOutSysNo, receiveAddress, filter.shipReson,
                                                           filter.customerMessage, invoice, filter.payType, receiveMoney,
                                                           CurrentUser.Base, filter.VoucherNo, filter.EasReceiptCode);

                    //写系统日志 2013/11/15 朱家宏 增加
                    var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店自提-门店提货转快递",
                                             LogStatus.系统日志目标类型.出库单, outStock.SysNo, null, ip,
                                             CurrentUser.Base.SysNo);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Message = ex.Message;

                //写系统日志 2013/11/15 朱家宏 增加
                var ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店自提-门店提货转快递",
                                         LogStatus.系统日志目标类型.出库单, outStock.SysNo, ex, ip,
                                         CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 检查出库号
        /// </summary>
        /// <param name="filter">门店提货参数</param>
        /// <returns>检查结果</returns>
        /// <remarks>2013-11-15 黄志勇 新增</remarks>
        private Result CheckShopDeliver(ParaShopDeliveryFilter filter)
        {
            Result res = new Result();
            if (filter.stockOutSysNo == 0)
            {
                res.StatusCode = -1;
                res.Message = "出库单号不能为空";
            }
            return res;
        }

        #endregion

        #region 仓库配送方式和取件方式维护 郑荣华 2013-07-09 创建

        /// <summary>
        /// 仓库配送方式和取件方式配置页面
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// <remarks> 
        /// 2013-08-19 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007201)]
        public ActionResult WhDeliveryType()
        {
            //一般默认只能编辑第三方快递，3代表父级为第三方快递
            var highPower = CurrentUser.PrivilegeList.HasPrivilege(PrivilegeCode.WH1007801);
            ViewBag.HighPower = highPower;

            return View();
        }

        /// <summary>
        /// 配置仓库配送取件
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">要添加的配送方式系统编号(1,2,3)</param>
        /// <param name="pickUpTypeSysNo">要添加的取件方式系统编号(1,2,3)</param>    
        /// <returns>是否成功</returns>
        /// <remarks> 
        /// 2013-09-19 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007201)]
        public bool SetWhDeliveryAndPickUp(int whSysNo, string deliveryTypeSysNo, string pickUpTypeSysNo)
        {
            var bd = AddWareHouseDeliveryType(whSysNo, deliveryTypeSysNo);
            var bp = true;
            if (CurrentUser.PrivilegeList.HasPrivilege(PrivilegeCode.WH1007801))
                bp = AddWareHousePickUpType(whSysNo, pickUpTypeSysNo);
            return bd && bp;
        }
        /// <summary>
        /// 获取运费模板
        /// </summary>
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1006001)]
        public JsonResult GetFreightModuleZTreeList()
        {

            var list = LgFreightModuleBo.Instance.GetFreightModuleList();
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                        {
                            id = c.SysNo
                            ,
                            name = c.ModuleName
                            ,
                            title = c.ModuleName
                            ,
                            open = false
                            ,
                            pId = 0
                        };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }
        #region 仓库配送方式

        /// <summary>
        /// 仓库快递方式维护页面
        /// </summary>
        /// <param name=""></param>
        /// <returns>仓库快递方式维护页面</returns>
        /// <remarks>
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        public ActionResult WareHouseDeliveryType()
        {
            //有权限操作的仓库列表
            var listWh = CurrentUser.Warehouses;
            return View(listWh);
        }

        /// <summary>
        /// 仓库配送方式列表页面
        /// </summary>
        /// <param name="filter">查询条件实体</param>
        /// <returns>仓库配送方式列表页面</returns>
        /// <remarks>
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007101)]
        public ActionResult WareHouseDeliveryTypeList(ParaWhDeliveryTypeFilter filter)
        {
            var list = WhWarehouseBo.Instance.GetLgDeliveryType(filter);
            return View(list);
        }
        /// <summary>
        /// 获取仓库物流关联详情
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-20 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1007001)]
        public JsonResult GetWarehouseDeliveryType(int warehouseSysNo, int deliveryTypeSysNo)
        {
            var freightModule = WhWarehouseBo.Instance.GetWarehouseDeliveryType(warehouseSysNo, deliveryTypeSysNo);

            var jsonStr = "[]";
            if (freightModule != null)
                jsonStr = "[" + LitJson.JsonMapper.ToJson(freightModule) + "]";
            //返回json数组
            return Json(jsonStr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新仓库配送方式关联运费模板
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="freightModuleSysNo">运费模板系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.CM1007002)]
        public JsonResult UpdateWarehouseDeliveryTypeAssoFreightModule(int warehouseSysNo, int deliveryTypeSysNo, int freightModuleSysNo)
        {
            Result result = new Result()
            {
                Status = WhWarehouseBo.Instance.UpdateWarehouseDeliveryTypeAssoFreightModule(warehouseSysNo, deliveryTypeSysNo, freightModuleSysNo)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加仓库快递方式
        /// </summary>
        /// <param name="whSysNo">要添加的仓库系统编号</param>
        /// <param name="deliveryTypeSysNo">要添加的快递方式系统编号(1,2,3)</param>
        /// <returns>是否添加成功</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007201)]
        public bool AddWareHouseDeliveryType(int whSysNo, string deliveryTypeSysNo)
        {
            if (deliveryTypeSysNo == "")
            {
                deliveryTypeSysNo = "0";
            }
            var dtSysNo = deliveryTypeSysNo.Split(',');

            var model = new WhWarehouseDeliveryType
                {
                    CreatedBy = CurrentUser.Base.SysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = CurrentUser.Base.SysNo,
                    LastUpdateDate = DateTime.Now,
                    Status = 1,
                    WarehouseSysNo = whSysNo
                };
            var list = Array.ConvertAll(dtSysNo, int.Parse).ToList(); //最终结果

            var whFilter = new ParaWhDeliveryTypeFilter { WareHouseSysNo = whSysNo };
            var listHadOwn =
                WhWarehouseBo.Instance.GetLgDeliveryType(whFilter).Select(p => p.DeliveryTypeSysNo).ToList(); //当前情况

            var listDel = listHadOwn.Except(list).ToList(); //要删除的
            var listAdd = list.Except(listHadOwn).ToList(); //要添加的
            if (deliveryTypeSysNo == "0")
            {
                listAdd.Remove(0);
            }

            try
            {
                foreach (var item in listAdd) //添加
                {
                    model.DeliveryTypeSysNo = item;
                    WhWarehouseBo.Instance.CreateWareHouseDeliveryType(model);
                }
                foreach (var item in listDel) //删除
                {
                    WhWarehouseBo.Instance.DeleteWareHouseDeliveryType(whSysNo, item);
                }
                return true;
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "添加仓库快递方式:" + ex.Message,
                                        LogStatus.系统日志目标类型.仓库快递方式, whSysNo, ex);
                return false;
            }



        }

        /// <summary>
        /// 删除仓库快递方式
        /// </summary>
        /// <param name="sysNo">要删除的仓库快递方式系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007201)]
        public bool DeleteWareHouseDeliveryType(int sysNo)
        {
            return WhWarehouseBo.Instance.DeleteWareHouseDeliveryType(sysNo);
        }

        /// <summary>
        /// 获取可操作的仓库
        /// </summary>     
        /// <param name=""></param>
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-08-19 郑荣华 创建</remarks>  
        [Privilege(PrivilegeCode.WH1007101)]
        public JsonResult GetWarehouseZTreeList()
        {
            //有权限操作的仓库列表 排除禁用仓库 余勇 2014-09-22
            var listWh = CurrentUser.Warehouses.Where(x => x.Status == 1).ToList();

            //模拟添加父级列 
            var par1 = new WhWarehouse { SysNo = -10, BackWarehouseName = "仓库", WarehouseType = 0, Status = 1 };

            listWh.Insert(0, par1);

            var par2 = new WhWarehouse { SysNo = -20, BackWarehouseName = "门店", WarehouseType = 0, Status = 1 };

            listWh.Insert(0, par2);

            var par3 = new WhWarehouse { SysNo = -30, BackWarehouseName = "保税", WarehouseType = 0, Status = 1 };

            listWh.Insert(0, par3);

            var par4 = new WhWarehouse { SysNo = -40, BackWarehouseName = "直邮", WarehouseType = 0, Status = 1 };

            listWh.Insert(0, par4);
            //通过Linq生产zTree节点结果集
            var nodes = from c in listWh
                        select new
                            {
                                id = c.SysNo
                                ,
                                name = c.BackWarehouseName
                                ,
                                title = c.BackWarehouseName
                                ,
                                open = false
                                ,
                                pId = -c.WarehouseType //加负号，避免冲突
                                ,
                                status = c.Status,
                            };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取配送方式
        /// </summary>   
        /// <param name=""></param>  
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-08-06 郑荣华 创建</remarks>  
        [Privilege(PrivilegeCode.WH1007101)]
        public JsonResult GetDeliveryTypeZTreeList()
        {

            var list = DeliveryTypeBo.Instance.GetLgDeliveryTypeList();
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                            {
                                id = c.SysNo
                                ,
                                name = c.DeliveryTypeName
                                ,
                                title = c.DeliveryTypeName
                                ,
                                open = false
                                ,
                                pId = c.ParentSysNo
                                ,
                                status = c.Status,
                            };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据仓库查询配送方式列表信息
        /// </summary>
        /// <param name="filter">查询条件，当前只包括WareHouseSysNo</param>
        /// <returns>仓库配送方式列表信息</returns>
        /// <remarks> 
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007101)]
        public JsonResult GetDeliveryType(ParaWhDeliveryTypeFilter filter)
        {
            var list = WhWarehouseBo.Instance.GetLgDeliveryType(filter);
            var rArray = list.Select(x => x.DeliveryTypeSysNo).ToArray();
            return Json("," + string.Join(",", rArray) + ",", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 仓库取件方式

        /// <summary>
        /// 获取取件方式
        /// </summary>  
        /// <param name=""></param>   
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-08-28 郑荣华 创建</remarks>  
        [Privilege(PrivilegeCode.WH1007101)]
        public JsonResult GetPickUpTypeZTreeList()
        {
            var list = LgPickUpTypeBo.Instance.GetLgPickupTypeList();
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                            {
                                id = c.SysNo
                                ,
                                name = c.PickupTypeName
                                ,
                                title = c.PickupTypeName
                                ,
                                open = false
                                ,
                                pId = c.ParentSysNo
                                ,
                                status = c.Status,
                            };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加仓库取件方式
        /// </summary>
        /// <param name="whSysNo">要添加的仓库系统编号</param>
        /// <param name="pickUpTypeSysNo">要添加的取件方式系统编号(1,2,3)</param>
        /// <returns>是否添加成功</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007201)]
        public bool AddWareHousePickUpType(int whSysNo, string pickUpTypeSysNo)
        {
            if (pickUpTypeSysNo == "")
            {
                pickUpTypeSysNo = "0";
            }
            var dtSysNo = pickUpTypeSysNo.Split(',');

            var model = new WhWarehousePickupType
                {
                    CreatedBy = CurrentUser.Base.SysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = CurrentUser.Base.SysNo,
                    LastUpdateDate = DateTime.Now,
                    Status = 1,
                    WarehouseSysNo = whSysNo
                };
            var list = Array.ConvertAll(dtSysNo, int.Parse).ToList(); //最终结果

            var listHadOwn =
                WhWarehouseBo.Instance.GetPickupTypeListByWarehouse(whSysNo).Select(p => p.SysNo).ToList(); //当前情况

            var listDel = listHadOwn.Except(list).ToList(); //要删除的
            var listAdd = list.Except(listHadOwn).ToList(); //要添加的
            if (pickUpTypeSysNo == "0")
            {
                listAdd.Remove(0);
            }

            try
            {
                foreach (var item in listAdd) //添加
                {
                    model.PickupTypeSysNo = item;
                    WhWarehouseBo.Instance.CreateWareHousePickUpType(model);
                }
                foreach (var item in listDel) //删除
                {
                    WhWarehouseBo.Instance.DeleteWareHousePickUpType(whSysNo, item);
                }
                return true;
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "维护仓库取件方式:" + ex.Message,
                                        LogStatus.系统日志目标类型.仓库取件方式, whSysNo, ex);
                return false;
            }




        }

        /// <summary>
        /// 根据仓库查询取件方式列表信息
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <returns>仓库配送方式列表信息</returns>
        /// <remarks> 
        /// 2013-08-28 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.WH1007101)]
        public JsonResult GetPickUpType(int whSysNo)
        {
            var list = WhWarehouseBo.Instance.GetPickupTypeListByWarehouse(whSysNo);
            var rArray = list.Select(x => x.SysNo).ToArray();
            return Json("," + string.Join(",", rArray) + ",", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 预收现金收款单管理 2013-10-14 沈强 创建

        /// <summary>
        /// 查看预收现金收款单管理页面
        /// </summary>
        /// <param name=""></param>
        /// <returns>返回预收现金收款单管理页面</returns>
        /// <remarks>2013-10-14 沈强 创建</remarks>
        [Privilege(PrivilegeCode.WH1008101)]
        public ActionResult ReceiptVoucherList()
        {
            var statusList = new List<SelectListItem>();
            EnumUtil.ToListItem<FinanceStatus.收款单状态>(ref statusList);

            //获取当前登录用户可管理的仓库
            var whWarehouses = AdminAuthenticationBo.Instance.Current.Warehouses;
            ViewBag.whWarehouses = whWarehouses ?? new List<WhWarehouse>();

            if (statusList.Count > 0) ViewBag.statusList = statusList;
            return View();
        }

        /// <summary>
        /// 获取分部现金收款单表格页面
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>返回现金收款单表格页面</returns>
        /// <remarks>2013-10-14 沈强 创建</remarks>
        [Privilege(PrivilegeCode.WH1008101)]
        public ActionResult DoReceiptVoucherQuery(ParaWarehouseFilter filter)
        {
            var pager = FinanceBo.Instance.GetFnReceipt(filter, CurrentUser.Base.SysNo);

            var list = new PagedList<CBFnReceiptVoucher>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("~/Views/Finance/_ReceiptVoucherListPager.cshtml", list);
        }

        /// <summary>
        /// 根据仓库系统编号获取配送员
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>返回配送员json信息</returns>
        /// <remarks>2013-10-14 沈强 创建</remarks>
        [Privilege(PrivilegeCode.WH1008101)]
        public ActionResult GetDeliverymanByWarehouseSysNo(int warehouseSysNo)
        {
            Result<IList<SyUser>> result = new Result<IList<SyUser>>();
            try
            {
                IList<SyUser> deliverymans = WhWarehouseBo.Instance
                                                .GetDeliveryUserList(new List<int> { warehouseSysNo });
                result.Status = true;
                result.Data = deliverymans.Where(d => d.Status == (int)SystemStatus.系统用户状态.启用).ToList();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return Json(result);
        }

        #endregion

        #region 商品库存 2013-11-06 何方

        /// <summary>
        /// 查询仓库商品eas库存
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNos">商品系统编号集合</param>
        /// <returns>
        /// 仓库商品eas库存
        /// </returns>
        /// <remarks>
        /// 2013-11-06 何方
        /// </remarks>
        [Privilege(PrivilegeCode.WH1003301)]
        [HttpPost]
        public JsonResult GetInventory(int warehouseSysNo, int[] productSysNos)
        {

            var result = new Result<dynamic> { Status = false };

            try
            {
                result.Data = WhWarehouseBo.Instance.GetGetInventory(warehouseSysNo, productSysNos);
                result.Status = true;
            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "eas库存查询错误:" + ex.Message, LogStatus.系统日志目标类型.EAS,
                                      warehouseSysNo, ex, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        #endregion

        #region 提货码及验证码分页查询

        /// <summary>
        /// 提货码及验证码查询
        /// </summary>
        /// <returns>验证码查询页面</returns>
        /// <remarks>2013-12-3 余勇 创建</remarks>
        [Privilege(PrivilegeCode.CR1004083)]
        public ActionResult WhPickUpCodeList()
        {
            return View();
        }
        /// <summary>
        /// 提货码及验证码分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>验证码分页查询页面</returns>
        /// <remarks>2013-12-3 余勇 创建</remarks>
        [Privilege(PrivilegeCode.CR1004083)]
        public ActionResult GetPickUpSmsList(ParaWhPickUpCodeFilter filter)
        {
            var pager = new Pager<CBWhPickUpCode> { CurrentPage = filter.id, PageSize = 10 };
            WhWarehouseBo.Instance.GetPickUpSmsList(ref pager, filter);

            var list = new PagedList<CBWhPickUpCode>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };

            return PartialView("_WhPickUpCodePager", list);
        }

        /// <summary>
        /// 短信剩余条数查询
        /// </summary>
        /// <returns>返回查询结果</returns>
        /// <remarks>
        /// 2014-04-01 余勇 创建
        /// 2016-05-27 刘伟豪 修改
        /// </remarks>
        [Privilege(PrivilegeCode.CR1004083)]
        public JsonResult QuerySmsBalance()
        {
            var result = string.Empty;
            try
            {
                //var smsSender = SmsProviderFactory.CreateProvider();
                //result = smsSender.Balance();

                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Grand.Service.EC.Core.SMS.Contract.ISMSService>())
                {
                    var response = service.Channel.GetBalance();
                    result = response.ErrCode;
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "短信剩余条数查询错误",
                            LogStatus.系统日志目标类型.短信咨询, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }
        #endregion

        #region 业务员库存查询 2013-12-11 周唐炬 创建

        /// <summary>
        /// 业务员库存查询
        /// </summary>
        /// <param name="id">当前页码</param>
        /// <param name="filter">查询参数</param>
        /// <returns>查询结果</returns>
        /// <remarks>2013-12-11 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.WH1005101)]
        public ActionResult InventorySalesman(int? id, ParaProductLendFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                filter.CurrentPage = id ?? 1;
                var list = ProductLendBo.Instance.GetInventoryProductList(filter);
                return PartialView("_AjaxPagerInventoryProductList", list);
            }
            //仓库
            ViewBag.Warehouse = CurrentUser.Warehouses;
            return View();
        }
        #endregion

        #region 拣货单，包裹单批量打印
        /// <summary>
        /// 导出快递单模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public void ImportExpressTemplate()
        {
            ExcelUtil.ExportFromTemplate(new List<object>(), @"\Templates\Excel\ImportExpress.xls", 1, "快递单号导入模板", null, false);
        }
        /// <summary>
        /// 导入快递单号
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-10-04 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult ImportExpress()
        {
            if (Request.Files.Count == 0)
                return View();

            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                try
                {
                    result = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.ImportExpress(httpPostedFileBase.InputStream);
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }


                ViewBag.result = Hyt.Util.WebUtil.UrlEncode(result.Message);
            }
            return View();
        }
        /// <summary>
        /// 第三方快递批量出库
        /// </summary>
        /// <returns></returns>
        /// <param name="id">页数</param>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        public ActionResult OutStockPrint(int? id, string status = "10", string warehouseSysNo = "", int orderSysNo = 0, string sort = "desc", string sortBy = "SysNo")
        {
            int pageIndex = id ?? 1;
            const int pageSize = 50;
            ViewBag.sort = sort;
            ViewBag.sortBy = sortBy;
            if (Request.IsAjaxRequest())
            {
                int _status = 0;
                if (!string.IsNullOrWhiteSpace(status))
                {
                    _status = int.Parse(status);
                }
                int _warehouseSysNo = 0;
                if (!string.IsNullOrWhiteSpace(warehouseSysNo))
                {
                    _warehouseSysNo = int.Parse(warehouseSysNo);
                }
                var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
                var pageList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.SearchThirdPartyStockOut(_status, CurrentUser.Base.SysNo, pageIndex, pageSize, orderSysNo, _warehouseSysNo, sort, sortBy);
                return PartialView("_AjaxOutStockPrint", pageList);
            }


            //出库单状态下拉绑定数据
            var statustList = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "", Selected = true}
                };
            EnumUtil.ToListItem<WarehouseStatus.出库单状态>(ref statustList);
            ViewData["status"] = new SelectList(statustList, "Value", "Text", "0");


            IList<WhWarehouse> whWarehouses = CurrentUser.Warehouses;
            ViewBag.WarehouseList = whWarehouses;

            ViewBag.DeliveryTypeList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetThirdPartyExpress(CurrentUser.Base);
            return View();
        }

        /// <summary>
        /// 订单批量出库
        /// </summary>
        /// <returns></returns>
        /// <param name="id">页数</param>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.WH1003101)]
        public ActionResult DRDOutStockPrint(int? id)
        {
            int pageIndex = id ?? 1;
            const int pageSize = 50;
            if (Request.IsAjaxRequest())
            {
                var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
                var pageList = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.SearchDRDStockOut((int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.待出库, CurrentUser.Base.SysNo, pageIndex, pageSize);
                return PartialView("_AjaxOutStockPrint", pageList);
            }
            return View();
        }

        /// <summary>
        /// 批量更新出库数据
        /// </summary>
        /// <param name="ids">出库单编号</param>
        /// <param name="deliverytypeno">配送方式编号</param>
        /// <returns></returns>
        /// <remarks>2014-07-10 朱成果 创建</remarks>
        /// <remarks>2014-08-04 余勇 修改 修改出库状态时判断只有状态为待出库才执行更新(解决并发问题)</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1003301)]
        public JsonResult MultiStockOut(List<int> ids, int? deliverytypeno)
        {
            Result r = new Result() { Status = true };

            foreach (var x in ids)
            {
                var sysNo = x;
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        var master = WhWarehouseBo.Instance.Get(sysNo);
                        int currectdeliverytypeno = master.DeliveryTypeSysNo;//当前配送方式
                        if (deliverytypeno.HasValue && deliverytypeno != -1)//新的配送方式
                        {
                            currectdeliverytypeno = deliverytypeno.Value;
                        }
                        if (master != null && master.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.待出库)
                        {
                            master.DeliveryTypeSysNo = currectdeliverytypeno;
                            master.StockOutDate = DateTime.Now;
                            master.StockOutBy = CurrentUser.Base.SysNo;
                            master.LastUpdateBy = CurrentUser.Base.SysNo;
                            master.LastUpdateDate = DateTime.Now;
                            master.IsPrintedPickupCover = 1;
                            master.IsPrintedPackageCover = 1;
                            master.Status = WarehouseStatus.出库单状态.待配送.GetHashCode();
                            var affectRow = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateStockOutByStatus(master, (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.待出库);
                            //出库状态更新成功后才继续执行
                            if (affectRow > 0)
                            {
                                foreach (var item in master.Items)
                                {
                                    item.IsScaned = true;
                                    item.ScanedQuantity = item.ProductQuantity;
                                    Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateOutItem(item);
                                }

                                //平台仓库库存
                                var _result = WhWarehouseBo.Instance.ReduceStock(-1, master.WarehouseSysNo, master.Items);
                                if (!_result.Status)
                                {
                                    return Json(_result, JsonRequestBehavior.AllowGet);
                                }

                                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "批量出库", LogStatus.系统日志目标类型.出库单, master.SysNo,
                                    CurrentUser.Base.SysNo);
                                var delivery = DeliveryTypeBo.Instance.GetDeliveryType(currectdeliverytypeno);
                                var deliveryName = (delivery == null)
                                    ? "未能找到编号为" + deliverytypeno + "的配送方式"
                                    : delivery.DeliveryTypeName;
                                var logTxt = "订单生成配送方式:<span style=\"color:red\">" + deliveryName + "</span>，待拣货打包";
                                SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo, logTxt,
                                    CurrentUser.Base.UserName);
                                Hyt.BLL.Order.SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo,
                                    "订单拣货打包完毕，待分配配送", CurrentUser.Base.UserName);
                                r.StatusCode++;
                            }
                            scope.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.出库单, sysNo, ex, CurrentUser.Base.SysNo);
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 商品库存
        /// <summary>
        /// 商品库存列表
        /// </summary>
        /// <returns>分页</returns>
        /// <remarks>
        /// 2015-08-27 王耀发 创建
        /// 2016-5-27 杨浩 修改，增加仓库选择
        /// </remarks>
        [Privilege(PrivilegeCode.WH1002303)]
        public ActionResult ProductStockList()
        {
            var warehouses = CurrentUser.Warehouses;
            int warehouseSysNo;
            WhWarehouse warehouseInfo = null;

            if (!int.TryParse(Request["WarehouseSysNo"], out warehouseSysNo) && warehouses.Count > 0)
                warehouseInfo = warehouses[0];

            ViewData["WarehouseSysNo"] = warehouseSysNo.ToString();

            if (warehouseInfo == null)
                warehouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo);

            ViewBag.WarehouseName = warehouseInfo == null ? "选择仓库" : warehouseInfo.BackWarehouseName;
            return View();
        }
        /// <summary>
        /// 获得商品库存，根据商品编号字符串
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-08-27 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public JsonResult GetPdProductStockList(int warehouseSysNo, int[] productSysNos)
        {
            var result = new Result<dynamic> { Status = false };

            try
            {

                result.Data = PdProductStockBo.Instance.GetPdProductStockList(warehouseSysNo, productSysNos);
                result.Status = true;
            }
            catch (Exception ex)
            {

                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "eas库存查询错误:" + ex.Message, LogStatus.系统日志目标类型.EAS,
                                      warehouseSysNo, ex, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 商品入库列表
        /// </summary>
        /// <returns>分页</returns>
        /// <remarks>2015-08-27 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002302)]
        public ActionResult ProductStockInList()
        {
            ViewData["WarehouseSysNo"] = this.Request["WarehouseSysNo"];
            return View();
        }

        /// <summary>
        /// 分页获取库存
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>库存列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002303)]
        public ActionResult DoPdProductStockQuery(ParaProductStockFilter filter)
        {
            filter.PageSize = 10;
            var pager = (!string.IsNullOrEmpty(filter.WarehouseSysNo) && filter.WarehouseSysNo != "0") ? PdProductStockBo.Instance.GetPdProductStockList(filter) : (new Pager<CBPdProductStockList>());
            List<string> erplist = new List<string>();
            var returnValue = new Dictionary<string, int>();

            //foreach (CBPdProductStockList cs in pager.Rows)
            //{
            //    erplist.Add(cs.ErpCode);
            //    if (!returnValue.Keys.Contains(cs.ErpCode))
            //        returnValue.Add(cs.ErpCode, cs.PdProductSysNo);
            //}
            //string[] erpCode = erplist.ToArray();
            //var warehouseInfo = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(filter.WarehouseSysNo);

            //var inventoryList = BLL.Warehouse.WhWarehouseBo.Instance.GetInventory(erpCode, warehouseInfo.ErpCode);
            //if (inventoryList.Status)
            //{

            //    foreach (var _item in inventoryList.Data)
            //    {
            //        if (returnValue.Keys.Contains(_item.MaterialNumber))
            //        {
            //            pager.Rows.ForEach(i =>
            //            {
            //                if (i.ErpCode == _item.MaterialNumber)
            //                {
            //                    i.StockQuantity = _item.Quantity;
            //                }

            //            });
            //            //int row = IPdProductStockDao.Instance.SynchronizeStock(filter.WarehouseSysNo, returnValue[_item.MaterialNumber], _item.Quantity);
            //            //if (row <= 0)
            //            //{
            //            //    var model = new PdProductStock();
            //            //    model.CreatedDate = DateTime.Now;
            //            //    model.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            //            //    model.LastUpdateBy = model.CreatedBy;
            //            //    model.LastUpdateDate = DateTime.Now;
            //            //    model.PdProductSysNo = returnValue[_item.MaterialNumber];
            //            //    model.StockQuantity = _item.Quantity;
            //            //    model.WarehouseSysNo = filter.WarehouseSysNo;
            //            //    IPdProductStockDao.Instance.Insert(model);
            //            //}
            //        }
            //    }
            //}

            foreach (var item in pager.Rows)
            {
                var SpecPricesList = PdProductSpecBo.Instance.GetPdProductSpecPrices(item.ProductSysNo, item.WarehouseSysNo);
                item.SpecPricesList = SpecPricesList;
            }

            var list = new PagedList<CBPdProductStockList>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_ProductStockListPager", list);
        }
        /// <summary>
        /// 创建库存信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-04-13 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002303)]
        public JsonResult CreateProductStockInfo(PdProductStock model)
        {
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            var r = new Result { Status = false, StatusCode = 0, Message = "操作失败" };
            try
            {
                IPdProductStockDao.Instance.Insert(model);
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            if (r.Status)
            {
                r.Message = "操作成功";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新库存信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-02-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002303)]
        public JsonResult UpdateProductStockInfo(PdProductStock model)
        {
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            var r = new Result { Status = false, StatusCode = 0, Message = "操作失败" };
            try
            {
                PdProductStockBo.Instance.UpdateProductStockInfo(model);
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            if (r.Status)
            {
                r.Message = "操作成功";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更新出库单备注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2017-07-18 罗勤尧 创建</remarks>
        [Privilege(PrivilegeCode.WH1003301)]
        public JsonResult UpdateOutStockInfo(int SysNo, string Remarks)
        {

            var r = new Result { Status = false, StatusCode = 0, Message = "操作失败" };
            try
            {
                WhWarehouseBo.Instance.UpdateStockOutRemarks(SysNo, Remarks);
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            if (r.Status)
            {
                r.Message = "操作成功";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除商品入库数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除数据编号</returns>
        /// <remarks>2016-08-1  罗远康 创建</remarks>
        [Privilege(PrivilegeCode.WH1002303)]
        public JsonResult DeleteStock(int sysNo)
        {
            var r = new Result { Status = false, Message = "操作失败" };
            try
            {
                PdProductStockBo.Instance.Delete(sysNo);
                r.Status = true;
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            if (r.Status)
            {
                r.Message = "操作成功";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 分页获取入存明细
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>入存明细列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002302)]
        public ActionResult DoPdProductStockInDetailQuery(ParaProductStockInDetailFilter filter)
        {
            filter.PageSize = 10;
            var pager = PdProductStockInDetailBo.Instance.GetPdProductStockInDetailList(filter);
            var list = new PagedList<PdProductStockInDetailList>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_ProductStockInDetailListPager", list);
        }

        /// <summary>
        /// 入存明细新建/编辑
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002302)]
        public ActionResult ProductStockInAudit()
        {
            if (this.Request["id"] != null)
            {
                int id = int.Parse(this.Request["id"]);
                ViewData["WarehouseSysNo"] = this.Request["WarehouseSysNo"];
                //ViewData["StockInFlag"] = this.Request["StockInFlag"];
                PdProductStockIn model;
                model = PdProductStockInBo.Instance.GetEntity(id);
                return View(model);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002302)]
        public ActionResult ProductStockInView()
        {
            if (this.Request["id"] != null)
            {
                int id = int.Parse(this.Request["id"]);
                ViewData["WarehouseSysNo"] = this.Request["WarehouseSysNo"];
                //ViewData["StockInFlag"] = this.Request["StockInFlag"];
                PdProductStockIn model;
                model = PdProductStockInBo.Instance.GetEntity(id);
                return View(model);
            }
            else
            {
                return null;
            }
        }
        [Privilege(PrivilegeCode.WH1002302)]
        public ActionResult ProductStockInCreate()
        {
            if (this.Request["id"] != null)
            {
                int id = int.Parse(this.Request["id"]);
                ViewData["WarehouseSysNo"] = this.Request["WarehouseSysNo"];
                PdProductStockIn model;
                if (id > 0)
                {
                    model = PdProductStockInBo.Instance.GetEntity(id);
                }
                else
                {
                    model = new PdProductStockIn();
                }
                return View(model);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 保存商品入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns>字符串</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult SavePdProductStockIn()
        {

            PdProductStockIn model = new PdProductStockIn();
            model.SysNo = int.Parse(this.Request["SysNo"]);
            model.StockInNo = this.Request["StockInNo"];
            model.StorageTime = DateTime.Parse(this.Request["StorageTime"]);
            string WarehouseSysNo = this.Request["WarehouseSysNo"];
            string PdProductStockInDetail = this.Request["PdProductStockInDetail"];
            Result result = new Result();
            try
            {
                result = PdProductStockInBo.Instance.SavePdProductStockIn(model, CurrentUser.Base);
                if (result.Status)
                {
                    string[] ProductArray = PdProductStockInDetail.Split(';');
                    foreach (string sArray in ProductArray)
                    {
                        string[] ssArray = sArray.Split(',');
                        PdProductStockInDetail modelInDetail = new PdProductStockInDetail();
                        modelInDetail.ProductStockInSysNo = result.StatusCode;
                        modelInDetail.WarehouseSysNo = int.Parse(WarehouseSysNo);
                        modelInDetail.PdProductSysNo = int.Parse(ssArray[0]);
                        modelInDetail.StorageQuantity = decimal.Parse(ssArray[1]);
                        PdProductStockInDetailBo.Instance.SavePdProductStockInDetail(modelInDetail, CurrentUser.Base);
                    }
                }
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存商品入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns>字符串</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult AduitPdProductStockIn()
        {
            int ProductStockInSysNo = int.Parse(this.Request["ProductStockInSysNo"]);
            string PdProductStockInDetail = this.Request["PdProductStockInDetail"];
            string[] ProductArray = PdProductStockInDetail.Split(';');
            Result result = new Result();
            try
            {
                foreach (string sArray in ProductArray)
                {
                    string[] ssArray = sArray.Split(',');
                    PdProductStockInDetail modelInDetail = new PdProductStockInDetail();
                    modelInDetail.SysNo = int.Parse(ssArray[0]);
                    modelInDetail.DoStorageQuantity = decimal.Parse(ssArray[3]);
                    PdProductStockInDetailBo.Instance.AduitUpdatePdProductStockInDetail(modelInDetail, CurrentUser.Base);
                    /*保存到库存中*/
                    PdProductStock smodel = new PdProductStock();
                    smodel.WarehouseSysNo = int.Parse(ssArray[1]);
                    smodel.PdProductSysNo = int.Parse(ssArray[2]);
                    smodel.StockQuantity = decimal.Parse(ssArray[3]);
                    PdProductStockBo.Instance.SavePdProductStock(smodel, CurrentUser.Base);
                }
                /*更新入库单状态*/
                List<PdProductStockInDetail> detailList = PdProductStockInDetailBo.Instance.GetProductStockInDetail(ProductStockInSysNo);
                WarehouseStatus.入库单状态 Status;
                if (detailList != null && detailList.Count != 0)
                {
                    Status = WarehouseStatus.入库单状态.部分入库;
                }
                else
                {
                    Status = WarehouseStatus.入库单状态.已入库;
                }
                PdProductStockInBo.Instance.UpdateStatus(ProductStockInSysNo, Status, CurrentUser.Base);

                result.Status = true;
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废商品入库单
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult CancelProductStockIn(int id)
        {
            Result r = new Result();
            WarehouseStatus.入库单状态 Status = WarehouseStatus.入库单状态.作废;
            try
            {
                PdProductStockInBo.Instance.UpdateStatus(id, Status, CurrentUser.Base);
                r.Status = true;
                r.Message = "操作成功！";
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// 推送包裹
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-20 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult InboundCreate()
        {
            int ProductStockInSysNo = int.Parse(this.Request["ProductStockInSysNo"]);
            string OverseaCarrier = this.Request["OverseaCarrier"];
            string OverseaTrackingNo = this.Request["OverseaTrackingNo"];
            string WarehouseSysNo = this.Request["WarehouseSysNo"];

            WhWarehouse WDate = WhWarehouseBo.GetEntity(int.Parse(WarehouseSysNo));
            string WarehouseId = WDate.ErpCode;

            Result result = new Result();

            IList<SendSoOrderModel> SendSoOrderModelList = PdProductStockInDetailBo.Instance.GetSendSoOrderModelByStockInSysNo(ProductStockInSysNo);

            string strJson = "{\"OverseaCarrier\":\"" + OverseaCarrier + "\",\"OverseaTrackingNo\":\"" + OverseaTrackingNo + "\",\"WarehouseId\":\"" + WarehouseId + "\",\"CustomerReference\":\"\",\"MerchantName\":\"\",\"MerchantOrderNo\":\"\",\"ConsigneeFirstName\":\"\",\"ConsigneeLastName\":\"\",";
            strJson += "\"CommodityList\":";
            strJson += "[";
            string str = "";
            foreach (SendSoOrderModel SendOrder in SendSoOrderModelList)
            {
                if (str == "")
                {
                    str += "{";
                    str += "\"SKU\":\"" + SendOrder.SKU + "\",\"UPC\":\"" + SendOrder.UPC + "\",\"CommodityName\":\"" + SendOrder.CommodityName + "\",\"Category\":\"" + SendOrder.Category + "\",\"Brand\":\"" + SendOrder.Brand + "\",\"Color\":\"" + SendOrder.Color + "\",";
                    str += "\"Size\":\"" + SendOrder.Size + "\",\"Material\":\"" + SendOrder.Material + "\",\"CommoditySourceURL\":\"" + SendOrder.CommoditySourceURL + "\",";
                    str += "\"CommodityImageUrlList\":[\"" + SendOrder.CommodityImageUrlList + "\"],\"UnitPrice\":" + SendOrder.UnitPrice + ",\"DeclaredValue\":" + SendOrder.DeclaredValue + ",\"ValueUnit\":\"" + SendOrder.ValueUnit + "\",\"Weight\":" + SendOrder.Weight + ",";
                    str += "\"WeightUnit\":\"" + SendOrder.WeightUnit + "\",\"Volume\":\"" + SendOrder.Volume + "\",\"VolumeUnit\":\"" + SendOrder.VolumeUnit + "\",\"Quantity\":" + SendOrder.Quantity + ",\"CustomerReference\":\"" + SendOrder.CustomerReference + "\"";
                    str += "}";
                }
                else
                {
                    str += ",{";
                    str += "\"SKU\":\"" + SendOrder.SKU + "\",\"UPC\":\"" + SendOrder.UPC + "\",\"CommodityName\":\"" + SendOrder.CommodityName + "\",\"Category\":\"" + SendOrder.Category + "\",\"Brand\":\"" + SendOrder.Brand + "\",\"Color\":\"" + SendOrder.Color + "\",";
                    str += "\"Size\":\"" + SendOrder.Size + "\",\"Material\":\"" + SendOrder.Material + "\",\"CommoditySourceURL\":\"" + SendOrder.CommoditySourceURL + "\",";
                    str += "\"CommodityImageUrlList\":[\"" + SendOrder.CommodityImageUrlList + "\"],\"UnitPrice\":" + SendOrder.UnitPrice + ",\"DeclaredValue\":" + SendOrder.DeclaredValue + ",\"ValueUnit\":\"" + SendOrder.ValueUnit + "\",\"Weight\":" + SendOrder.Weight + ",";
                    str += "\"WeightUnit\":\"" + SendOrder.WeightUnit + "\",\"Volume\":\"" + SendOrder.Volume + "\",\"VolumeUnit\":\"" + SendOrder.VolumeUnit + "\",\"Quantity\":" + SendOrder.Quantity + ",\"CustomerReference\":\"" + SendOrder.CustomerReference + "\"";
                    str += "}";
                }
            }
            strJson += str;
            strJson += "],\"Remark\":\"\"}";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("data", strJson);
            ///获得接口返回值
            var sAPIResult = "";
            try
            {
                sAPIResult = Extra.Logistics.Client.Post(Extra.Logistics.ApiUrl.InboundCreate, data);
                var jsonObject = JObject.Parse(sAPIResult);
                SendOrderReturn m = new SendOrderReturn();

                m.OverseaCarrier = OverseaCarrier;
                m.OverseaTrackingNo = OverseaTrackingNo;
                m.soOrderSysNo = 0;
                m.Code = jsonObject["code"].ToString();
                m.Msg = jsonObject["msg"].ToString();
                if (jsonObject["code"].ToString() == "0")
                {
                    m.OrderNo = jsonObject["orderno"].ToString(); ;
                    result.Status = true;
                    result.Message = "推送成功！";
                    Hyt.BLL.Warehouse.PdProductStockInBo.Instance.UpdateSendStatus(ProductStockInSysNo, 1);
                }
                if (m.Code == "1")
                {
                    m.OrderNo = "";
                    result.Status = false;
                    result.Message = m.Msg;
                }

                Hyt.BLL.Order.SoOrderBo.Instance.InsertSendOrderReturn(m, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增推送包裹
        /// </summary>
        /// <param name="id">推送包裹</param>
        /// <returns>视图</returns>
        /// <remarks>2015-09-2 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1002302)]
        public ActionResult SendProductStockInDialog()
        {
            ViewBag.ProductStockInSysNo = this.Request["ProductStockInSysNo"];
            ViewBag.WarehouseSysNo = this.Request["WarehouseSysNo"];
            return View();
        }

        #region 库存导入导出
        public static bool _starting;

        /// <summary>
        /// 导入库存
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult ImportExcel()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {

                        result = Hyt.BLL.Warehouse.PdProductStockBo.Instance.ImportExcel(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo, ExcelUtil.GetDicColsMapping<ProductStockTemplate>());
                    }
                    catch
                    {
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
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            return View();

        }

        /// <summary>
        /// 根据条形码导入库存
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-06-23 罗勤尧 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult ImportExcelSn()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {

                        result = Hyt.BLL.Warehouse.PdProductStockBo.Instance.ImportExcelSn(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo, ExcelUtil.GetDicColsMapping<ProductStockTemplateSn>());
                    }
                    catch
                    {
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
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            return View();

        }
        /// <summary>
        /// 根据编码导入库存日期
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2017-06-23 罗勤尧 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public ActionResult ImportExcelSnDate()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = new Result();
                if (!_starting)
                {
                    _starting = true;
                    try
                    {

                        result = Hyt.BLL.Warehouse.PdProductStockBo.Instance.ImportExcelSnDate(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo, ExcelUtil.GetDicColsMapping<ProductStockTemplateSnDate>());
                    }
                    catch
                    {
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
                ViewBag.result = HttpUtility.UrlEncode(result.Message);
            }

            return View();

        }
        /// <summary>
        /// 导出库存
        /// </summary>
        /// <remarks>2017-1-12 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.WH1006101)]
        public void ExportProductStocks(ParaProductStockFilter filter)
        {
            filter.PageSize = 9999999;
            filter.Id = 1;
            var pager = PdProductStockBo.Instance.GetPdProductStockList(filter);
            foreach (var item in pager.Rows)
            {
                item.Barcode = PdProductStockBo.Instance.GetBarcode(item.ErpCode);
                item.ProductQuantity=Hyt.BLL.Product.PdProductBo.Instance.GetPdPending(item.PdProductSysNo, item.WarehouseSysNo);
                string[] erpCode = { item.ErpCode};

                Extra.Erp.Model.Inventory inventory = null;
                try
                {
                    inventory = SoOrderBo.Instance.GetErpInventory(erpCode, item.WarehouseSysNo.ToString()).FirstOrDefault();
                }
                catch (Exception)
                {

                    inventory = null;
                }
                
                if (inventory == null)
                {
                    item.KisStock = "无数据";
                }
                else {
                    item.KisStock = inventory.Quantity.ToString();
                }
                
            }
            var dt = pager.Rows.ToDataTable();
            ExcelUtil.Export<ProductStockTemplate>(dt);
        }



     


        #endregion


        #region 库存调拨

        /// <summary>
        /// 调拨单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult AtAllocation()
        {
            return View();
        }

        /// <summary>
        /// 分页查询调拨单列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult QueryAtAllocationList(CBAtAllocation model)
        {
            var pager = new Pager<CBAtAllocation> { CurrentPage = model.id, PageSize = 10 };
            var checkData = true;
            if (CurrentUser.IsBindDealer && !CurrentUser.IsBindAllDealer)
            {
                DsDealerWharehouse dealerWare = DsDealerWharehouseBo.Instance.GetByDsUserSysNo(CurrentUser.Dealer.SysNo);
                pager.PageFilter.OutWarehouseSysNo = dealerWare.WarehouseSysNo;
                pager.PageFilter.EnterWarehouseSysNo = dealerWare.WarehouseSysNo;
                checkData = false;
            }
            ViewBag.checkData = checkData;
            pager = BLL.Warehouse.AtAllocationBo.Instance.QueryAtAllocationPager(pager);

            return PartialView("_AtAllocationPager", pager.Map());
        }

        /// <summary>
        /// 新增/编辑调拨单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        public ActionResult AtAllocationAddOrEdit(int? SysNo, bool isEdit = false)
        {

            ViewBag.OutWarehouse = WhWarehouseBo.Instance.GetAllWarehouseList();
            ViewBag.Warehouse = WhWarehouseBo.Instance.GetAllWarehouseList();
            AtAllocation model = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(TConvert.ToInt32(SysNo));
            if (model == null)
            {
                model = new AtAllocation();
            }
            ViewBag.isEdit = isEdit;
            return View(model);
        }

        /// <summary>
        /// 将选中商品加入调拨单
        /// </summary>
        /// <param name="detailListStr"></param>
        /// <param name="inventorySysNo"></param>
        /// <param name="outWarehouseSysNo"></param>
        /// <param name="inWarehouseSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CM1005808)]
        public JsonResult AddProductToAtAllocation(string detailListStr, int atAllocationSysNo, int outWarehouseSysNo, int inWarehouseSysNo)
        {
            Result result = new Result();

            List<AtAllocationItem> detailList = Util.Serialization.JsonUtil.ToObject<List<AtAllocationItem>>(detailListStr);
            bool createNewOrder = atAllocationSysNo == 0;

            if (createNewOrder == false)
            {
                // 调拨单是否为待审核状态
                var atAllocEntity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(atAllocationSysNo);
                if (atAllocEntity.OutWarehouseSysNo != outWarehouseSysNo || atAllocEntity.EnterWarehouseSysNo != inWarehouseSysNo)
                {
                    createNewOrder = true;
                }
                else
                {
                    if (atAllocEntity.Status != (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.待审核)
                    {
                        result.Message = "操作失败，调拨单不是“待审核”状态，请刷新页面再试";
                        return Json(result);
                    }

                    // 更新
                    List<int> atAllocProducts = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationProducts(atAllocationSysNo).Select(o => o.ProductSysNo).ToList();
                    for (int i = 0; i < detailList.Count; i++)
                    {
                        if (!atAllocProducts.Contains(detailList[i].ProductSysNo))
                        {
                            result = BLL.Warehouse.AtAllocationBo.Instance.AddAtAllocationItem(detailList[i]);
                        }
                    }
                    result.Status = true;
                    result.StatusCode = atAllocEntity.SysNo;
                }
            }

            if (createNewOrder == true)
            {
                // 新增
                // 创建调拨单
                AtAllocation atAllocation = new Model.AtAllocation();
                atAllocation.AllocationCode = BLL.Basic.ReceiptNumberBo.Instance.GetAtAllocationNo();
                atAllocation.EnterWarehouseSysNo = inWarehouseSysNo;
                atAllocation.OutWarehouseSysNo = outWarehouseSysNo;
                atAllocation.CreatedBy = CurrentUser.Base.SysNo;
                atAllocation = BLL.Warehouse.AtAllocationBo.Instance.CreateAtAllocation(atAllocation);

                // 插入调拨单明细
                if (atAllocation.SysNo > 0)
                {
                    for (int i = 0; i < detailList.Count; i++)
                    {
                        detailList[i].AllocationSysNo = atAllocation.SysNo;
                        PdProduct product = BLL.Product.PdProductBo.Instance.GetProduct(detailList[i].ProductSysNo);
                        PdProductStock proSto = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(outWarehouseSysNo, detailList[i].ProductSysNo);
                        if (proSto == null)
                        {
                            result.Status = false;
                            result.Message = "创建调拨单失败,选中商品未查找到库存。";
                            continue;
                        }
                        detailList[i].ErpCode = product.ErpCode;
                        detailList[i].ProductName = product.EasName;
                        detailList[i].Quantity = 0;
                        result = BLL.Warehouse.AtAllocationBo.Instance.AddAtAllocationItem(detailList[i]);
                        if (result.Status == false)
                        {
                            break;
                        }
                        result.Message = "创建调拨单成功";
                    }

                    result.StatusCode = atAllocation.SysNo;
                }
                else
                {
                    result.Status = false;
                    result.Message = "创建调拨单失败";
                }
            }

            return Json(result);
        }

        /// <summary>
        /// 获取库存调拨单商品列表（添加调拨商品用）
        /// </summary>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        public JsonResult GetAtAllocationProducts(int atAllocationSysNo)
        {
            return Json(BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationProducts(atAllocationSysNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除调拨单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        [HttpPost]
        public JsonResult DeleteAtAllocationItem(int sysNo, int atAllocationSysNo)
        {
            Result result = new Result();
            result.Status = false;
            result.Message = "删除失败";

            // 调拨单是否为待审核状态
            var entity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(atAllocationSysNo);
            if (entity.Status != (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.待审核)
            {
                result.Message = "操作失败，调拨单不是“待审核”状态，请刷新页面再试";
                return Json(result);
            }

            if (BLL.Warehouse.AtAllocationBo.Instance.DeleteAtAllocationItem(sysNo) == true)
            {
                result.Status = true;
                result.Message = "删除成功";
            }

            return Json(result);
        }

        /// <summary>
        /// 分页查询调拨单明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        public ActionResult QueryAtAllocationItemList(int? id)
        {
            int atAllocationSysNo = 0;
            if (!string.IsNullOrWhiteSpace(Request["atAllocationSysNo"]))
            {
                atAllocationSysNo = TConvert.ToInt32(Request["atAllocationSysNo"]);
            }
            var pager = new Pager<AtAllocationItem> { CurrentPage = TConvert.ToInt32(id), PageSize = 100 };
            pager.PageFilter.AllocationSysNo = atAllocationSysNo;
            pager = BLL.Warehouse.AtAllocationBo.Instance.QueryAtAllocationItemPager(pager);
            if (atAllocationSysNo > 0)
            {
                ViewBag.OutWarehouseSysNo = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(atAllocationSysNo).OutWarehouseSysNo;

                List<int> SysNos = new List<int>();
                foreach (var mod in pager.Rows)
                {
                    SysNos.Add(mod.ProductSysNo);
                }
                IList<CBWhProductWarehousePositionAssociation> postionList = BLL.Warehouse.WhProductWarehousePositionAssociationBo.Instance.GetPositionAssociationDetail(SysNos, ViewBag.OutWarehouseSysNo);
                ViewBag.postionList = postionList;
            }

            ViewBag.isEdit = Convert.ToBoolean(Request["isEdit"]);
            return PartialView("_AtAllocationAddOrEditPager", pager.Map());
        }

        /// <summary>
        /// 保存调拨单详情
        /// </summary>
        /// <param name="detailListStr"></param>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        [HttpPost]
        public JsonResult SaveAtAllocationItems(string detailListStr, int atAllocationSysNo)
        {
            Result result = new Result();
            result.Status = false;
            result.Message = "保存失败";

            // 调拨单是否为待审核状态
            var entity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(atAllocationSysNo);
            if (entity.Status != (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.待审核)
            {
                result.Message = "操作失败，调拨单不是“待审核”状态，请刷新页面再试";
                return Json(result);
            }

            List<AtAllocationItem> detailList = Util.Serialization.JsonUtil.ToObject<List<AtAllocationItem>>(detailListStr);
            // 判断列表是否为空
            if (detailList == null || detailList.Count == 0)
            {
                return Json(result);
            }
            // 筛选调拨单详情中存在的项
            List<AtAllocationItem> detailListInDB = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationProducts(atAllocationSysNo);
            List<AtAllocationItem> validDetails = new List<AtAllocationItem>();
            for (int i = 0; i < detailList.Count; i++)
            {
                AtAllocationItem tempDetail = detailListInDB.Where(o => o.SysNo == detailList[i].SysNo).FirstOrDefault();
                if (tempDetail != null)
                {
                    validDetails.Add(tempDetail);
                }
            }

            // 保存调拨单详情
            foreach (AtAllocationItem item in validDetails)
            {
                AtAllocationItem tempDetail = detailList.Where(o => o.SysNo == item.SysNo).FirstOrDefault();
                if (tempDetail.Quantity > 0)
                {
                    // 判断调出数量是否大于调出仓库库存数量
                    PdProductStock proStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(entity.OutWarehouseSysNo, item.ProductSysNo);
                    if (proStock == null || proStock.StockQuantity < tempDetail.Quantity)
                    {
                        result.Status = false;
                        result.Message = "操作失败,第" + (validDetails.IndexOf(item) + 1) + "条商品调出数量大于商品库存,目前库存数量为" + proStock.StockQuantity + "件。";
                        return Json(result);
                    }
                    item.Quantity = tempDetail.Quantity;
                    if (BLL.Warehouse.AtAllocationBo.Instance.UpdateAtAllocationItem(item) == true)
                    {
                        result.Status = true;
                        result.Message = "操作成功";
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = "操作失败,调出商品数量需大于0";
                    return Json(result);
                }
            }

            return Json(result);
        }

        /// <summary>
        /// 审核调拨单（自动执行出库入库）
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        /// <remarks>2018-01-16 罗熙 修改</remarks>
        [Privilege(PrivilegeCode.CM1005808)]
        [HttpPost]
        public JsonResult CheckAtAllocation(int sysNo, int? id)
        {
            var result = new Result() { Status = false, Message = "操作失败" };
            //调拨单是否为待审核状态
            var entity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(sysNo);
            if (entity.Status != (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.待审核)
            {
                result.Message = "操作失败，调拨单不是“待审核”状态，请刷新页面再试";
                return Json(result);
            }

            //检查是否存在调拨数量为0的商品
            if (BLL.Warehouse.AtAllocationBo.Instance.ExistAtAllocationProductQtyZero(sysNo))
            {
                result.Message = "操作失败，调拨数量不能有数量为“0”的产品！";
                return Json(result);
            }
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    //生成调拨出库单
                    int invenOutSysNo = BLL.Warehouse.AtAllocationBo.Instance.CreateALCInventoryOutOrder(sysNo);
                    if (invenOutSysNo > 0)
                    {
                        //更新调拨单状态
                        entity.Status = (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.出库中;
                        entity.CheckDate = DateTime.Now;
                        entity.CheckUserSysNo = CurrentUser.Base.SysNo;
                        if (BLL.Warehouse.AtAllocationBo.Instance.UpdateAtAllocation(entity) == false)
                            return Json(result);

                        var inventoryOut = WhInventoryOutBo.Instance.GetWhInventoryOutToSourceSysNo(sysNo);
                        result = InStockBo.Instance.CompleteTransfer(inventoryOut);

                        if (!result.Status)
                            return Json(result);

                        result.Status = true;
                        result.Message = "操作成功";
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "审核调拨单[" + sysNo + "]", LogStatus.系统日志目标类型.调拨单, invenOutSysNo, CurrentUser.Base.SysNo);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return Json(result);
        }
   
        /// <summary>
        /// 调拨出库单管理
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult AtAllocationOutList()
        {
            ViewBag.Warehouse = CurrentUser.Warehouses;
            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };
            var statustList = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<WarehouseStatus.库存调拨出库单状态>(ref statustList);
            ViewBag.Status = new SelectList(statustList, "Value", "Text");

            return View();
        }

        /// <summary>
        /// 分页查询调拨出库单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-07-01 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult QueryAtAllocationOutListPager(int? id, ParaInventoryOutFilter filter)
        {
            var wareHouseList = CurrentUser.Warehouses;
            filter.CurrentPage = id ?? 1;
            filter.SourceType = (int)WarehouseStatus.出库单来源.调货单;
            var pageList = new PagedList<WhInventoryOut>();

            if (filter.SysNo.HasValue) //优先查询条件为出库单编号
            {
                var model = WhInventoryOutBo.Instance.GetWhInventoryOut(filter.SysNo.Value);
                if (null != model)
                {
                    var result = new List<WhInventoryOut> { model };
                    pageList.TData = result;
                    pageList.TotalItemCount = result.Count;
                    pageList.CurrentPageIndex = filter.CurrentPage;
                }
            }
            else
            {
                filter.WarehouseSysNoList = new List<int>();
                if (filter.WarehouseSysNo.HasValue)
                {
                    filter.WarehouseSysNoList.Add(filter.WarehouseSysNo.Value);
                }
                else
                {
                    if (wareHouseList.Count > 0)
                    {
                        wareHouseList.ForEach(x => filter.WarehouseSysNoList.Add(x.SysNo));
                    }
                }
                pageList = WhInventoryOutBo.Instance.GetWhInventoryOutListTo(filter, pageList.PageSize);
            }
            return PartialView("_AtAllocationOutListPager", pageList);
        }

        /// <summary>
        /// 库存调拨出库单出库确认窗口
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>2016-07-01 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult AtAllocationOutConfirm(int sysNo, int? id)
        {
            if (Request.IsAjaxRequest())
            {
                var model = new PagedList<WhInventoryOutItem> { PageSize = int.MaxValue, CurrentPageIndex = id ?? 1 };
                model = WhInventoryOutBo.Instance.GetInventoryOutItemList(sysNo, model.CurrentPageIndex, model.PageSize);
                return PartialView("_AjaxPagerInventoryOutItem", model);
            }
            else
            {
                var data = WhInventoryOutBo.Instance.GetWhInventoryOut(sysNo);
                if (null != data)
                {
                    ViewBag.Status = ((WarehouseStatus.库存调拨出库单状态)data.Status).ToString();
                }
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(data.WarehouseSysNo))
                {
                    var message = string.Format("没有权限读取仓库:{0}的信息", WhWarehouseBo.Instance.GetWarehouseName(data.WarehouseSysNo));
                    return View("ErrorPrivilegeWithMessage", (object)(message));
                }
                return View(data);
            }
        }

        /// <summary>
        /// 调拨出库单确认出库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-07-01 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        [HttpPost]
        public JsonResult DoAAOutConfirm(WhInventoryOut model)
        {
            Result result = new Result();
            result.Status = false;
            result.Message = "操作失败";

            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    // 操作用户是否有相应的仓库权限
                    if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(model.WarehouseSysNo))
                    {
                        throw new HytException("用户没有编号为" + model.WarehouseSysNo + "仓库的权限");
                    }
                    // 出库
                    var tempResult = BLL.Warehouse.AtAllocationBo.Instance.DoAAOutConfirm(model);
                    if (tempResult.Status == false)
                    {
                        return Json(tempResult);
                    }
                    // 生成入库单
                    int whStockInSysNo = BLL.Logistics.LgDeliveryBo.Instance.CreateInStockByInventoryOut(model, CurrentUser.Base.SysNo, "仓库调拨出库生成入库单");
                    if (whStockInSysNo > 0)
                    {
                        var temInvenOut = BLL.Warehouse.WhInventoryOutBo.Instance.GetWhInventoryOut(model.SysNo);
                        // 更新库存调拨单状态
                        AtAllocation AAEntity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(temInvenOut.SourceSysNO);
                        if (AAEntity != null)
                        {
                            AAEntity.Status = (int)Model.WorkflowStatus.WarehouseStatus.库存调拨单状态.入库中;
                            if (BLL.Warehouse.AtAllocationBo.Instance.UpdateAtAllocation(AAEntity) == false)
                            {
                                result.Message = "操作失败，更新调拨单状态失败";
                                return Json(result);
                            }
                            result.Status = true;
                            result.Message = "操作成功";
                            BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "仓库调拨出库生成入库单", LogStatus.系统日志目标类型.入库单, whStockInSysNo, CurrentUser.Base.SysNo);
                        }
                        else
                        {
                            result.Message = "操作失败，调拨单不存在";
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "库存调拨出库单出库" + ex.Message, LogStatus.系统日志目标类型.出库单, model.SysNo, ex);
            }

            return Json(result);
        }



        /// <summary>
        /// 出库单明细
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="id">当前页码</param>
        /// <returns>返回出库单明细</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult InventoryOutDetails(int sysNo, int? id)
        {
            if (Request.IsAjaxRequest())
            {
                var model = new PagedList<WhInventoryOutItem>() { PageSize = int.MaxValue, CurrentPageIndex = id ?? 1 };
                model = WhInventoryOutBo.Instance.GetInventoryOutItemList(sysNo, model.CurrentPageIndex, model.PageSize);
                return PartialView("_AjaxPagerInventoryOutItemDetail", model);
            }
            else
            {
                var data = GetInventoryOutDetails(sysNo);
                if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(data.WarehouseSysNo))
                {
                    var message = string.Format("没有权限读取仓库:{0}的信息",
                                                WhWarehouseBo.Instance.GetWarehouseName(data.WarehouseSysNo));
                    return View("ErrorPrivilegeWithMessage", (object)(message));
                }
                return View(data);
            }
        }

        ///  <summary>
        ///  出库单明细
        ///  </summary>
        ///  <param name="id">出库单系统编号</param>
        /// <returns>返回出库单明细</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>  
        [Privilege(PrivilegeCode.None)]
        private WhInventoryOut GetInventoryOutDetails(int id)
        {
            var data = WhInventoryOutBo.Instance.GetWhInventoryOut(id);
            if (null != data)
            {
                ViewBag.Status = ((WarehouseStatus.采购退货出库单状态)data.Status).ToString();
            }
            return data;
        }

        #endregion


        #region 导出出库单
        /// <summary>
        /// 导出出库单 
        /// </summary>
        /// <param name="idstr">导出的出库单Id</param>
        /// <returns></returns>
        /// 吴琨 2017-7-10
        [Privilege(PrivilegeCode.None)]
        public void WareHouseExcel(string data)
        {
            var condition = Hyt.Util.Serialization.JsonUtil.ToObject<StockOutSearchCondition>(data);

            try
            {
                if (!string.IsNullOrWhiteSpace(condition.OrderSysNoList))
                    condition.OrderSysNoList = string.Join(",", condition.OrderSysNoList.Split(',').Select(x => int.Parse(x)).ToArray());
            }
            catch
            {
                condition.OrderSysNoList = "";
            }


            var dataTable = BLL.Report.ReportBO.Instance.GetStockOutList(condition);
            Util.ExcelUtil.Export(dataTable);
        }



        /// <summary>
        /// 导出当前全部出库单 
        /// </summary>
        /// <param name="idstr">导出的出库单Id</param>
        /// <returns></returns>
        /// 吴琨 2017-7-10
        [Privilege(PrivilegeCode.None)]
        public void WareHouseToExcel()
        {
            var List = StockOutSearchConditionList.RList;
            foreach (var t in StockOutSearchConditionList.RList)
            {
                var soReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(t.ReceiveAddressSysNo);
                if (soReceiveAddress != null)
                {
                    t.ReceiverName = soReceiveAddress.Name;
                }
                else
                {
                    t.ReceiverName = "找不到系统编号为：" + t.ReceiveAddressSysNo + "的收件地址";
                }
                // 获取订单来源 2014-03-07 唐文均
                var orderEnt = SoOrderBo.Instance.GetEntity(t.OrderSysNO);
                if (orderEnt != null)
                {
                    t.OrderSource = EnumUtil.GetDescription(typeof(OrderStatus.销售单来源), orderEnt.OrderSource);
                }
            }
            Hyt.BLL.Warehouse.WhInventoryBo.Instance.ChuKuExportData(List, "", 0);
        }
        #endregion
        #region 判断是否已包裹
        /// <summary>
        /// 判断是否已包裹
        /// </summary>
        /// <param name="sysno">出库单号</param>
        /// <returns></returns>
        /// <remarks>2017-11-28 廖移凤</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult GetEl(int? sysno)
        {
            bool bl = BLL.ExpressList.ExpressListBLL.Instance.GetExpressList(sysno != null ? sysno.Value : 0);
            if (bl)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        #endregion
    }
}
