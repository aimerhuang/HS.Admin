import clr
import System

clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from Hyt.Model import *

#购买指定商品金额满99元可获得赠品
def CalculateCart(shoppingCart): 
    
    #变量：CurrSpPromotion
    currSpPromotion = CurrSpPromotion

    #当前促销对促销类型为(商品,分类)有效
    #if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType
    #!= 20):
    #    return shoppingCart

    #参与活动的商品
    join_productSysno = [20,21,22,23]
    #满条件
    achieve_product = 99

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
                            if (currSpPromotion.OverlayPromotions == None 
                                or currSpPromotion.OverlayPromotions.FirstOrDefault(lambda op : op.SysNo == up.SysNo) == None):

                                return shoppingCart

                #循环商品等级价格
                for levelPrice in shopCartProduct.Product.Prices:
                    #当前会员等级价格满{指定价格}
                    if ((levelPrice.SourceSysNo == current_customer_level) 
                        and ((shopCartProduct.ProductNumber * levelPrice.Price) >= achieve_product)):

                        currSpPromotion.IsSetUp = True
                        #if (currSpPromotion.IsAutoChoosed):
                            #shopCartProduct.ProductPrice =
                            #shopCartProduct.ProductNumber * levelPrice.Price -
                            #reduce_product
                            #cartProductGroup.SaleTotalAmount =
                            #shopCartProduct.ProductNumber * levelPrice.Price
                            #cartProductGroup.DiscountAmount = reduce_product

                            #if (cartProductGroup.UsedPromotions == None):
                            #    cartProductGroup.UsedPromotions =
                            #    List[SpPromotion]()
                            #cartProductGroup.UsedPromotions.Add(currSpPromotion) 
    return shoppingCart

#购买指定商品金额满99元可获得赠品
def AddGiftProduct(shoppingCart): 
     
    #变量：CurrSpPromotion
    currSpPromotion = CurrSpPromotion
    
    #变量：CurrGroupCode 当前购物车组代码
    currGroupCode = CurrGroupCode

    #变量：currAddGiftProductSysNo 当前添加赠品/加够商品系统编号
    currAddGiftProductSysNo = GiftProductSysNo

    #参与活动的商品
    join_productSysno = [20,21,22,23]

     #满条件
    achieve_product = 99

    #当前用户等级
    current_customer_level = 1

    #组商品价格合计
    groupTotalPrice = 0

     #循环购物车商品组
    for cartProductGroup in shoppingCart.CartProductGroups:
        #判断当前组商品
        if cartProductGroup.GroupCode == currGroupCode:
            #循环购物车商品对象
            for shopCartProduct in cartProductGroup.ShopCartProducts:
                #判断购物车商品是否在指定参与活动的商品
                if (join_productSysno.count(shopCartProduct.ProductSysNo) > 0):
                    #循环商品等级价格
                    for levelPrice in shopCartProduct.Product.Prices:
                        #当前会员等级价格满{指定价格}
                        if (levelPrice.SourceSysNo == current_customer_level):
                            groupTotalPrice += (shopCartProduct.ProductNumber * levelPrice.Price)

    #添加赠品/加购商品
    #当前组价格>=满足条件的价格则可以添加商品
    if groupTotalPrice >= achieve_product:
        #在当前促销中查找赠品
        giftProduct = currSpPromotion.AvailableGiftProducts.FirstOrDefault(lambda p : p.ProductSysNo == currAddGiftProductSysNo)
        if giftProduct != None:
            #在购物车找到需要添加赠品的购物车组
            cartProductGroup = shoppingCart.CartProductGroups.FirstOrDefault(lambda cpg : cpg.GroupCode == currGroupCode)
            if cartProductGroup != None:
                #添加使用促销
                if (cartProductGroup.UsedPromotions == None):
                    cartProductGroup.UsedPromotions = List[SpPromotion]()
                if cartProductGroup.UsedPromotions.FirstOrDefault(lambda up : up.SysNo == currSpPromotion.SysNo) == None:
                    cartProductGroup.UsedPromotions.Add(currSpPromotion) 

                #添加赠品
                if (cartProductGroup.UsedPromotionGiftProducts == None):
                    cartProductGroup.UsedPromotionGiftProducts = List[PromotionGiftProduct]()
                cartProductGroup.UsedPromotionGiftProducts.Add(giftProduct)
                
                #计算组 销售合计金额(不扣除优惠金额)
                cartProductGroup.SaleTotalAmount += giftProduct.ProductPrice

                #计算购物车 商品销售合计金额(优惠前的金额)
                shoppingCart.ProductSaleTotalAmount += giftProduct.ProductPrice
                
                #计算购物车 订单结算金额(=商品销售金额-商品优惠金额-订单优惠金额)
                shoppingCart.OrderSettlementAmount += giftProduct.ProductPrice

    return shoppingCart

#import copy
#sysnos = [20,21,22,23]
#a=copy.deepcopy(sysnos)
#for sysno in sysnos:
    
#    if(sysno==21):
#        sysnos.Add(sysno)

#    #print sysno
#    if(sysno==23):
#        sysnos.Remove(21)


#print sysnos.Count
#print a.Count
#raw_input()