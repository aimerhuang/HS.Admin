using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品关联维护
    /// </summary>
    /// <remarks>2013-07-23 邵斌 创建</remarks>
    public abstract class IPdProductAssociationDao : DaoBase<IPdProductAssociationDao>
    {

        /// <summary>
        /// 获取指定商品的关联商品列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>关联商品列表</returns>
        /// <remarks>2013-07-23 邵斌 创建</remarks>
        public abstract IList<CBProductAssociation> ProductList(int productSysNo);

        /// <summary>
        /// 获取指定商品的关联商品列表
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回B2CAPP接口转用商品对象</returns>
        /// <remarks>2013-08-23 邵斌 创建</remarks>
        public abstract IList<CBProductAssociation> GetProductList(int productSysNo);

        /// <summary>
        /// 创建商品的商品关联关系
        /// </summary>
        /// <param name="model">商品关联关系模型</param>
        /// <param name="context">共享数据库操作上线文</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public abstract bool Create(PdProductAssociation model, IDbContext context = null);

        /// <summary>
        /// 创建一组商品的商品关联关系
        /// </summary>
        /// <param name="mainProductSysNo">主商品系统编号</param>
        /// <param name="associationProductSysNoList">关联商品系统编号</param>
        /// <param name="updateUser">更新操作人</param>
        /// <param name="relactionCode">关联关系码</param>
        /// <returns>返回 true:成功 false:失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public abstract bool Create(int mainProductSysNo, int[] associationProductSysNoList, int updateUser, string relactionCode);

        /// <summary>
        /// 清理指定商品的关联关系
        /// </summary>
        /// <param name="relationCode">关联关系码</param>
        /// <returns>返回 True：清理成功 False：清理失败</returns>
        /// <remarks>2013-07-24 邵斌 创建</remarks>
        public abstract bool Clear(string relationCode);

        /// <summary>
        /// 获取商品关联关系码
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="context">数据库操作上下文</param>
        /// <returns>返回管理关系码</returns>
        /// <remarks>2013-08-16 邵斌 创建</remarks>
        public abstract string GetRelactionCode(int productSysNo, IDbContext context = null);

        /// <summary>
        /// 根据商品关联码读取商品所以关联属性
        /// </summary>
        /// <param name="relationCode">关联关系码</param>
        /// <returns>返回：关联属性列表</returns>
        /// <remarks>2013-08-16 邵斌 创建</remarks>
        public abstract IList<PdProductAttribute> GetAssociationAttributeList(string relationCode);

    }
}
