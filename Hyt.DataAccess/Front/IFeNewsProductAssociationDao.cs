using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 新闻商品关联表接口抽象类
    /// </summary>
    /// <remarks>2013－01-14 苟治国 创建</remarks>
    public abstract class IFeNewsProductAssociationDao : DaoBase<IFeNewsProductAssociationDao>
    {
        /// <summary>
        /// 查看新闻商品关联表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新闻商品表</returns>
        /// <remarks>2014-9-12 陈俊 创建</remarks>
        public abstract Model.FeNewsProductAssociation GetEntity(int sysNo);

        /// <summary>
        /// 查看新闻商品关联表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新闻商品关联表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract Model.CBFeNewsProductAssociation GetModel(int sysNo);

        /// <summary>
        /// 根据条件获取新闻商品关联表
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <returns>新闻商品关联表列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract IList<Model.CBFeNewsProductAssociation> Seach(int newsSysNo);

        /// <summary>
        /// 查看在当前类型中是否有相同产品称
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>总数</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract int GetCount(int newsSysNo, int productSysNo);

        /// <summary>
        /// 插入新闻商品关联表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract int Insert(Model.FeNewsProductAssociation model);

        /// <summary>
        /// 更新新闻商品关联表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract int Update(Model.FeNewsProductAssociation model);

        /// <summary>
        /// 删除新闻商品关联表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract bool Delete(int sysNo);
    }
}
