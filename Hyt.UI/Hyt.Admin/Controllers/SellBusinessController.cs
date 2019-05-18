using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.BLL.Authentication;
using Hyt.BLL.SellBusiness;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.RMA;
using Hyt.BLL.Web;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;
using Hyt.BLL.Warehouse;
using Hyt.Util;
using System.Text.RegularExpressions;
using Hyt.Model.Common;

namespace Hyt.Admin.Controllers
{
    public class SellBusinessController : BaseController
    {
        #region 分销等级等级
        /// <summary>
        /// 分销等级列表查询
        /// </summary>
        /// <returns>分销等级列表</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1001)]
        public ActionResult SellBusinessGradeList()
        {
            return View();
        }

        /// <summary>
        /// 分页获取分销等级
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>分销等级列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1001)]
        public ActionResult DoCrSellBusinessGradeQuery(ParaSellBusinessGradeFilter filter)
        {
            filter.PageSize = 10;
            var pager = CrSellBusinessGradeBo.Instance.GetCrSellBusinessGradeList(filter);
            var list = new PagedList<CrSellBusinessGrade>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_SellBusinessGradePager", list);
        }

        /// <summary>
        /// 新增分销等级
        /// </summary>
        /// <param name="id">分销等级</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1001)]
        public ActionResult CrSellBusinessGradeCreate(int? id)
        {
            CrSellBusinessGrade model = new CrSellBusinessGrade();
            if (id.HasValue)
            {
                model = Hyt.BLL.SellBusiness.CrSellBusinessGradeBo.Instance.GetEntity(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 删除分销等级
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1001)]
        public JsonResult Delete(int id)
        {
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    result = Hyt.BLL.SellBusiness.CrSellBusinessGradeBo.Instance.Delete(id);
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存分销等级
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1001)]
        public JsonResult SaveCrSellBusinessGrade(CrSellBusinessGrade model)
        {
            var result = new Result();
            try
            {
                result = CrSellBusinessGradeBo.Instance.SaveCrSellBusinessGrade(model, CurrentUser.Base);
                result.Status = true;
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 返利记录
        /// <summary>
        /// 返利记录列表查询
        /// </summary>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1002)]
        public ActionResult CustomerRebatesRecordList()
        {
            return View();
        }

        /// <summary>
        /// 分页获取返利记录
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1002)]
        public ActionResult DoCrCustomerRebatesRecordQuery(ParaCustomerRebatesRecordFilter filter)
        {
            filter.PageSize = 10;
            var pager = CrCustomerRebatesRecordBo.Instance.GetCrCustomerRebatesRecordList(filter);
            var list = new PagedList<CBCrCustomerRebatesRecord>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_CustomerRebatesRecordPager", list);
        }

        /// <summary>
        /// 删除返利记录
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1002)]
        public JsonResult DeleteRebatesRecord(int id)
        {
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    result = Hyt.BLL.SellBusiness.CrCustomerRebatesRecordBo.Instance.Delete(id);
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分销商关系图
        [Privilege(PrivilegeCode.SB1003)]
        public ActionResult CustomerRelationSchemaList()
        {
            return View();
        }
        #endregion

        #region 会员提现
        [Privilege(PrivilegeCode.SB1004)]
        public ActionResult CrPredepositCashList()
        {
            return View();
        }
        /// <summary>
        /// 获得会员提现记录
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2015-09-19 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SB1004)]
        public ActionResult DoCrPredepositCashQuery(ParaCrPredepositCashFilter filter)
        {
            filter.PageSize = 10;
            var pager = CrPredepositCashBo.Instance.GetCrPredepositCashList(filter);
            var list = new PagedList<CrPredepositCash>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_CrPredepositCashPager", list);
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.SB1004)]
        public JsonResult Aduit()
        {
            int SysNo = int.Parse(this.Request["SysNo"]) ;
            int PdcPayState = int.Parse(this.Request["PdcPayState"]);
            var result = new Result();
            try
            {
                if (SysNo > 0)
                {
                    Hyt.BLL.SellBusiness.CrPredepositCashBo.Instance.UpdatePdcPayState(SysNo, PdcPayState);
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
