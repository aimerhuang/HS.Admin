#import clr
#import System
#clr.AddReference("Hyt.Model")
#clr.AddReference("System.Core")
#clr.ImportExtensions(System.Linq)

#from System.Collections.Generic import *
#from Hyt.Model import *

#指定商品满199送立减99
def CalculateCart(shoppingCart): 
    
    #变量：CurrSpPromotion
    currSpPromotion = CurrSpPromotion

    #当前促销对促销类型为(商品,分类)有效
    if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType != 20):
        return shoppingCart

    #参与活动的商品
    join_productSysno = [2]
    #满条件
    achieve_product = 299
    #立减
    reduce_product = 99

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
                    #当前会员等级价格满{指定价格}时减去{优惠价格}
                    if (levelPrice.SourceSysNo == current_customer_level) and ((shopCartProduct.ProductNumber * levelPrice.Price) >= achieve_product):
                        if (currSpPromotion.IsAutoChoosed):
                            shopCartProduct.ProductPrice = shopCartProduct.ProductNumber * levelPrice.Price - reduce_product
                            cartProductGroup.SaleTotalAmount = shopCartProduct.ProductNumber * levelPrice.Price
                            cartProductGroup.DiscountAmount = reduce_product

                            if (cartProductGroup.UsedPromotions == None):
                                cartProductGroup.UsedPromotions = List[SpPromotion]()
                            cartProductGroup.UsedPromotions.Add(currSpPromotion)

                        #已满足条件的促销
                        #shopCartProduct.Promotions.Add(currSpPromotion)

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
    return shoppingCart

join_productSysno=[1,2,4,5]
print (oin_productSysno.count(1))
print (oin_productSysno.count(4))
print (oin_productSysno.count(5))
print (oin_productSysno.count(6))
raw_input()