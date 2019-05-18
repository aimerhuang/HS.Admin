using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;
using Hyt.Util;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 购物车明细数
    /// </summary>
    /// <remarks>2013-08-16 吴文强 创建</remarks>
    public class CrShoppingCartItemDaoImpl : ICrShoppingCartItemDao
    {
        /// <summary>
        /// 获取顾客的购物车
        /// </summary>
        /// <param name="pager">购物车分页对象</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-03-07 杨文兵 创建
        /// </remarks>
        public override void GetPage(ref Pager<CrShoppingCartItem> pager)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrShoppingCartItem>("*")
                              .From("CrShoppingCart")
                              .Where("CustomerSysno = @CustomerSysno")
                              .Parameter("CustomerSysno", pager.PageFilter.CustomerSysNo)
                              .OrderBy(" sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                              .From("CrShoppingCart")
                              .Where("CustomerSysno = @CustomerSysno")
                              .Parameter("CustomerSysno", pager.PageFilter.CustomerSysNo)
                              .QuerySingle();
            }
        }

        /// <summary>
        /// 根据顾客ID和产品Id查询购物车
        /// </summary>
        /// <param name="customerSysNo">客户SysNo.</param>
        /// <param name="productSysNo">产品SysNo.</param>
        /// <returns>购物车</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>2013-08-05 唐永勤 创建</remarks>
        public override CrShoppingCart GetShoppingCart(int customerSysNo, int productSysNo)
        {
            CrShoppingCart entity = Context.Select<CrShoppingCart>("*")
                                           .From("CrShoppingCart")
                                           .Where("CustomerSysno = @CustomerSysno and ProductSysNo = @ProductSysNo")
                                           .Parameter("CustomerSysno", customerSysNo)
                                           .Parameter("ProductSysNo", productSysNo)
                                           .QuerySingle();
            return entity;
        }

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-08-16 吴文强 创建</remarks>
        public override List<CrShoppingCartItem> Add(List<CrShoppingCartItem> shoppingCartItems)
        {
            //检查是否已存在未锁定商品，存在+1，不存在新增

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                if (shoppingCartItem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
                {
                    Context.Delete("CrShoppingCartItem")
                        .Where("ProductSalesType", shoppingCartItem.ProductSalesType)
                        .Where("CustomerSysNo", shoppingCartItem.CustomerSysNo)
                        .Where("Promotions", shoppingCartItem.Promotions).Execute();
                }
                int itemSysNo = 0;
                if (shoppingCartItem.IsLock == (int)CustomerStatus.购物车是否锁定.否)
                {
                    //判断是否存在，存在为更新，不存在为新增
                    var item = Context.Select<CrShoppingCartItem>("*")
                                      .From("CrShoppingCartItem")
                                      .Where(
                                          "CustomerSysno = @CustomerSysno and ProductSysNo = @ProductSysNo and IsLock = @IsLock and (@Promotions is null or Promotions = @Promotions)")
                                      .Parameter("CustomerSysno", shoppingCartItem.CustomerSysNo)
                                      .Parameter("ProductSysNo", shoppingCartItem.ProductSysNo)
                                      .Parameter("IsLock", (int)CustomerStatus.购物车是否锁定.否)
                                      .Parameter("Promotions", shoppingCartItem.Promotions)
                                      .QuerySingle();
                    itemSysNo = item == null ? 0 : item.SysNo;
                }
                else
                {
                    //判断是否存在，存在为更新，不存在为新增
                    var item = Context.Select<CrShoppingCartItem>("*")
                                      .From("CrShoppingCartItem")
                                      .Where(
                                          "CustomerSysno = @CustomerSysno and ProductSysNo = @ProductSysNo and IsLock = @IsLock and GroupCode = @GroupCode and (@Promotions is null or Promotions = @Promotions)")
                                      .Parameter("CustomerSysno", shoppingCartItem.CustomerSysNo)
                                      .Parameter("ProductSysNo", shoppingCartItem.ProductSysNo)
                                      .Parameter("IsLock", (int)CustomerStatus.购物车是否锁定.是)
                                      .Parameter("GroupCode", shoppingCartItem.GroupCode)
                                      .Parameter("Promotions", shoppingCartItem.Promotions)
                                      .QuerySingle();
                    itemSysNo = item == null ? 0 : item.SysNo;
                }

                if (itemSysNo == 0)
                {
                    Context.Insert<CrShoppingCartItem>("CrShoppingCartItem", shoppingCartItem)
                           .AutoMap(x => x.SysNo)
                           .ExecuteReturnLastId<int>("sysno");
                }
                else
                {
                    Context.Sql("update CrShoppingCartItem set Quantity = Quantity + @Quantity where SysNo = @SysNo")
                           .Parameter("Quantity", shoppingCartItem.Quantity)
                           .Parameter("SysNo", itemSysNo)
                           .Execute();
                }
            }
            return shoppingCartItems;
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override void Delete(int customerSysNo, int[] sysNo)
        {
            const string strSql = @"
                        delete from CrShoppingCartItem 
                        where customersysno = @customersysno 
                          and sysno in(@sysNo)";
            Context.Sql(strSql)
                .Parameter("customersysno", customerSysNo)
                .Parameter("sysNo", sysNo).Execute();
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override void DeleteByProductSysNo(int customerSysNo, int[] sysNo)
        {
            const string strSql = @"
                        delete from CrShoppingCartItem 
                        where customersysno = @customersysno 
                          and ProductSysNo in(@sysNo)";
            Context.Sql(strSql)
                .Parameter("customersysno", customerSysNo)
                .Parameter("sysNo", sysNo).Execute();
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isCheckedItem">是否是选择的明细,true:删除选中明细;false:删除该用户全部明细</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override void Delete(int customerSysNo, bool isCheckedItem)
        {
            const string strSql = @"
                        delete from CrShoppingCartItem 
                        where customersysno = @customersysno 
                          and (-1 = @isCheckedItem or isChecked = @isCheckedItem)";
            Context.Sql(strSql)
                .Parameter("customersysno", customerSysNo)
                .Parameter("isCheckedItem", isCheckedItem ? (int)CustomerStatus.是否选中.是 : -1)
                .Execute();
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override void Delete(int customerSysNo, string groupCode, string promotionSysNo)
        {
            const string strSql = @"
                        delete from CrShoppingCartItem 
                        where customersysno = @customersysno 
                          and IsLock = @islock
                          and GroupCode = @groupcode
                          and promotions = @promotions";
            Context.Sql(strSql)
                .Parameter("customersysno", customerSysNo)
                .Parameter("islock", (int)CustomerStatus.购物车是否锁定.是)
                .Parameter("groupcode", groupCode)
                .Parameter("promotions", promotionSysNo).Execute();
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override void Delete(int customerSysNo, int productSysNo, int promotionSysNo)
        {
            const string strSql = @"
                        delete from CrShoppingCartItem 
                        where CustomerSysNo = @CustomerSysNo 
                          and ProductSysNo = @ProductSysNo
                          and UsedPromotions = @UsedPromotions";
            Context.Sql(strSql)
                .Parameter("CustomerSysNo", customerSysNo)
                .Parameter("ProductSysNo", productSysNo)
                .Parameter("UsedPromotions", promotionSysNo.ToString())
                .Execute();
        }

        /// <summary>
        /// 获取购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认查询全部</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override IList<CBCrShoppingCartItem> GetShoppingCartItems(int customerSysNo, int[] sysNo, bool isChecked = false)
        {
            const string strSql = @"
                        select * from CrShoppingCartItem 
                        where customersysno = @customersysno
                          and (0 = @nosysno or sysno in(@sysNo))
                          and (0 = @IsChecked or IsChecked = @IsChecked)";

            var entity = Context.Sql(strSql)
                                .Parameter("customersysno", customerSysNo)
                                .Parameter("nosysno", sysNo.IsNullOrEmpty() ? 0 : sysNo.Length)
                                .Parameter("sysNo", sysNo.IsNullOrEmpty() ? new[] { 0 } : sysNo)
                                .Parameter("IsChecked", isChecked ? 1 : 0)
                                .QueryMany<CBCrShoppingCartItem>();

            return entity;
        }

        /// <summary>
        /// 获取购物车已选择赠品明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public override IList<CBCrShoppingCartItem> GetShoppingCartGiftItems(int customerSysNo)
        {
            const string strSql = @"
                        select * from CrShoppingCartItem 
                        where customersysno = @customersysno
                          and ProductSalesType = @ProductSalesType";

            var entity = Context.Sql(strSql)
                                .Parameter("customersysno", customerSysNo)
                                .Parameter("ProductSalesType", (int)CustomerStatus.商品销售类型.赠品)
                                .QueryMany<CBCrShoppingCartItem>();

            return entity;
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="isChecked">是否选中(CustomerStatus.是否选中)</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public override void UpdateCheckedItem(int customerSysNo, string groupCode, string promotionSysNo, CustomerStatus.是否选中 isChecked)
        {
            if (groupCode == null || promotionSysNo == null)
            {
                return;
            }

            const string strSql = @"
                        update CrShoppingCartItem 
                        set IsChecked = @IsChecked
                        where customersysno = @customersysno 
                          and IsLock = @islock
                          and GroupCode = @groupcode
                          and promotions = @promotions";
            Context.Sql(strSql)
                   .Parameter("IsChecked", (int)isChecked)
                   .Parameter("customersysno", customerSysNo)
                   .Parameter("islock", (int)CustomerStatus.购物车是否锁定.是)
                   .Parameter("groupcode", groupCode)
                   .Parameter("promotions", promotionSysNo).Execute();
        }

        /// <summary>
        /// 选择/取消选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isChecked">是否选中(CustomerStatus.是否选中)</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public override void UpdateCheckedItem(int customerSysNo, CustomerStatus.是否选中 isChecked)
        {
            const string strSql = @"
                        update CrShoppingCartItem 
                        set IsChecked = @IsChecked
                        where customersysno = @customersysno 
                          and ProductSalesType != @ProductSalesType";

            Context.Sql(strSql)
                   .Parameter("IsChecked", (int)isChecked)
                   .Parameter("customersysno", customerSysNo)
                   .Parameter("ProductSalesType", (int)CustomerStatus.商品销售类型.赠品).Execute();
        }

        /// <summary>
        /// 选择/取消选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isChecked">是否选中(CustomerStatus.是否选中)</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public override void UpdateCheckedItem(int customerSysNo, int[] sysNo, CustomerStatus.是否选中 isChecked)
        {
            if (sysNo == null || sysNo.Count() == 0)
            {
                return;
            }

            const string strSql = @"
                        update CrShoppingCartItem 
                        set IsChecked = @IsChecked
                        where customersysno = @customersysno 
                          and sysno in(@sysNo)";
            Context.Sql(strSql)
                .Parameter("IsChecked", (int)isChecked)
                .Parameter("customersysno", customerSysNo)
                .Parameter("sysNo", sysNo).Execute();
        }

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public override void UpdateQuantity(int customerSysNo, int[] sysNo, int quantity)
        {
            if (sysNo == null || sysNo.Count() == 0)
            {
                return;
            }

            const string strSql = @"
                        update CrShoppingCartItem 
                        set Quantity = @Quantity
                        where customersysno = @customersysno 
                          and sysno in(@sysNo)";
            Context.Sql(strSql)
                .Parameter("Quantity", quantity)
                .Parameter("customersysno", customerSysNo)
                .Parameter("sysNo", sysNo).Execute();
        }

        /// <summary>
        /// 更新购物车组商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public override void UpdateQuantity(int customerSysNo, string groupCode, string promotionSysNo, int quantity)
        {
            if (groupCode == null || promotionSysNo == null)
            {
                return;
            }

            const string strSql = @"
                        update CrShoppingCartItem 
                        set Quantity = @Quantity
                        where customersysno = @customersysno 
                          and IsLock = @islock
                          and GroupCode = @groupcode
                          and promotions = @promotions";
            Context.Sql(strSql)
                   .Parameter("Quantity", quantity)
                   .Parameter("customersysno", customerSysNo)
                   .Parameter("islock", (int)CustomerStatus.购物车是否锁定.是)
                   .Parameter("groupcode", groupCode)
                   .Parameter("promotions", promotionSysNo).Execute();
        }

        /// <summary>
        /// 根据有效赠品移除无效赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="currContainSysNo">有效赠品列表</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public override void RemoveInvalidGift(int customerSysNo, int[] currContainSysNo)
        {
            const string strSql = @"
                        delete CrShoppingCartItem 
                        where customersysno = @customersysno 
                          and ProductSalesType = @ProductSalesType
                          and (0 = @nosysno or sysno not in(@sysNo))";

            Context.Sql(strSql)
                .Parameter("customersysno", customerSysNo)
                .Parameter("ProductSalesType", (int)CustomerStatus.商品销售类型.赠品)
                .Parameter("nosysno", currContainSysNo.IsNullOrEmpty() ? 0 : currContainSysNo.Length)
                .Parameter("sysNo", currContainSysNo.IsNullOrEmpty() ? new[] { 0 } : currContainSysNo).Execute();
        }
    }
}
