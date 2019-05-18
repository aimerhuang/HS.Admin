"""
[满减][商品合计]商品金额满0.00元立减0.00元.py
eg:商品金额满99.99元立减19.00元

创建配置：SpPromotionRule
FrontText = %s购满%0.2f元，立减%0.2f元
TrueText = %s已购满%0.2f元，已减%0.2f元现金
FalseText = %s购满%0.2f元，即可享受满减优惠

创建配置：SpPromotionRuleKeyValue
rulekey = rulevalue
achieve_price = 199.00(注:商品合计金额满多少元)
reduce_price = 19.00(注:立减多少元)
"""
import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *
from Hyt.Model.Transfer import *

#检查可使用促销的商品
def CheckPromotionProduct(shoppingCart):

    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion

    #当前促销对促销类型为(50:应用到商品合计)有效
    if (currSpPromotion.PromotionType != 50):
        return shoppingCart
    
    #商品无促销，默认添加当前促销
    if shoppingCart.GroupPromotions == None or len(shoppingCart.GroupPromotions) == 0 :
        shoppingCartGroupPromotion = CrShoppingCartGroupPromotion()
        shoppingCartGroupPromotion.PromotionSysNo = currSpPromotion.SysNo
        shoppingCartGroupPromotion.RuleType = currSpPromotion.PromotionRule.RuleType

        shoppingCart.GroupPromotions = List[CrShoppingCartGroupPromotion]()
        shoppingCart.GroupPromotions.Add(shoppingCartGroupPromotion)
        return shoppingCart

    #商品已有促销
    #是否与当前促销叠加，叠加则加入当前促销
    if currSpPromotion.PromotionOverlays != None and map(int,currSpPromotion.PromotionOverlays).count(shoppingCart.GroupPromotions[0].PromotionSysNo) > 0:
        shoppingCartGroupPromotion = CrShoppingCartGroupPromotion()
        shoppingCartGroupPromotion.PromotionSysNo = currSpPromotion.SysNo
        shoppingCartGroupPromotion.RuleType = currSpPromotion.PromotionRule.RuleType

        shoppingCart.GroupPromotions.Add(shoppingCartGroupPromotion)

    #不叠加,判断已有优先级<当前促销，则使用当前促销
    else:
        pmt = currAllPromotion.FirstOrDefault(lambda p : p.SysNo == shoppingCart.GroupPromotions[0].PromotionSysNo)
        if pmt == None or pmt.Priority < currSpPromotion.Priority:
            shoppingCartGroupPromotion = CrShoppingCartGroupPromotion()
            shoppingCartGroupPromotion.PromotionSysNo = currSpPromotion.SysNo
            shoppingCartGroupPromotion.RuleType = currSpPromotion.PromotionRule.RuleType

            shoppingCart.GroupPromotions = List[CrShoppingCartGroupPromotion]()
            shoppingCart.GroupPromotions.Add(shoppingCartGroupPromotion)

    return shoppingCart


#商品合计金额满199送立减99
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
    currCartGiftItems = CurrCartGiftItems

     #满条件
    kv_achieve_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "achieve_price").RuleValue)
    #立减
    kv_reduce_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "reduce_price").RuleValue)

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "商品合计"
    description_true = CurrSpPromotion.PromotionRule.TrueText
    description_false = CurrSpPromotion.PromotionRule.FalseText

    #查询组促销信息
    if shoppingCart.GroupPromotions == None:
        return shoppingCart
    scgp = shoppingCart.GroupPromotions.FirstOrDefault(lambda p : p.PromotionSysNo == currSpPromotion.SysNo)
    if scgp == None:
        return shoppingCart

    #当前组满足条件的商品销售总价
    productTotalAmount = (shoppingCart.ProductAmount - shoppingCart.ProductDiscountAmount - shoppingCart.SettlementDiscountAmount)
    
    if productTotalAmount >= kv_achieve_price:
        #当前促销成立时提示促销描述并计算促销价格
        scgp.Description = description_true % (displayPrefix,kv_achieve_price,kv_reduce_price)
        scgp.IsSatisfy = True
        scgp.IsUsed = True

        shoppingCart.SettlementDiscountAmount += kv_reduce_price

    else:
        #当前促销不成立时提示促销描述
        scgp.Description = description_false % (displayPrefix,kv_achieve_price)
        scgp.IsSatisfy = False
        scgp.IsUsed = False

    return shoppingCart