using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品搭配销售
    /// </summary>
    /// <remarks>2013-07-09 邵斌 创建</remarks>
    public class PdProductCollocationDaoImpl : IPdProductCollocationDao
    {
        /// <summary>
        /// 读取一个商品的搭配销售商品集合
        /// </summary>
        /// <param name="productSysNo">主商品系统编号(主商品系统编号是作为Code来使用)</param>
        /// <returns>返回指定商品的搭配销售列表</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override IList<CBProductListItem> GetList(int productSysNo)
        {
            #region 测试SQL
            /*
                     select 
                        pd.SysNo,Pd.ErpCode,pd.ProductName,pdp.price,pdc.categoryname
                    from 
                        pdproduct pd 
                        inner join PdProductCollocation pdpc on pd.sysno = pdpc.productsysno
                        inner join PdCategoryAssociation pda on pda.productsysno = pd.sysno
                        inner join pdcategory pdc on pdc.sysno = pda.categorysysno
                        left join pdprice pdp on pdp.productsysno = pd.sysno and pdp.status=0 and pdp.pricesource = 0   //只显示基础价格并且价格状态要为正常
                    where
                        pdpc.code = [主商品系统编号] 
                        and pda.ismaster = 1
                     */
            #endregion

            return Context.Select<CBProductListItem>("pd.SysNo,Pd.ErpCode,pd.EasName as ProductName,pdp.price,pdc.categoryname")
                          .From(@"pdproduct pd 
                        inner join PdProductCollocation pdpc on pd.sysno = pdpc.productsysno
                        inner join PdCategoryAssociation pda on pda.productsysno = pd.sysno
                        inner join pdcategory pdc on pdc.sysno = pda.categorysysno
                        left join pdprice pdp on pdp.productsysno = pd.sysno and pdp.status=@status and pdp.pricesource = @pricesource")
                          .Where("pdpc.code = @code and pda.ismaster = @ismaster")
                          .Parameter("status", (int)ProductStatus.产品价格状态.有效)
                          .Parameter("pricesource", (int)ProductStatus.产品价格来源.基础价格)
                          .Parameter("Code", productSysNo)
                          .Parameter("ismaster", (int)ProductStatus.是否是主分类.是)
                          .QueryMany();
        }

        /// <summary>
        /// 添加搭配商品
        /// </summary>
        /// <param name="productCollocation">搭配商品对象</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override bool Create(PdProductCollocation productCollocation)
        {
            /*
             * 一条sql语句就不用事务了吧
             * transaction.Delete
             * transaction.Insert
             */

            return Context.Insert<PdProductCollocation>("PdProductCollocation", productCollocation)
                              .AutoMap(c => c.SysNo)
                              .ExecuteReturnLastId<int>("SysNo") > 0;
        }

        /// <summary>
        /// 添加一组搭配商品
        /// </summary>
        /// <param name="productCollocations">搭配商品对象组</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override bool Create(IList<PdProductCollocation> productCollocations)
        {
            int result = 0;
            using (var _context = Context.UseSharedConnection(true))
            {
                //遍历搭配商品对象集合
                foreach (PdProductCollocation productCollocation in productCollocations)
                {
                    //写入数据库
                    result = CreateProductCollocation(productCollocation, _context);

                    //如果插入失败将结束创建操作
                    if (result == 0)
                    {
                        return false;
                    }

                }

            }

            return true;
        }

        /// <summary>
        /// 更新商品的搭配销售商品列表
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号(作为code查询条件)</param>
        /// <param name="removeProductSysNoList">要从搭配商品列表中移除的商品列表</param>
        /// <param name="newProductSysNoList">要被加入到搭配商品中的商品列表</param>
        /// <returns>返回 True:更新成功 False:更新失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override bool Update(int masterProductSysNo, int[] removeProductSysNoList, IList<PdProductCollocation> newProductSysNoList)
        {
            bool success = false;

            using (var _context = Context.UseSharedConnection(true))
            {
                //删除要被移除的商品
                success = _context.Sql(
                    "delete from PdProductCollocation where code=@masterProductSysNo and productsysno  in (" + (removeProductSysNoList.AsDelimited(",")) + ")")
                                 .Parameter("masterProductSysNo", masterProductSysNo)
                                 .Execute() == removeProductSysNoList.Length;

                //如果移除失败将终止操作
                if (!success)
                {
                    return false;
                }

                //添加新商品到搭配销售商品
                foreach (PdProductCollocation newPdProductCollocation in newProductSysNoList)
                {
                    success = _context.Insert<PdProductCollocation>("PdProductCollocation", newPdProductCollocation)
                                     .AutoMap(c => c.SysNo)
                                     .ExecuteReturnLastId<int>("SysNo") > 0;

                    //添加失败将终止操作
                    if (!success)
                    {
                        return false;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// 删除搭配商品
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号(主商品系统编号是作为Code来使用)： masterProductSysNo 不为0时表示删除的是指定商品的搭配销售商品</param>
        /// <param name="collocationProductSysNo">搭配商品系统编号： collocationProductSysNo 不为0时表示将一个商品从所有搭配销售商品中删除掉</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>
        ///     主商品系统编号和搭配商品系统编号可以任意组合
        ///     masterProductSysNo 和 collocationProductSysNo 同时不为0时表示指定删除一个商品的指定搭配销售商品
        ///     masterProductSysNo 和 collocationProductSysNo  不能同时为0
        /// </remarks>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override bool Delete(int masterProductSysNo = 0, int collocationProductSysNo = 0)
        {
            //masterProductSysNo 和 collocationProductSysNo  不能同时为0
            //如果都为0就变成了删除所有的搭配销售产品
            if (masterProductSysNo + collocationProductSysNo <= 0)
                return false;

            int result;

            //删除数据
            result = Context.Sql(
                @"Delete from PdProductCollocation 
                        where (@masterProductSysNo = 0 or code = @masterProductSysNo) 
                        and (@collocationProductSysNo = 0 or ProductSysNo=@collocationProductSysNo)")
                            .Parameter("masterProductSysNo", masterProductSysNo)
                            //.Parameter("masterProductSysNo", masterProductSysNo)
                            .Parameter("collocationProductSysNo", collocationProductSysNo)
                           // .Parameter("collocationProductSysNo", collocationProductSysNo)
                            .Execute();

            return result >= 0;
        }

        /// <summary>
        /// 检查搭配商品是否已经在主商品搭配商品列表中
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号</param>
        /// <param name="collocationProductSysNo">搭配商品系统编号</param>
        /// <returns>返回 true:存在 false:不存在</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override bool IsExist(int masterProductSysNo, int collocationProductSysNo)
        {
            #region 测试SQL
            /*
            select count(sysno) from PdProductCollocation where code =1 and productsysno=2
            */
            #endregion

            return Context.Sql(
                    "select count(sysno) from PdProductCollocation where code =@code and productsysno=@productsysno")
                       .Parameter("code", masterProductSysNo)
                       .Parameter("productsysno", collocationProductSysNo).QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 检查一组搭配商品是否已经在主商品搭配商品列表中
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号</param>
        /// <param name="collocationProductSysNoList">搭配商品系统编号数组</param>
        /// <returns>返回 true:存在 false:不存在</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public override bool IsExist(int masterProductSysNo, int[] collocationProductSysNoList)
        {
            #region 测试SQL
            /*
            select count(sysno) from PdProductCollocation where code =1 and productsysno in (2)
            */
            #endregion

            int result = Context.Sql(
                    "select count(sysno) from PdProductCollocation where code =@code and productsysno in (" + collocationProductSysNoList.AsDelimited(",") + ")")
                       .Parameter("code", masterProductSysNo).QuerySingle<int>();
            return result == collocationProductSysNoList.Length;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-22 苟治国 创建</remarks>
        public override int Insert(PdProductCollocation model)
        {
            int id = Context.Insert<PdProductCollocation>("PdProductCollocation", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
            return id;
        }

        #region 私有方法

        /// <summary>
        /// 添加搭配商品
        /// </summary>
        /// <param name="productCollocation">搭配商品对象</param>
        /// <param name="context">数组库操作对象（未来实现外部事务操作）</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        private int CreateProductCollocation(PdProductCollocation productCollocation, Base.IDbContext context = null)
        {
            context = context ?? Context;           //设置默认值
            return context.Insert<PdProductCollocation>("PdProductCollocation", productCollocation)
                              .AutoMap(c => c.SysNo)
                              .ExecuteReturnLastId<int>("SysNo");
        }

        #endregion

    }
}
