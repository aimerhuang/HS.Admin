"""
【优惠券】验证优惠券是否成立
成立：true
不成立：false

eg:订单满199.00(优惠券满足金额)元时可以使用

"""
import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *

def CheckCoupon(shoppingCart):
    #传入参数变量
    #促销信息
    currSpPromotion = CurrSpPromotion
    #客户信息
    currCrCustomer = CurrCrCustomer
    #优惠券信息
    currSpCoupon = CurrSpCoupon

    shoppingCart.WriteDebug(shoppingCart)

    if (shoppingCart.SettlementAmount - (shoppingCart.FreightAmount - shoppingCart.FreightDiscountAmount) + shoppingCart.CouponAmount) >= currSpCoupon.RequirementAmount:
        return True
    else:
        return False