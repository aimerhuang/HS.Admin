"""
eg:活动商品购买满299.00元立减99.00元

创建配置：SpPromotionRule
FrontText = %s购满%0.2f元，立减%0.2f元
TrueText = %s已购满%0.2f元，已减%0.2f元现金
FalseText = %s购满%0.2f元，即可享受满减优惠

创建配置：SpPromotionRuleKeyValue
rulekey = rulevalue
product_sysno = 商品系统编号1;商品系统编号2(注:多个商品系统编号分号分隔)
achieve_price = 199.00(注:商品购买满多少元)
reduce_price = 19.00(注:立减多少元)

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

    #满条件
    kv_achieve_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "achieve_price").RuleValue)
    #立减
    kv_reduce_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "reduce_price").RuleValue)

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "活动商品"
    frontText = CurrSpPromotion.PromotionRule.FrontText
    frontText = frontText % (displayPrefix,kv_achieve_price,kv_reduce_price)

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


#指定商品满199送立减99
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCrCustomer = CurrCrCustomer
    currAllPromotion = CurrAllPromotion
    
    #KeyValue
    #参与活动商品
    originalValue_product_sysno = CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "product_sysno").RuleValue
    kv_product_sysno = map(int,originalValue_product_sysno.split(';'))
    #满条件
    kv_achieve_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "achieve_price").RuleValue)
    shoppingCart.WriteDebug(float(kv_achieve_price))
    #立减
    kv_reduce_price = float(CurrSpPromotion.PromotionRuleKeyValues.FirstOrDefault(lambda kv : kv.RuleKey == "reduce_price").RuleValue)
    shoppingCart.WriteDebug(float(kv_reduce_price))

    #提示描述
    displayPrefix = CurrSpPromotion.DisplayPrefix if CurrSpPromotion.DisplayPrefix != None and len(CurrSpPromotion.DisplayPrefix) != 0 else "活动商品"
    description_true = CurrSpPromotion.PromotionRule.TrueText
    description_false = CurrSpPromotion.PromotionRule.FalseText
    

    #当前组满足条件的商品销售总价
    product_sale_total_amount = 0
    product_row_number = 0

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
            product_row_number += 1
            product_sale_total_amount += sc.SaleTotalAmount

        else:
            if product_sale_total_amount >= kv_achieve_price:
                #当前促销成立时提示促销描述并计算促销价格

                scgp.Description = description_true % (displayPrefix,kv_achieve_price,kv_reduce_price)
                scgp.IsSatisfy = True
                scgp.IsUsed = True

                i = 0
                totalDiscountAmount = 0
                
                #计算促销优惠金额
                for sc in scg.ShoppingCartItems:
                    if sc.Promotions == None or len(sc.Promotions) == 0 or map(int,sc.Promotions.split(';')).count(currSpPromotion.SysNo) == 0 or sc.IsChecked == 0:
                        continue
                    i += 1
                    scDiscountAmount = 0

                    if i != product_row_number:
                        scDiscountAmount = round(sc.SaleTotalAmount / product_sale_total_amount * kv_reduce_price ,1)
                        totalDiscountAmount += scDiscountAmount
                    else:
                        scDiscountAmount = round(kv_reduce_price - totalDiscountAmount,2)

                    sc.DiscountAmount += scDiscountAmount
                    #sc.SaleTotalAmount -= scDiscountAmount
            else:
                #当前促销不成立时提示促销描述
                scgp.Description = description_false % (displayPrefix,kv_achieve_price)
                scgp.IsSatisfy = False
                scgp.IsUsed = False

    return shoppingCart
