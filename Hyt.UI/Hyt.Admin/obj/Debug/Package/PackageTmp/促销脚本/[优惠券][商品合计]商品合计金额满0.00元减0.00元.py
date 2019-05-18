"""
[优惠券][商品合计]商品合计金额满0.00元减0.00元.py
eg:商品(不包含指定商品)合计满199.00(优惠券满足金额)元时可以使用

【优惠券】验证优惠券是否成立
成立：true
不成立：false

创建配置：SpPromotionRuleKeyValue
rulekey = rulevalue
product_sysno = 指定商品:商品系统编号1(分号)商品系统编号2(注:多个商品系统编号分号分隔),为空时代表所有商品,排除商品有效。
exclude_product_sysno = 排除商品:商品系统编号1(分号)商品系统编号2(注:多个商品系统编号分号分隔),商品编号为空时,排除商品有效。
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

    #KeyValue
    #参与活动商品
    originalValue_product_sysno = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "product_sysno")
    kv_product_sysno = []
    #if originalValue_product_sysno != None and originalValue_product_sysno.RuleValue != None:
    if type(originalValue_product_sysno) != type(None) and type(originalValue_product_sysno.RuleValue) != type(None):
        kv_product_sysno = map(int,originalValue_product_sysno.RuleValue.split(';'))

    #排除商品
    originalValue_exclude_product_sysno = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "exclude_product_sysno")
    kv_exclude_product_sysno = []
    #if originalValue_exclude_product_sysno != None and originalValue_exclude_product_sysno.RuleValue != None:
    if type(originalValue_exclude_product_sysno) != type(None) and type(originalValue_exclude_product_sysno.RuleValue) != type(None):
        kv_exclude_product_sysno = map(int,originalValue_exclude_product_sysno.RuleValue .split(';'))

    product_sale_total_amount = 0

    if len(kv_product_sysno) > 0:
        #指定商品可以使用优惠券
        for scg in shoppingCart.ShoppingCartGroups:
            for sc in scg.ShoppingCartItems:
                if kv_product_sysno.count(sc.ProductSysNo) > 0:
                    product_sale_total_amount += (sc.SaleTotalAmount - sc.DiscountAmount)
    else:
        #未指定商品，优惠券适合所有的商品
        product_sale_total_amount = shoppingCart.SettlementAmount - (shoppingCart.FreightAmount - shoppingCart.FreightDiscountAmount) + shoppingCart.CouponAmount

    if len(kv_product_sysno) == 0 and len(kv_exclude_product_sysno) > 0:
        #未指定商品，排除商品
        for scg in shoppingCart.ShoppingCartGroups:
            for sc in scg.ShoppingCartItems:
                if kv_exclude_product_sysno.count(sc.ProductSysNo) > 0:
                    product_sale_total_amount -= (sc.SaleTotalAmount - sc.DiscountAmount)

    if product_sale_total_amount >= currSpCoupon.RequirementAmount:
        return True
    else:
        return False