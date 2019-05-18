using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.CRM;
using Hyt.BLL.Log;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 优惠券引擎
    /// </summary>
    /// <remarks>2013-08-31 吴文强 创建</remarks>
    public class SpCouponEngineBo : BOBase<SpCouponEngineBo>
    {
        /// <summary>
        /// 检查适合当前购物车的优惠券
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="coupons">优惠券集合</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="isExpired">是否过期(是否忽略已使用和过期促销)</param>
        /// <returns>当前购物车可使用的优惠券集合</returns>
        /// <remarks>2013-08-31 吴文强 创建</remarks>
        public IList<SpCoupon> CheckCoupon(int customerSysNo, IList<SpCoupon> coupons,
                                                                 CrShoppingCart shoppingCart, bool isExpired = true)
        {
            //为空返回空对象
            if (coupons == null || coupons.Count == 0 || shoppingCart == null)
            {
                return new List<SpCoupon>();
            }

            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            //客户信息
            var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
            //优惠券的促销信息
            var promotions = SpPromotionBo.Instance.GetPromotions(coupons.Select(c => c.PromotionSysNo).ToArray());
            IList<SpCoupon> validCoupon = new List<SpCoupon>();

            foreach (var coupon in coupons)
            {
                var currPromotion = promotions.FirstOrDefault(p => p.SysNo == coupon.PromotionSysNo);

                if (currPromotion == null) continue; //无效优惠券
                //是否过期=true：检查使用次数和过期时间；是否过期=false：忽略当前过期和已使用                
                if (isExpired == true) //忽略已使用和过期促销
                {
                    if (coupon.UsedQuantity >= coupon.UseQuantity)             //优惠券 已使用数量>=使用数量   
                    {
                        if (coupon.CouponCode != shoppingCart.CouponCode) continue;   //(无效优惠券) 优惠券代码 != 购物车优惠券代码
                    }
                    if (coupon.StartTime > DateTime.Now) continue;             //(无效优惠券) 优惠券开始时间>当前时间 
                    if (DateTime.Now > coupon.EndTime) continue;               //(无效优惠券) 当前时间>优惠券结束时间                     
                }
                else //不忽略已使用和过期促销    
                {
                    //不执行任何操作
                }

                //修改下面代码的实现方式并加入新条件解决修改订单时优惠券丢失问题  by ywb 2014-01-03
                //if ((currPromotion == null)
                //    || (isExpired && (coupon.UseQuantity <= coupon.UsedQuantity || coupon.StartTime >= DateTime.Now ||
                //    coupon.EndTime < DateTime.Now)))
                //{
                //    continue;
                //}




                ScriptSource sourceCode = engine.CreateScriptSourceFromString(currPromotion.PromotionRule.RuleScript);
                scope.SetVariable("CurrSpPromotion", currPromotion);
                scope.SetVariable("CurrCrCustomer", customer);
                scope.SetVariable("CurrSpCoupon", coupon);
                sourceCode.Execute(scope);
                try
                {
                    var pyCheckCoupon = scope.GetVariable<Func<CrShoppingCart, bool>>("CheckCoupon");
                    var result = pyCheckCoupon(shoppingCart);
                    if (result)
                    {
                        validCoupon.Add(coupon);
                    }
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.促销, coupon.SysNo, ex);
                }
            }

            return validCoupon;
        }

    }
}
