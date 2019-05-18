using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Front;
using Hyt.DataAccess.Promotion;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Hyt.Util;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 优惠券业务
    /// </summary>
    /// <remarks>2013-08-30 吴文强 创建</remarks>
    public class SpCouponBo : BOBase<SpCouponBo>
    {
        /// <summary>
        /// 根据客户系统编号获取客户所有优惠券信息
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">优惠券状态(Null:所有)</param>
        /// <param name="platformType">优惠券使用平台类型</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2013-08-30 吴文强 创建</remarks>
        public IList<SpCoupon> GetCustomerCoupons(int customerSysNo, PromotionStatus.优惠券状态 status,
                                                  PromotionStatus.促销使用平台[] platformType = null)
        {
            return ISpCouponDao.Instance.GetCustomerCoupons(customerSysNo, status, platformType);
        }

        /// <summary>
        /// 获取当前购物车有效可使用的优惠券
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="platformType">优惠券使用平台类型</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2013-08-30 吴文强 创建</remarks>
        public IList<SpCoupon> GetCurrentCartValidCoupons(int customerSysNo, CrShoppingCart shoppingCart, PromotionStatus.促销使用平台[] platformType)
        {
            //获取未使用的优惠券
            var coupons = GetCustomerCoupons(customerSysNo, PromotionStatus.优惠券状态.已审核, platformType);

            return SpCouponEngineBo.Instance.CheckCoupon(customerSysNo, coupons, shoppingCart);
        }

        /// <summary>
        /// 根据优惠券代码获取优惠券
        /// </summary>
        /// <param name="couponCode">优惠券代码</param>
        /// <returns>优惠券信息</returns>
        /// <remarks>2014-03-28 唐永勤 创建</remarks>
        public SpCoupon GetSpCouponByCouponCode(string couponCode)
        {
            return ISpCouponDao.Instance.GetSpCouponByCouponCode(couponCode);
        }

        /// <summary>
        /// 获取优惠卷信息分页方法
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="type">优惠券状态</param>
        /// <returns>优惠券列表</returns>
        /// <remarks>2013-09-16 杨晗 创建</remarks>
        public PagedList<SpCoupon> Seach(int? pageIndex, int customerSysNo, int type)
        {
            DateTime? nowTime = null;
            DateTime? endTime = null;
            PromotionStatus.优惠券状态 status;
            if (type == 1)
            {
                status = PromotionStatus.优惠券状态.已审核;
                nowTime = DateTime.Now;
            }
            else if (type == 2)
            {
                status = PromotionStatus.优惠券状态.已使用;
            }
            else
            {
                status = PromotionStatus.优惠券状态.已审核;
                endTime = DateTime.Now;
            }
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<SpCoupon>();
            int count;
            var list = ISpCouponDao.Instance.Seach((int)pageIndex, model.PageSize, customerSysNo, status,
                                                   nowTime, endTime, type, out count);
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int)pageIndex;
            model.Style = PagedList.StyleEnum.WebSmall;
            return model;
        }

        /// <summary>
        /// 为客户分配优惠卷
        /// </summary>
        /// <param name="couponSysNo">优惠卷编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="syUser">当前用户</param>
        /// <returns>分配成功后的优惠卷新编号</returns>
        /// <remarks>2013-12-05 朱家宏 创建</remarks>
        public int AssignToCustomer(int couponSysNo, int customerSysNo, SyUser syUser = null)
        {
            /*
             * 1.查询条件：所选优惠卷为系统、优惠卷状态为已审核、结束日期大于当前、允许使用数量>0
             * 
             * 2.复制后初始数据：优惠卷类型为私有，允许使用数量为1，已使用数量为0，客户系统编号为指定客户，生成新的优惠卷随机代码，
             *   状态为待审核，审核人、创建人为当前用户，审核日期、创建日期为当前日期
             * 
             * 3.更新源优惠卷：已使用数量+1，更新人为当前用户，更新日期为当前日期
             * 
             * 4.完成复制后，对该优惠卷仅提供审核和作废
             * 
             * 5.操作权限，具备优惠卷操作权限的用户，或优惠卷的创建用户
             */
            var newCouponSysNo = 0;
            var coupon = PromotionBo.Instance.GetEntity(couponSysNo);
            if (syUser == null)
            {
                syUser = AdminAuthenticationBo.Instance.Current.Base;
            }
            #region 数据校验

            if (coupon.Type != (int)PromotionStatus.优惠券类型.系统 ||
                coupon.Status != (int)PromotionStatus.优惠券状态.已审核 ||
                coupon.EndTime < DateTime.Now ||
                coupon.UseQuantity <= 0 ||
                coupon.UsedQuantity >= coupon.UseQuantity)
            {
                return newCouponSysNo;
            }

            #endregion

            #region 复制优惠卷，初始数据

            var newCoupon = new SpCoupon
                {
                    SysNo = 0,
                    Type = (int)PromotionStatus.优惠券类型.私有,
                    UseQuantity = 1,
                    UsedQuantity = 0,
                    CustomerSysNo = customerSysNo,
                    CouponCode = GenerateNewCouponCode(),
                    Status = (int)PromotionStatus.优惠券状态.待审核,

                    CouponAmount = coupon.CouponAmount,
                    Description = coupon.Description,
                    StartTime = coupon.StartTime,
                    EndTime = coupon.EndTime,
                    ParentSysNo = coupon.ParentSysNo,
                    PromotionSysNo = coupon.PromotionSysNo,
                    RequirementAmount = coupon.RequirementAmount,
                    SourceDescription = Constant.COUPONDESCRIPTION_BINDUSERCOUPON,
                    IsCouponCard = coupon.IsCouponCard,
                    WebPlatform = coupon.WebPlatform,
                    ShopPlatform = coupon.ShopPlatform,
                    MallAppPlatform = coupon.MallAppPlatform,
                    LogisticsAppPlatform = coupon.LogisticsAppPlatform
                };
            newCouponSysNo = PromotionBo.Instance.SaveCoupon(newCoupon, syUser);

            #endregion

            #region 更新原优惠卷

            coupon.UsedQuantity++;
            PromotionBo.Instance.SaveCoupon(coupon, syUser);

            #endregion

            return newCouponSysNo;
        }

        /// <summary>
        /// 审核优惠卷
        /// </summary>
        /// <param name="couponSysNo">优惠卷编号</param>
        /// <param name="syUser">当前用户</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-12-05 朱家宏 创建</remarks>
        public bool Audit(int couponSysNo, SyUser syUser = null)
        {
            var coupon = PromotionBo.Instance.GetEntity(couponSysNo);
            if (coupon.UsedQuantity > 0)
            {
                throw new HytException("已使用的优惠卷不能作废");
            }
            if (syUser == null)
            {
                syUser = AdminAuthenticationBo.Instance.Current.Base;
            }
            coupon.Status = (int)PromotionStatus.优惠券状态.已审核;
            coupon.AuditorSysNo = syUser.SysNo;
            coupon.AuditDate = DateTime.Now;
            //保存优惠券
            return PromotionBo.Instance.SaveCoupon(coupon, syUser) > 0;
        }

        /// <summary>
        /// 作废优惠卷
        /// </summary>
        /// <param name="couponSysNo">优惠卷编号</param>
        /// <param name="syUser">当前用户</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-12-05 朱家宏 创建</remarks>
        public bool Cancel(int couponSysNo, SyUser syUser = null)
        {
            var result = false;
            var coupon = PromotionBo.Instance.GetEntity(couponSysNo);
            if (coupon == null)
            {
                throw new HytException("未找到优惠卷");
            }
            if (coupon.UsedQuantity > 0)
            {
                throw new HytException("已使用的优惠卷不能作废");
            }
            if (coupon.Status == (int)PromotionStatus.优惠券状态.已审核 || coupon.Status == (int)PromotionStatus.优惠券状态.待审核)
            {
                coupon.Status = (int)PromotionStatus.优惠券状态.作废;
                if (syUser == null)
                {
                    syUser = AdminAuthenticationBo.Instance.Current.Base;
                }
                result = PromotionBo.Instance.SaveCoupon(coupon, syUser) > 0;
            }
            return result;
        }

        /// <summary>
        /// 优惠券代码是否存在
        /// </summary>
        /// <param name="couponCode">优惠卷代码</param>
        /// <param name="couponSysNo">优惠券系统编号</param>
        /// <returns>t:存在 f:不存在</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        public bool ExsitsCouponCode(string couponCode, int couponSysNo = 0)
        {
            var model = PromotionBo.Instance.GetCoupon(couponCode);
            if (couponSysNo == 0 && model != null)
                return true;
            if (couponSysNo != 0 && model != null && model.SysNo != couponSysNo)
                return true;
            return false;
        }

        /// <summary>
        /// 生成新优惠卷代码
        /// </summary>
        /// <returns>新代码</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        public string GenerateNewCouponCode()
        {
            var newCode = RandomString.GetRndStrOnlyFor(8, false, true, true);
            return ExsitsCouponCode(newCode) ? GenerateNewCouponCode() : newCode;
        }

        /// <summary>
        /// 待绑定到用户的优惠卷
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2013-12-06 朱家宏 创建</remarks>
        public Pager<CBSpCoupon> GetCouponsToBeAssigned(ParaCoupon filter)
        {
            /*
             * 查询条件：所选优惠卷为系统、优惠卷状态为已审核、结束日期大于当前、允许使用数量>0、平台查询权限
             */
            if (filter == null)
                filter = new ParaCoupon();

            filter.Type = (int)PromotionStatus.优惠券类型.系统;
            filter.Status = (int)PromotionStatus.优惠券状态.已审核;
            filter.ExpiredTime = DateTime.Now;
            filter.UseQuantity = 1;
            filter.IsCouponCard = (int)PromotionStatus.是否优惠卡.否;

            if (filter.WebPlatform == null && filter.ShopPlatform == null &&
                filter.MallAppPlatform == null && filter.LogisticsAppPlatform == null)
            {
                filter.Permissions = GetQueryPermissions();
                if (!filter.Permissions.Contains(true))
                {
                    //无权限
                    return new Pager<CBSpCoupon>();
                }
            }

            return ISpCouponDao.Instance.GetCoupon(filter);
        }

        /// <summary>
        /// 获取用户优惠券查询权限
        /// </summary>
        /// <returns>权限数组[] true:有权 false:无权</returns>
        /// <remarks>2014-01-02 朱家宏 创建</remarks>
        public bool[] GetQueryPermissions()
        {
            var permissions = new bool[4];

            if (AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.SP1005302))
            {
                //网站使用
                permissions[0] = true;
            }
            if (AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.SP1005303))
            {
                //门店使用
                permissions[1] = true;
            }
            if (AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.SP1005304))
            {
                //手机商城使用
                permissions[2] = true;
            }
            if (AdminAuthenticationBo.Instance.Current.PrivilegeList.HasPrivilege(PrivilegeCode.SP1005305))
            {
                //物流App使用
                permissions[3] = true;
            }
            return permissions;
        }

        
        /// <summary>
        /// 根据客户系统编号获取客户所有优惠券信息(已经优惠卡号)
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="status">优惠券状态(Null:所有)</param>
        /// <param name="platformType">使用平台</param>
        /// <returns>优惠券信息集合</returns>
        /// <remarks>2014-06-18 朱成果 创建</remarks>
        public  IList<CBSpCoupon> GetCustomerCouponsWithCard(int customerSysNo, PromotionStatus.优惠券状态 status, PromotionStatus.促销使用平台[] platformType)
        {
            return Hyt.DataAccess.Promotion.ISpCouponDao.Instance.GetCustomerCouponsWithCard(customerSysNo, status, platformType);
        }

    }
}