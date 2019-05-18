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

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 促销管理
    /// </summary>
    /// <remarks>2014-1-20 黄志勇　添加注释</remarks>
    public class PromotionController : BaseController
    {
        #region 促销管理

        /// <summary>
        /// 促销管理列表
        /// </summary>
        /// <returns>分页</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        /// 
        [Privilege(PrivilegeCode.SP1001101)]
        public ActionResult Promotions()
        {
            ViewBag.PromotionTypes = MvcHtmlString.Create(MvcCreateHtml.EnumToString<PromotionStatus.促销应用类型>(null, null).ToString());
            ViewBag.Statuses = MvcHtmlString.Create(MvcCreateHtml.EnumToString<PromotionStatus.促销状态>(o => o.Value == ((int)PromotionStatus.促销状态.待审).ToString(), null).ToString());
            return View();
        }

        /// <summary>
        /// 新建促销
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public ActionResult PromotionCreate(int? id)
        {
            CBSpPromotion model = new CBSpPromotion();
            if (id != null && id > 0)
            {
                model.Promotion = PromotionBo.Instance.GetModel(id.Value);
                model.PromotionGifts = PromotionBo.Instance.GetListByPromotionSysNo(id.Value);
                model.PromotionOverlays = PromotionBo.Instance.GetPromotionOverlayBySysNo(id.Value).Where(x => x.PromotionSysNo != id).Select(x => x.PromotionSysNo).ToArray();
                model.PromotionRuleKeyValues = PromotionBo.Instance.GetRuleKeyValueListSysNo(id.Value);
                var rules = SpPromotionRuleBo.Instance.GetListByPromotionSysNo(id.Value);
                if (rules != null && rules.Count > 0)
                {
                    model.PromotionRule = SpPromotionRuleBo.Instance.GetListByPromotionSysNo(id.Value).First();
                }
                else
                {
                    model.PromotionRule = new SpPromotionRule();
                }
            }
            else
            {
                ViewBag.PromotionCode = PromotionBo.Instance.GenerateNewPromotionCode();
            }
            return View(model);
        }

        /// <summary>
        /// 新建促销
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public ActionResult PromotionView(int id)
        {
            CBSpPromotion model = new CBSpPromotion();
            if (id > 0)
            {
                model.Promotion = PromotionBo.Instance.GetModel(id);
                model.PromotionGifts = PromotionBo.Instance.GetListByPromotionSysNo(id);
                model.PromotionOverlays =
                    PromotionBo.Instance.GetPromotionOverlayBySysNo(id)
                               .Where(x => x.PromotionSysNo != id)
                               .Select(x => x.PromotionSysNo)
                               .ToArray();
                model.PromotionRuleKeyValues = PromotionBo.Instance.GetRuleKeyValueListSysNo(id);
                var rules = SpPromotionRuleBo.Instance.GetListByPromotionSysNo(id);
                if (rules != null && rules.Count > 0)
                {
                    model.PromotionRule = SpPromotionRuleBo.Instance.GetListByPromotionSysNo(id).First();
                }
                else
                {
                    model.PromotionRule = new SpPromotionRule();
                }
            }
            return View("PromotionCreate");
        }

        /// <summary>
        /// 促销码是否存在
        /// </summary>
        /// <param name="promotionCode">促销码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>t:存在 f:不存在</returns>
        /// <remarks>2013-12-09 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public JsonResult ExsitsPromotionCode(string promotionCode, int promotionSysNo = 0)
        {
            bool flg = PromotionBo.Instance.ExsitsPromotionCode(promotionCode, promotionSysNo);
            return Json(flg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分页获取促销
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>促销列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1001101)]
        public ActionResult DoPromotionQuery(ParaPromotion filter)
        {
            var pager = PromotionBo.Instance.DoPromotionQuery(filter);
            var list = new PagedList<SpPromotion>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PromotionListPager", list);
        }

        /// <summary>
        /// 促销选择
        /// </summary>
        /// <returns>促销列表</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005806)]
        public ActionResult PromotionSelector()
        {
            var promotionType = Request.QueryString["_pType"];

            var multiple = Request.QueryString["_multiple"];

            var isOverlay = Request.QueryString["_isOverlay"];

            ViewBag.multiple = multiple;
            ViewBag.isOverlay = isOverlay;

            ViewBag.PromotionTypes = MvcHtmlString.Create(MvcCreateHtml.EnumToString<PromotionStatus.促销应用类型>(o => o.Value == promotionType, null).ToString());
            return View();
        }

        /// <summary>
        /// 促销选择分页
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>促销列表</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005806)]
        public ActionResult DoPromotionSelectorQuery(ParaPromotion filter)
        {
            filter.Status = (int)PromotionStatus.促销状态.已审;
            filter.ExpiredTime = DateTime.Now;
            var pager = PromotionBo.Instance.DoPromotionQuery(filter);
            var list = new PagedList<SpPromotion>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            list.OnComplete = "AjaxOnComplete";
            return PartialView("_PromotionSelectorPager", list);
        }

        /// <summary>
        /// 促销有效验证
        /// </summary>
        /// <param name="sysNo">促销编号</param>
        /// <returns>t:有效 f:无效</returns>
        /// <remarks>2013-12-31 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005806)]
        public JsonResult IsValid(int sysNo)
        {
            var result = new Result();
            result.Status = PromotionBo.Instance.IsValidPromotion(sysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存促销
        /// </summary>
        /// <param name="cbSpPromotion">促销参数实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public ActionResult SavePromotion(CBSpPromotion cbSpPromotion)
        {
            Result result = new Result();
            try
            {

                result = PromotionBo.Instance.Save(cbSpPromotion, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核促销
        /// </summary>
        /// <param name="cbSpPromotion">促销参数实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SP1001601)]
        public ActionResult AuditPromotion(CBSpPromotion cbSpPromotion)
        {
            Result result = new Result();
            try
            {

                result = PromotionBo.Instance.Audit(cbSpPromotion, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 页面按钮促销审核
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-3-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1001601)]
        public ActionResult AuditPromotionInPage(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = PromotionBo.Instance.AuditPromotion(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消促销审核
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-10-17 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1001601)]
        public ActionResult CalcelAuditPromotion(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = PromotionBo.Instance.CalcelAuditPromotion(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废促销
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SP1001601)]
        public ActionResult InvalidPromotion(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = PromotionBo.Instance.Invalid(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 过期促销
        /// </summary>
        /// <param name="sysNo">促销系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SP1001601)]
        public ActionResult ExpiredPromotion(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = PromotionBo.Instance.Expired(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出赠品模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-01-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public void ExportPromotionGift()
        {
            ExportExcel(@"\Templates\Excel\PromotionGift.xls", "促销赠品导入模板");
        }

        /// <summary>
        /// 导入赠品excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2014-01-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public ActionResult ImportPromotionGift()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();
            var httpPostedFileBase = Request.Files[0];
            if (httpPostedFileBase != null)
            {
                var result = PromotionBo.Instance.ImportPromotionGiftExcel(httpPostedFileBase.InputStream, CurrentUser.Base.SysNo);
                result.Message = HttpUtility.UrlEncode(result.Message);
                ViewBag.Result = result;
            }

            //return to excute the page script
            return View();

        }
        #endregion

        #region 优惠券管理
        /// <summary>
        /// 优惠券
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SP1004101)]
        public ActionResult CouponList()
        {

            return View();
        }

        /// <summary>
        /// 分页获取优惠券
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>优惠券列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004101)]
        public ActionResult DoCouponQuery(ParaCoupon filter)
        {
            filter.Id = filter.Id ?? 1;
            var pager = PromotionBo.Instance.DoCouponQuery(filter);
            var list = new PagedList<CBSpCoupon>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_CouponListPager", list);
        }

        /// <summary>
        /// 新增编辑优惠券
        /// </summary>
        /// <param name="id">优惠券编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004201)]
        public ActionResult Coupon(int? id)
        {
            CBSpCoupon mode = null;
            if (id.HasValue)
            {
                mode = PromotionBo.Instance.GetCoupon(id.Value);
            }
            else
            {
                //2013/12/9 朱家宏 添加随机优惠卷代码
                ViewBag.randomCode = SpCouponBo.Instance.GenerateNewCouponCode();
            }

            //2013-12-30 朱家宏 添加使用平台
            if (mode == null)
            {
                mode = new CBSpCoupon();
            }
            return View(mode);
        }

        /// <summary>
        /// 保存优惠券
        /// </summary>
        /// <param name="coupon">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004201)]
        public ActionResult SaveCoupon(SpCoupon coupon)
        {
            coupon.Status = (int)PromotionStatus.优惠券状态.待审核;
            var r = SavingCoupon(coupon);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存优惠券
        /// </summary>
        /// <param name="coupon">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-10-24 黄志勇 创建</remarks>
        private Result SavingCoupon(SpCoupon coupon)
        {
            var r = new Result { Status = false };
            if (IsExsitCouponCode(coupon.SysNo, coupon.CouponCode))
            {
                r.Message = "优惠券已存在";
            }
            else
            {
                if (coupon.Type == (int)PromotionStatus.优惠券类型.系统)
                    coupon.CustomerSysNo = 0;
                try
                {
                    if (PromotionBo.Instance.SaveCoupon(coupon, CurrentUser.Base) > 0)
                        r.Status = true;
                }
                catch (Exception ex)
                {
                    r.Message = ex.Message;
                }
            }
            return r;
        }

        /// <summary>
        /// 作废优惠券
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004601)]
        public ActionResult CancelCoupon(int sysNo)
        {
            var r = new Result { Status = false };
            try
            {
                var coupon = PromotionBo.Instance.GetEntity(sysNo);
                //2014-04-18  朱家宏 根据需求取消对已使用过的系统优惠卷修改的限制
                if (coupon != null && (coupon.Status == (int)PromotionStatus.优惠券状态.待审核 || coupon.Status == (int)PromotionStatus.优惠券状态.已审核))
                {
                    coupon.Status = (int)PromotionStatus.优惠券状态.作废;
                    if (PromotionBo.Instance.SaveCoupon(coupon, CurrentUser.Base) > 0)
                    {
                        r.Status = true;
                    }
                }
                else
                {
                    r.Message = "不能操作已使用过的优惠卷";
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 审核优惠券
        /// </summary>
        /// <param name="coupon">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004601)]
        public ActionResult AuditCoupon(SpCoupon coupon)
        {
            var r = new Result { Status = false };
            try
            {
                var entity = PromotionBo.Instance.GetEntity(coupon.SysNo);
                coupon.CreatedBy = entity.CreatedBy;
                coupon.CreatedDate = entity.CreatedDate;
                coupon.Status = (int)PromotionStatus.优惠券状态.已审核;
                coupon.AuditorSysNo = CurrentUser.Base.SysNo;
                coupon.AuditDate = DateTime.Now;
                //保存优惠券
                r = SavingCoupon(coupon);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 取消审核优惠券
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004601)]
        public ActionResult CalcelAuditCoupon(int sysNo)
        {
            var r = new Result
            {
                Status = false
            };
            try
            {
                //2014-04-18  朱家宏 根据需求取消对已使用过的系统优惠卷修改的限制
                var coupon = PromotionBo.Instance.GetEntity(sysNo);
                if (coupon != null && coupon.Status == (int)PromotionStatus.优惠券状态.已审核)
                {
                    coupon.Status = (int)PromotionStatus.优惠券状态.待审核;
                    if (PromotionBo.Instance.SaveCoupon(coupon, CurrentUser.Base) > 0)
                    {
                        r.Status = true;
                    }
                }
                else
                {
                    r.Message = "不能操作已使用过的优惠卷";
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 优惠券代码是否存在
        /// </summary>
        /// <param name="sysNo">优惠券系统编号</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns>true:存在 flase:不存在</returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004201)]
        public bool IsExsitCouponCode(int sysNo, string couponCode)
        {
            var model = PromotionBo.Instance.GetCoupon(couponCode);
            if (sysNo == 0 && model != null)
                return true;
            if (sysNo != 0 && model != null && model.SysNo != sysNo)
                return true;
            return false;
        }

        /// <summary>
        /// 优惠券代码是否存在
        /// </summary>
        /// <param name="sysNo">优惠券系统编号</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns>true:通过 flase:不通过</returns>
        /// <remarks>2013-08-27 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1004201)]
        public ActionResult IsExsitCoupon(int sysNo, string couponCode)
        {
            var result = !IsExsitCouponCode(sysNo, couponCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 客户优惠卷绑定

        /// <summary>
        /// 客户优惠卷
        /// </summary>
        /// <param name="customerAccount">用户帐号</param>
        /// <returns>view</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005301)]
        public ActionResult UserCoupon(string customerAccount)
        {
            var customer = new CBCrCustomer();
            if (!string.IsNullOrWhiteSpace(customerAccount))
            {
                customer = CrCustomerBo.Instance.GetCrCustomer(customerAccount);
                if (customer == null || customer.Status == (int)CustomerStatus.会员状态.无效)
                    customer = new CBCrCustomer();
            }
            return View(customer);
        }

        /// <summary>
        /// 当前客户优惠券分页
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>优惠券列表</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005301)]
        [OutputCache(Duration = 0)]
        public ActionResult DoUserBoundCouponsQuery(int customerSysNo)
        {
            var customerCoupons = new List<CBSpCoupon>();
            if (customerSysNo != 0)
            {
                var platformTypes = new[]
                    {
                        PromotionStatus.促销使用平台.PC商城, PromotionStatus.促销使用平台.门店, 
                        PromotionStatus.促销使用平台.手机商城, PromotionStatus.促销使用平台.物流App
                    };
                customerCoupons =
                    SpCouponBo.Instance.GetCustomerCouponsWithCard(customerSysNo, 0, platformTypes)
                              .Where(
                                  o =>
                                  o.Status != (int)PromotionStatus.优惠券状态.作废 && o.Type == (int)PromotionStatus.优惠券类型.私有)
                              .OrderByDescending(o => o.SysNo)
                              .ToList();
            }
            return PartialView("_UserBoundCouponsPager", customerCoupons);
        }

        /// <summary>
        /// 给客户绑定优惠券分页
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005101)]
        public ActionResult SelectUserCoupons()
        {
            ViewBag.Permission = SpCouponBo.Instance.GetQueryPermissions();
            return View();
        }

        /// <summary>
        /// 给客户绑定优惠券分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>优惠券列表</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005101)]
        public ActionResult DoUserCouponsQuery(ParaCoupon filter)
        {
            filter.Id = filter.Id ?? 1;

            var pager = SpCouponBo.Instance.GetCouponsToBeAssigned(filter);
            var list = new PagedList<CBSpCoupon>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_UserCouponsPager", list);
        }

        /// <summary>
        /// 使用平台显示文本
        /// </summary>
        /// <param name="platsValue">Dictionary(int, int):(使用平台枚举，使用平台值)</param>
        /// <returns>text</returns>
        /// <remarks>2014-01-02 朱家宏 创建</remarks>
        public static string GetUserPlatText(Dictionary<int, int> platsValue)
        {
            var plats = Util.EnumUtil.ToDictionary(typeof(PromotionStatus.促销使用平台));

            string text = null;
            foreach (var plat in plats)
            {
                var selectedValue = platsValue.First(o => o.Key == plat.Key).Value;
                if (selectedValue == (int)PromotionStatus.商城使用.是)
                {
                    text += plat.Value + "/";
                }
            }

            if (text != null) text = text.TrimEnd('/');
            return text ?? "--";
        }

        /// <summary>
        /// 用户绑定优惠券作废
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005301)]
        public ActionResult CancelUserCoupon(int sysNo)
        {
            var r = new Result { Status = false, Message = "操作失败。" };

            try
            {
                if (HasUserCouponPermission(sysNo))
                {
                    r.Status = SpCouponBo.Instance.Cancel(sysNo);
                }
                else
                {
                    r.Message = "抱歉，您没有此权限。";
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 用户绑定优惠券审核
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005301)]
        public ActionResult AuditUserCoupon(int sysNo)
        {
            var r = new Result { Status = false, Message = "操作失败。" };

            try
            {
                if (HasUserCouponPermission(sysNo))
                {
                    r.Status = SpCouponBo.Instance.Audit(sysNo);
                }
                else
                {
                    r.Message = "抱歉，您没有此权限。";
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 绑定优惠券
        /// </summary>
        /// <param name="sysNo">优惠券编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005101)]
        public ActionResult AssignUserCoupon(int sysNo, int customerSysNo)
        {
            var r = new Result { Status = false, Message = "操作失败。" };
            try
            {

                r.StatusCode = SpCouponBo.Instance.AssignToCustomer(sysNo, customerSysNo);
                r.Status = r.StatusCode > 0;

            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r);
        }

        /// <summary>
        /// 获取优惠卷
        /// </summary>
        /// <param name="id">优惠卷编号</param>
        /// <returns>优惠券扩展</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1005101)]
        public ActionResult GetCoupon(int? id)
        {
            CBSpCoupon mode = null;
            if (id.HasValue)
            {
                mode = PromotionBo.Instance.GetCoupon(id.Value);
            }
            return Json(mode);
        }

        /// <summary>
        /// 是否具有用户优惠卷审核/作废权限
        /// </summary>
        /// <param name="couponSysNo">优惠卷编号</param>
        /// <returns>t:有 f:无</returns>
        /// <remarks>2013-12-9 朱家宏 创建</remarks>
        private bool HasUserCouponPermission(int couponSysNo)
        {
            var coupon = PromotionBo.Instance.GetCoupon(couponSysNo);
            if (coupon != null)
            {
                return AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.SP1005201) ||
                       coupon.CreatedBy == CurrentUser.Base.SysNo;
            }
            return false;
        }

        #endregion

        #region 客户优惠卡绑定

        /// <summary>
        /// 选择优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <returns>优惠卡选择视图</returns>
        /// <remarks>2014-01-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1006401)]
        public ActionResult CouponCardSelector(string couponCardNo)
        {
            CBCouponCard card = null;
            if (!string.IsNullOrWhiteSpace(couponCardNo))
            {
                try
                {
                    card = SpCouponCardBo.Instance.GetAggregatedCouponCard(couponCardNo);
                }
                catch (HytException e)
                {
                    ViewBag.Message = e.Message;
                }
            }
            return View(card);
        }

        /// <summary>
        /// 绑定优惠卡
        /// </summary>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <param name="couponSysNos">优惠卷号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>绑定结果</returns>
        /// <remarks>2014-01-09 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1006401)]
        public JsonResult AssignCouponCard(string couponCardNo, List<int> couponSysNos, int customerSysNo)
        {
            var result = new Result { Message = "绑定失败" };
            try
            {

                var i = 1;
                foreach (var couponSysNo in couponSysNos)
                {
                    result.StatusCode += SpCouponCardBo.Instance.AssignToCustomer(couponCardNo, couponSysNo,
                                                                                  customerSysNo, i);
                    i++;
                }

                result.Status = result.StatusCode > 0;
            }
            catch (HytException e)
            {
                result.Message = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 优惠券卡号管理
        /// <summary>
        /// 优惠券卡号管理
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1006101)]
        public ActionResult CouponCard()
        {
            return View();
        }

        /// <summary>
        /// 分页获取优惠券卡号
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>优惠券卡号列表</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1006101)]
        public ActionResult DoCouponCardQuery(ParaCouponCard filter)
        {
            filter.Id = filter.Id ?? 1;
            var pager = SpCouponCardBo.Instance.DoCouponCardQuery(filter);
            var list = new PagedList<CBSpCouponCard>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_CouponCardPager", list);
        }

        /// <summary>
        /// 更新优惠券卡号状态
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1006201)]
        public ActionResult SetCouponCardStatus(int sysNo, int status)
        {
            var result = new Result { Message = "", Status = false };
            try
            {
                if (SpCouponCardBo.Instance.UpdateCouponCardStatus(sysNo, status) > 0)
                {
                    result.Status = true;
                }
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "优惠券卡号状态",
                                         LogStatus.系统日志目标类型.促销, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "优惠券卡号状态错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.促销, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 导出优惠卡模板
        /// </summary>
        /// <returns>空</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1006301)]
        public void ExportTemplate()
        {
            ExportExcel(@"\Templates\Excel\CouponCard.xls", "优惠劵卡号导入模板");
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
        public static bool _starting;
        /// <summary>
        /// 导入优惠卡excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2014-01-08 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1006301)]
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
                        result = SpCouponCardBo.Instance.ImportExcel(httpPostedFileBase.InputStream,
                            CurrentUser.Base.SysNo);
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

            //return to excute the page script
            return View();

        }

        /// <summary>
        /// 退回优惠卡
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2014-01-21 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1006501)]
        public ActionResult ReutrnCouponCard()
        {
            return View();
        }

        /// <summary>
        /// 退回优惠卡
        /// </summary>
        /// <param name="customerAccount">客户帐号</param>
        /// <param name="couponCardNo">优惠卡号</param>
        /// <returns>json</returns>
        /// <remarks>2014-01-21 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1006501)]
        public ActionResult DoCouponCardReutrn(string customerAccount, string couponCardNo)
        {
            var result = new Result { Message = "退回失败", Status = false };
            if (!string.IsNullOrWhiteSpace(customerAccount) && !string.IsNullOrWhiteSpace(couponCardNo))
            {
                try
                {

                    var r = SpCouponCardBo.Instance.CancelCouponCard(customerAccount, couponCardNo);
                    result.Message = "优惠卡退回成功!";
                    result.Status = true;

                }
                catch (HytException e)
                {
                    result.Message = e.Message;
                }
            }
            return Json(result);
        }

        #endregion

        #endregion

        #region 促销规则
        /// <summary>
        /// 分页获取促销规则
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>促销规则列表</returns>
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1003101)]
        public ActionResult DoPromotionRuleQuery(ParaPromotionRule filter)
        {
            filter.PageSize = 10;
            var pager = PromotionBo.Instance.DoPromotionRuleQuery(filter);
            var list = new PagedList<SpPromotionRule>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PromotionRuleListPager", list);
        }

        /// <summary>
        /// 促销规则列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1003101)]
        public ActionResult PromotionRuleList()
        {
            return View();
        }

        /// <summary>
        /// 促销规则新建/编辑
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-23 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1003201)]
        public ActionResult PromotionRuleDetail(int? id)
        {
            SpPromotionRule model;
            if (id.HasValue)
            {
                model = SpPromotionRuleBo.Instance.GetEntity(id.Value);
            }
            else
            {
                model = new SpPromotionRule();
            }
            return View(model);
        }

        /// <summary>
        /// 促销规则选择
        /// </summary>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005805)]
        public ActionResult PromotionRuleSelector()
        {
            return View();
        }

        /// <summary>
        /// 促销规则选择
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-08-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.CM1005805)]
        public ActionResult DoPromotionRuleSelectorQuery(ParaPromotionRule filter)
        {
            filter.Status = (int)PromotionStatus.促销规则状态.已审;
            var pager = PromotionBo.Instance.DoPromotionRuleQuery(filter);
            var list = new PagedList<SpPromotionRule>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PromotionRuleSelectorPager", list);
        }

        /// <summary>
        /// 判断规则名称是否存在
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="excludesysNo">排除的规则编号</param>
        /// <returns>是否存在</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1003201)]
        public ActionResult NotExistsRule(string ruleName, int excludesysNo)
        {
            bool flg = SpPromotionRuleBo.Instance.ExistsRule(ruleName, excludesysNo);
            return Json(!flg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存促销信息
        /// </summary>
        /// <param name="model">促销规则</param>
        /// <param name="form">表单参数</param>
        /// <returns>字符串</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SP1003201)]
        public ActionResult SavePromotionRule(SpPromotionRule model, FormCollection form)
        {
            Result r = SpPromotionRuleBo.Instance.SavePromotionRule(model, CurrentUser.Base);
            return Content(@"
    <script type='text/javascript' src='/Theme/scripts/jquery.min.js'></script>
    <script type='text/javascrip' src='/Theme/scripts/common.yui.js'></script>
    <script type='text/javascript' src='/Theme/Plugins/Dialog/Dialog.yui.js'></script>
    <script type='text/javascript' src='/Theme/scripts/UI.yui.js'></script>
    <script type='text/javascript' src='/Theme/scripts/Utils.yui.js'></script>
    <script type='text/javascript' src='/Theme/scripts/DAO.yui.js'></script><script> Utils.alert('保存成功！', function () {window.location.href='/Promotion/PromotionRuleDetail/" + r.StatusCode + "'}, 'succeed');</script>");
        }

        /// <summary>
        /// 审核促销规则
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SP1003601)]
        public ActionResult AuditPromotionRule(int id)
        {
            Result r = new Result();
            try
            {
                r = SpPromotionRuleBo.Instance.AuditPromotionRule(id, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废促销规则
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.SP1003601)]
        public ActionResult CancelPromotionRule(int id)
        {
            Result r = new Result();
            try
            {
                r = SpPromotionRuleBo.Instance.CancelPromotionRule(id, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新建团购
        /// <summary>
        /// 新建团购
        /// </summary>
        /// <param name="id">团购编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-08-26 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1002201)]
        public ActionResult GroupShoppingCreate(int? id)
        {
            GsGroupShopping model = new GsGroupShopping();
            if (id != null)
            {
                model = GroupShoppingBo.Instance.Get(id.Value);

                ViewBag.GsGroupShoppingItemList = GroupShoppingBo.Instance.GetGroupShoppingItem(id.Value);
            }
            //----王耀发 2016-1-23 创建-----
            ParaDsDealerFilter filter = new ParaDsDealerFilter();
            //当前用户自己对应分销商
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            ViewBag.DealerList = DsDealerBo.Instance.GetDealersListByCurUser(filter);
            ViewBag.WhWareHouseList = WhWarehouseBo.Instance.GetWhWareHouseList();
            //----------------------------------
            return View(model);
        }

        /// <summary>
        /// 获取团购的覆盖地区
        /// </summary>
        /// <param name="id">团购系统编号</param>
        /// <returns>地区列表</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1002201)]
        public ActionResult GetAreaTreeByGroupShoppingSysNo(int? id)
        {
            var groupShoppingSysNo = id ?? 0;
            var lst = GroupShoppingBo.Instance.GetAreaTreeByGroupShoppingSysNo(groupShoppingSysNo);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存团购信息
        /// </summary>
        /// <param name="model">团购实体</param>
        /// <param name="gsGroupShoppingItemList">团购商品列表</param>
        /// <param name="gsSupportAreaList">团购支持区域列表</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1002201)]
        public ActionResult SaveGroupShopping(GsGroupShopping model, IList<GsGroupShoppingItem> gsGroupShoppingItemList, IList<GsSupportArea> gsSupportAreaList)
        {
            Result result = new Result();
            try
            {

                model.PromotionSysNo = (int)Promotion.团购;
                result = GroupShoppingBo.Instance.Save(model, gsGroupShoppingItemList, gsSupportAreaList, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核团购
        /// </summary>
        /// <param name="model">团购实体</param>
        /// <param name="gsGroupShoppingItemList">团购商品列表</param>
        /// <param name="gsSupportAreaList">团购支持地区列表</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SP1002601)]
        public ActionResult AuditGroupShopping(GsGroupShopping model, IList<GsGroupShoppingItem> gsGroupShoppingItemList, IList<GsSupportArea> gsSupportAreaList)
        {
            Result result = new Result();
            try
            {

                result = GroupShoppingBo.Instance.Audit(model, gsGroupShoppingItemList, gsSupportAreaList, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消团购审核
        /// </summary>
        /// <param name="sysNo">团购编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SP1002601)]
        public ActionResult CalcelAuditGroup(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = GroupShoppingBo.Instance.CalcelAuditGroup(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废团购
        /// </summary>
        /// <param name="sysNo">团购编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-09-02 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.SP1002601)]
        public ActionResult InvalidGroupShopping(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = GroupShoppingBo.Instance.Invalid(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 团购查询

        /// <summary>
        /// 团购管理
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2013-08-22 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1002101)]
        public ActionResult GroupShoppings()
        {
            ViewBag.Statuses = MvcHtmlString.Create(MvcCreateHtml.EnumToString<GroupShoppingStatus.团购状态>(null, null).ToString());
            return View();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分布视图</returns>
        /// <remarks>2013-08-22 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SP1002101)]
        public ActionResult DoGroupShoppingPaging(ParaGroupShoppingFilter filter)
        {
            filter.PageSize = 10;
            var pager = GroupShoppingBo.Instance.GetPagerList(filter);

            var list = new PagedList<GsGroupShopping>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_GroupShoppingsPager", list);
        }

        #endregion

        #region 优惠卡类型
        /// <summary>
        /// 优惠卡类型
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006901)]
        public ActionResult CouponCardType()
        {
            return View();
        }

        /// <summary>
        /// 优惠卡类型分页
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>视图</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006901)]
        public ActionResult _CouponCardType(ParaCouponCardType filter)
        {
            filter.PageSize = 15;
            var pager = SpCouponCardBo.Instance.GetCouponCardTypePageList(filter);
            var list = new PagedList<SpCouponCardType>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_CouponCardType", list);
        }

        /// <summary>
        /// 新增编辑优惠卡类型
        /// </summary>
        /// <param name="id">优惠卡类型编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006902)]
        public ActionResult CouponCardTypeCreate(int? id)
        {
            CBSpCouponCardType model = new CBSpCouponCardType();
            model.Status = Hyt.Model.WorkflowStatus.PromotionStatus.优惠券卡状态.启用.GetHashCode();
            if (id.HasValue)
            {
                model = Hyt.BLL.Promotion.SpCouponCardBo.Instance.GetCouponCardType(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 选择卡优惠券
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006902)]
        public ActionResult SelectCardCoupon()
        {
            return View();

        }

        /// <summary>
        /// 选择卡优惠券
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="description">描述信息</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006902)]
        public ActionResult _SelectCardCoupon(int id, string description)
        {
            ParaCoupon filter = new ParaCoupon();
            filter.Id = id;
            filter.PageSize = 10;
            filter.Description = description;
            var pager = SpCouponCardBo.Instance.GetCouponsToCard(filter);
            var list = new PagedList<CBSpCoupon>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_SelectCardCoupon", list);
        }

        /// <summary>
        /// 保存优惠卡类型
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006902)]
        public ActionResult SaveCouponCardType(CBSpCouponCardType model)
        {
            Result r = new Result() { Status = false };
            try
            {
                model.LastUpdateDate = DateTime.Now;
                model.LastUpdateBy = CurrentUser.Base.SysNo;

                SpCouponCardBo.Instance.SaveCouponCardType(model);
                r.Status = true;

            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除优惠卡类型
        /// </summary>
        /// <param name="id">优惠卡类型</param>
        /// <returns>json</returns>
        /// <remarks>2013-01-18 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.SP1006902)]
        public ActionResult DeleteCouponCardType(int id)
        {
            Result r = new Result() { Status = false };
            try
            {

                SpCouponCardBo.Instance.DeleteCouponCardType(id);
                r.Status = true;

            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 组合套餐
        /// <summary>
        /// 组合套餐
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006602)]
        public ActionResult SpComboList()
        {
            ViewBag.Statuses = MvcHtmlString.Create(MvcCreateHtml.EnumToString<PromotionStatus.组合套餐状态>(null, null).ToString());
            return View();
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <returns>列表</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006602)]
        public ActionResult DoSpComboPaging(ParaSpComboFilter filter)
        {
            filter.PageSize = 10;
            var pager = SpComboBo.Instance.GetPagerList(filter);

            var list = new PagedList<CBSpCombo>
            {
                PageSize = filter.PageSize,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_SpComboPager", list);
        }
        /// <summary>
        /// 新建组合套餐
        /// </summary>
        /// <param name="id">套餐编号</param>
        /// <returns>视图</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006601)]
        public ActionResult SpComboCreate(int? id)
        {
            SpCombo model = new SpCombo();
            if (id != null)
            {
                model = SpComboBo.Instance.GetEntity(id.Value);
                ViewBag.SpComboItemList = SpComboBo.Instance.GetListByComboSysNo(id.Value);
            }
            //----王耀发 2016-1-23 创建-----
            ParaDsDealerFilter filter = new ParaDsDealerFilter();
            //当前用户自己对应分销商
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            ViewBag.DealerList = DsDealerBo.Instance.GetDealersListByCurUser(filter);
            ViewBag.WhWareHouseList = WhWarehouseBo.Instance.GetWhWareHouseList();
            //----------------------------------
            ViewBag.PromotionList = PromotionBo.Instance.GetSpPromotionList((int)PromotionStatus.促销应用类型.组合套餐);
            return View(model);
        }
        /// <summary>
        /// 保存组合套餐信息
        /// </summary>
        /// <param name="model">组合套餐实体</param>
        /// <param name="gsGroupShoppingItemList">组合套餐商品列表</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006601)]
        public ActionResult SaveCombo(SpCombo model, IList<SpComboItem> spComboItemList)
        {
            Result result = new Result();
            try
            {
                result = SpComboBo.Instance.Save(model, spComboItemList, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 审核该套餐
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spComboItemList"></param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006601)]
        public ActionResult AuditCombo(SpCombo model, IList<SpComboItem> spComboItemList)
        {
            Result result = new Result();
            try
            {

                result = SpComboBo.Instance.Audit(model, spComboItemList, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取消审核该套餐
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spComboItemList"></param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006601)]
        public ActionResult CalcelAuditCombo(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = SpComboBo.Instance.CalcelAuditCombo(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 作废该套餐
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spComboItemList"></param>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1006601)]
        public ActionResult InvalidComboBo(int sysNo)
        {
            Result result = new Result();
            try
            {

                result = SpComboBo.Instance.Invalid(sysNo, CurrentUser.Base);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 仓库免运费
        /// <summary>
        /// 免运费信息
        /// </summary>
        /// <param name="filter">免运费信息</param>
        /// <returns>返回免运费信息</returns>
        /// <remarks>2016-04-20 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WFF1001101)]
        public ActionResult WhouseFreightFreeList()
        {
            return View();
        }

        /// <summary>
        /// 分页获取库存
        /// </summary>
        /// <param name="filter">免运费信息</param>
        /// <returns>返回免运费信息</returns>
        /// <remarks>2016-04-20 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WFF1001101)]
        public ActionResult DoWhouseFreightFreeQuery(ParaWhouseFreightFreeFilter filter)
        {
            filter.PageSize = 10;
            var pager = WhouseFreightFreeBo.Instance.GetWhouseFreightFreeList(filter);
            var list = new PagedList<CBWhouseFreightFree>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_WhouseFreightFreeListPager", list);
        }

        /// <summary>
        /// 创建免运费信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-04-13 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.WFF1001201)]
        public JsonResult SaveWhouseFreightFree(WhouseFreightFree model)
        {
            var r = new Result();
            r = WhouseFreightFreeBo.Instance.SaveWhouseFreightFree(model, CurrentUser.Base);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 获取促销选择商品
        /// </summary>
        /// <param name="promotionSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-4-28 陈海裕 创建</remarks>
        [Privilege(PrivilegeCode.SP1001201)]
        public JsonResult GetPromotionProductSysNos(int promotionSysNo)
        {
            if (promotionSysNo == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var promotionGroup = BLL.Promotion.SpPromotionBo.Instance.GetPromotions(new int[] { promotionSysNo });
            if (promotionGroup.Count > 0 && promotionGroup[0].PromotionRuleKeyValues.Count > 0)
            {
                foreach (var keyValue in promotionGroup[0].PromotionRuleKeyValues)
                {
                    if (keyValue.RuleKey == "product_sysno" && keyValue.RuleValue != null)
                    {
                        return Json(keyValue.RuleValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
