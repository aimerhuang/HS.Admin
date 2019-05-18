"""
eg:商品合计购满58.00，108.00,158.00元可以任选1件赠品

创建配置：SpPromotionRule
FrontText = %s购买满%s元可以任选1件赠品
TrueText = %s已购满%0.2f元，可领取%s件赠品
FalseText = %s购满%0.2f元，即可领取%s件赠品

创建配置：SpPromotionRuleKeyValue
rulekey = rulevalue
exclude_product_sysno = 排除商品:商品系统编号1(分号)商品系统编号2(注:多个商品系统编号分号分隔),排除的商品不做金额合计计算。

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


#指定商品满199获取赠品
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

    #KeyValue
    #排除商品
    originalValue_exclude_product_sysno = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "exclude_product_sysno")
    kv_exclude_product_sysno = []
    if type(originalValue_exclude_product_sysno) != type(None) and type(originalValue_exclude_product_sysno.RuleValue) != type(None):
        kv_exclude_product_sysno = map(int,originalValue_exclude_product_sysno.RuleValue .split(';'))

    #当前组满足条件的商品销售总价
    giftProductRequirementAmount = currSpPromotion.PromotionGifts.Select(lambda p : p.RequirementMinAmount).Distinct().OrderByDescending(lambda s : s).ToList()
    productTotalAmount = 0

    productTotalAmount = (shoppingCart.ProductAmount - shoppingCart.ProductDiscountAmount - shoppingCart.SettlementDiscountAmount)

    #计算排除商品（商品合计金额=排除商品销售金额-排除商品折扣金额）
    if len(kv_exclude_product_sysno) > 0:
        #指定商品可以使用优惠券
        for scg in shoppingCart.ShoppingCartGroups:
            for sc in scg.ShoppingCartItems:
                if kv_exclude_product_sysno.count(sc.ProductSysNo) > 0:
                    productTotalAmount -= (sc.SaleTotalAmount - sc.DiscountAmount)
        
    requirementAmount = giftProductRequirementAmount.FirstOrDefault(lambda n : n <= productTotalAmount)
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
            if productTotalAmount >= agp.RequirementMinAmount and productTotalAmount < agp.RequirementMaxAmount:
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