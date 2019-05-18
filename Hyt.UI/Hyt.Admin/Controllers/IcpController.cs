using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.BLL.Authentication;
using Hyt.BLL.CRM;
using Hyt.BLL.Log;
using Hyt.BLL.Promotion;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Distribution;
using Hyt.BLL.ApiIcq;

namespace Hyt.Admin.Controllers
{
    public class IcpController : BaseController
    {
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult IcpGoodsList()
        {
            ViewBag.IcpTypes = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.CommonEnum.商检>(null, null).ToString());
            return View();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult DoIcpGoodsPaging(ParaIcpGoodsFilter filter)
        {
            filter.PageSize = 10;
            var pager = new Pager<CIcp>();
            if (filter.IcpType == null)
            {
                pager = IcpBo.Instance.GetGoodsPagerList(filter);
            }
            else
            {
                pager = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance((int)filter.IcpType).GetGoodsPagerList(filter);
            }

            var list = new PagedList<CIcp>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_IcpGoodsPager", list);
        }

        [Privilege(PrivilegeCode.ICPOrder10001)]
        public ActionResult IcpOrderList()
        {
            ViewBag.IcpTypes = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.CommonEnum.商检>(null, null).ToString());
            return View();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPOrder10001)]
        public ActionResult DoIcpOrderPaging(ParaIcpGoodsFilter filter)
        {
            filter.PageSize = 10;
            var pager = new Pager<CIcp>();
            if (filter.IcpType == null)
            {
                pager = IcpBo.Instance.GetOrderPagerList(filter);
            }
            else
            {
                pager = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance((int)filter.IcpType).GetOrderPagerList(filter);
            }

            var list = new PagedList<CIcp>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_IcpOrderPager", list);
        }
        /// <summary>
        /// 商品备案管理
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.ICPGoodsItem10001)]
        public ActionResult IcpGoodsItemList()
        {
            ViewBag.IcpTypes = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.CommonEnum.商检>(null, null).ToString());
            return View();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoodsItem10001)]
        public ActionResult DoIcpGoodsItemPaging(ParaIcpGoodsFilter filter)
        {
            filter.PageSize = 10;
            var pager = new Pager<CBIcpGoodsItem>();
            if (filter.IcpType == null)
            {
                pager = IcpBo.Instance.IcpGoodsItemQuery(filter);
            }
            else
            {
                pager = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance((int)filter.IcpType).IcpGoodsItemQuery(filter);
            }

            var list = new PagedList<CBIcpGoodsItem>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_IcpGoodsItemPager", list);
        }

        /// <summary>
        /// 新建商品商检
        /// </summary>
        /// <param name="id">商品商检编号</param>
        /// <returns>视图</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult IcpGoodsCreate(int? id)
        {
            CIcp model = new CIcp();
            if (id != null)
            {
                model = IcpBo.Instance.GetEntity(id.Value);
                ViewBag.IcpGoodsItemList = IcpBo.Instance.GetListByIcpGoodsSysNo(id.Value);
                ViewBag.OpType = "Update";
            }
            else
            {
                ViewBag.OpType = "Add";
            }
            ViewBag.IcpTypes = MvcHtmlString.Create(MvcCreateHtml.EnumToString<Hyt.Model.CommonEnum.商检>(null, null).ToString());
            return View(model);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult DoIcpProductListPaging(ParaIcpGoodsItemFilter filter)
        {
            filter.PageSize = 10;
            var pager = IcpBo.Instance.GetIcpProductList(filter);

            var list = new PagedList<CBIcpGoodsItem>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_IcpProductPager", list);
        }

        /// <summary>
        /// 推送商品商检信息
        /// </summary>
        /// <param name="IcpType">海关类型</param>
        /// <param name="ProductSysNoList">商品商检商品列表</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult PushIcpGoods(string IcpType, string ProductSysNoList)
        {
            Result result = new Result();
            try
            {
                result = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance(int.Parse(IcpType)).PushGoods(ProductSysNoList);
            }
            catch (Exception ex)
            {
                //result.Message = ex.Message;
                result.Message = "该商检类型尚未对接";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取商品备案回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-3-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult GetGoodsRec(string IcpType)
        {
            Result result = new Result();
            int intIcpType = int.Parse(IcpType);
            try
            {
                result = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance(intIcpType).GetGoodsRec();
            }
            catch (Exception ex)
            {
                result.Message = "该商检类型尚未对接";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取订单备案回执
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-3-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ICPOrder10001)]
        public ActionResult GetOrderRec(string IcpType)
        {
            Result result = new Result();
            int intIcpType = int.Parse(IcpType);
            try
            {
                result = BLL.ApiFactory.ApiProviderFactory.GetIcqInstance(intIcpType).GetOrderRec();
            }
            catch (Exception ex)
            {
                result.Message = "该商检类型尚未对接";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <returns></returns>
        // <remarks>2014-06-05 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.ICPGoods10001)]
        public ActionResult GetIcp(int sysNo)
        {
            CIcp mod = IcpBo.Instance.GetEntity(sysNo);
            if (mod.PlatDocRec != null)
            {
                mod.PlatDocRec = mod.PlatDocRec.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>").Replace(" ", "&nbsp;&nbsp;&nbsp;");
            }
            if (mod.CiqDocRec != null)
            {
                mod.CiqDocRec = mod.CiqDocRec.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>").Replace(" ", "&nbsp;&nbsp;&nbsp;");
            }
            return Json(mod);
        }
    }
}
