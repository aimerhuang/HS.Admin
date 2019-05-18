"""
eg:订单购满58.00，108.00,158.00元可以任选1件赠品

创建配置：SpPromotionRule
FrontText = %s购买满%s元可以任选1件赠品
TrueText = %s已购满%0.2f元，可领取%s件赠品
FalseText = %s购满%0.2f元，即可领取%s件赠品

创建配置：SpPromotionRuleKeyValue
rulekey = rulevalue

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

    #当前促销对促销类型为(60:应用到订单合计)有效
    if (currSpPromotion.PromotionType != 60):
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


#指定商品满199送立减99
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
    currCartGiftItems = CurrCartGiftItems

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) !=0 else "活动商品"
    description_true = CurrSpPromotion.PromotionRule.TrueText
    description_false = CurrSpPromotion.PromotionRule.FalseText

    #查询组促销信息
    if shoppingCart.GroupPromotions == None:
        return shoppingCart
    scgp = shoppingCart.GroupPromotions.FirstOrDefault(lambda p : p.PromotionSysNo == currSpPromotion.SysNo)
    if scgp == None:
        return shoppingCart

    #当前组满足条件的商品销售总价
    giftProductRequirementAmount = currSpPromotion.PromotionGifts.Select(lambda p : p.RequirementMinAmount).Distinct().OrderByDescending(lambda s : s).ToList()
    settlementAmount = 0

    settlementAmount = (shoppingCart.ProductAmount + shoppingCart.FreightAmount 
                                - shoppingCart.ProductDiscountAmount - shoppingCart.FreightDiscountAmount - shoppingCart.SettlementDiscountAmount)
        
    requirementAmount = giftProductRequirementAmount.FirstOrDefault(lambda n : n <= settlementAmount)
    shoppingCart.WriteDebug(description_true)
    shoppingCart.WriteDebug(description_false)
    if requirementAmount != None:
        #当前促销成立时提示促销描述并计算促销价格
        scgp.Description = description_true%(displayPrefix,requirementAmount, "1")
        scgp.IsSatisfy = True
        scgp.IsUsed = False

        if scgp.GiftProducts == None:
            scgp.GiftProducts = List[CBSpPromotionGift]()

        for agp in currSpPromotion.PromotionGifts:
            if settlementAmount >= agp.RequirementMinAmount and settlementAmount < agp.RequirementMaxAmount:
                scgp.GiftProducts.Add(agp)
                #判断促销商品是否已被用户选择
                giftItem = currCartGiftItems.FirstOrDefault(lambda sci : sci.UsedPromotions == agp.PromotionSysNo.ToString() and sci.ProductSysNo == agp.ProductSysNo)
                if giftItem != None:
                    if scgp.UsedGiftProducts == None:
                        scgp.UsedGiftProducts = List[CBSpPromotionGift]()

                    scgp.IsUsed = True
                    scgp.UsedGiftProducts.Add(agp)

    else:
        #当前促销不成立时提示促销描述
        scgp.Description = description_false%(displayPrefix,giftProductRequirementAmount[giftProductRequirementAmount.Count-1], "1")
        scgp.IsSatisfy = False
        scgp.IsUsed = False

    return shoppingCart