using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.BLL.Basic;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.Report;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// ReportController
    /// </summary>
    /// <remarks>2013-9-16 黄伟 创建</remarks>
    public class ReportController : BaseController
    {
        //
        // GET: /Report/
        private const int ExcelRowMax = 1048576;

        #region 升舱明细

        /// <summary>
        /// 升舱明细
        /// </summary>
        /// <param name="id">当前页编号</param>
        /// <param name="para">CBReportDsorderDetail para</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1002101)]
        public ActionResult UpgradeDetails(int? id, CBReportDsorderDetail para)
        {
            var totalAmount = decimal.Zero;
            ViewBag.totalAmount = totalAmount;
            var model = new PagedList<ReportDsorderDetail>
            {
                TData = null,
                CurrentPageIndex = 1,
                TotalItemCount = 0
            };
            if (Request.IsAjaxRequest())
            {
                var dic = ReportBO.Instance.QueryUpgradeDetails(para, ref totalAmount, id ?? 1);
                model = new PagedList<ReportDsorderDetail>
                {
                    TData = dic.Any() ? dic.First().Value : null,
                    CurrentPageIndex = id ?? 1,
                    TotalItemCount = dic.Any() ? dic.First().Key : 0
                };
                ViewBag.totalAmount = totalAmount;
                return PartialView("pAjaxPager_UpgradeDetails", model);
            }
            var lstMallType = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Selected = true,
                            Text = @"全部",
                            Value = "-1"
                        }
                };
            var lst = ReportBO.Instance.GetMallType();
            lstMallType.AddRange(lst.Select(p => new SelectListItem
                {
                    Selected = false,
                    Text = p.MallName,
                    Value = p.SysNo + ""
                }));
            ViewBag.lstMallType = lstMallType;
            return View(model);
        }

        /// <summary>
        /// 升舱明细-导出excel
        /// </summary>
        /// <param name=" "></param>
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1002101)]
        public ActionResult UpgradeDetailsToExcel()
        {
            return File(ReportBO.Instance.UpgradeDetailsToExcel(ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo), "application/octet-stream", "升舱明细.xls");
        }

        #endregion

        #region 销售明细

        /// <summary>
        /// 设定仓库参数
        /// </summary>
        /// <param name="whSelected">选中的仓库</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-6 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1003101, PrivilegeCode.RT1005101, PrivilegeCode.RT1006101, PrivilegeCode.RP1004801,
            PrivilegeCode.RT1007101, PrivilegeCode.RT1008101, PrivilegeCode.RT101201, PrivilegeCode.RT101203)]
        public void SetWhSelected(string whSelected)
        {
            TempData["whSelected"] = whSelected;
            //Session["whSelected"] = whSelected;
            Response.Write("OK");
        }

        /// <summary>
        /// 获取仓库参数
        /// </summary>
        /// <param name=" "></param>
        /// <returns>选中的仓库</returns>
        /// <remarks>2013-12-6 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1003101, PrivilegeCode.RT1005101, PrivilegeCode.RT1006101, PrivilegeCode.RP1004801, PrivilegeCode.RT1007101, PrivilegeCode.RT1008101)]
        public List<int> GetWhSelected()
        {
            var whSelected = new List<int>();
            if (TempData["whSelected"] != null && TempData["whSelected"].ToString() != "")
            {
                whSelected = TempData["whSelected"].ToString().Split(',').Select(int.Parse).ToList();
            }
            return whSelected;
        }

        /// <summary>
        /// 销售明细
        /// </summary>
        /// <param name="id">当前分页编号</param>
        /// <param name="cbReportSaleDetail">字符串序列化->SalesRmaParams</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1003101)]
        public ActionResult SaleDetails(int? id, string cbReportSaleDetail)
        {
            var para = cbReportSaleDetail == null
                           ? new SalesRmaParams()
                           : new JavaScriptSerializer().Deserialize<SalesRmaParams>(cbReportSaleDetail);

            //if (para.BeginDate == null && para.EndDate == null)
            //{
            //    para.BeginDate = DateTime.Now.AddDays(-1).Date;
            //    para.EndDate = DateTime.Now.AddDays(-1).Date;   //查询时间默认为前一天 余勇 修改 2014-06-25
            //    ViewBag.BeginDate = ((DateTime)para.BeginDate).ToString("yyyy-MM-dd"); //Month,minute
            //    ViewBag.EndDate = ((DateTime)para.EndDate).ToString("yyyy-MM-dd");
            //}
            if (!Request.IsAjaxRequest() && !para.IsSelfSupport.HasValue) //设置IsSelfSupport默认值为1
            {
                para.IsSelfSupport = 1; 
            }
            //var whSelected = new List<int>();
            //if (Session["whSelected"] != null && Session["whSelected"].ToString() != "")
            //{
            //    whSelected = Session["whSelected"].ToString().Split(',').Select(int.Parse).ToList();
            //}
            var whSelected = GetWhSelected();
            List<CBRptPaymentRecord> PaymentRecords = new List<CBRptPaymentRecord>();
            var dic = ReportBO.Instance.QuerySaleDetails(ref PaymentRecords, para, whSelected, CurrentUser.Base.SysNo, id ?? 1);
            //Session.Remove("whSelected");
            //TempData["res"] = dic;//cannot keep record while click paging

            var model = new PagedList<RP_销售明细>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            #region 汇总

            //数量	优惠	销售金额	实收金额
            var resultSet = dic.Last().Value ?? new List<RP_销售明细>();
            ViewBag.qtyTotal = resultSet.Sum(p => p.数量);
            ViewBag.disCountTotal = resultSet.Sum(p => p.优惠);
            ViewBag.salesAmountTotal = resultSet.Sum(p => p.销售金额);
            ViewBag.paidAmountTotal = resultSet.Sum(p => p.实收金额);
            //增加支付方式统计 罗勤瑶 20171010
            List<CBRptPaymentRecord> res = new List<CBRptPaymentRecord>();

            var g = PaymentRecords.GroupBy(p => p.PaymentName);
            var results = g.Select(x => new CBRptPaymentRecord()
            {
                PaymentName = x.First().PaymentName,
                ALLAmount = x.Sum(s => s.ALLAmount),
                Amount = x.Sum(s => s.Amount)
            });
            foreach (var item in results)
            {
                res.Add(item);
            }
            //支付方式		支付方式实收总金额	
            ViewBag.PaymentRecords = res;
            #endregion

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_SaleDetails", model);
            }

            //配送方式
            ViewBag.lstDelType = DeliveryTypeBo.Instance.GetLgDeliveryTypeList();
            //取件方式
            //ViewBag.lstPickType = LgPickUpTypeBo.Instance.GetLgPickupTypeList();
            //支付方式
            ViewBag.lstPaymentType = PaymentTypeBo.Instance.GetAll();
            //结算状态


            //结算状态
            //var lstStatus = new List<SelectListItem> { new SelectListItem { Text = @"全部", Value = "-1", Selected = true } };
            //lstStatus.AddRange(from key in Enum.GetNames(typeof(LogisticsStatus.结算单状态))
            //                   let val = Enum.Parse(typeof(LogisticsStatus.结算单状态), key).GetHashCode()
            //                   select new SelectListItem
            //                   {
            //                       Text = key,
            //                       Value = val + "",
            //                       Selected = false
            //                   });
            //ViewBag.lstStatus = lstStatus;
            //开票状态
            ViewBag.lstInvStatus = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "-1", Selected = true},
                    new SelectListItem {Text = @"已开发票", Value = "1", Selected = false},
                    new SelectListItem {Text = @"未开发票", Value = "2", Selected = false}
                };
            //订单来源
            var lstOrderSource = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "-1", Selected = true}
                };
            lstOrderSource.AddRange(from key in Enum.GetNames(typeof(OrderStatus.销售单来源))
                                    let val = Enum.Parse(typeof(OrderStatus.销售单来源), key).GetHashCode()
                                    select new SelectListItem
                                    {
                                        Text = key,
                                        Value = val + "",
                                        Selected = false
                                    });
            ViewBag.lstOrderSource = lstOrderSource;

            return View(model);
        }

        /// <summary>
        /// 销售明细-导出excel
        /// </summary>
        /// <param name="cbReportSaleDetail">字符串序列化->SalesRmaParams</param
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1003101)]
        public void ExportSaleDetails(string cbReportSaleDetail)
        {
            var para = cbReportSaleDetail == null
                           ? new SalesRmaParams()
                           : new JavaScriptSerializer().Deserialize<SalesRmaParams>(cbReportSaleDetail);
            var whSelected = GetWhSelected();

            ReportBO.Instance.ExportSaleDetails(para,
                                                ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"],
                                                CurrentUser.Base.SysNo, whSelected);
        }

        #endregion

        #region 退换货明细

        /// <summary>
        /// 退换货明细
        /// </summary>
        /// <param name="id">当前分页编号</param>
        /// <param name="cbReportSaleDetail">字符串序列化->SalesRmaParams</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1005101)]
        public ActionResult RmaDetails(int? id, string cbReportSaleDetail)
        {
            var para = cbReportSaleDetail == null
                           ? new SalesRmaParams()
                           : new JavaScriptSerializer().Deserialize<SalesRmaParams>(cbReportSaleDetail);

            //if (para.BeginDate == null && para.EndDate == null)
            //{
            //    para.BeginDate = DateTime.Now.Date;
            //    para.EndDate = DateTime.Now.Date;
            //    ViewBag.BeginDate = ((DateTime)para.BeginDate).ToString("yyyy-MM-dd"); //Month,minute
            //    ViewBag.EndDate = ((DateTime)para.EndDate).ToString("yyyy-MM-dd");
            //}
            if (!Request.IsAjaxRequest() && !para.IsSelfSupport.HasValue) //设置IsSelfSupport默认值为1
            {
                para.IsSelfSupport = 1;
            }
            var whSelected = GetWhSelected();

            //用户选择的仓库
            var dic = ReportBO.Instance.QueryRmaDetails(para, whSelected, CurrentUser.Base.SysNo, id ?? 1);
            //TempData["res"] = dic;//cannot keep record while click paging

            var model = new PagedList<RP_退换货明细>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            #region 汇总

            //数量	优惠	销售金额	实收金额
            var resultSet = dic.Last().Value ?? new List<RP_退换货明细>();
            ViewBag.qtyTotal = resultSet.Sum(p => p.数量);
            ViewBag.disCountTotal = resultSet.Sum(p => p.优惠);
            ViewBag.salesAmountTotal = resultSet.Sum(p => p.退款金额);
            ViewBag.paidAmountTotal = resultSet.Sum(p => p.实退金额);

            #endregion

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_RmaDetails", model);
            }

            //配送方式
            ViewBag.lstDelType = DeliveryTypeBo.Instance.GetLgDeliveryTypeParent();
            //取件方式
            ViewBag.lstPickType = LgPickUpTypeBo.Instance.GetLgPickupTypeList();
            //支付方式
            ViewBag.lstPaymentType = PaymentTypeBo.Instance.GetAll();

            //结算状态
            var lstStatus = new List<SelectListItem> { new SelectListItem { Text = @"全部", Value = "-1", Selected = true } };
            lstStatus.AddRange(from key in Enum.GetNames(typeof(WarehouseStatus.退换货单状态))
                               let val = Enum.Parse(typeof(WarehouseStatus.退换货单状态), key).GetHashCode()
                               select new SelectListItem
                               {
                                   Text = key,
                                   Value = val + "",
                                   Selected = false
                               });
            ViewBag.lstStatus = lstStatus;
            //开票状态
            ViewBag.lstInvStatus = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "-1", Selected = true},
                    new SelectListItem {Text = @"已开发票", Value = "1", Selected = false},
                    new SelectListItem {Text = @"未开发票", Value = "2", Selected = false}
                };
            //订单来源
            var lstOrderSource = new List<SelectListItem>
                {
                    new SelectListItem {Text = @"全部", Value = "-1", Selected = true}
                };
            lstOrderSource.AddRange(from key in Enum.GetNames(typeof(OrderStatus.销售单来源))
                                    let val = Enum.Parse(typeof(OrderStatus.销售单来源), key).GetHashCode()
                                    select new SelectListItem
                                    {
                                        Text = key,
                                        Value = val + "",
                                        Selected = false
                                    });
            ViewBag.lstOrderSource = lstOrderSource;

            return View(model);
        }

        /// <summary>
        /// 销售明细-导出excel
        /// </summary>
        /// <param name="cbReportSaleDetail">字符串序列化->SalesRmaParams</param>
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1005101)]
        public void ExportRmaDetails(string cbReportSaleDetail)
        {
            var para = cbReportSaleDetail == null
                           ? new SalesRmaParams()
                           : new JavaScriptSerializer().Deserialize<SalesRmaParams>(cbReportSaleDetail);

            var whSelected = GetWhSelected();

            ReportBO.Instance.ExportRmaDetails(para, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"],
                                               CurrentUser.Base.SysNo, whSelected);
        }

        #endregion

        #region 市场部赠送明细

        /// <summary>
        /// 市场部赠送明细
        /// </summary>
        /// <param name="id">当前分页编号</param>
        /// <param name="para">CBReportMarketDepartmentSale参数</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1001101)]
        public ActionResult MarketPresentDetails(int? id, CBReportMarketDepartmentSale para)
        {
            var dic = ReportBO.Instance.QueryMarketPresentDetails(para, id ?? 1);

            var model = new PagedList<ReportMarketDepartmentSale>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_MarketPresentDetails", model);
            }

            var lstMallType = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Selected = true,
                            Text = @"全部",
                            Value = "-1"
                        }
                };
            var lst = ReportBO.Instance.GetMallType();
            lstMallType.AddRange(lst.Select(p => new SelectListItem
            {
                Selected = false,
                Text = p.MallName,
                Value = p.SysNo + ""
            }));
            ViewBag.lstMallType = lstMallType;

            return View(model);
        }

        /// <summary>
        /// 市场部赠送明细-导出excel
        /// </summary>
        /// <param name=" "></param>
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1001101)]
        public void ExportMarketPresentDetails()
        {

            ReportBO.Instance.ExportMarketPresentDetails(
                ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }

        #endregion

        #region 运营综述日报

        /// <summary>
        /// 运营综述
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-24 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT1004101)]
        public ActionResult BusinessSummary()
        {
            return View();
        }

        /// <summary>
        /// 运营综述列表
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>报表列表</returns>
        /// <remarks>2013-10-24 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT1004101)]
        public ActionResult DoBusinessSummaryList(ParaRptBusinessSummaryFilter filter)
        {
            var list = ReportBO.Instance.QueryBusinessSummary(filter);

            return PartialView("_BusinessSummaryList", list);
        }

        /// <summary>
        /// 运营综述列表
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>报表图表</returns>
        /// <remarks>2013-10-24 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.RT1004101)]
        public ActionResult DoBusinessSummaryMap(ParaRptBusinessSummaryFilter filter)
        {
            var res = new CBReportBusinessSummaryMap();
            filter.Sort = 1;    //升序
            var list = ReportBO.Instance.QueryBusinessSummary(filter);

            switch (filter.DataType)
            {
                case 1:
                    res.SerieName = "流量";
                    res.yValues = list.Select(x => (object)x.流量).ToArray();
                    break;
                case 2:
                    res.SerieName = "访客";
                    res.yValues = list.Select(x => (object)x.访客).ToArray();
                    break;
                case 3:
                    res.SerieName = "下单数";
                    res.yValues = list.Select(x => (object)x.下单数).ToArray();
                    break;
                case 4:
                    res.SerieName = "销售额";
                    res.yValues = list.Select(x => (object)x.销售额).ToArray();
                    break;
                case 5:
                    res.SerieName = "退款总额";
                    res.yValues = list.Select(x => (object)x.退款总额).ToArray();
                    break;
                case 6:
                    res.SerieName = "净销售额";
                    res.yValues = list.Select(x => (object)x.净销售额).ToArray();
                    break;
                case 7:
                    res.SerieName = "客单价";
                    res.yValues = list.Select(x => (object)x.客单价).ToArray();
                    break;
                case 8:
                    res.SerieName = "转换率";
                    res.yValues = list.Select(x => (object)x.转换率).ToArray();
                    break;
                default:
                    res.SerieName = "流量";
                    res.yValues = list.Select(x => (object)x.流量).ToArray();
                    break;
            }

            res.xValues = list.Select(x => x.日期.ToString("yyyy-MM-dd")).ToArray();
            return Json(res);
        }

        /// <summary>
        /// 运营综述导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-17 余勇 创建</remarks>
        /// <remarks>2013-12-20 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RT1004101)]
        public void ExportBusinessSummary(ParaRptBusinessSummaryFilter filter)
        {
            var result = new List<CBReportBusinessSummary>();
            var r = ReportBO.Instance.QueryBusinessSummary(filter); ;
            if (r != null && r.Count > 0) result = r.ToList();
            var excel = result.Select(i => new
            {
                i.日期,
                i.流量,
                i.访客,
                i.下单数,
                i.销售额,
                i.退款总额,
                i.净销售额,
                i.客单价,
                转换率 = (i.转换率.ToString() + "%")
            }).ToList();
            var fileName = string.Format("运营综述报表({0}至{1})",
                                         filter.BeginDate.HasValue ? filter.BeginDate.Value.ToString("yy-MM-dd") : "",
                                         (filter.EndDate.HasValue ? filter.EndDate.Value.AddDays(-1) : DateTime.Now).ToString("yy-MM-dd"));
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\BusinessSummary.xls", 2, fileName, "yyyy-MM-dd");
        }

        #endregion

        #region 门店会员消费

        /// <summary>
        /// 门店会员消费报表
        /// </summary>
        /// <param name=" ">导出参数</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2014-01-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT1009101)]
        public ActionResult ShopCustomerConsume()
        {
            return View();
        }

        /// <summary>
        /// 门店会员消费报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2014-01-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT1009101)]
        public ActionResult DoShopCustomerConsumeQuery(ParaRptShopCustomerConsumeFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var pager = ReportBO.Instance.QueryShopCustomerConsume(filter);
            var list = new PagedList<CBReportShopCustomerConsume>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_ShopCustomerConsumePager", list);
        }

        /// <summary>
        /// 门店会员消费报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-01-06 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT1009801)]
        public void ExportShopCustomerConsume(ParaRptShopCustomerConsumeFilter filter)
        {
            filter.Id = 1;
            filter.PageSize = ExcelRowMax;
            var result = new List<CBReportShopCustomerConsume>();
            var r = ReportBO.Instance.QueryShopCustomerConsume(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                i.门店编号,
                i.门店名称,
                i.会员消费笔数,
                i.日期
            }).ToList();
            var fileName = "门店会员消费报表(" + filter.Reptdt + ")";
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\ShopCustomerConsume.xls", 2, fileName);
        }

        #endregion

        #region 统计报表

        /// <summary>
        /// 销售排行统计表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>view</returns>
        /// <remarks>2013-10-22 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT1003102)]
        public ActionResult SalesRanking(ParaRptSalesRankingFilter filter)
        {
            ViewBag.DateRange = MvcHtmlString.Create(MvcCreateHtml.EnumToString<ParaDateRange>(null, null).ToString());

            var result = ReportBO.Instance.GetSalesRanking(filter).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SalesRanking", result);
            }

            return View(result);
        }

        /// <summary>
        /// 销售排行统计表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-23 朱家宏 创建</remarks>
        /// <remarks>2013-12-20 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.RT1003102)]
        public void ExportSalesRanking(ParaRptSalesRankingFilter filter)
        {
            var result = new List<RptSalesRanking>();
            var r = ReportBO.Instance.GetSalesRanking(filter);
            if (r != null && r.Count > 0) result = r.ToList();
            var excel = result.Select(i => new
            {
                序号 = i.RowNumber,
                商品分类 = i.ProductCategoryName,
                商品编号 = i.ProductSysNo,
                商品名称 = i.ProductName,
                销售数量 = i.SalesQuantity,
                销售金额 = i.SalesAmount
            }).ToList();
            var fileName = "销售排行统计报表";
            if (filter.BeginDate.HasValue || filter.EndDate.HasValue)
            {
                fileName += string.Format("({0}至{1})",
                                          filter.BeginDate.HasValue ? filter.BeginDate.Value.ToString("yy-MM-dd") : "",
                                          (filter.EndDate ?? DateTime.Now).ToString("yy-MM-dd"));
            }
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\SalesRanking.xls", 2, fileName);
        }

        #region 电商

        /// <summary>
        /// 电商中心绩效报表
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1001101)]
        public ActionResult EBusinessCenterPerformance()
        {
            //var list = Hyt.BLL.MallSeller.DsEasBo.Instance.GetAllMall();
            //if (list != null && list.Count > 0) ViewBag.Mall = list.OrderBy(i => i.MallTypeSysNo);
            return View();
        }

        /// <summary>
        /// 电商中心绩效报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1001101)]
        public ActionResult DoEBusinessCenterPerformanceQuery(ParaEBusinessCenterPerformanceFilter filter)
        {
            var malls = Hyt.BLL.MallSeller.DsEasBo.Instance.GetAllMall();

            if (filter.Id == 0) filter.Id = 1;
            var pager = ReportBO.Instance.GetListEBusinessCenter(filter);
            var list = new PagedList<RP_绩效_电商中心>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_EBusinessCenterPerformancePager", list);
        }

        /// <summary>
        /// 电商中心绩效报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1001801)]
        public void ExportEBusinessCenterPerformance(ParaEBusinessCenterPerformanceFilter filter)
        {
            filter.Id = 1;
            filter.PageSize = ExcelRowMax;
            var result = new List<RP_绩效_电商中心>();
            var r = ReportBO.Instance.GetListEBusinessCenter(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
                {
                    i.分销商,
                    i.商城类型,
                    i.店铺名称,
                    i.升舱金额_百城达,
                    i.升舱金额_第三方,
                    i.升舱单量_百城达,
                    i.升舱单量_第三方
                }).ToList();
            var fileName = "电商中心绩效报表(" + filter.Month + ")";
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\EBusinessCenterPerformance.xls", 3, fileName);
        }

        #endregion

        #region 客服

        /// <summary>
        /// 客服绩效报表
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1002101)]
        public ActionResult ServicePerformance()
        {
            var list = ReportBO.Instance.GetAllService();
            if (list != null && list.Count > 0) ViewBag.AllService = list.OrderBy(i => i.UserName);
            return View();
        }

        /// <summary>
        /// 客服绩效报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1002101)]
        public ActionResult DoServicePerformanceQuery(ParaServicePerformanceFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var pager = ReportBO.Instance.GetListServicePerformance(filter);
            var list = new PagedList<RP_绩效_客服>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
            return PartialView("_ServicePerformancePager", list);
        }

        /// <summary>
        /// 客服绩效报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1002801)]
        public void ExportServicePerformance(ParaServicePerformanceFilter filter)
        {
            filter.Id = 1;
            filter.PageSize = ExcelRowMax;
            var result = new List<RP_绩效_客服>();
            var r = ReportBO.Instance.GetListServicePerformance(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                i.客服名,
                i.单量,
                i.订单金额,
                i.新增会员
            }).ToList();
            var fileName = "客服绩效报表(" + filter.Reptdt + ")";
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\ServicePerformance.xls", 2, fileName);
        }

        #endregion

        #region 门店
        /// <summary>
        /// 门店新增会员报表
        /// </summary>
        /// <param></param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1003101)]
        public ActionResult ShopNewCustomer()
        {
            var list = Hyt.BLL.Order.ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
            if (list != null && list.Count > 0) ViewBag.Warehouses = list;
            return View();
        }

        /// <summary>
        /// 门店新增会员报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1003101)]
        public ActionResult DoShopNewCustomerQuery(ParaShopNewCustomerFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            if (BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo))
                filter.HasAllShop = true; //拥有所有仓库权限
            else
                filter.UserNo = CurrentUser.Base.SysNo; //拥有部分仓库权限
            var pager = ReportBO.Instance.GetListShopNewCustomer(filter);
            var list = new PagedList<RP_绩效_门店新增会员>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows
                };
            return PartialView("_ShopNewCustomerPager", list);
        }

        /// <summary>
        /// 门店新增会员报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1003801)]
        public void ExportShopNewCustomer(ParaShopNewCustomerFilter filter)
        {
            if (BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo))
                filter.HasAllShop = true; //拥有所有仓库权限
            else
                filter.UserNo = CurrentUser.Base.SysNo; //拥有部分仓库权限
            filter.Id = 1;
            filter.PageSize = ExcelRowMax;
            var result = new List<RP_绩效_门店新增会员>();
            var r = ReportBO.Instance.GetListShopNewCustomer(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                i.门店名称,
                i.新增会员总数,
                i.消费金额满30的会员数,
                i.新增会员销售
            }).ToList();
            var fileName = "门店新增会员报表(" + filter.Reptdt + ")";
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\ShopNewCustomer.xls", 2, fileName);
        }

        #endregion

        #region 门店明细
        /// <summary>
        /// 门店新增会员明细报表
        /// </summary>
        /// <param></param>
        /// <returns>视图</returns>
        /// <remarks>2014-1-8 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1005101)]
        public ActionResult ShopNewCustomerDetail()
        {
            var shopId = Request.QueryString["shopId"];
            var reptdt = Request.QueryString["reptdt"];
            ViewBag.shopId = shopId;
            ViewBag.reptdt = reptdt;

            ViewBag.Shops = BLL.Order.ShopOrderBo.Instance.GetShopsFromUserSession(CurrentUser.Warehouses);
            return View();
        }

        /// <summary>
        /// 门店新增会员明细报表查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>视图</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1005101)]
        public ActionResult DoShopNewCustomerDetailQuery(ParaShopNewCustomerFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            if (BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo))
                filter.HasAllShop = true; //拥有所有仓库权限
            else
                filter.UserNo = CurrentUser.Base.SysNo; //拥有部分仓库权限
            var pager = ReportBO.Instance.GetListShopNewCustomerDetail(filter);
            var list = new PagedList<rp_ShopNewCustomerDetail>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_ShopNewCustomerDetailPager", list);
        }

        /// <summary>
        /// 门店新增会员明细报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2013-12-10 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.RP1005801)]
        public void ExportShopNewCustomerDetail(ParaShopNewCustomerFilter filter)
        {
            if (BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo))
                filter.HasAllShop = true; //拥有所有仓库权限
            else
                filter.UserNo = CurrentUser.Base.SysNo; //拥有部分仓库权限
            filter.Id = 1;
            filter.PageSize = ExcelRowMax;
            var result = new List<rp_ShopNewCustomerDetail>();
            var r = ReportBO.Instance.GetListShopNewCustomerDetail(filter);
            if (r != null && r.Rows.Count > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
                {
                    仓库 = i.Warehousename,
                    内勤姓名 = i.IndoorStaffName,
                    客户姓名 = i.CustomerName,
                    客户手机 = i.MobilePhoneNumber,
                    消费金额 = i.Amount,
                    注册时间 = i.RegisterDate
                }).ToList();
            var fileName = "门店新增会员明细报表(" + filter.Reptdt + ")";
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\ShopNewCustomerDetail.xls", 2, fileName);
        }

        #endregion

        #region 业务员绩效

        /// <summary>
        /// 业务员绩效
        /// </summary>
        /// <param name="id">id of pageno</param>
        /// <param name="para">ParaBusinessManPerformance para</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1007101)]
        public ActionResult BusinessManPerformance(int? id, ParaBusinessManPerformance para)
        {
            var whSelected = GetWhSelected();
            para.IsSelfSupport = para.IsSelfSupport.HasValue ? para.IsSelfSupport : WarehouseStatus.是否自营.是.GetHashCode();
            var dic = ReportBO.Instance.QueryBusinessManPerformance(para, whSelected, id ?? 1);

            var model = new PagedList<RP_绩效_业务员>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_BusinessManPerformance", model);
            }

            return View(model);
        }

        /// <summary>
        /// 业务员绩效-导出excel
        /// </summary>
        /// <param name="para">ParaBusinessManPerformance para</param>
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1007101)]
        public void BusinessManPerformanceToExcel(ParaBusinessManPerformance para)
        {
            var whSelected = GetWhSelected();
            para.IsSelfSupport = para.IsSelfSupport.HasValue ? para.IsSelfSupport : WarehouseStatus.是否自营.是.GetHashCode();
            ReportBO.Instance.ExportBusinessManPerformance(para, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo, whSelected);
        }
        #endregion

        #region 办事处绩效

        /// <summary>
        /// 办事处绩效
        /// </summary>
        /// <param name="id">current page no</param>
        /// <param name="para">para for rp_绩效_办事处</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1008101)]
        public ActionResult OfficePerformance(int? id, rp_绩效_办事处 para)
        {
            var whSelected = GetWhSelected();
            var dic = ReportBO.Instance.QueryOfficePerformance(para, whSelected, id ?? 1);

            var model = new PagedList<rp_绩效_办事处>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_OfficePerformance", model);
            }

            return View(model);
        }

        /// <summary>
        /// 办事处绩效-导出excel
        /// </summary>
        /// <param name="para">para for rp_绩效_办事处</param>
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1008101)]
        public void OfficePerformanceToExcel(rp_绩效_办事处 para)
        {
            var whSelected = GetWhSelected();
            ReportBO.Instance.ExportOfficePerformance(para, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo, whSelected);
        }
        #endregion

        #region 仓库内勤绩效报表
        /// <summary>
        /// 仓库内勤绩效报表
        /// </summary>
        /// <param name="id">页索引</param>
        /// <param name="dateCalculated">统计时间</param>
        /// <returns>ActionResult</returns>
        [Privilege(PrivilegeCode.RT1006101)]
        public ActionResult WarehouseInsideStaff(int? id, string dateCalculated)
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }

            #region 设置过滤条件
            var filter = new ParaWarehouseInsideStaffFilter();
            filter.DateCalculated = string.IsNullOrEmpty(dateCalculated) ? null : dateCalculated;
            List<int> whSelected = GetWhSelected();

            filter.WarehouseSysNos = whSelected.Count == 0 ? null : whSelected;
            #endregion

            var pagerFilter = new Pager<ParaWarehouseInsideStaffFilter>
                {
                    PageFilter = filter,
                    CurrentPage = id ?? 1,
                    PageSize = 10
                };

            var hasAllWarehouse = BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            var pager = ReportBO.Instance.SearchWarehouseInsideStaff(pagerFilter, CurrentUser.Base.SysNo, hasAllWarehouse);

            var pageList = new PagedList<rp_仓库内勤>
                {
                    CurrentPageIndex = pager.CurrentPage,
                    PageSize = pager.PageSize,
                    TotalItemCount = pager.TotalRows,
                    TData = pager.Rows
                };

            return PartialView("_WarehouseInsideStaffList", pageList);
        }

        /// <summary>
        /// 导出仓库内勤报表
        /// </summary>
        /// <param name=" "></param>
        /// <returns>void</returns>
        /// <remarks>2014-01-07 黄伟 添加注释</remarks>
        [Privilege(PrivilegeCode.RT1006101)]
        public void ExportWarehouseInsideStaff()
        {
            string sysNos = Request.Params["warehouseSysNos"];
            string date = Request.Params["date"];

            #region 设置过滤条件
            var filter = new ParaWarehouseInsideStaffFilter();
            filter.DateCalculated = string.IsNullOrEmpty(date) ? null : date;
            List<int> whSelected = null;
            if (!string.IsNullOrEmpty(sysNos))
            {
                whSelected = sysNos.Split(',').Select(int.Parse).ToList();
            }
            filter.WarehouseSysNos = whSelected;
            #endregion

            var pagerFilter = new Pager<ParaWarehouseInsideStaffFilter>
            {
                PageFilter = filter,
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var hasAllWarehouse = BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            var pager = ReportBO.Instance.SearchWarehouseInsideStaff(pagerFilter, CurrentUser.Base.SysNo, hasAllWarehouse);

            var excel = pager.Rows.Select(c => new CBExportWarehouseInsideStaff()
                {
                    仓库 = c.仓库,
                    内勤 = c.内勤,
                    处理单量_百城达 = c.处理单量_百城达,
                    处理单量_第三方 = c.处理单量_第三方,
                    //统计日期 = c.统计日期.ToString("yyyy年MM月")
                }).ToList();

            //导出Excel，并设置表头列名
            filter.DateCalculated = filter.DateCalculated ?? DateTime.Now.Year + "-" + DateTime.Now.Month;

            string dateStart = filter.DateCalculated.Split('-')[0].Substring(2, 2) + "-" + filter.DateCalculated.Split('-')[1] + "-" + "01",
                                lastDayInMonth = DateTime.DaysInMonth(int.Parse(filter.DateCalculated.Split('-')[0]), int.Parse(filter.DateCalculated.Split('-')[1])) + "",
                                dateEnd = filter.DateCalculated.Split('-')[0].Substring(2, 2) + "-" + filter.DateCalculated.Split('-')[1] + "-" + lastDayInMonth;

            var fileName = "仓库内勤绩效报表(" + dateStart + "至" + dateEnd + ")";

            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\WarehouseInsideStaff.xls", 3, fileName);
        }

        #endregion

        #endregion

        #region 网站流量统计报表

        /// <summary>
        /// 网站流量统计报表
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="isMobilePlatform">是否是移动平台</param>
        /// <returns>返回视图</returns>
        /// <remarks>2014-01-07 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.RT101101)]
        public ActionResult MkTrafficStatistics(int? id, bool isMobilePlatform = false)
        {
            //设置有效展示类型
            if (id.HasValue && id.Value < 4)
            {
                ViewBag.INtimeType = id.Value;
            }
            else
            {
                ViewBag.INtimeType = 0;
            }

            return View();
        }

        /// <summary>
        /// 实时监控详情
        /// </summary>
        /// <returns>返回视图</returns>
        /// <remarks>2014-01-13 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.RT101101)]
        public ActionResult MkTrafficStatisticsRealTime()
        {
            return View();
        }

        #endregion

        #region 配货单报表
        /// <summary>
        /// 分销商铺配货单报表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101109)]
        public ActionResult PickingReport()
        {
            var configPath = System.Configuration.ConfigurationManager.AppSettings["reportfilepath"];
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = "~/download/reportfile/";
            }
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath(configPath);
            List<System.IO.FileInfo> lstfile = new List<System.IO.FileInfo>();
            try
            {
                var lst = System.IO.Directory.GetFiles(filepath, "*.xls");
                foreach (string path in lst)
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(path);

                    if (info.Length > 0)
                    {
                        lstfile.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "获取分销商铺配货单报表:" + ex.Message,
                                            LogStatus.系统日志目标类型.ExcelExporting, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return View(lstfile);
        }
        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns></returns>
        /// <remarks>2014-02-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101109)]
        public ActionResult DownloadPickingReport(string filename)
        {
            var configPath = System.Configuration.ConfigurationManager.AppSettings["reportfilepath"];
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = "~/download/reportfile/";
            }
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath(configPath) + "\\" + filename;
            if (!System.IO.File.Exists(filepath))
            {
                return Content("文件不存在");
            }
            return File(System.IO.File.ReadAllBytes(filepath), "application/octet-stream", filename);
        }
        #endregion

        #region 优惠卡统计表

        /// <summary>
        /// 优惠卡统计表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>view</returns>
        /// <remarks>2014-02-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101201)]
        public ActionResult CouponCards(ParaRptCouponCardFilter filter)
        {
            var result = ReportBO.Instance.GetCouponCards(filter).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CouponCards", result);
            }
            return View(result);
        }

        /// <summary>
        /// 优惠卡统计表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>view</returns>
        /// <remarks>2014-04-02 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101201)]
        public ActionResult RptCouponCards(ParaRptCouponCardFilter filter)
        {

            if (TempData["whSelected"] != null && TempData["whSelected"].ToString() != "")
            {
                var whSelected = TempData["whSelected"].ToString();
                filter.Warehouses = whSelected;
            }

            var result = ReportBO.Instance.GetCouponCardsNew(filter);
            ViewBag.month = filter.Month;

            var list = new PagedList<CBRptCouponCard>
            {
                TData = result.Rows,
                CurrentPageIndex = result.CurrentPage,
                TotalItemCount = result.TotalRows
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RptCouponCards", list);
            }
            return View();
        }

        /// <summary>
        /// 优惠卡统计表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-04-04 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101201)]
        public void ExportCouponCards(ParaRptCouponCardFilter filter)
        {
            var result = new List<CBRptCouponCard>();
            filter.PageSize = int.MaxValue;
            filter.Id = 1;
            var r = ReportBO.Instance.GetCouponCardsNew(filter);

            if (r != null && r.TotalRows > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                月份 = filter.Month,
                办事处 = i.AreaName,
                仓库 = i.WarehouseName,
                新绑定优惠卡数量 = i.TotalNumber,
                优惠金额合计 = i.TotalAmount
            }).ToList();

            var fileName = "会员卡月统计报表({0})";
            fileName = string.Format(fileName, filter.Month);

            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\CouponCards.xls", 2, fileName);
        }

        #endregion

        #region 仓库商品销售排行报表
        /// <summary>
        /// 仓库商品销售排行报表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-04-04 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101202)]
        public ActionResult WarehouseProductSales()
        {
            return View();
        }
        /// <summary>
        /// 仓库商品销售排行报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-04-04 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101202)]
        public ActionResult _WarehouseProductSales(ParaWarehouseProductSalesFilter filter)
        {

            var dat = Hyt.BLL.Report.ReportBO.Instance.QueryWarehouseProductSales(filter);
            return PartialView("_WarehouseProductSales", dat);
        }
        /// <summary>
        /// 销售排行统计表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-04-04 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101202)]
        public void ExportWarehouseProductSales(ParaWarehouseProductSalesFilter filter)
        {

            var result = new List<RptSalesRanking>();
            var r = Hyt.BLL.Report.ReportBO.Instance.QueryWarehouseProductSales(filter);
            if (r != null && r.Count > 0) result = r.ToList();
            var excel = result.Select(i => new
            {
                序号 = i.RowNumber,
                商品分类 = i.ProductCategoryName,
                商品编号 = i.ProductSysNo,
                商品名称 = i.ProductName,
                销售数量 = i.SalesQuantity
            }).ToList();
            var fileName = "销售排行统计表";
            if (filter.BeginDate.HasValue || filter.EndDate.HasValue)
            {
                fileName += string.Format("({0}至{1})",
                                          filter.BeginDate.HasValue ? filter.BeginDate.Value.ToString("yy-MM-dd") : "",
                                          (filter.EndDate ?? DateTime.Now).ToString("yy-MM-dd"));
            }
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\WarehouseSalesRanking.xls", 2, fileName);
        }
        #endregion

        #region 销量统计报表

        /// <summary>
        /// 销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>view</returns>
        /// <remarks>2014-04-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101203)]
        public ActionResult Sales(ParaRptSalesFilter filter)
        {
            if (String.IsNullOrEmpty(filter.Month))
            {
                filter.Month = DateTime.Now.ToString("yyyy-MM");
            }
            ViewBag.CompanyTitle = Hyt.BLL.Log.SysLog.Instance.GetPlatCompanyName();
            
            if (Request.IsAjaxRequest())
            {
                if (TempData["whSelected"] != null && TempData["whSelected"].ToString() != "")
                {
                    var whSelected = TempData["whSelected"].ToString();
                    filter.Warehouses = whSelected;
                }

                var result = ReportBO.Instance.GetSales(filter);

                var list = new PagedList<CBRptSales>
                {
                    TData = result.Rows,
                    CurrentPageIndex = result.CurrentPage,
                    TotalItemCount = result.TotalRows
                };

                return PartialView("_Sales", list);
            }
            return View();
        }

        /// <summary>
        /// 销量统计报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-04-04 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101203)]
        public void ExportSales(ParaRptSalesFilter filter)
        {
            var result = new List<CBRptSales>();
            filter.PageSize = int.MaxValue;
            filter.Id = 1;
            var r = ReportBO.Instance.GetSales(filter);

            if (r != null && r.TotalRows > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                月份 = i.Month,
                办事处 = i.AreaName,
                仓库 = i.Warehouse,
                //门店_订单数 = i.CountOfStore,
                //门店_订单金额 = i.SummationOfStore,
                商城百城达_订单数 = i.CountOfHytBcd,
                商城百城达_订单金额 = i.SummationOfHytBcd,
                商城第三方_订单数 = i.CountOfHytDsf,
                商城第三方_订单金额 = i.SummationOfHytDsf,
                升舱_百城达订单数 = i.CountOfScBcd,
                升舱_第三方订单数 = i.CountOfScDsf,
                业务员_订单数 = i.CountOfSalesman,
                业务员_订单金额 = i.SummationOfSalesman,
                总单数 = i.TotalCount,
                总金额 = i.TotalSummation
            }).ToList();

            var fileName = "销量统计报表({0})";
            fileName = string.Format(fileName, filter.Month);

            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\Sales.xls", 3, fileName);
        }

        #endregion

        #region 升舱销量统计报表

        /// <summary>
        /// 升舱销量统计报表
        /// </summary>
        /// <param name="filter">统计参数</param>
        /// <returns>view</returns>
        /// <remarks>2014-04-16 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101204)]
        public ActionResult UpgradeSales(ParaRptUpgradeSalesFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                var result = ReportBO.Instance.GetUpgradeSales(filter);

                var list = new PagedList<CBRptUpgradeSales>
                {
                    TData = result.Rows,
                    CurrentPageIndex = result.CurrentPage,
                    TotalItemCount = result.TotalRows
                };

                return PartialView("_UpgradeSales", list);
            }
            return View();
        }

        /// <summary>
        /// 升舱销量统计报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-04-16 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT101204)]
        public void ExportUpgradeSales(ParaRptUpgradeSalesFilter filter)
        {
            var result = new List<CBRptUpgradeSales>();
            filter.PageSize = int.MaxValue;
            filter.Id = 1;
            var r = ReportBO.Instance.GetUpgradeSales(filter);

            if (r != null && r.TotalRows > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                分销商 = i.DealerName,
                商城类型 = i.MallName,
                店铺名称 = i.ShopName,
                升舱金额_百城当日达 = i.BcdSum,
                升舱金额_第三方 = i.DsfSum,
                升舱单量_百城当日达 = i.BcdCount,
                升舱单量_第三方 = i.DsfCount
            }).ToList();

            var fileName = "升舱销量统计报表({0})";
            fileName = string.Format(fileName, (filter.Month + "月"));

            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\UpgradeSales.xls", 3, fileName);
        }

        #endregion

        #region 运营综述月报

        /// <summary>
        /// 运营综述月报
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT1004102)]
        public ActionResult BusinessSummaryMonthly()
        {
            return View();
        }

        /// <summary>
        /// 运营综述月报列表
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>报表列表</returns>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT1004102)]
        public ActionResult DoBusinessSummaryListMonthly(ParaRptBusinessSummaryFilter filter)
        {
            var list = ReportBO.Instance.QueryBusinessSummaryMonthly(filter);

            return PartialView("_BusinessSummaryListMonthly", list);
        }

        /// <summary>
        /// 运营综述月报列表
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>报表图表</returns>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.RT1004102)]
        public ActionResult DoBusinessSummaryMapMonthly(ParaRptBusinessSummaryFilter filter)
        {
            var res = new CBReportBusinessSummaryMap();
            filter.Sort = 1;    //升序
            var list = ReportBO.Instance.QueryBusinessSummaryMonthly(filter);

            switch (filter.DataType)
            {
                case 1:
                    res.SerieName = "流量";
                    res.yValues = list.Select(x => (object)x.流量).ToArray();
                    break;
                case 2:
                    res.SerieName = "访客";
                    res.yValues = list.Select(x => (object)x.访客).ToArray();
                    break;
                case 3:
                    res.SerieName = "下单数";
                    res.yValues = list.Select(x => (object)x.下单数).ToArray();
                    break;
                case 4:
                    res.SerieName = "销售额";
                    res.yValues = list.Select(x => (object)x.销售额).ToArray();
                    break;
                case 5:
                    res.SerieName = "退款总额";
                    res.yValues = list.Select(x => (object)x.退款总额).ToArray();
                    break;
                case 6:
                    res.SerieName = "净销售额";
                    res.yValues = list.Select(x => (object)x.净销售额).ToArray();
                    break;
                case 7:
                    res.SerieName = "客单价";
                    res.yValues = list.Select(x => (object)x.客单价).ToArray();
                    break;
                case 8:
                    res.SerieName = "转换率";
                    res.yValues = list.Select(x => (object)x.转换率).ToArray();
                    break;
                default:
                    res.SerieName = "流量";
                    res.yValues = list.Select(x => (object)x.流量).ToArray();
                    break;
            }

            res.xValues = list.Select(x => x.日期.ToString("yyyy-MM")).ToArray();
            return Json(res);
        }

        /// <summary>
        /// 运营综述月报导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-04-24 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.RT1004102)]
        public void ExportBusinessSummaryMonthly(ParaRptBusinessSummaryFilter filter)
        {
            var result = new List<CBReportBusinessSummary>();
            var r = ReportBO.Instance.QueryBusinessSummaryMonthly(filter); ;
            if (r != null && r.Count > 0) result = r.ToList();
            var excel = result.Select(i => new
            {
                i.日期,
                i.流量,
                i.访客,
                i.下单数,
                i.销售额,
                i.退款总额,
                i.净销售额,
                i.客单价,
                转换率 = (i.转换率.ToString() + "%")
            }).ToList();
            const string fileName = "运营综述月报表";
            Util.ExcelUtil.ExportFromTemplate(excel, @"\Templates\Excel\BusinessSummary.xls", 2, fileName, "yyyy-MM");
        }

        #endregion

        #region 快递100相关报表
        /// <summary>
        /// 快递100月报表
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 修改</remarks>
        [Privilege(PrivilegeCode.RT101205)]
        public ActionResult MonthExpressList(int? year)
        {
            int nyear = DateTime.Now.Year;
            if (year.HasValue)
            {
                nyear = year.Value;
            }
            var lst = Hyt.BLL.Report.ReportBO.Instance.GetMonthExpressListByYear(nyear);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_MonthExpressList", lst);
            }
            return View("MonthExpressList", lst);
        }

        /// <summary>
        /// 快递月报明细
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 修改</remarks>
        [Privilege(PrivilegeCode.RT101205)]
        public ActionResult LgExpressDetail()
        {
            ParaExpressInfoFilter filter = new ParaExpressInfoFilter();
            filter.StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            if (Request.QueryString["StartTime"] != null)
            {
                DateTime dt;
                DateTime.TryParse(Request.QueryString["StartTime"].ToString(), out dt);
                if (dt != null && dt > DateTime.MinValue)
                {
                    filter.StartTime = dt;
                    filter.EndTime = dt.AddMonths(1).AddDays(-1);
                }
            }
            filter.IsSuccess = null;
            if (Request.QueryString["IsSuccess"] != null)
            {
                bool r;
                bool.TryParse(Request.QueryString["IsSuccess"].ToString(), out r);
                filter.IsSuccess = r;
            }
            return View("LgExpressDetail", filter);
        }

        /// <summary>
        /// 快递月报明细
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-05-20 朱成果 修改</remarks>
        [Privilege(PrivilegeCode.RT101205)]
        public ActionResult _LgExpressDetail(ParaExpressInfoFilter filter)
        {
            filter.PageSize = 40;
            if (filter.Id < 1)
            {
                filter.Id = 1;
            }
            var pager = Hyt.BLL.Report.ReportBO.Instance.GetLgExpressList(filter);
            var list = new PagedList<CBLgExpressDetail>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize
            };
            return View("_LgExpressDetail", list);
        }
        #endregion

        #region 区域销售统计报表
        /// <summary>
        /// 区域销售统计报表
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>页面</returns>
        /// <remarks>2014-08-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101206)]
        public ActionResult RegionalSales(ParaRptRegionalSales filter)
        {
            if (string.IsNullOrEmpty(filter.Month))
            {
                filter.Month = DateTime.Now.ToString("yyyy-MM");
            }
            filter.PageSize = 10;
            if (filter.Id < 1)
            {
                filter.Id = 1;
            }
            CBRptRegionalSales lstResultAll;
            var pager = Hyt.BLL.Report.ReportBO.Instance.GetRegionalSales(filter, out lstResultAll);
            var model = new PagedList<CBRptRegionalSales>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = lstResultAll.RowCount,
                PageSize = pager.PageSize
            };
            ViewBag.Model = lstResultAll ?? new CBRptRegionalSales(); 

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RegionalSales", model);
            }

            return View(model);
        }

        /// <summary>
        /// 区域销售统计报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-08-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101206)]
        public void ExportRegionalSales(ParaRptRegionalSales filter)
        {
            ReportBO.Instance.ExportRegionalSales(filter, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }

        #endregion

        #region 加盟商报表
        /// <summary>
        /// 加盟商报表
        /// </summary>
        /// <returns>页面</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101208)]
        public ActionResult Franchisees()
        {
            return View();
        }

        /// <summary>
        /// 加盟商当日达对账报表
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>页面</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101207)]
        public ActionResult FranchiseesSaleDetail(ParaFranchiseesSaleDetail filter)
        {
            if (string.IsNullOrEmpty(filter.Month))
            {
                filter.Month = DateTime.Now.ToString("yyyy-MM");
            }
            filter.PageSize = 10;
            if (filter.Id < 1)
            {
                filter.Id = 1;
            }
            filter.WhSelected = GetWhSelected();
           
            var pager = Hyt.BLL.Report.ReportBO.Instance.GetFranchiseesSaleDetail(filter);
            var model = new PagedList<RP_非自营销售明细>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_FranchiseesSaleDetail", model);
            }

            return View(model);
        }

        /// <summary>
        /// 加盟商当日达对账报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-08-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101207)]
        public void ExportFranchiseesSaleDetail(ParaFranchiseesSaleDetail filter)
        {
           filter.WhSelected = GetWhSelected();
           ReportBO.Instance.ExportFranchiseesSaleDetail(filter, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }

        /// <summary>
        /// 加盟商退换货对账报表
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>页面</returns>
        /// <remarks>2014-08-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101209)]
        public ActionResult FranchiseesRmaDetail(ParaFranchiseesSaleDetail filter)
        {
            if (string.IsNullOrEmpty(filter.Month))
            {
                filter.Month = DateTime.Now.ToString("yyyy-MM");
            }
            filter.PageSize = 10;
            if (filter.Id < 1)
            {
                filter.Id = 1;
            }
            filter.WhSelected = GetWhSelected();
            var pager = Hyt.BLL.Report.ReportBO.Instance.GetFranchiseesRmaDetail(filter);
            var model = new PagedList<RP_非自营退换货明细>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_FranchiseesRmaDetail", model);
            }

            return View(model);
        }

        /// <summary>
        /// 加盟商退换货对账报表导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-08-11 余勇 创建</remarks>
        [Privilege(PrivilegeCode.RT101209)]
        public void ExportFranchiseesRmaDetail(ParaFranchiseesSaleDetail filter)
        {
            filter.WhSelected = GetWhSelected();
            ReportBO.Instance.ExportFranchiseesRmaDetail(filter, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }
        #endregion

        #region 二次销售订单业务员对账报表
        /// <summary>
        /// 二次销售订单 业务员对账报表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101301)]
        public ActionResult TwoSaleCheck()
        {

            return View();
        }

        /// <summary>
        /// 二次销售详情
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101301)]
        public ActionResult TwoSaleDetail(string CreateTime, int UserID, int SelectWarehouseSysNo)
        {
            ViewBag.CreateTime = CreateTime;
            ViewBag.UserID = UserID;
            ViewBag.SelectWarehouseSysNo = SelectWarehouseSysNo;
            return View();
        }
     

        /// <summary>
        /// 二次销售详情
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101301)]
        public ActionResult _TwoSaleDetail(ParaTwoSaleFilter filter)
        {
            if (filter.Id < 1) filter.Id = 1;
            filter.PageSize = 40;

            var lst = ReportBO.Instance.GetTwoSaleDetailList(filter);
            var model = new PagedList<CBTwoSaleDetail>
            {
                TData = lst.Rows,
                CurrentPageIndex = lst.CurrentPage,
                TotalItemCount = lst.TotalRows,
                PageSize = lst.PageSize
            };
            return PartialView("_TwoSaleDetail", model);

        }

        /// <summary>
        ///二次销售订单业务员对账
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101301)]
        public ActionResult _TwoSaleCheck(ParaTwoSaleFilter filter)
        {
            if (filter.Id < 1) filter.Id = 1;
            filter.PageSize = 40;
            filter.WarehouseSysNos= GetWhSelected();
            var lst = ReportBO.Instance.GetTwoSaleList(filter, CurrentUser.Base.SysNo);
            var model = new PagedList<CBTwoSale>
            {
                TData = lst.Rows,
                CurrentPageIndex = lst.CurrentPage,
                TotalItemCount = lst.TotalRows,
                PageSize = lst.PageSize
            };
            return PartialView("_TwoSaleCheck", model);
        }

        /// <summary>
        ///导出 二次销售订单业务员对账报表
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-09-22 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.RT101301)]
        public ActionResult ExportTwoSale(ParaTwoSaleFilter filter)
        {
            string filename=string.Format("二次销售订单业务员对账报表({0}).xls",filter.CreateTime.HasValue?filter.CreateTime.Value.ToString("yyyy-MM-dd"):string.Empty);
            var ms = ReportBO.Instance.ExporTwoSale(filter, CurrentUser.Base.SysNo);
            return File(ms, "application/octet-stream", filename);
        }
        #endregion

        #region 办事处快递发货量统计

        /// <summary>
        /// 办事处快递发货量统计
        /// </summary>
        /// <param name="id">current page no</param>
        /// <param name="para">para for rp_绩效_办事处</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-12-11 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1008101)]
        public ActionResult ExpressLgDelivery(int? id, CBRptExpressLgDelivery para)
        {
            if (string.IsNullOrEmpty(para.统计日期))
            {
                para.统计日期 = DateTime.Now.ToString("yyyy-MM");
            }

            var whSelected = GetWhSelected();
            var dic = ReportBO.Instance.QueryExpressLgDelivery(para, whSelected, id ?? 1);

            var model = new PagedList<CBRptExpressLgDelivery>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ExpressLgDelivery", model);
            }

            return View(model);
        }

        /// <summary>
        /// 办事处快递发货量统计-导出excel
        /// </summary>
        /// <param name="para">para for rp_绩效_办事处</param>
        /// <returns>void</returns>
        /// <remarks>2013-9-16 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.RT1008101)]
        public void ExpressLgDeliveryToExcel(CBRptExpressLgDelivery para)
        {
            var whSelected = GetWhSelected();
            ReportBO.Instance.ExpressLgDeliveryToExcel(para, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo, whSelected);
        }
        #endregion

        #region 经销商报表
        /// <summary>
        /// 会员涨势统计
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-2-4 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public ActionResult DealerSales()
        {
            return View();
        }

        /// <summary>
        /// 分页获取会员涨势统计
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>会员涨势统计列表</returns>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public ActionResult DoDealerSalesQuery(ParaRptDealerSalesFilter filter)
        {
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            filter.PageSize = 10;
            var pager = ReportBO.Instance.GetDealerSalesList(filter);
            var list = new PagedList<CBRptDealerSales>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DealerSalesPager", list);
        }
        /// <summary>
        /// 会员涨势统计导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public void ExportDealerSales(string SelectedAgentSysNo, string SelectedDealerSysNo, string Subscribe, string dealerSysNos = "", string BeginDate = "", string EndDate = "")
        {
            ParaRptDealerSalesFilter filter = new ParaRptDealerSalesFilter();
            filter.SelectedAgentSysNo = int.Parse(SelectedAgentSysNo);
            filter.SelectedDealerSysNo = int.Parse(SelectedDealerSysNo);
            filter.Subscribe = Subscribe;
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            filter.BeginDate = BeginDate;
            filter.EndDate = EndDate;
            List<int> sysNos = null;
            if (!string.IsNullOrWhiteSpace(dealerSysNos))
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(dealerSysNos);
            }
            ReportBO.Instance.ExportDealerSales(filter,sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }
        /// <summary>
        /// 会员涨势统计导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public ActionResult DoDealerSalesQueryMap(ParaRptDealerSalesFilter filter)
        {
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            filter.PageSize = 10;
            var pager = ReportBO.Instance.GetDealerSalesList(filter);
            var list = pager.Rows;

            var res = new CBReportBusinessSummaryMap();
            res.SerieName = "会员涨势统计图";
            res.yValues = list.Select(p => (object)p.CustomerNums).ToArray();
            res.xValues = list.Select(p => p.DealerName).ToArray();
            return Json(res);
        }
        #endregion

        #region 支付记录报表
        /// <summary>
        /// 支付记录报表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>

        public ActionResult MethodPayment()
        {
            return View();
        }

        /// <summary>
        ///支付记录报表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        [Privilege(PrivilegeCode.RT105410)]
        public ActionResult DoRebatesMethodPayment(MethodPaymentRecordFilter filter)
        {
            //当前用户对应分销商，2015-12-19 王耀发 创建
            //if (CurrentUser.IsBindDealer)
            //{
            //    int DealerSysNo = CurrentUser.Dealer.SysNo;
            //    filter.DealerSysNo = DealerSysNo;
            //    filter.IsBindDealer = CurrentUser.IsBindDealer;
            //}
            ////是否绑定所有经销商
            //filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            //filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            filter.PageSize = 10;
            var pager = ReportBO.Instance.GetMethodPaymentRecordList(filter);
            var list = new PagedList<CBRptPaymentRecord>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_RebatesPaymentPager", list);
        }

        /// <summary>
        /// 支付记录导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public void ExportMethodPaymentRecord(string BeginDate = "", string EndDate = "")
        {
            MethodPaymentRecordFilter filter = new MethodPaymentRecordFilter();


            filter.BeginDate = BeginDate;
            filter.EndDate = EndDate;
            List<int> sysNos = null;

            ReportBO.Instance.ExportMethodPaymentRecord(filter, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }

        #endregion

        #region 会员返利记录报表
        /// <summary>
        /// 会员返利记录统计
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT105410)]
        public ActionResult RebatesRecord()
        {
            return View();
        }

        /// <summary>
        /// 会员返利记录统计
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT105410)]
        public ActionResult DoRebatesRecordQuery(ParaRptRebatesRecordFilter filter)
        {
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            filter.PageSize = 10;
            var pager = ReportBO.Instance.GetRebatesRecordList(filter);
            var list = new PagedList<CBRptRebatesRecord>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_RebatesRecordPager", list);
        }
        /// <summary>
        /// 返利记录导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public void ExportRebatesRecord(string SelectedAgentSysNo, string SelectedDealerSysNo, string Subscribe, string recordSysNos = "", string BeginDate = "", string EndDate = "", string Condition = "")
        {
            ParaRptRebatesRecordFilter filter = new ParaRptRebatesRecordFilter();
            filter.SelectedAgentSysNo = int.Parse(SelectedAgentSysNo);
            filter.SelectedDealerSysNo = int.Parse(SelectedDealerSysNo);
            filter.Condition = Condition;
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            filter.BeginDate = BeginDate;
            filter.EndDate = EndDate;
            List<int> sysNos = null;
            if (!string.IsNullOrWhiteSpace(recordSysNos))
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(recordSysNos);
            }
            ReportBO.Instance.ExportRebatesRecord(filter, sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }

        /// <summary>
        /// 分销商返利记录统计
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT106410)]
        public ActionResult DealerRebatesRecord()
        {
            return View();
        }

        /// <summary>
        /// 分销商返利记录统计
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT106410)]
        public ActionResult DoDealerRebatesRecordQuery(ParaRptRebatesRecordFilter filter)
        {
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            filter.PageSize = 10;
            var pager = ReportBO.Instance.GetDealerRebatesRecordList(filter);
            var list = new PagedList<CBRptRebatesRecord>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DealerRebatesRecordPager", list);
        }

        /// <summary>
        /// 分销商返利记录导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-02-04 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.RT101410)]
        public void ExportDealerRebatesRecord(string SelectedAgentSysNo, string SelectedDealerSysNo, string Subscribe, string recordSysNos = "", string BeginDate = "", string EndDate = "", string Condition = "")
        {
            ParaRptRebatesRecordFilter filter = new ParaRptRebatesRecordFilter();
            filter.SelectedAgentSysNo = int.Parse(SelectedAgentSysNo);
            filter.SelectedDealerSysNo = int.Parse(SelectedDealerSysNo);
            filter.Condition = Condition;
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            filter.BeginDate = BeginDate;
            filter.EndDate = EndDate;
            List<int> sysNos = null;
            if (!string.IsNullOrWhiteSpace(recordSysNos))
            {
                sysNos = new JavaScriptSerializer().Deserialize<List<int>>(recordSysNos);
            }
            ReportBO.Instance.ExportDealerRebatesRecord(filter, sysNos, ControllerContext.HttpContext.Request.ServerVariables["Remote_ADD"], CurrentUser.Base.SysNo);
        }
        #endregion

        /// <summary>
        /// 同步销售单
        /// </summary>
        /// <returns>王耀发 2016-6-4 创建</returns>
        [Privilege(PrivilegeCode.RT1003101)]
        public ActionResult ProCreateSaleDetail()
        {
            Result result = new Result();
            int affectRows = ReportBO.Instance.ProCreateSaleDetail();
            if (affectRows >= 0)
            {
                result.Status = true;
                result.Message = "同步成功";
            }
            else
            {
                result.Status = false;
                result.Message = "同步失败";
            }
            return Json(result);
        }




        /// <summary>
        /// 同步退换货单
        /// </summary>
        /// <returns>吴琨 2017-9-27 创建</returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult SynchronousRma()
        {
            Result result = new Result();
            int affectRows = ReportBO.Instance.SynchronousRma();
            if (affectRows >= 0)
            {
                result.Status = true;
                result.Message = "同步成功";
            }
            else
            {
                result.Status = false;
                result.Message = "同步失败";
            }
            return Json(result);
        }
    }
}
