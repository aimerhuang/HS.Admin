import clr
import System
clr.AddReference("Hyt.Model")
clr.AddReference("System.Core")
clr.ImportExtensions(System.Linq)

from System.Collections.Generic import *
from System import *
from Hyt.Model import *

#指定商品满199送立减99
def CalculateCart(shoppingCart): 
    
    #变量：CurrSpPromotion
    currSpPromotion = CurrSpPromotion
    AllPromotion = CurrAllPromotion

    shoppingCart.WriteDebug("新促销开始")
    shoppingCart.WriteDebug(currSpPromotion.SysNo)
    shoppingCart.WriteDebug(currSpPromotion)

    #当前促销对促销类型为(商品,分类)有效
    if (currSpPromotion.PromotionType != 10 and currSpPromotion.PromotionType != 20):
        return shoppingCart

    #参与活动的商品
    join_productSysno = [2,1]
    #满条件
    achieve_product = 299
    #立减
    reduce_product = 99

    #当前用户等级
    current_customer_level = 1
    
    tempShoppingCartGroup = CrShoppingCartGroup()
    tempShoppingCartGroup.ShoppingCartProducts = List[CrShoppingCartProduct]()
    tempShoppingCartGroup.DataStatus = "insert"

    #循环购物车组
    for scg in shoppingCart.ShoppingCartGroups:
        
        #组为锁定 [or 有促销代码 or scg.Promotions != ""] or 有删除状态 则跳过当前组
        if scg.IsLock == 1 or scg.DataStatus == "delete":
            continue

        #循环购物车商品对象
        for scp in scg.ShoppingCartProducts:
            #有删除状态 则跳过当前组 or 判断商品是否可以参与当前促销
            if scp.DataStatus == "delete" or join_productSysno.count(scp.ProductSysNo) == 0:
                continue
            #TODO:如果当前组有使用当前促销，判断组商品是否满足当前促销（不满足：清除使用当前促销，重新计算所有促销）
            #TODO:此段代码检查叠加和优先级，需重复检查

            #如scg.Promotions包含当前促销currSpPromotion.SysNo，且scg.Promotions总数 ==1 ；则该商品无需移动break
            #如scg.Promotions不包含当前促销currSpPromotion.SysNo，且scg.Promotions>0 ；则判断所有促销是否满足当前叠加
                #满足叠加的保留
                #不满足叠加的判断不满足规则优先级是否大于当前促销，优先级大于当前促销优先级，保留最大的

            #如scg.Promotions包含当前促销currSpPromotion.SysNo，且scg.Promotions总数 >=1 ；则判断所有促销是否在叠加中

            arrPromotionSysNos = []
            shoppingCart.WriteDebug("scg.Promotions是否为空")
            shoppingCart.WriteDebug(scg.Promotions)
            if scg.Promotions != None:

                scgPromotions = map(int,scg.Promotions.split(';'))
                for pSysNo in scgPromotions:
                    shoppingCart.WriteDebug("pSysNo=")
                    shoppingCart.WriteDebug(pSysNo)
                    scgP = AllPromotion.FirstOrDefault(lambda p : p.SysNo == pSysNo)
                    shoppingCart.WriteDebug("AllPromotion=")
                    shoppingCart.WriteDebug(AllPromotion)
                    shoppingCart.WriteDebug("scgP=")
                    shoppingCart.WriteDebug(scgP)
                    if scgP != None and (scgP.SysNo == currSpPromotion.SysNo or scgP.Priority >= currSpPromotion.Priority):
                        arrPromotionSysNos.Add(scgP)
                        shoppingCart.WriteDebug("1if scgP != None and (scgP.SysNo == currSpPromotion.SysNo or scgP.Priority >= currSpPromotion.Priority):")
                        break
                    shoppingCart.WriteDebug("未跳出")
                    arrPromotionSysNos.Add(scgP)
               
                    #shoppingCart.WriteDebug("2if scgP != None and (scgP.SysNo == currSpPromotion.SysNo or scgP.Priority >= currSpPromotion.Priority):")
                shoppingCart.WriteDebug("3if scgP != None and (scgP.SysNo == currSpPromotion.SysNo or scgP.Priority >= currSpPromotion.Priority):")
                
            #添加到临时组商品集合
            arrPromotionSysNos.Add(currSpPromotion.SysNo.ToString())
            tempScp = CrShoppingCartProduct()
            tempScp.SysNo = scp.SysNo
            tempScp.ProductSysNo = scp.ProductSysNo
            tempScp.Quantity = scp.Quantity
            tempScp.ProductPrice = scp.ProductPrice
            tempScp.OriginalUnitPrice = scp.OriginalUnitPrice;
            tempScp.SaleTotalAmount = scp.SaleTotalAmount;
            tempScp.DiscountAmount = scp.DiscountAmount;
            tempScp.UsedGiftProducts = scp.UsedGiftProducts
            tempScp.CreateDate = scp.CreateDate
            tempScp.Source  = scp.Source
            tempScp.ProductSalesType  = scp.ProductSalesType
            tempScp.Product  = scp.Product
            tempShoppingCartGroup.Promotions = currSpPromotion.SysNo.ToString() #';'.join(arrPromotionSysNos)
            tempShoppingCartGroup.ShoppingCartProducts.Add(tempScp)

            #标识当前商品为删除状态
            scp.DataStatus = "delete"

            #tempShoppingCartGroup.UsedPromotions = '%s;%s' % (tempShoppingCartGroup.UsedPromotions if tempShoppingCartGroup.UsedPromotions != None else "", currSpPromotion.SysNo.ToString())

            shoppingCart.WriteDebug(tempShoppingCartGroup.UsedPromotions)
            
            #循环商品等级价格
            #for levelPrice in scp.Product.Prices:
            #    #当前会员等级价格满{指定价格}时减去{优惠价格} and ((scp.ProductNumber * levelPrice.Price) >= achieve_product)
            #    if levelPrice.SourceSysNo == current_customer_level:
            #        scp.ProductPrice = levelPrice.Price
            #        break
        else:
            if not scg.ShoppingCartProducts.Any(lambda p : p.DataStatus != "delete"):
                shoppingCart.WriteDebug(scg)
                scg.DataStatus = "delete"
    else:
        existScg = shoppingCart.ShoppingCartGroups.FirstOrDefault(
                                                            lambda g : g.Promotions != None 
                                                            and g.Promotions.split(';').count(currSpPromotion.SysNo.ToString())>0
                                                            and g.DataStatus != 'delete')
        shoppingCart.WriteDebug(existScg)
        
        if existScg != None:
            existScg.DataStatus = "update"
            existScg.ShoppingCartProducts.Add(tempShoppingCartGroup.ShoppingCartProducts[0])
        else:
            shoppingCart.ShoppingCartGroups.Add(tempShoppingCartGroup)
            existScg = tempShoppingCartGroup

        shoppingCart.WriteDebug("存在组")
        shoppingCart.WriteDebug(existScg)

        useCurrPromotionsScg = shoppingCart.ShoppingCartGroups.Where(
                                                            lambda g : g.Promotions != None 
                                                            and g.Promotions.split(';').count(currSpPromotion.SysNo.ToString())>0
                                                            and g.DataStatus != 'delete')
        #当前组满足条件的商品销售总价
        product_sale_total_amount = 0
        product_number = 0

        #判断当前组商品是否满足当前促销
        for ucpscg in useCurrPromotionsScg:
            for scp in ucpscg.ShoppingCartProducts:
                if scp.DataStatus == "delete" or join_productSysno.count(scp.ProductSysNo) == 0:
                    continue
                product_number = product_number + 1

                product_sale_total_amount += scp.SaleTotalAmount 

                shoppingCart.WriteDebug(product_number)
                shoppingCart.WriteDebug(product_sale_total_amount)

                # #循环商品等级价格
                #for levelPrice in scp.Product.Prices:
                #    #商品销售总价
                #    if levelPrice.SourceSysNo == current_customer_level:
                #        ProductPrice = levelPrice.Price
                #        product_sale_total_amount += scp.Quantity * levelPrice.Price
                #        break
            else:
                shoppingCart.WriteDebug("是否成立")
                shoppingCart.WriteDebug(product_sale_total_amount)
                if product_sale_total_amount >= achieve_product:
                    ucpscg.UsedPromotions = currSpPromotion.SysNo.ToString() #TODO:多促销处理
                    i = 0
                    totalDiscountAmount = 0

                    for scp in ucpscg.ShoppingCartProducts:
                        if scp.DataStatus == "delete" or join_productSysno.count(scp.ProductSysNo) == 0:
                            continue
                        i = i + 1
                        if i != product_number:
                            discountAmount = round(scp.SaleTotalAmount / product_sale_total_amount * reduce_product ,1)
                            totalDiscountAmount += discountAmount
                            scp.DiscountAmount += discountAmount
                            scp.SaleTotalAmount -= discountAmount
                        else:
                            discountAmount = reduce_product - totalDiscountAmount
                            scp.DiscountAmount += discountAmount
                            scp.SaleTotalAmount -= discountAmount

        print ("计算所有满足当前促销组是否成立")
            ##如果购物车的商品满足当前促销，将优先级最高的促销加入商品“可使用的促销”
            #if (cartProductGroup.Promotions == None):
            #    cartProductGroup.Promotions = List[SpPromotion]()
            #if cartProductGroup.Promotions.FirstOrDefault(lambda p : p.SysNo == currSpPromotion.SysNo) == None:
            #    cartProductGroup.Promotions.Add(currSpPromotion)

            ##已使用促销,判断已使用促销优先级是否高于当前促销优先级,如果高于当前促销优先级，不继续判断当前促销
            #if cartProductGroup.UsedPromotions == None:
            #    cartProductGroup.UsedPromotions = List[SpPromotion]()
            #else:
            #    for up in cartProductGroup.UsedPromotions:
            #        #p.PromotionType == currSpPromotion.PromotionType and
            #        #使用的促销优先级大于当前促销优先级则返回
            #        if (up.Priority > currSpPromotion.Priority):
            #            return shoppingCart
            #        else:
            #            #当前促销与使用促销不属叠加促销时返回
            #            if (currSpPromotion.OverlayPromotions == None or currSpPromotion.OverlayPromotions.FirstOrDefault(lambda op : op.SysNo == up.SysNo) == None):
            #                return shoppingCart

           
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

#join_productSysno=[1,2,1,4,5]
#print join_productSysno.count(1)
#print join_productSysno.count(4)
#print join_productSysno.count(5)
#print join_productSysno.count(6)
#raw_input()

#a = '12a;b;c;;'
#a=a.split(';')
#print a.Count
#print ';'.join(a)
#raw_input()
#print 20.00/69.99*99
#print round(20.00/69.99*99,1)

#raw_input()