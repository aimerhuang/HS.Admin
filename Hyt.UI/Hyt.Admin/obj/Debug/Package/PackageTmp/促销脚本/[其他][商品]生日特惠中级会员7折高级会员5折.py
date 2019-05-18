"""
[其他][商品]生日特惠中级会员7折高级会员5折
eg:生日特惠中级会员7折高级会员5折

创建配置：SpPromotionRule
FrontText = %s{%s会员%0.2f折}
TrueText = %s%s会员已享受%0.2f折优惠
FalseText = %s%s会员可享受%0.2f折优惠

创建配置：SpPromotionRuleKeyValue
level_1 = 初级会员折扣,必须大于0小于10,输入0时不参与促销(如8.8折请输入:8.8)
level_2 = 中级会员折扣,必须大于0小于10,输入0时不参与促销(如7折请输入:7.0)
level_3 = 高级会员折扣,必须大于0小于10,输入0时不参与促销(如5折请输入:5.0)

"""
import clr
import System
import datetime
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.AddReference("Hyt.BLL")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *
from Hyt.Model.Transfer import *
from Hyt.BLL.Promotion import *

#检查可使用促销的商品
def CheckPromotionProduct(shoppingCart):

    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
    spPromotionToPython = CurrSpPromotionToPython

    #私有变量
    customerLevelSysNo = 1

    #当前促销对促销类型为(商品,分类)有效
    if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType != 20):
        return shoppingCart

    if currCrCustomer == None or currCrCustomer.Birthday == None:
        return shoppingCart

    customerLevelSysNo = int(currCrCustomer.LevelSysNo)

    if spPromotionToPython == None or spPromotionToPython.Order == None:
        #获取今天日期
        today = datetime.date.today()

        #年龄超过120岁，当天不是会员生日则跳出该促销,
        if int(today.strftime('%Y')) - int(currCrCustomer.Birthday.ToString("yyyy")) > 120 or today.strftime('%m%d') != currCrCustomer.Birthday.ToString("MMdd"):
            return shoppingCart

        #12个月内会员使用过该促销将跳出该促销
        if SpPromotionDataExtensions.UsedPromotionNum(currCrCustomer.SysNo, System.DateTime.Now.AddYears(-1), currSpPromotion.SysNo) > 0:
            return shoppingCart
    else:
        customerLevelSysNo = int(spPromotionToPython.Order.LevelSysNo)

    #KeyValue
    #等级1
    kv_level_1, kv_level_2, kv_level_3 = 0, 0, 0
    try:
        kv_level_1 = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "level_1").RuleValue
        kv_level_1 = float(kv_level_1)
        kv_level_1 = kv_level_1 if kv_level_1 > 0 and kv_level_1 < 10 else 0
    except:
        kv_level_1 = 0

    try:
        kv_level_2 = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "level_2").RuleValue
        kv_level_2 = float(kv_level_2)
        kv_level_2 = kv_level_2 if kv_level_2 > 0 and kv_level_2 < 10 else 0
    except:
        kv_level_2 = 0

    try:
        kv_level_3 = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "level_3").RuleValue
        kv_level_3 = float(kv_level_3)
        kv_level_3 = kv_level_3 if kv_level_3 > 0 and kv_level_3 < 10 else 0
    except:
        kv_level_3 = 0
    
    currLevelDiscount = 0
    #设置当前用户等级折扣
    if customerLevelSysNo == 1:
        currLevelDiscount = kv_level_1

    if customerLevelSysNo == 2:
        currLevelDiscount = kv_level_2

    if customerLevelSysNo == 3:
        currLevelDiscount = kv_level_3

    #如果当前用户等级未设置折扣则跳出该促销
    if not (currLevelDiscount > 0 and currLevelDiscount < 10):
        return shoppingCart

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "生日特惠"
    frontText = CurrSpPromotion.PromotionRule.FrontText
    whereFrontText = frontText[frontText.find('{') : frontText.find('}')]
    frontText = frontText[0 : frontText.find('{')]

    if kv_level_1 != 0 : 
        frontText += whereFrontText % ('初级', kv_level_1)
    if kv_level_2 != 0 : 
        frontText += whereFrontText % ('中级', kv_level_2)
    if kv_level_3 != 0 : 
        frontText += whereFrontText % ('高级', kv_level_3)

    frontText = frontText % (displayPrefix)
    
    #最大原单价的商品
    pdt = shoppingCart.OrderByDescending(lambda i : i.OriginPrice).FirstOrDefault()

    #跳过锁定商品
    if pdt != None and pdt.IsLock != 1:
        #商品满足当前促销
        if pdt.PromotionHints == None :
            pdt.PromotionHints = List[SpPromotionHint]()
            
        promotionHint = SpPromotionHint()
        promotionHint.PromotionSysNo = currSpPromotion.SysNo
        promotionHint.RuleType = currSpPromotion.PromotionRule.RuleType
        promotionHint.FrontText = frontText
        promotionHint.SubjectUrl = currSpPromotion.SubjectUrl

        #商品无促销，默认添加当前促销
        if pdt.Promotions == None or len(pdt.Promotions) == 0 :
            pdt.Promotions = currSpPromotion.SysNo.ToString()
                
            pdt.PromotionHints.Add(promotionHint)
        else:
            #商品已有促销
            pdtPromotions = map(int,pdt.Promotions.split(';'))
            
            #是否与当前促销叠加，叠加则加入当前促销
            if currSpPromotion.PromotionOverlays != None and map(int,currSpPromotion.PromotionOverlays).count(pdtPromotions[0]) > 0:
                pdtPromotions.Add(currSpPromotion.SysNo)
                pdt.Promotions = ';'.join(map(str,pdtPromotions))          
                pdt.PromotionHints.Add(promotionHint)

            #不叠加,判断已有优先级<当前促销，则使用当前促销
            else:
                pmt = currAllPromotion.FirstOrDefault(lambda p : p.SysNo == pdtPromotions[0])
                if pmt == None or pmt.Priority < currSpPromotion.Priority:
                    pdt.Promotions = currSpPromotion.SysNo.ToString()
                    pdt.PromotionHints.Clear()
                    pdt.PromotionHints.Add(promotionHint)

    return shoppingCart


#计算购物车
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
    currCartGiftItems = CurrCartGiftItems
    spPromotionToPython = CurrSpPromotionToPython

    #私有变量
    customerLevelSysNo = 1

    #KeyValue
    #等级1
    kv_level_1, kv_level_2, kv_level_3 = 0, 0, 0
    try:
        kv_level_1 = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "level_1").RuleValue
        kv_level_1 = float(kv_level_1)
        kv_level_1 = kv_level_1 if kv_level_1 > 0 and kv_level_1 < 10 else 0
    except:
        kv_level_1 = 0

    try:
        kv_level_2 = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "level_2").RuleValue
        kv_level_2 = float(kv_level_2)
        kv_level_2 = kv_level_2 if kv_level_2 > 0 and kv_level_2 < 10 else 0
    except:
        kv_level_2 = 0

    try:
        kv_level_3 = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "level_3").RuleValue
        kv_level_3 = float(kv_level_3)
        kv_level_3 = kv_level_3 if kv_level_3 > 0 and kv_level_3 < 10 else 0
    except:
        kv_level_3 = 0
    
    if spPromotionToPython == None or spPromotionToPython.Order == None:
        customerLevelSysNo = int(currCrCustomer.LevelSysNo)
    else:
        customerLevelSysNo = int(spPromotionToPython.Order.LevelSysNo)

    currLevelName = ''
    currLevelDiscount = 0
    #设置当前用户等级折扣
    if customerLevelSysNo == 1:
        currLevelDiscount = kv_level_1
        currLevelName = "初级"

    if customerLevelSysNo == 2:
        currLevelDiscount = kv_level_2
        currLevelName = "中级"

    if customerLevelSysNo == 3:
        currLevelDiscount = kv_level_3
        currLevelName = "高级"

    #如果当前用户等级未设置折扣则跳出该促销
    if not (currLevelDiscount > 0 and currLevelDiscount < 10):
        return shoppingCart
    
    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "生日特惠"
    description_true = CurrSpPromotion.PromotionRule.TrueText
    description_false = CurrSpPromotion.PromotionRule.FalseText

    #判断当前组商品是否满足当前促销
    for scg in shoppingCart.ShoppingCartGroups:
        if scg.GroupPromotions == None:
            continue

        #查询组促销信息
        scgp = scg.GroupPromotions.FirstOrDefault(lambda p : p.PromotionSysNo == currSpPromotion.SysNo)
        if scgp == None:
            continue

        for sc in scg.ShoppingCartItems:
            
            #当前促销成立时提示促销描述并计算促销价格
            scgp.Description = description_true % (displayPrefix,currLevelName, currLevelDiscount)
            scgp.IsSatisfy = True
            scgp.IsUsed = True

            pdDiscountAmount = sc.SaleTotalAmount/sc.Quantity - round((sc.SaleTotalAmount/sc.Quantity) * currLevelDiscount * 0.1,2)
            sc.DiscountAmount += pdDiscountAmount

    return shoppingCart