using Hyt.DataAccess.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品与商品分类关联关系
    /// </summary>
    /// <remarks>2013-07-18 邵斌 创建</remarks>
    public class PdCategoryAssociationDaoImpl : IPdCategoryAssociationDao
    {
        /// <summary>
        /// 添加商品的的对应关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public override int Create(Model.PdCategoryAssociation newPdCategoryAssociation)
        {
            return Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", newPdCategoryAssociation)
                        .AutoMap(p => p.SysNo)
                        .ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 添加商品的的对应关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public override int CreateB2B(Model.PdCategoryAssociation newPdCategoryAssociation)
        {
            return ContextB2B.Insert<PdCategoryAssociation>("PdCategoryAssociation", newPdCategoryAssociation)
                        .AutoMap(p => p.SysNo)
                        .ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 删除指定的商品分类关联关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联关系对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public override bool Delete(PdCategoryAssociation newPdCategoryAssociation)
        {
            return Context.Delete("PdCategoryAssociation", newPdCategoryAssociation).Execute() >= 0;
        }

        /// <summary>
        /// 删除指定商品的指定关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public override bool Delete(int productSysNo, int categorySysNo)
        {
            return Context.Sql("delete from PdCategoryAssociation where productsysno=@0 and categorysysno=@1")
                          .Parameter("0", productSysNo)
                          .Parameter("1", categorySysNo)
                          .Execute() >= 0;
        }

        /// <summary>
        /// 删除指定商品的一组关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="categorySysNoList">商品分类编号列表</param>
        /// <param name="exceptMaster">排除主分类</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>
        /// 2013-07-18 邵斌 创建
        /// 2016-05-06 陈海裕 修改
        /// </remarks>
        public override bool Delete(int productSysNo, IList<int> categorySysNoList, bool exceptMaster = false)
        {
            string sqlText = string.Format("delete from PdCategoryAssociation where productsysno=@0 and categorysysno in ({0})", categorySysNoList.Join(","));
            if (exceptMaster == true)
            {
                sqlText = string.Format("delete from PdCategoryAssociation where productsysno=@0 AND IsMaster=0 and categorysysno in ({0})", categorySysNoList.Join(","));
            }
            return Context.Sql(sqlText).Parameter("0", productSysNo).Execute() >= 0;
        }

        /// <summary>
        /// 删除指定商品的指定关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        // <remarks>2014-08-08 余勇 创建</remarks>
        public override bool Delete(int productSysNo)
        {
            return Context.Sql("delete from PdCategoryAssociation where productsysno=@0")
                          .Parameter("0", productSysNo)
                          .Execute() >= 0;
        }

        /// <summary>
        /// 检查和商品分类对应关系是否存在
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="categorySysNo"></param>
        /// <returns>返回 true:包含 false：不包含</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public override bool IsExist(int productSysNo, int categorySysNo)
        {
            return
                Context.Sql(
                    "select Count(SysNo) from  PdCategoryAssociation where productsysno=@0 and CategorySysNo=@1",
                    productSysNo, categorySysNo).QuerySingle<int>() > 0;
        }
        /// <summary>
        /// 检查b2b商品分类对应关系是否存在
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="categorySysNo"></param>
        /// <returns>返回 true:包含 false：不包含</returns>
        /// <remarks>2017-010-11 罗勤瑶 创建</remarks>
        public override bool IsExistInB2B(int productSysNo, int categorySysNo)
        {
            return
                ContextB2B.Sql(
                    "select Count(SysNo) from  PdCategoryAssociation where productsysno=@0 and CategorySysNo=@1",
                    productSysNo, categorySysNo).QuerySingle<int>() > 0;
        }

        /// <summary>
        /// 更换商品主分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="newMasterCategorySysNo">新主商品分类系统编号</param>
        /// <returns>返回 true:修改成功 false:修改失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        /// <remarks>2013-01-16 邵斌 优化更新方式</remarks>
        public override bool ChangeProductMasterCategory(int productSysNo, int newMasterCategorySysNo)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                _context.Sql(
                     "update PdCategoryAssociation set IsMaster=@0 where productSysNo=@1 and IsMaster=@2",
                     (int)ProductStatus.是否是主分类.否, productSysNo, (int)ProductStatus.是否是主分类.是).Execute();

                return _context.Sql(
                    "update PdCategoryAssociation set IsMaster=@0 where productSysNo=@1 and CategorySysNo=@2",
                    (int)ProductStatus.是否是主分类.是, productSysNo, newMasterCategorySysNo).Execute() > 0;

            }

            //默认是返回更新失败
            return false;
        }
    }
}
