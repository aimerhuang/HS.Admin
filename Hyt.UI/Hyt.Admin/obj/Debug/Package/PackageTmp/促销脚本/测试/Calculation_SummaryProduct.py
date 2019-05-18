import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *

def CalculateCart(shoppingCart): 
    
    #当前用户等级
    current_customer_level = 1
    
    #变量：商品销售合计金额(优惠前的金额)
    productSaleTotalAmount = 0
    #变量：商品优惠金额(优惠的金额)
    productDiscountAmount = 0

    #循环购物车商品组
    for cartProductGroup in shoppingCart.CartProductGroups:
        
        #循环购物车商品对象
        for shopCartProduct in cartProductGroup.ShopCartProducts:
            
            #未使用促销的商品：计算商品价格
            if cartProductGroup.UsedPromotions == None or cartProductGroup.UsedPromotions.Count == 0:
                #循环商品等级价格
                for levelPrice in shopCartProduct.Product.Prices:
                    #当前会员等级价格满{指定价格}时减去{优惠价格}
                    if (levelPrice.SourceSysNo == current_customer_level):
                        shopCartProduct.ProductPrice = shopCartProduct.ProductNumber * levelPrice.Price
                        cartProductGroup.SaleTotalAmount = shopCartProduct.ProductNumber * levelPrice.Price
                        cartProductGroup.DiscountAmount = 0

        #汇总商品销售金额和优惠金额
        productSaleTotalAmount += cartProductGroup.SaleTotalAmount
        productDiscountAmount += cartProductGroup.DiscountAmount

    #将变量值赋给购物车对象
    shoppingCart.ProductSaleTotalAmount = productSaleTotalAmount
    shoppingCart.ProductDiscountAmount = productDiscountAmount

    return shoppingCart


###循环购物车商品对象
##for shopCartProduct in shoppingCart.ShopCartProducts.Where(lambda pdt :
##join_productSysno.Contains(pdt.ProductSysNo)):
##    #循环商品等级价格
##    levelPrice = shopCartProduct.Product.Prices.FirstOrDefault(lambda prc
##    : prc.PriceSource==10
##                                                               and
##                                                               prc.SourceSysNo==current_customer_level
##                                                               and
##                                                               (shopCartProduct.ProductNumber
##                                                               *
##                                                               prc.Price)
##                                                               >=
##                                                               achieve_product)
##                                                               ## if
##                                                               levelPrice
##                                                               != None :
##        shopCartProduct.ProductPrice = (shopCartProduct.ProductNumber *
##        levelPrice.Price) - reduce_product
   
#        promotions = List[SpPromotion]()
#        promotion = SpPromotion()
#        promotion.IsAutoChoosed = True
#        promotion.AvailableGiftNumber = 10
#        promotions.Add(promotion)
#        promotions.Add(promotion)
#        shopCartProduct.Promotions = promotions

#join_productSysno=[1,2,4,5]
#print join_productSysno.count(1)
#print join_productSysno.count(4)
#print join_productSysno.count(5)
#print join_productSysno.count(6)
#raw_input()