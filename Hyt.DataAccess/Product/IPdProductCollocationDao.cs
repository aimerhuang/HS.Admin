using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品搭配销售
    /// </summary>
    /// <remarks>2013-07-09 邵斌 创建</remarks>
    public abstract class IPdProductCollocationDao : DaoBase<IPdProductCollocationDao>
    {
        /// <summary>
        /// 读取一个商品的搭配销售商品集合
        /// </summary>
        /// <param name="productSysNo">主商品系统编号(主商品系统编号是作为Code来使用)</param>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract IList<CBProductListItem> GetList(int productSysNo);

        /// <summary>
        /// 添加搭配商品
        /// </summary>
        /// <param name="productCollocation">搭配商品对象</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract bool Create(PdProductCollocation productCollocation);

        /// <summary>
        /// 添加一组搭配商品
        /// </summary>
        /// <param name="productCollocations">搭配商品对象组</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract bool Create(IList<PdProductCollocation> productCollocations);

        /// <summary>
        /// 更新商品的搭配销售商品列表
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号(作为code查询条件)</param>
        /// <param name="removeProductSysNoList">要从搭配商品列表中移除的商品列表</param>
        /// <param name="newProductSysNoList">要被加入到搭配商品中的商品列表</param>
        /// <returns>返回 True:更新成功 False:更新失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract bool Update(int masterProductSysNo, int[] removeProductSysNoList, IList<PdProductCollocation> newProductSysNoList);

        /// <summary>
        /// 删除搭配商品
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号(主商品系统编号是作为Code来使用)</param>
        /// <param name="collocationProductSysNo">搭配商品系统编号</param>
        /// <returns>返回 true:操作成功 false:操作失败</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract bool Delete(int masterProductSysNo = 0, int collocationProductSysNo = 0);

        /// <summary>
        /// 检查搭配商品是否已经在主商品搭配商品列表中
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号</param>
        /// <param name="collocationProductSysNo">搭配商品系统编号</param>
        /// <returns>返回 true:存在 false:不存在</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract bool IsExist(int masterProductSysNo, int collocationProductSysNo);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-22 苟治国 创建</remarks>
        public abstract int Insert(PdProductCollocation model);

        /// <summary>
        /// 检查一组搭配商品是否已经在主商品搭配商品列表中
        /// </summary>
        /// <param name="masterProductSysNo">主商品系统编号</param>
        /// <param name="collocationProductSysNoList">搭配商品系统编号数组</param>
        /// <returns>返回 true:存在 false:不存在</returns>
        /// <remarks>2013-07-09 邵斌 创建</remarks>
        public abstract bool IsExist(int masterProductSysNo, int[] collocationProductSysNoList);
    }
}
