import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *

#指定商品每满100减10
def CalculateCart(shoppingCart): 
    
    #变量：CurrSpPromotion
    currSpPromotion = CurrSpPromotion

    #当前促销对促销类型为(商品,分类)有效
    if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType != 20):
        return shoppingCart

    #参与活动的商品
    join_productSysno = [50]
    #每满条件
    achieve_product = 100
    #立减
    reduce_product = 10

    #当前用户等级
    current_customer_level = 1
    
    #循环购物车商品组
    for cartProductGroup in shoppingCart.CartProductGroups:
        
        #循环购物车商品对象
        for shopCartProduct in cartProductGroup.ShopCartProducts:
            
            #判断购物车商品是否在指定参与活动的商品
            if (join_productSysno.count(shopCartProduct.ProductSysNo) > 0):
                
                #如果购物车的商品满足当前促销，将优先级最高的促销加入商品“可使用的促销”
                if (cartProductGroup.Promotions == None):
                    cartProductGroup.Promotions = List[SpPromotion]()
                if cartProductGroup.Promotions.FirstOrDefault(lambda p : p.SysNo == currSpPromotion.SysNo) == None:
                    cartProductGroup.Promotions.Add(currSpPromotion)

                #已使用促销,判断已使用促销优先级是否高于当前促销优先级,如果高于当前促销优先级，不继续判断当前促销
                if cartProductGroup.UsedPromotions == None:
                    cartProductGroup.UsedPromotions = List[SpPromotion]()
                else:
                    for up in cartProductGroup.UsedPromotions:
                        #p.PromotionType == currSpPromotion.PromotionType and
                        #使用的促销优先级大于当前促销优先级则返回
                        if (up.Priority > currSpPromotion.Priority):
                            return shoppingCart
                        else:
                            #当前促销与使用促销不属叠加促销时返回
                            if (currSpPromotion.OverlayPromotions == None or currSpPromotion.OverlayPromotions.FirstOrDefault(lambda op : op.SysNo == up.SysNo) == None):
                                return shoppingCart

                #循环商品等级价格
                for levelPrice in shopCartProduct.Product.Prices:
                    #当前会员等级价格每满{指定价格}时减去{优惠价格}
                    if (levelPrice.SourceSysNo == current_customer_level) and ((shopCartProduct.ProductNumber * levelPrice.Price) >= achieve_product):
                        if (currSpPromotion.IsAutoChoosed):
                            cartProductGroup.SaleTotalAmount = shopCartProduct.ProductNumber * levelPrice.Price
                            cartProductGroup.DiscountAmount = (int(shopCartProduct.ProductNumber * levelPrice.Price / achieve_product) * reduce_product)

                            shopCartProduct.ProductPrice = cartProductGroup.SaleTotalAmount - cartProductGroup.DiscountAmount

                            if (cartProductGroup.UsedPromotions == None):
                                cartProductGroup.UsedPromotions = List[SpPromotion]()
                            cartProductGroup.UsedPromotions.Add(currSpPromotion)

    return shoppingCart

#print int(199.99*2/100.00)*9.90
#raw_input()