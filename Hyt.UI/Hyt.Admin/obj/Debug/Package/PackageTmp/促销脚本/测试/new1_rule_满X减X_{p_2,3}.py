"""
活动商品购买满299.00元立减99.00元
"""
import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from System import *
from Hyt.Model import *

#参与活动的商品
join_productSysno = [2,3]

description_true = "活动商品已购满299.00元，已减99.00元现金"

description_false = "活动商品购满299.00元，即可享受满减优惠"

#检查可使用促销的商品
def CheckPromotionProduct(shoppingCart):

    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCustomerSysNo = CurrCustomerSysNo
    currAllPromotion = CurrAllPromotion

    #当前促销对促销类型为(商品,分类)有效
    if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType != 20):
        return shoppingCart

    for pdt in shoppingCart:
        #跳过锁定商品
        if pdt.IsLock == 1:
            continue
        
        #商品满足当前促销
        if join_productSysno.count(pdt.ProductSysNo) != 0:
            #商品无促销
            if pdt.Promotions == None or len(pdt.Promotions) == 0 :
                pdt.Promotions = currSpPromotion.SysNo.ToString()
                continue
            
            #商品已有促销
            pdtPromotions = map(int,pdt.Promotions.split(';'))
            
            #是否与当前促销叠加，叠加则加入当前促销
            if currSpPromotion.PromotionOverlays != None and currSpPromotion.PromotionOverlays.IndexOf(pdtPromotions[0]) >= 0:
                pdtPromotions.Add(currSpPromotion.SysNo)
                pdt.Promotions = ';'.join(map(String,pdtPromotions))

            #不叠加,判断已有优先级<当前促销，则使用当前促销
            else:
                pmt = currAllPromotion.FirstOrDefault(lambda p : p.SysNo == pdtPromotions[0])
                if pmt == None or pmt.Priority < currSpPromotion.Priority:
                    pdt.Promotions = currSpPromotion.SysNo.ToString()

    return shoppingCart


#指定商品满199送立减99
def CalculateCart(shoppingCart): 
    
    #传入参数变量
    currSpPromotion = CurrSpPromotion
    currCustomerSysNo = CurrCustomerSysNo
    currAllPromotion = CurrAllPromotion
    
    #满条件
    achieve_product = 299
    #立减
    reduce_product = 99

    #当前组满足条件的商品销售总价
    product_sale_total_amount = 0
    product_row_number = 0

    #判断当前组商品是否满足当前促销
    for scg in shoppingCart.ShoppingCartGroups:
        if scg.ShoppingCartGroupPromotions == None:
            continue

        #查询组促销信息
        scgp = scg.ShoppingCartGroupPromotions.FirstOrDefault(lambda p : p.PromotionSysNo == currSpPromotion.SysNo)
        if scgp == None:
            continue

        for sc in scg.ShoppingCarts:
            #判断促销成立条件
            if sc.Promotions == None or len(sc.Promotions) == 0 or map(int,sc.Promotions.split(';')).count(currSpPromotion.SysNo) == 0:
                continue
            product_row_number += 1
            product_sale_total_amount += sc.SaleTotalAmount

        else:
            if product_sale_total_amount >= achieve_product:
                #当前促销成立时提示促销描述并计算促销价格

                scgp.Description = description_true
                scgp.IsSatisfy = True
                scgp.IsUsed = True

                i = 0
                totalDiscountAmount = 0
                
                #计算促销优惠金额
                for sc in scg.ShoppingCarts:
                    if sc.Promotions == None or len(sc.Promotions) == 0 or map(int,sc.Promotions.split(';')).count(currSpPromotion.SysNo) == 0:
                        continue
                    i += 1
                    scDiscountAmount = 0

                    if i != product_row_number:
                        scDiscountAmount = round(sc.SaleTotalAmount / product_sale_total_amount * reduce_product ,1)
                        totalDiscountAmount += scDiscountAmount                        
                    else:
                        scDiscountAmount = round(reduce_product - totalDiscountAmount,1)

                    sc.DiscountAmount += scDiscountAmount
                    sc.SaleTotalAmount -= scDiscountAmount
            else:
                #当前促销不成立时提示促销描述
                scgp.Description = description_false
                scgp.IsSatisfy = False
                scgp.IsUsed = False

    return shoppingCart