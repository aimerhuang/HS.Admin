"""
活动商品购买满58.00，108.00,158.00元可以任选1件赠品
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
join_productSysno = [20,21]

description_true = "活动商品已购满%s元，可领取%s件赠品"

description_false = "活动商品购满%s元，即可领取%s件赠品"

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
            if currSpPromotion.OverlayPromotions != None and currSpPromotion.OverlayPromotions.IndexOf(pdtPromotions[0]) >= 0:
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
    achieve_product = currSpPromotion.AvailableGiftProducts.Select(lambda p : p.SatisfyPrice).Distinct().OrderByDescending(lambda s : s).ToList()
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
            satisfyPrice = achieve_product.FirstOrDefault(lambda n : n <= product_sale_total_amount)

            if satisfyPrice != None:
                #当前促销成立时提示促销描述并计算促销价格
                scgp.Description = description_true%(satisfyPrice, currSpPromotion.AvailableGiftNumber)
                scgp.IsSatisfy = True
                scgp.IsUsed = False

                if scgp.GiftProducts == None:
                    scgp.GiftProducts = List[int]()

                for agp in currSpPromotion.AvailableGiftProducts:
                    if agp.SatisfyPrice <= product_sale_total_amount:
                        scgp.GiftProducts.Add(agp.ProductSysNo)

            else:
                #当前促销不成立时提示促销描述
                scgp.Description = description_false%(achieve_product[achieve_product.Count-1], currSpPromotion.AvailableGiftNumber)
                scgp.IsSatisfy = False
                scgp.IsUsed = False

    return shoppingCart

#achieve_product = [58,108,158]
#print achieve_product.FirstOrDefault(lambda n : n <= 68)
#print achieve_product.Count
#print achieve_product[achieve_product.Count-1]
#raw_input()