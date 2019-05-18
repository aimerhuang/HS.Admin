#指定商品满199送立减29
def CalculateCart(shoppingCart): 
    #参与活动的商品
    join_productSysno = [2]
    #满条件
    achieve_product = 299
    #立减
    reduce_product = 99

    #当前用户等级
    current_customer_level = 1
    
    #循环购物车商品对象
    for shopCartProduct in shoppingCart.ShopCartProducts:
        #判断购物车商品是否在指定参与活动的商品
        if (join_productSysno.count(shopCartProduct.ProductSysNo) > 0):
            #循环商品等级价格
            for levelPrice in shopCartProduct.Product.Prices:
                #当前会员等级价格满{指定价格}时减去{优惠价格}
                if (levelPrice.SourceSysNo == current_customer_level) and ((shopCartProduct.ProductNumber * levelPrice.Price) >= achieve_product):
                    shopCartProduct.ProductPrice = (shopCartProduct.ProductNumber * levelPrice.Price) - reduce_product
    return shoppingCart

#join_productSysno=[1,2,4,5]
#print join_productSysno.count(1)
#print join_productSysno.count(4)
#print join_productSysno.count(5)
#print join_productSysno.count(6)
#raw_input()