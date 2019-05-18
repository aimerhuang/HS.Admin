"""
【团购】
"""
import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *

#检查可使用促销的商品
def CheckPromotionProduct(shoppingCartItems):

    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion

    cartItems = shoppingCartItems.Where(lambda item : item.IsLock == 1 and item.Promotions == str(currSpPromotion.SysNo))

    for item in cartItems:
        if item.PromotionHints == None :
            item.PromotionHints = List[SpPromotionHint]()
        
        promotionHint = SpPromotionHint()
        promotionHint.PromotionSysNo = currSpPromotion.SysNo
        promotionHint.RuleType = currSpPromotion.PromotionRule.RuleType
        promotionHint.FrontText = "团购商品"
        promotionHint.SubjectUrl = currSpPromotion.SubjectUrl
        
        item.PromotionHints.Add(promotionHint)

    return shoppingCartItems

#团购
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
  
    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "活动商品"

    #判断当前组商品是否满足当前促销
    for scg in shoppingCart.ShoppingCartGroups:
        if scg.GroupPromotions == None:
            continue

        #查询组促销信息
        scgp = scg.GroupPromotions.FirstOrDefault(lambda p : p.PromotionSysNo == currSpPromotion.SysNo)
        if scgp == None:
            continue

        scgp.Description = "团购商品"
        scgp.IsSatisfy = True
        scgp.IsUsed = True

    return shoppingCart
