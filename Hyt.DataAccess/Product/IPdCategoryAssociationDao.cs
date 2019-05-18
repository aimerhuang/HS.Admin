using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品与商品分类关联关系
    /// </summary>
    /// <remarks>2013-07-18 邵斌 创建</remarks>
    public abstract class IPdCategoryAssociationDao : DaoBase<IPdCategoryAssociationDao>
    {
        /// <summary>
        /// 添加商品的的对应关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public abstract int Create(PdCategoryAssociation newPdCategoryAssociation);

        /// <summary>
        /// 添加B2B商品的的对应关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public abstract int CreateB2B(PdCategoryAssociation newPdCategoryAssociation);

        /// <summary>
        /// 删除指定的商品分类关联关系
        /// </summary>
        /// <param name="newPdCategoryAssociation">商品分类关联关系对象</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public abstract bool Delete(PdCategoryAssociation newPdCategoryAssociation);

        /// <summary>
        /// 删除指定商品的指定关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public abstract bool Delete(int productSysNo, int categorySysNo);

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
        public abstract bool Delete(int productSysNo, IList<int> categorySysNoList, bool exceptMaster = false);

        /// <summary>
        /// 删除指定商品的指定关联关系分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回 true:添加成功 false:添加失败</returns>
        // <remarks>2014-08-08 余勇 创建</remarks>
        public abstract bool Delete(int productSysNo);

        /// <summary>
        /// 检查和商品分类对应关系是否存在
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="categorySysNo"></param>
        /// <returns>返回 true:存在 false:不存在</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public abstract bool IsExist(int productSysNo, int categorySysNo);

        /// <summary>
        /// 检查b2b商品分类对应关系是否存在
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="categorySysNo"></param>
        /// <returns>返回 true:包含 false：不包含</returns>
        /// <remarks>2017-010-11 罗勤瑶 创建</remarks>
        public abstract bool IsExistInB2B(int productSysNo, int categorySysNo);

        /// <summary>
        /// 更换商品主分类
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="newMasterCategorySysNo">新主商品分类系统编号</param>
        /// <returns>返回 true:修改成功 false:修改成功</returns>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public abstract bool ChangeProductMasterCategory(int productSysNo, int newMasterCategorySysNo);

    }
}
