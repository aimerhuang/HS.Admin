using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Hyt.BLL.Promotion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using Hyt.Service.Implement.B2CApp;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 PromotionBoTest 的测试类，旨在
    ///包含所有 PromotionBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class PromotionBoTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        ///// <summary>
        /////CalculatePromotion 的测试
        /////</summary>
        //[TestMethod()]
        //public void CalculatePromotionTest()
        //{
        //    PromotionBo target = new PromotionBo();
        //    Nullable<int> customerSysNo = new Nullable<int>(1);
        //    //ShoppingCart shoppingCart = new ShoppingCart();
        //    CrShoppingCart shoppingCart = new CrShoppingCart();
        //    //ShoppingCart expected = new ShoppingCart();
        //    CrShoppingCart expected = new CrShoppingCart();
        //    //ShoppingCart actual;
        //    CrShoppingCart actual;
          

        //    #region
        //    shoppingCart.ShoppingCartGroups = new List<CrShoppingCartGroup>()
        //        {
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupCode = Guid.NewGuid().ToString(),
        //                    GroupName = "商品2，满199减99",
        //                    ShoppingCartItems = new List<CBCrShoppingCartItem>()
        //                        {
        //                            #region 商品
        //                            new CBCrShoppingCartItem()
        //                                {
        //                                    SysNo = 1,
        //                                    ProductSysNo = 2,
        //                                    Quantity = 4,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                    new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                    new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupCode = Guid.NewGuid().ToString(),
        //                    GroupName = "商品1,3，满99减29",
        //                    ShoppingCartItems = new List<CBCrShoppingCartItem>()
        //                        {
        //                            #region 商品
        //                            new CBCrShoppingCartItem()
        //                                {
        //                                    SysNo = 2,
        //                                    ProductSysNo = 1,
        //                                    Quantity = 3,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸3代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 99},
        //                                                    new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 88},
        //                                                    new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 77}
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },

        //            new CartProductGroup()
        //            {
        //                GroupCode = Guid.NewGuid().ToString(),
        //                GroupName = "无任何优惠",
        //                ShopCartProducts = new List<ShopCartProduct>()
        //                    {
        //                        #region 商品
        //                        new ShopCartProduct()
        //                            {
        //                                SysNo = 3,
        //                                ProductSysNo = 4,
        //                                ProductNumber = 2,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            }
        //                        #endregion
        //                    }
        //            },
        //            new CartProductGroup()
        //            {
        //                GroupCode = Guid.NewGuid().ToString(),
        //                GroupName = "每满100元减10元",
        //                ShopCartProducts = new List<ShopCartProduct>()
        //                    {
        //                        #region 商品
        //                        new ShopCartProduct()
        //                            {
        //                                SysNo = 4,
        //                                ProductSysNo = 50,
        //                                ProductNumber = 2,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            }
        //                        #endregion
        //                    }
        //            },
        //            new CartProductGroup()
        //            {
        //                GroupCode = "6F9619FF-8B86-D011-B42D-00C04FC964FF",
        //                GroupName = "满99元有赠品",
        //                ShopCartProducts = new List<ShopCartProduct>()
        //                    {
        //                        #region 商品
        //                        new ShopCartProduct()
        //                            {
        //                                SysNo = 5,
        //                                ProductSysNo = 20,
        //                                ProductNumber = 2,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            }
        //                        #endregion
        //                    }
        //            },
        //            new CartProductGroup()
        //            {
        //                GroupCode = "3F2504E0-4F89-11D3-9A0C-0305E82C3301",
        //                GroupName = "需合并组",
        //                ShopCartProducts = new List<ShopCartProduct>()
        //                    {
        //                        #region 商品
        //                        new ShopCartProduct()
        //                            {
        //                                SysNo = 5,
        //                                ProductSysNo = 1996,
        //                                ProductNumber = 1,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            },
        //                             new ShopCartProduct()
        //                            {
        //                                SysNo = 5,
        //                                ProductSysNo = 1997,
        //                                ProductNumber = 1,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            },
        //                        #endregion
        //                    }
        //            },
        //             new CartProductGroup()
        //            {
        //                GroupCode = "3F2504E0-4F89-11D3-9A0C-0305E82C3302",
        //                GroupName = "需合并组",
        //                ShopCartProducts = new List<ShopCartProduct>()
        //                    {
        //                        #region 商品
        //                        new ShopCartProduct()
        //                            {
        //                                SysNo = 5,
        //                                ProductSysNo = 1918,
        //                                ProductNumber = 1,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            }
        //                        #endregion
        //                    }
        //            },
        //            new CartProductGroup()
        //            {
        //                GroupCode = "3F2504E0-4F89-11D3-9A0C-0305E82C3303",
        //                GroupName = "需合并组",
        //                ShopCartProducts = new List<ShopCartProduct>()
        //                    {
        //                        #region 商品
        //                        new ShopCartProduct()
        //                            {
        //                                SysNo = 5,
        //                                ProductSysNo = 2031,
        //                                ProductNumber = 1,
        //                                PurchasePrice = 0,
        //                                Product = new PdProduct()
        //                                    {
        //                                        ProductName = "电霸4代",
        //                                        Prices = new List<PdPrice>()
        //                                            {
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 1, Price = 199},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 2, Price = 189},
        //                                                new PdPrice() {PriceSource = 10, SourceSysNo = 3, Price = 179}
        //                                            }
        //                                    },
        //                            }
        //                        #endregion
        //                    }
        //            },
        //        };
        //    #endregion
        //    //var sysnos = new List<int>() { 1996, 1918, 2031 };
        //    //var cpgs =
        //    //    (shoppingCart.CartProductGroups.Where(
        //    //        cpg =>
        //    //        cpg.ShopCartProducts.Any(scp => sysnos.Contains(scp.ProductSysNo)) &&
        //    //        (cpg.UsedPromotions == null || cpg.UsedPromotions.Count == 0)).ToList());

        //    //foreach (var cartProductGroup in cpgs)
        //    //{
        //    //    foreach (var shopCartProduct in cartProductGroup.ShopCartProducts)
        //    //    {
        //    //        cartProductGroup.ShopCartProducts.Remove(shopCartProduct);
        //    //    }
        //    //}

        //    actual = target.CalculatePromotion(customerSysNo, shoppingCart);

        //    actual = target.AddGiftProducts(customerSysNo, actual, "6F9619FF-8B86-D011-B42D-00C04FC964FF",
        //                           1004, 4);
        //    actual = target.AddGiftProducts(customerSysNo, actual, "6F9619FF-8B86-D011-B42D-00C04FC964FF",
        //                           1001, 4);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("验证此测试方法的正确性。");
        //}

        ///// <summary>
        /////CalculatePromotions 的测试
        /////</summary>
        //[TestMethod()]
        //public void CalculatePromotionsTest()
        //{
        //    PromotionBo target = new PromotionBo(); // TODO: 初始化为适当的值
        //    Nullable<int> customerSysNo = new Nullable<int>(1); // TODO: 初始化为适当的值
        //    NewShoppingCart shoppingCart = new NewShoppingCart(); // TODO: 初始化为适当的值
        //    NewShoppingCart expected = new NewShoppingCart(); // TODO: 初始化为适当的值
        //    NewShoppingCart actual;

        //    #region

        //    shoppingCart.ShoppingCartGroups = new List<CrShoppingCartGroup>()
        //        {
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "商品2，满199减99",
        //                    Promotions = "13;21;1;123;;",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 1,
        //                                    ProductSysNo = 2,
        //                                    Quantity = 4,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "商品1,3，满99减29",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 2,
        //                                    ProductSysNo = 1,
        //                                    Quantity = 3,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸3代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 99
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 88
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 77
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },

        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "无任何优惠",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 3,
        //                                    ProductSysNo = 4,
        //                                    Quantity = 2,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "每满100元减10元",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 4,
        //                                    ProductSysNo = 50,
        //                                    Quantity = 2,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "满99元有赠品",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 5,
        //                                    ProductSysNo = 20,
        //                                    Quantity = 2,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "需合并组",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 5,
        //                                    ProductSysNo = 1996,
        //                                    Quantity = 1,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                },
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 5,
        //                                    ProductSysNo = 1997,
        //                                    Quantity = 1,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                },

        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "需合并组",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 5,
        //                                    ProductSysNo = 1918,
        //                                    Quantity = 1,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //            new CrShoppingCartGroup()
        //                {
        //                    GroupName = "需合并组",
        //                    ShoppingCartProducts = new List<CrShoppingCartProduct>()
        //                        {
        //                            #region 商品
        //                            new CrShoppingCartProduct()
        //                                {
        //                                    SysNo = 5,
        //                                    ProductSysNo = 2031,
        //                                    Quantity = 1,
        //                                    PurchasePrice = 0,
        //                                    Product = new PdProduct()
        //                                        {
        //                                            ProductName = "电霸4代",
        //                                            Prices = new List<PdPrice>()
        //                                                {
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 1,
        //                                                            Price = 199
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 2,
        //                                                            Price = 189
        //                                                        },
        //                                                    new PdPrice()
        //                                                        {
        //                                                            PriceSource = 10,
        //                                                            SourceSysNo = 3,
        //                                                            Price = 179
        //                                                        }
        //                                                }
        //                                        },
        //                                }
        //                            #endregion
        //                        }
        //                },
        //        };

        //    #endregion

        //    actual = target.CalculatePromotions(customerSysNo, shoppingCart);
        //    //Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("验证此测试方法的正确性。");
        //}
      
    }
}
