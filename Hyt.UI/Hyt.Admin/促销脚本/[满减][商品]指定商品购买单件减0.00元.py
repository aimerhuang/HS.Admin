"""
[满减][商品]指定商品购买单件减0.00元.py
eg:活动商品买1件减30.00元,买2件减60.00元,以此类推。

创建配置：SpPromotionRule
FrontText = %s买1件减%0.2f元,买2件减%0.2f元,以此类推。
TrueText = %s，已减%0.2f元现金
FalseText = %s，购买即可享受优惠

创建配置：SpPromotionRuleKeyValue
rulekey = rulevalue
product_sysno = 商品系统编号1;商品系统编号2(注:多个商品系统编号分号分隔)
reduce_price = 19.00(注:每件商品立减多少元)

"""
import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *

#检查可使用促销的商品
def CheckPromotionProduct(shoppingCart):

    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion

    #KeyValue
    #参与活动商品
    originalValue_product_sysno = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "product_sysno").RuleValue
    kv_product_sysno = map(int,originalValue_product_sysno.split(';'))

    #立减
    kv_reduce_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "reduce_price").RuleValue)

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "活动商品"
    frontText = CurrSpPromotion.PromotionRule.FrontText
    frontText = frontText % (displayPrefix,kv_reduce_price,2 * kv_reduce_price)

    #当前促销对促销类型为(商品,分类)有效
    if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType != 20):
        return shoppingCart

    for pdt in shoppingCart:
        #跳过锁定商品
        if pdt.IsLock == 1:
            continue
        
        #商品满足当前促销
        if kv_product_sysno.count(pdt.ProductSysNo) != 0:
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
                continue
            
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


#指定商品购买单件减0.00元
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
    
    #KeyValue
    #参与活动商品
    originalValue_product_sysno = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "product_sysno").RuleValue
    kv_product_sysno = map(int,originalValue_product_sysno.split(';'))
    
    #立减
    kv_reduce_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "reduce_price").RuleValue)
    shoppingCart.WriteDebug(float(kv_reduce_price))

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "活动商品"
    description_true = CurrSpPromotion.PromotionRule.TrueText
    description_false = CurrSpPromotion.PromotionRule.FalseText
    

    #当前组满足条件的商品总数
    product_number = 0

    #判断当前组商品是否满足当前促销
    for scg in shoppingCart.ShoppingCartGroups:
        if scg.GroupPromotions == None:
            continue

        #查询组促销信息
        scgp = scg.GroupPromotions.FirstOrDefault(lambda p : p.PromotionSysNo == currSpPromotion.SysNo)
        if scgp == None:
            continue

        for sc in scg.ShoppingCartItems:
            #判断促销成立条件
            if sc.Promotions == None or len(sc.Promotions) == 0 or map(int,sc.Promotions.split(';')).count(currSpPromotion.SysNo) == 0 or sc.IsChecked == 0:
                continue
            product_number += sc.Quantity

        else:
            if product_number != 0:
                #当前促销成立时提示促销描述并计算促销价格

                scgp.Description = description_true % (displayPrefix, product_number * kv_reduce_price)
                scgp.IsSatisfy = True
                scgp.IsUsed = True

                #计算促销优惠金额
                for sc in scg.ShoppingCartItems:
                    if sc.Promotions == None or len(sc.Promotions) == 0 or map(int,sc.Promotions.split(';')).count(currSpPromotion.SysNo) == 0 or sc.IsChecked == 0:
                        continue
                  
                    sc.DiscountAmount += sc.Quantity * kv_reduce_price

            else:
                #当前促销不成立时提示促销描述
                scgp.Description = description_false % (displayPrefix)
                scgp.IsSatisfy = False
                scgp.IsUsed = False

    return shoppingCart
